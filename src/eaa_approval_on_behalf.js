
window.EAAApprovalOnBehalfSDK = window.EAAApprovalOnBehalfSDK || {}


EAAApprovalOnBehalfSDK.onFormLoad = () => {
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false);
}

EAAApprovalOnBehalfSDK.onSubmit = async () => {
    Xrm.Utility.showProgressIndicator("Loading")

    try {

        formContext = Xrm.Page.ui.formContext
        const formId = EAAApprovalOnBehalfSDK.getFormId()

        await EAAApprovalOnBehalfSDK.validation()

        const form = await EAAShareSdk.getFormData(formId)
        const formStatus = form.aia_eaa_form_status
        const formType = form.aia_sys_form_type
        const teamid = form._ownerid_value
        const currentUserId = EAAShareSdk.getCurrentUserId()
        const currentUserName = EAAShareSdk.getCurrentUserName()
        let title = EAAApprovalOnBehalfSDK.getFormName()


        const action = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.my_action).getValue()

        //autofill approval data
        const nextStage = EAAApprovalOnBehalfSDK.getNextWorkflowStage(action, formStatus)
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.action_on).setValue(new Date())
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.workflow_stage).setValue(nextStage)
        EAAShareSdk.setLookupValue(currentUserId, currentUserName, EAAShareSdk.Tables.systemuser, EAAShareSdk.ApprovalFields.approver)
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.on_behalf).setValue(true)

        //generate new title 
        if (action == EAAShareSdk.ApprovalStatus.Approved && EAAShareSdk.IsFormAtFinalStage(formType, formStatus)) {
            title = title + '-' + EAAShareSdk.randomCode()
        }

        //change form data- final approver  
        await EAAShareSdk.onBehalfOfApprovalFormProcess(action, formId, formType, formStatus, title, currentUserId)
        //share this request to specific team
        await EAAShareSdk.shareToTeam(formId, teamid)
        await Xrm.Page.data.save()

        formContext.ui.close()
        EAAShareSdk.popupSuccess(title, EAAApprovalOnBehalfSDK.getMessageTypeByAction(action))

    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

EAAApprovalOnBehalfSDK.getNextWorkflowStage = (action, formStatus) => {

    if (action === EAAShareSdk.ApprovalStatus.Approved) {
        if (formStatus === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval) {
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxApprove
        }
        else {
            return EAAShareSdk.WorkflowStages.RegionalCFOApprove
        }
    }
    else if (action === EAAShareSdk.ApprovalStatus.Rejected) {
        if (formStatus === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval) {
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxReject
        }
        else {
            return EAAShareSdk.WorkflowStages.RegionalCFOReject
        }
    }
    else if (action === EAAShareSdk.ApprovalStatus.ResubmissionPending) {
        if (formStatus === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval) {
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxReopen
        }
        else {
            return EAAShareSdk.WorkflowStages.RegionalCFOReopen
        }
    }
}

EAAApprovalOnBehalfSDK.getMessageTypeByAction = (action) => {
    if (action === EAAShareSdk.ApprovalStatus.Approved) {
        return EAAShareSdk.MessageType.Approve
    }
    else if (action === EAAShareSdk.ApprovalStatus.Rejected) {

        return EAAShareSdk.MessageType.Reject
    }
    else if (action === EAAShareSdk.ApprovalStatus.ResubmissionPending) {

        return EAAShareSdk.MessageType.Reopen
    }
}

EAAApprovalOnBehalfSDK.validation = () => {

    return EAAApprovalOnBehalfSDK.getAttachments().then(files => {
        if (files.length < 1) {
            throw new Error("Need upload “Approval Evidence” before submission")
        }
        return true
    })
}

EAAApprovalOnBehalfSDK.getAttachments = () => {

    const formId = EAAApprovalOnBehalfSDK.getFormId()
    const timelineid = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.timelineid).getValue()
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.form_attachment}s/?$filter=_aia_sys_eaa_form_value eq ${formId} and aia_attachment_type eq ${EAAShareSdk.AttachmentType.ApprovalEvidence} and aia_eaa_timelineid eq ${timelineid} `).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }

        return res.value
    }).catch(err => {
        throw new Error(err.message)
    })

}

EAAApprovalOnBehalfSDK.getFormId = () => {
    var replaceSymbol = /(\w*){(.*)}(.*)/g
    const form = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form).getValue()
    return form[0].id.replace(replaceSymbol, "$1$2");
}

EAAApprovalOnBehalfSDK.getFormName = () => {
    const form = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ApprovalFields.eaa_form).getValue()
    return form[0].name;
}

EAAApprovalOnBehalfSDK.enableSumbitButton = () => {
    return true
} 