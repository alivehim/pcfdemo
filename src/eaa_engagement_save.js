window.EngagementSaveSDK = window.EngagementSaveSDK || {}

/**
 * triggered when clicking "Save" button
 */
EngagementSaveSDK.onSave = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    //if title is empty
    const sys_active = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_active).getValue()

    if (!sys_active) {
        Xrm.Utility.showProgressIndicator("Loading")
        const title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()

        EAAShareSdk.generateReferenceIfNotExists(title).then(newtitle => {
            EngagementSaveSDK.saveFields(newtitle).then(res => {

                //set "Delete Buttion" visiable?
                //set "Cancel Buttion" invisible?
                Xrm.Page.ui.refreshRibbon()
            }).then(r => Xrm.Utility.closeProgressIndicator()).catch(err => {
                Xrm.Utility.closeProgressIndicator()
            });

        }).catch(err => {
            EAAShareSdk.formError(err)
            Xrm.Utility.closeProgressIndicator()
        })
    } else {

        EngagementSaveSDK.saveWithValidation()
    }


}

EngagementSaveSDK.saveWithValidation = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        let needRevokeAccess = false
        let needRevokeCountries = false
        let oldLocalApproverId = null
        let oldCountries = null
        let formId = EAAShareSdk.getFormId()
        var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.local_approver).getIsDirty()
            && EAAShareSdk.IsAdmin()
            && status != EAAShareSdk.FormStatus.Unpublished && status != EAAShareSdk.FormStatus.Initiator_Pending
            && status != EAAShareSdk.FormStatus.Initiator_ResubmissionPending) {
            needRevokeAccess = true
            let formData = await EAAShareSdk.getFormData(formId)
            oldLocalApproverId = formData["_aia_eaa_local_approver_value"]
        }

        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.countries).getIsDirty()
            && EAAShareSdk.IsAdmin()
            && status != EAAShareSdk.FormStatus.Unpublished
            && status != EAAShareSdk.FormStatus.Initiator_Pending
            && status != EAAShareSdk.FormStatus.Initiator_ResubmissionPending) {
            needRevokeCountries = true
            let formData = await EAAShareSdk.getFormData(formId)
            oldCountries = formData["_aia_eaa_countries_value"]

        }

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())
        await Xrm.Page.data.save()

        if (needRevokeAccess) {
            let newLocalApproverId = EAAShareSdk.getLocalApproverId()


            if (status === EAAShareSdk.FormStatus.PendingLocalApproverApproval) {

                Xrm.Page.data.entity.attributes.getByName("ownerid").setValue(Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.local_approver).getValue())
                //EAAShareSdk.setLookupValue(newLocalApproverId, EAAShareSdk.getLocalApproverName(), EAAShareSdk.Tables.systemuser, "ownerid")
                await Xrm.Page.data.save()
                await EAAShareSdk.revoke(formId, oldLocalApproverId)
            }
            else {
                await EAAShareSdk.revoke(formId, oldLocalApproverId)
                await EAAShareSdk.share(formId, newLocalApproverId)
            }

        }

        if (needRevokeCountries) {
            let country = EAAShareSdk.getCountries()

            let teamid = await EAAShareSdk.getTeamIdByCountry(country.id)
            if (teamid) {
                await EAAShareSdk.shareToTeam(formId, teamid)
            }

            let oldteamid = await EAAShareSdk.getTeamIdByCountry(oldCountries)
            if (oldteamid) {
                await EAAShareSdk.revokeTeam(formId, oldteamid)
            }
        }
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

EngagementSaveSDK.saveFields = (newtitle) => {
    //Turn "aia_sys_active" to yes
    // data[EAAShareSdk.FormFields.sys_active] = true;
    //active the engagement

    const sys_active = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_active).getValue()

    //change workflow status and workflow stage
    // Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.workflow_stage).setValue(EAAShareSdk.WorkflowStages.New)
    // Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).setValue(EAAShareSdk.FormStatus.Initiator_Pending)
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())
    var data = EngagementSaveSDK.buildRequestData()

    if (!sys_active) {

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_active).setValue(true)
        data.aia_sys_active = true
    }

    if (newtitle) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).setValue(newtitle)
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.reference_createdon).setValue(new Date())
        data.aia_name = newtitle
        data.aia_reference_createdon = new Date()
    }

    var RequestGUID = EAAShareSdk.getFormId();

    return fetch(
        `/api/data/v9.0/aia_eaa_forms(${RequestGUID})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify([data])
        }
    ).then(res => res.json())
        .then(res => {
            if (res.error) {
                return Promise.reject(res.error.message)
            }

            // var referenceno = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()

            if (newtitle) {
                //update the DOM
                window.parent.document.querySelector('h1[id*=formHeaderTitle]').innerText = newtitle

                notification =
                {
                    type: 2,
                    level: 1,
                    message: `EAA Workflow ${newtitle} has been created.`,
                    showCloseButton: true
                };
            } else {

                notification =
                {
                    type: 2,
                    level: 1,
                    message: `Save succeed`,
                    showCloseButton: true
                };
            }


            Xrm.App.addGlobalNotification(notification).then(
                function success(result) {
                    // Wait for 3 seconds and then clear the notification
                    window.setTimeout(function () {
                        Xrm.App.clearGlobalNotification(result);
                    }, 3000);
                },
                function (error) {
                    console.log(error.message);
                }
            );

            return Promise.resolve()

        }).catch(err => {
            // const msgKey = new Date().getTime().toString(16)
            // Xrm.Page.ui.formContext.ui.setFormNotification(
            //     err,
            //     'ERROR',
            //     msgKey);

            // window.setTimeout(function () {
            //     Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
            // }, 10000);

            EAAShareSdk.formError(err)
        })
}





EngagementSaveSDK.buildRequestData = () => {

    var fieldMapping = {
        aia_eaa_pocket: "aia_eaa_pocket",
        aia_eaa_est_start_date: "aia_eaa_est_start_date",
        aia_eaa_approver_date: "aia_eaa_approver_date",
        aia_eaa_indrect_taxes_base: "aia_eaa_indrect_taxes_base",
        aia_eaa_head_date: "aia_eaa_head_date",
        aia_eaa_other_remarks: "aia_eaa_other_remarks",
        aia_eaa_description_long: "aia_eaa_description_long",
        aia_eaa_attachments: "aia_eaa_attachments",
        aia_eaa_sub_cate_tax: "aia_eaa_sub_cate_tax",
        aia_eaa_requester_date: "aia_eaa_requester_date",
        aia_eaa_entity_sign: "aia_eaa_entity_sign",
        aia_eaa_defined_policy: "aia_eaa_defined_policy",
        aia_eaa_requester: "aia_eaa_requester",
        aia_eaa_pocket_base: "aia_eaa_pocket_base",
        aia_eaa_countries: "aia_eaa_countries",
        aia_eaa_cost_arrangement: "aia_eaa_cost_arrangement",
        aia_eaa_final_date: "aia_eaa_final_date",
        aia_eaa_entity_fee: "aia_eaa_entity_fee",
        aia_eaa_fee_included_audit_fee_template: "aia_eaa_fee_included_audit_fee_template",
        aia_eaa_prof_firm: "aia_eaa_prof_firm",
        aia_eaa_indrect_taxes: "aia_eaa_indrect_taxes",
        aia_eaa_est_cost: "aia_eaa_est_cost",
        aia_eaa_est_cost_base: "aia_eaa_est_cost_base",
        aia_eaa_service_number: "aia_eaa_service_number",
        aia_eaa_revised_date: "aia_eaa_revised_date",
        aia_eaa_sla_reminder: "aia_eaa_sla_reminder",
        aia_eaa_fee_basis: "aia_eaa_fee_basis",
        aia_eaa_form_status: "aia_eaa_form_status",
        aia_eaa_local_approver: "aia_eaa_local_approver",
        aia_eaa_category: "aia_eaa_category",
        aia_eaa_completed: "aia_eaa_completed",
        aia_sys_wf_request_guid: "aia_sys_wf_request_guid",
        aia_eaa_est_duration: "aia_eaa_est_duration",
        aia_eaa_state_not_included_audit_fee_template: "aia_eaa_state_not_included_audit_fee_template",
        aia_sys_processlock: "aia_sys_processlock",
        aia_eaa_approval_status: "aia_eaa_approval_status",
        aia_eaa_approval_evidence_head: "aia_eaa_approval_evidence_head",
        aia_eaa_policy_why: "aia_eaa_policy_why",
        aia_eaa_name_of_pro: "aia_eaa_name_of_pro",
        aia_eaa_fully_budgeted: "aia_eaa_fully_budgeted",
        aia_eaa_head: "aia_eaa_head",
        aia_eaa_approval_evidence_cfo: "aia_eaa_approval_evidence_cfo",
        aia_eaa_act_taxes: "aia_eaa_act_taxes",
        aia_eaa_act_fee: "aia_eaa_act_fee",
        aia_eaa_tax_date: "aia_eaa_tax_date",
        aia_eaa_base_fee: "aia_eaa_base_fee",
        aia_eaa_define_pwc: "aia_eaa_define_pwc",
        aia_eaa_tax_manager: "aia_eaa_tax_manager",
        aia_eaa_recurring: "aia_eaa_recurring",
        aia_eaa_policy_read: "aia_eaa_policy_read",
        aia_eaa_completion_date: "aia_eaa_completion_date",
        aia_eaa_quarter: "aia_eaa_quarter",
        aia_eaa_cfo_date: "aia_eaa_cfo_date",
        aia_eaa_unpublish_user: "aia_eaa_unpublish_user",
        aia_eaa_description_short: "aia_eaa_description_short",
        aia_eaa_rationale_of_engaging_pwc: "aia_eaa_rationale_of_engaging_pwc",
        aia_eaa_act_pocket: "aia_eaa_act_pocket",
        aia_eaa_act_fee_base: "aia_eaa_act_fee_base",
        aia_eaa_cfo: "aia_eaa_cfo",
        aia_eaa_department: "aia_eaa_department",
        aia_eaa_base_fee_base: "aia_eaa_base_fee_base",
        aia_eaa_on_behalf_rcfo: "aia_eaa_on_behalf_rcfo",
        aia_eaa_final_approver: "aia_eaa_final_approver",
        aia_eaa_est_end_date: "aia_eaa_est_end_date",
        aia_eaa_year: "aia_eaa_year",
        aia_eaa_act_pocket_base: "aia_eaa_act_pocket_base",
        aia_eaa_sub_cate_nontax: "aia_eaa_sub_cate_nontax",
        aia_eaa_finance_date: "aia_eaa_finance_date",
        aia_eaa_act_taxes_base: "aia_eaa_act_taxes_base",
        aia_eaa_finance_manager: "aia_eaa_finance_manager",
        aia_eaa_unpublish_date: "aia_eaa_unpublish_date",
        transactioncurrencyid: "transactioncurrencyid",
        aia_eaa_act_currency: "aia_eaa_act_currency",

        aia_eaa_state_why_permissible: "aia_eaa_state_why_permissible",
        aia_reason_for_engaging_pwc: "aia_reason_for_engaging_pwc",
        aia_eaa_cost_arrangement_detail: "aia_eaa_cost_arrangement_detail",
        aia_eaa_fee_other_remarks: "aia_eaa_fee_other_remarks",
        aia_eaa_why_not_included_audit_fee_template: "aia_eaa_why_not_included_audit_fee_template",
        aia_eaa_reason_for_pro_firm: "aia_eaa_reason_for_pro_firm",

        aia_eaa_usd_rate: "aia_eaa_usd_rate",
        aia_eaa_usd_est_base_fee: "aia_eaa_usd_est_base_fee",
        aia_eaa_usd_est_indrect_taxes: "aia_eaa_usd_est_indrect_taxes",
        aia_eaa_usd_est_pocket: "aia_eaa_usd_est_pocket",
        aia_eaa_usd_est_cost: "aia_eaa_usd_est_cost",

        aia_eaa_report_year: "aia_eaa_report_year",
        aia_eaa_quarter: "aia_eaa_quarter",

        aia_eaa_est_usd_rate: "aia_eaa_est_usd_rate",

        aia_reference_createdon: "aia_reference_createdon",
        aia_eaa_latest_modified_on: "aia_eaa_latest_modified_on"
    }

    data = {}
    collection = Xrm.Page.data.entity.attributes._collection
    for (const key in collection) {
        const attrDescriptor = collection[key].getAttrDescriptor()
        if (!attrDescriptor) continue;
        if (attrDescriptor) {
            var fieldKey = fieldMapping[key]

            if (fieldKey) {
                //console.log(fieldMapping[key])
                if (attrDescriptor.Type === 'lookup') {
                    const entityType = attrDescriptor.Targets[0]
                    //console.log(fieldKey)
                    if (collection[key].getValue()) {
                        var guid = collection[key].getValue()[0].id.replace("{", "").replace("}", "")
                        data[`${fieldKey}@odata.bind`] = `/${EngagementSaveSDK.tableNameConverter(entityType)}(${guid})`
                    }
                } else {
                    data[key] = collection[key].getValue()
                }
            }
        }
    }

    return data
}




EngagementSaveSDK.tableNameConverter = (logicName) => {

    if (logicName.endsWith("y")) {

        return logicName = logicName.substr(0, logicName.length - 1) + "ies"
    } else {
        return `${logicName}${logicName.endsWith("s") ? "e" : ''}s`
    }
}

EngagementSaveSDK.enableSaveButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }


    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
    var activeStatus = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.statecode).getValue()


    if (activeStatus != EAAShareSdk.ActiveStatus.Active) {
        return false
    }

    if ((status === EAAShareSdk.FormStatus.LocalApprover_Rejected
        || status === EAAShareSdk.FormStatus.GroupFinanceCoordinator_Rejected
        || status === EAAShareSdk.FormStatus.HeadOfGroupTax_Rejected
        || status === EAAShareSdk.FormStatus.RegionalCFO_Rejected) && !EAAShareSdk.IsAdmin()) {
        return false
    }

    if (status != EAAShareSdk.FormStatus.ActualFeeUpdate_Completed && EAAShareSdk.getCurrentUserId() === requesterid) {
        return true
    }

    if (EAAShareSdk.IsLocalViewer()) {

        if (status != EAAShareSdk.FormStatus.Initiator_Pending
            && status != EAAShareSdk.FormStatus.Initiator_ResubmissionPending
            && status != EAAShareSdk.FormStatus.ActualFeeUpdate_Completed) {
            return true
        }

        return false
    }

    if (status != EAAShareSdk.FormStatus.ActualFeeUpdate_Completed && EAAShareSdk.IsCurrentUserCanUpdateActualFee()) {
        return true
    }

    return EAAShareSdk.IsAdmin()
}