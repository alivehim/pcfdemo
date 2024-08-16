var PROXCardReqExtendSDK = window.PROXCardReqExtendSDK || {};

PROXCardReqExtendSDK.onAction = async () => {
    const getRequestDetail = async (id) => {
        Xrm.Utility.showProgressIndicator("Loading")
        try {

            if (id.length === 0) {
                return false;
            }
            let r_id = id.slice(1, id.length - 1);
            const formid = ShareSdk.getCurrentFormId()

            const result = await ShareSdk.getPROXCardRequestAsync(r_id)

            console.log("Retrieved values: ", result);

            await copydetailAsync(r_id, formid, true)

            copyDataIntoForm(result);
            await copySubstation(r_id, formid, true)
            // perform operations on record retrieval

            ShareSdk.setLookupValue(r_id, result.clp_name, ShareSdk.Tables.proximitycardrequest, ShareSdk.PROXCardReqFields.extend_from_request)

        } catch (err) {
            ShareSdk.formError(err)
        }
        finally {
            Xrm.Utility.closeProgressIndicator()
        }

    }

    const propertyKey = (attr) => {
        let key = attr + "@Microsoft.Dynamics.CRM.associatednavigationproperty";
        return key;
    }
    const lookupEntityNameKey = (attr) => {
        let key = attr + "@Microsoft.Dynamics.CRM.lookuplogicalname";
        return key;
    }
    const formatValueKey = (attr) => {
        let key = attr + "@OData.Community.Display.V1.FormattedValue";
        return key;
    }

    function copyDataIntoForm(result) {
        let date_keys = ["clp_accessperiodfrom", "clp_accessperiodto"];
        let abandon_keys =
            ["clp_name",
                "clp_workflow_status",
                "clp_mm",
                "clp_mm_dept_branch",
                "clp_reviewer",
                "clp_reviewed_date",
                "clp_chosen_mm",
                "clp_approver",
                "clp_appr_date",
                "clp_security",
                "clp_sla_count",
                "clp_collection_reminder_date",
                "clp_expiry_3_wd",
                "clp_expiry_5_wd",
                "clp_expiry_10_wd",
                "clp_is_locked",
                "clp_requestdate",
                "clp_is_reminder_email_sended",
                "clp_rejected_date",
                "clp_approver_deptbranch",
                "clp_nosofss",
                "clp_admin",
                "clp_appr_remarks",
                "clp_admin_remarks",
                "clp_mm_remarks",
                "clp_security_remarks",
                "clp_remarks",
                "clp_extend_from_request",
                "clp_readunderstoodandagreed",
                "clp_is_migration_data"
            ]

        let lookup_keys = ["_clp_lut_ctrcomp_value", "_clp_responsibleperson_value", "_clp_chosenapprover_value", "_clp_responsible_cp_value", "_clp_raw_requester_value"];
        let keys = Object.keys(result)
            .filter(key => key.indexOf("@") === -1 && date_keys.indexOf(key) === -1 && lookup_keys.indexOf(key) === -1)
            .filter(key => abandon_keys.indexOf(key) === -1);
        console.log("result: ", result);
        let ctx = Xrm.Page.ui.formContext;

        // å¯¹lookupç‰¹æ®Šå¤„ç†
        for (let key of lookup_keys) {
            // "2022-12-27T00:00:00Z"
            try {
                let lookupValue = [];
                let lookupObject = new Object();
                if (result[key]) {
                    lookupObject.id = result[key];

                    lookupObject.name = result[formatValueKey(key)];

                    lookupObject.entityType = result[lookupEntityNameKey(key)];

                    lookupValue.push(lookupObject);
                    let control = ctx.getAttribute(key.slice(1, key.length - 6)); // å–å‡ºform key
                    console.log("lookup populate", lookupObject);
                    control.setValue(lookupValue);
                }
            } catch (e) {
                console.log("lookup failed", e)
            } finally {

            }
        }

        // å…¶ä»–ç›¸å…³å­—æ®µ
        for (let key of keys) {
            try {
                let control = ctx.getAttribute(key);
                control.setValue(result[key]);
            } catch (e) {
                // console.log("no such key control")
            } finally {

            }
        }

        ShareSdk.setLookupValue(result.clp_proximitycardrequestid, result.clp_name, ShareSdk.Tables.proximitycardrequest, ShareSdk.PROXCardReqFields.extend_from_request)

        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).setValue(new Date())

        ShareSdk.setDisableControl(ShareSdk.PROXCardReqFields.accessperiodfrom,true)

        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).fireOnChange()
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsibleperson).fireOnChange()
    }

    async function copySubstation(id, targetFormId, refreshData = false) {
        // const formid = ShareSdk.getCurrentFormId()

        const currentSubstation = await ShareSdk.getPROXCardSubstationsAsync(targetFormId)
        for (var item of currentSubstation) {
            await ShareSdk.removeSSAsync(targetFormId, item.clp_ssms_lut_substationid)
        }

        const selectRequest = await ShareSdk.getPROXCardRequestAsync(id)
        if (selectRequest.clp_is_migration_data === true) {

            await copySubstationsFromMigratedRequestAsync(targetFormId, selectRequest.clp_ss_display)

        } else {
            const result = await ShareSdk.getPROXCardSubstationsAsync(id)
            await ShareSdk.addSubstionsAsync(targetFormId, result.map(p => p.clp_ssms_lut_substationid))
        }

        if (refreshData) {
            formContext = Xrm.Page.ui.formContext
            var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.Subgrid_station);
            subgrid.refresh();
        }

    }

    const copySubstationsFromMigratedRequestAsync = async (formid, ss_display) => {
        const substations = ss_display.split(",")

        for (var item of substations) {
            const sub = await ShareSdk.getSubstationByScadaCodeAsync(item.trim())

            if (sub) {
                await ShareSdk.addSubstionAsync(formid, sub.clp_ssms_lut_substationid)
            }
        }
    }

    const copydetailAsync = async (id, targetFormId, refreshData = false) => {

        // const formid = ShareSdk.getCurrentFormId()
        const currentDetails = await ShareSdk.getPROXCardRequestDetailsAsync(targetFormId)
        for (var item of currentDetails) {
            await ShareSdk.deletePROXCardREQDTLAsync(item.clp_proximity_card_request_detailid)
        }

        const details = await ShareSdk.getPROXCardRequestDetailsForExtendAsync(id)
        for (var item of details) {
            let cardno = null
            let cardid = null
            let apptype = 0
            if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
                apptype = ShareSdk.PROXCardReqAPPType.EncodeC_Card
                cardno = item["card.clp_name"]
                cardid = item._clp_proximity_card_value
            }
            else if (item._clp_proximity_card_applicant_value === item["card.clp_holder"]) {
                apptype = ShareSdk.PROXCardReqAPPType.EncodeISMS
                cardno = item["card.clp_name"]
                cardid = item._clp_proximity_card_value
            } else {

                var cards = await getCardByHolderAsync(item._clp_proximity_card_applicant_value)
                if (cards.length > 0) {
                    apptype = ShareSdk.PROXCardReqAPPType.EncodeISMS
                    cardno = cards[0].clp_name
                    cardid = cards[0].clp_proximitycardinventoryid
                } else {

                    apptype = ShareSdk.PROXCardReqAPPType.NewISMS
                }
            }

            let clp_contractor_value = item._clp_contractor_value
            if (!item._clp_contractor_value) {
                if (item["request.clp_is_migration_data"] === true) {
                    const clpuser = await ShareSdk.getCLPUserByPersionIdAsync(item.clp_contractor_id)
                    if (clpuser) {
                        clp_contractor_value = clpuser.clp_userid
                    }
                }
            }
            await ShareSdk.addNewPROXCardRequestDetailAsync(
                targetFormId, null,
                apptype,
                item._clp_proximity_card_applicant_value,
                item.clp_applicant_name,
                clp_contractor_value,
                item.clp_contractor_id,
                cardno, cardid, item.clp_phone_no,
                item["card.clp_card_expiry_date"]
            )
        }

        if (refreshData) {

            ShareSdk.refreshPROXCardREQDTLGrid()
        }

    }

    const getCardByHolderAsync = (card_holder_id) => {
        return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=clp_card_type eq 768230000 and _clp_holder_value eq '${card_holder_id}'`,
            {
                headers: {
                    "Content-Type": "application/json",
                    'Prefer': 'odata.include-annotations="*"',
                }
            }
        ).then(res => res.json()).then(res => {
            if (res.error) {
                throw new Error(res.error.message)
            }
            return Promise.resolve(res.value)
        }).catch(err => {
            console.error(err)
            throw err;
        })
    }


    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()

    if (!status) {
        var lookupOptions = {
            defaultEntityType: "clp_proximitycardrequest",
            entityTypes: ["clp_proximitycardrequest"],
            allowMultiSelect: false,
            disableMru: true,
            // defaultViewId: "0D5D377B-5E7C-47B5-BAB1-A5CB8B4AC10",
            // viewIds: ["0D5D377B-5E7C-47B5-BAB1-A5CB8B4AC10", "00000000-0000-0000-00AA-000010001003"],
            searchText: "*",
            filters: [{
                filterXml: "<filter><condition attribute='clp_workflow_status' operator='eq' value='768230005' /></filter>",
                entityLogicalName: "clp_proximitycardrequest"
            }]
        };


        let r_pickup = Xrm.Utility.lookupObjects(lookupOptions).then(
            function (success) {
                // Xrm.Utility.closeProgressIndicator();
                console.log("success callback: ", success);
                return success[0]["id"];
            },
            function (error) {
                console.log(error);
            });
        r_pickup.then(getRequestDetail);

    } else {

        Xrm.Utility.showProgressIndicator("Loading")

        try {

            var currentUserId = ShareSdk.getCurrentUserId()
            var user = await ShareSdk.getUserBySystemUserAsync(currentUserId)

            //create new request 

            const formId = ShareSdk.getCurrentFormId()
            const result = await ShareSdk.getPROXCardRequestAsync(formId)

            const body = PROXCardReqExtendSDK.getDataForm(result)

            const newReq = await PROXCardReqExtendSDK.createPROXCardReqFormAsync(user.clp_userid, body)

            console.log("Retrieved values: ", result);

            await copydetailAsync(formId, newReq.clp_proximitycardrequestid)

            await copySubstation(formId, newReq.clp_proximitycardrequestid)


            //
            Xrm.Navigation.navigateTo({
                pageType: 'entityrecord',
                entityName: 'clp_proximitycardrequest',
                entityId: newReq.clp_proximitycardrequestid,
                formId: window.parent.ShareSdk.Forms.Request_Main,
                data: {
                }
            })

        }
        catch (err) {
            ShareSdk.formError(err)
        }
        finally {
            Xrm.Utility.closeProgressIndicator()
        }

    }


}

PROXCardReqExtendSDK.createPROXCardReqFormAsync = (userid, body) => {

    const currentUserId = ShareSdk.getCurrentUserId()

    body["clp_name"] = 'New ISMS Proximity Card/Existing Proximity Card Encoding for Substation Access Requisition Form 進入中電輸電變電站 新電腦磁咭/現有磁咭編碼 申請表'

    body["clp_raw_requester@odata.bind"] = `/systemusers(${currentUserId})`
    body["clp_Requester@odata.bind"] = `/clp_users(${userid})`

    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

PROXCardReqExtendSDK.getDataForm = (result) => {

    var body = {}

    let date_keys = ["clp_accessperiodfrom", "clp_accessperiodto"];
    let abandon_keys =
        ["clp_name",
            "clp_workflow_status",
            "clp_mm",
            "clp_mm_dept_branch",
            "clp_reviewer",
            "clp_reviewed_date",
            "clp_chosen_mm",
            "clp_approver",
            "clp_appr_date",
            "clp_security",
            "clp_sla_count",
            "clp_collection_reminder_date",
            "clp_expiry_3_wd",
            "clp_expiry_5_wd",
            "clp_expiry_10_wd",
            "clp_is_locked",
            "clp_requestdate",
            "clp_is_reminder_email_sended",
            "clp_rejected_date",
            "clp_approver_deptbranch",
            "clp_nosofss",
            "clp_admin",
            "clp_appr_remarks",
            "clp_admin_remarks",
            "clp_mm_remarks",
            "clp_security_remarks",
            "clp_remarks",
            "clp_extend_from_request",
            "clp_readunderstoodandagreed",
            "clp_is_migration_data",
            "clp_pc_request_detail_pc_request",
            "clp_proximitycardrequestid",
            "clp_card_allocation_date",
            "clp_Requester",
            "clp_raw_requester"
        ]

    let lookup_keys = ["_clp_lut_ctrcomp_value", "_clp_responsibleperson_value", "_clp_chosenapprover_value", "_clp_responsible_cp_value"];
    let keys = Object.keys(result)
        .filter(key => key.indexOf("@") === -1 && date_keys.indexOf(key) === -1 && lookup_keys.indexOf(key) === -1)
        .filter(key => abandon_keys.indexOf(key) === -1);

    for (let key of lookup_keys) {
        if (result[key]) {

            if (!key.includes('responsibleperson') && !key.includes('chosenapprover')) {

                if (key.includes('_clp_responsible_cp_value')) {
                    body[`${key.replace('_', '').replace('_value', '')}@odata.bind`] = `/clp_users(${result[key]})`
                } else {

                    body[`${key.replace('_', '').replace('_value', '')}@odata.bind`] = `/clp_ssms_lut_ctrcomps(${result[key]})`
                }
            } else {

                if (key.includes('responsibleperson')) {
                    body["clp_ResponsiblePerson@odata.bind"] = `/clp_users(${result[key]})`
                }
                else {
                    body["clp_ChosenApprover@odata.bind"] = `/clp_users(${result[key]})`
                }
            }
        }
    }

    for (let key of keys) {
        if (key.startsWith('clp_')) {
            body[key] = result[key]
        }
    }

    body[ShareSdk.PROXCardReqFields.accessperiodfrom] = new Date()
    body["clp_extend_from_request@odata.bind"] = `/clp_proximitycardrequests(${result.clp_proximitycardrequestid})`
    return body
}

PROXCardReqExtendSDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    if (ShareSdk.isSSMSViewer()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        return ShareSdk.isPROXCardRequestor() || ShareSdk.isSSMSUserRepresentative()
    }



    if (status === ShareSdk.PROXCardReqStatus.RequestCompleted) {
        return true
    }

    return false
}