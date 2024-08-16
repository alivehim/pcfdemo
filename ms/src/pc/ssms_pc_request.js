var PROXCardRequestSDK = window.PROXCardRequestSDK || {};


PROXCardRequestSDK.onFormLoad = async (context) => {
    PROXCardRequestSDK.changeFormType()
    PROXCardRequestSDK.showSection()
    PROXCardRequestSDK.unlockFields()
    await PROXCardRequestSDK.autofillAsync()
    PROXCardRequestSDK.initIssueDepartment()
    PROXCardRequestSDK.changeViewOfSubgrid()
    PROXCardRequestSDK.hideWebResource()

    PROXCardRequestSDK.renderPICS()


    const responsible_engineer = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsibleperson).getValue()
    const chosenapprover = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).getValue()



    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()

    if (!status) {

        if (responsible_engineer) {
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsibleperson).fireOnChange()
        }

        if (chosenapprover) {
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).fireOnChange()
        }
    }


    // var gridContext = Xrm.Page.ui.formContext.getControl(ShareSdk.ProximityCardRequestSubGrids.EditableDetail)

    // gridContext.addOnLoad((exeContext) => {
    //     // var _formContext = exeContext.getFormContext();
    //     // var currentEntity = _formContext.data.entity;
    //     // currentEntity.attributes.forEach(function (attribute, i) {

    //     //     var attributeToDisable = attribute.controls.get(0);
    //     //     attributeToDisable.setDisabled(true);

    //     // });

    //     // var _formContext = exeContext.getFormContext();
    //     // var _gridContext = _formContext.getControl(ShareSdk.ProximityCardRequestSubGrids.EditableDetail)
    //     // var allRows = _gridContext.getGrid().getRows()

    //     // allRows.forEach(function (row, i) {
    //     //     var gridColumns = row.data.entity.attributes;
    //     //     gridColumns.forEach(function (column, j) {
    //     //         var attributeToDisable = column.controls.get(0);
    //     //         attributeToDisable.setDisabled(true);
    //     //     });

    //     // });

    //     // let oFormContext = exeContext.getFormContext();
    //     // if (oFormContext) {

    //     //     let objEntity = oFormContext.data.entity;
    //     //     objEntity.attributes.forEach(function (attribute, i) {
    //     //         console.log(attribute)

    //     //         let attributeToDisable = attribute.controls.get(0);
    //     //         attributeToDisable.setVisible(false);

    //     //     });
    //     // }

    //     var scRows = Xrm.Page.getControl(ShareSdk.ProximityCardRequestSubGrids.EditableDetail).getGrid().getRows();

    //     if (scRows != null && scRows !== "undefined") {

    //         scRows.forEach(
    //             function (scRow, i) {

    //                 var scControls = scRow.getData().getEntity().attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).controls;

    //                 if (scControls != null && scControls !== "undefined") {
    //                     scControls.forEach(
    //                         function (ctl, j) {

    //                             ctl.setVisible(false);
    //                         })
    //                 }

    //             })
    //     }
    // })
}
PROXCardRequestSDK.hideWebResource = () => {
    let visiable = false
    const status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        visiable = ShareSdk.isPROXCardRequestor()
    }

    let control = Xrm.Page.ui.controls.get("WebResource_applicant")
    control.setVisible(visiable);
}

PROXCardRequestSDK.changeFormType = () => {
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Request_Main) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Request_Main);
        formItem.navigate();
    }
}

/**
 * change the view to hide the status and remarks
 * https://nebulaaitsolutions.com/how-to-change-view-of-subgrid-conditionally-using-javascript/
 * @param {*} executionContext 
 */
PROXCardRequestSDK.changeViewOfSubgrid = (executionContext) => {
    var oFormContext = Xrm.Page.ui.formContext
    if (oFormContext != null) {
        var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
        if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {

            const editablegrid = () => {
                var viewSelector = oFormContext.getControl(ShareSdk.ProximityCardRequestSubGrids.EditableDetail).getViewSelector();
                var ProjectTemplateView = {
                    entityType: 1039, // SavedQuery
                    id: "7c9514fc-fc85-ed11-81ac-000d3a07ca3c",
                    name: "View Name"
                }
                viewSelector.setCurrentView(ProjectTemplateView);
            }
            var viewSelector = oFormContext.getControl(ShareSdk.ProximityCardRequestSubGrids.SubGridDetail).getViewSelector();
            var ProjectTemplateView = {
                entityType: 1039, // SavedQuery
                id: "7c9514fc-fc85-ed11-81ac-000d3a07ca3c",
                name: "View Name"
            }
            viewSelector.setCurrentView(ProjectTemplateView);

            //editablegrid()
        }
    }
}

PROXCardRequestSDK.showSection = () => {
    formContext = Xrm.Page.ui.formContext
    var tabObj = formContext.ui.tabs.get('General');
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status) {
        ShareSdk.setVisiableControl(ShareSdk.PROXCardReqFields.nosofss, true)

        if (Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_migration_data).getValue()) {
            ShareSdk.setVisiableControl(ShareSdk.PROXCardReqFields.ss_display, true)
            // var sectionObj = tabObj.sections.get('Substation_Access')
            // sectionObj.setVisible(false)

            const subgrid_station = Xrm.Page.ui.formContext.getControl(ShareSdk.ProximityCardRequestSubGrids.Subgrid_station)
            subgrid_station.setVisible(false)
        }
    }

    if (status === ShareSdk.PROXCardReqStatus.WaitingforReview ||
        status === ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval) {

        // show 'filled by reviewer' section
        var sectionObj = tabObj.sections.get('General_section_Filled_By_Reiviewer');
        sectionObj.setVisible(true);
    }


    var remarksSection = tabObj.sections.get('General_section_remarks');
    remarksSection.setVisible(!status);

    var remarkTableSection = tabObj.sections.get('General_section_remarks_table');
    remarkTableSection.setVisible(status != null);

    var clp_responsiblecp = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsiblecp).getValue()
    var responsible_cp = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsible_cp).getValue()
    var extend_from_request = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.extend_from_request).getValue()
    if (clp_responsiblecp && !responsible_cp && !extend_from_request) {
        ShareSdk.setVisiableControl(ShareSdk.PROXCardReqFields.responsiblecp, true)
        ShareSdk.setVisiableControl(ShareSdk.PROXCardReqFields.responsible_cp, false)
    }

}

PROXCardRequestSDK.autofillAsync = async () => {
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {

        var re = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsibleperson).getValue()
        if (!re) {
            var currentUserId = ShareSdk.getCurrentUserId()

            var user = await ShareSdk.getUserBySystemUserAsync(currentUserId)
            var departmentBranch = (user[ShareSdk.CLPUserFields.dept_name] ?? '') + ' / ' + (user[ShareSdk.CLPUserFields.branch_name] ?? '')
            ShareSdk.setLookupValue(user.clp_userid, user.clp_name, ShareSdk.Tables.user, ShareSdk.PROXCardReqFields.responsibleperson)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.re_departmentbranch).setValue(departmentBranch)
        }
    }
}

PROXCardRequestSDK.unlockFields = () => {


    const unlockfileds = (lockFields) => {
        for (let attr in Xrm.Page.data.entity.attributes._collection) {
            if (!lockFields.some(p => p == attr)) {
                ShareSdk.setDisableControl(attr, false)
                // Xrm.Page.data.entity.attributes.getByName(attr).controls.getAll()[0]?.setDisabled(false);
            }
        }
    }
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {

        if (ShareSdk.isPROXCardRequestor()) {
            let lockFields = [
                ShareSdk.PROXCardReqFields.applicationno,
                ShareSdk.PROXCardReqFields.workflow_status,
                ShareSdk.PROXCardReqFields.re_departmentbranch,
                ShareSdk.PROXCardReqFields.approver_deptbranch,
                ShareSdk.PROXCardReqFields.requestdate,
                ShareSdk.PROXCardReqFields.mm_dept_branch]

            if (Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.extend_from_request).getValue()) {
                lockFields.push(ShareSdk.PROXCardReqFields.accessperiodfrom)
            }

            if (status === ShareSdk.PROXCardReqStatus.RequestRejected) {
                lockFields.push(ShareSdk.PROXCardReqFields.issuedepartment)
            }

            unlockfileds(lockFields)
        }
    }
    else if (status === ShareSdk.PROXCardReqStatus.WaitingforReview) {
        if (ShareSdk.isProximityCardAdmin()) {
            ShareSdk.setDisableControl(ShareSdk.PROXCardReqFields.chosen_mm, false)
            ShareSdk.setRequiredControl(ShareSdk.PROXCardReqFields.chosen_mm)
        }
    }
}

PROXCardRequestSDK.initIssueDepartment = () => {
    //   Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).getAttribute().getOptions();
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).clearOptions();
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).addOption({ text: 'AMD資產管理', value: 768230004 });
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).addOption({ text: 'East & West Region東西區', value: 768230000 });
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).addOption({ text: 'North Region北區', value: 768230001 });
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).addOption({ text: 'Transmission輸電', value: 768230002 });
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardReqFields.issuedepartment).addOption({ text: 'TSD技術服務', value: 768230003 });
    }

}

PROXCardRequestSDK.onResponsibleCPChanged = async () => {
    Xrm.Page.ui.refreshRibbon()
}

PROXCardRequestSDK.onREChanged = async () => {
    var re = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsibleperson).getValue()
    if (re) {
        var reid = ShareSdk.getLookupId(re)
        var departmentBranch = await ShareSdk.getDeptBranchAsync(reid)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.re_departmentbranch).setValue(departmentBranch || '')
    } else {
        //clear Dept./Branch 部門/支部
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.re_departmentbranch).setValue()
    }
}

PROXCardRequestSDK.onApproverChanged = async () => {
    var approver = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).getValue()
    if (approver) {
        var approverid = ShareSdk.getLookupId(approver)
        var departmentBranch = await ShareSdk.getDeptBranchAsync(approverid)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.approver_deptbranch).setValue(departmentBranch || '')
    } else {
        //clear Dept./Branch 部門/支部
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.approver_deptbranch).setValue()
    }
}

PROXCardRequestSDK.onSeniorCivilMgrChanged = async () => {
    var mm = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosen_mm).getValue()
    if (mm) {
        var mmid = ShareSdk.getLookupId(mm)
        var departmentBranch = await ShareSdk.getDeptBranchAsync(mmid)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_dept_branch).setValue(departmentBranch || '')
    } else {
        //clear Dept./Branch 部門/支部
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_dept_branch).setValue()
    }
}

PROXCardRequestSDK.onAccessperiodfromChanged = () => {
    formContext = Xrm.Page.ui.formContext
    var accessperiodfrom = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue()
    if (accessperiodfrom) {

        formContext.getControl(ShareSdk.PROXCardReqFields.accessperiodfrom).clearNotification(ShareSdk.PROXCardErrors.AccessPeriodFrom.id);

        var today = new Date()
        today.setHours(0, 0, 0, 0);
        accessperiodfrom.setHours(0, 0, 0, 0);
        if (accessperiodfrom < today) {

            formContext.getControl(ShareSdk.PROXCardReqFields.accessperiodfrom).addNotification({
                notificationLevel: 'ERROR',
                messages: [ShareSdk.PROXCardErrors.AccessPeriodFrom.message],
                uniqueId: ShareSdk.PROXCardErrors.AccessPeriodFrom.id
            });

        }
    }

}

PROXCardRequestSDK.onAccessperiodtoChanged = () => {
    formContext = Xrm.Page.ui.formContext
    var accessperiodto = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()
    var accessperiodfrom = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue()
    if (accessperiodto && accessperiodfrom) {

        formContext.getControl(ShareSdk.PROXCardReqFields.accessperiodto).clearNotification(ShareSdk.PROXCardErrors.AccessPeriodTo.id);


        if (accessperiodfrom > accessperiodto) {

            formContext.getControl(ShareSdk.PROXCardReqFields.accessperiodto).addNotification({
                notificationLevel: 'ERROR',
                messages: [ShareSdk.PROXCardErrors.AccessPeriodTo.message],
                uniqueId: ShareSdk.PROXCardErrors.AccessPeriodTo.id
            });

        }
    }
}

PROXCardRequestSDK.renderPICS = () => {
    // const selector = () => window.parent.document.querySelector(`div[data-id*="clp_readunderstoodandagreed"]`)
    const selector = () => window.parent.document.querySelector(`section[data-id*="PICS"]>div>div`)
    const notificationId = "SSMS_PICS_Link_ID"

    ShareSdk._doUntil(
        () => {
            if (window.parent.document.querySelector(`#${notificationId}`)) {
                return
            }

            ShareSdk.getMappingValue("SSMS_PICS_Link").then(link => {

                if (link) {
                    const parentEle = selector()
                    const ele = document.createElement('div')
                    ele.setAttribute("id", "questionNotification")
                    ele.style.padding = '5px 20px'
                    ele.style.margin = "10px 0"
                    ele.style.border = 'solid 1px #5C646C'
                    ele.innerHTML = `The applicants have fully read, understood and agreed to the <a target='_blank' href='${link}' style="color=blue; font-style:italic;">CLP Personal information collection statement.</a> </br>申請人已閱讀和明白並同意中華電力的<a target='_blank' href='${link}' style="color=blue;">個人資料收集聲明</a>`
                    parentEle.insertBefore(ele, parentEle.childNodes[0])
                }

            })

        },
        selector,
    )
}

