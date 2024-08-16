
window.EngagementApproveSDK = window.EngagementApproveSDK || {}

EngagementApproveSDK.onApprove = () => {

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requestType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()

    if (EAAShareSdk.IsAdmin() && (status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval || status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval)) {
        EAAShareSdk.onBehalfOfApproval(status, requestType, EAAShareSdk.ApprovalStatus.Approved)
    } else {

        var nextRole = EngagementApproveSDK.getNextRoleName(requestType, status);
        if (nextRole === EAAShareSdk.RolesName.Requester) {
            confirmationMsg = `Confirm to approve?`
        } else {
            confirmationMsg = `Confirm to submit to the next approver?`
        }

        var confirmStrings = { confirmButtonLabel: "Confirm", text: confirmationMsg, title: "Confirmation" };
        var confirmOptions = { height: 200, width: 450 };
        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {
                    EngagementApproveSDK.approvalProcess(status, requestType)
                }
            }
        )
    }
}


EngagementApproveSDK.approvalProcess = async (status, requestType) => {
    //change the status
    var requestGuid = EAAShareSdk.getFormId()
    var currentUserId = EAAShareSdk.getCurrentUserId()
    var currentUserName = EAAShareSdk.getCurrentUserName()
    var nextStage = EngagementApproveSDK.getApprovalStage(status)
    Xrm.Utility.showProgressIndicator("Loading")

    //set aia_sys_processlock with "true"
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_sys_processlock).setValue(true)
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())

    //navigate to "Awaiting My Response"
    if (EAAShareSdk.IsFormAtFinalStage(requestType, status)) {
        var title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()

        if (title.length <= 15) {
            title = title + '-' + EAAShareSdk.randomCode()
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).setValue(title)
        }
        EngagementApproveSDK.setApprover(status, currentUserId, currentUserName)
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.final_date).setValue(new Date())
        EngagementApproveSDK.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.final_approver)
    }
    else {
        EngagementApproveSDK.setApprover(status, currentUserId, currentUserName)
    }

    try {
        await Xrm.Page.data.save()
        await EAAShareSdk.Approve(requestGuid, currentUserId, nextStage)
        var title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()
        await EAAShareSdk.popupSuccess(title, EAAShareSdk.MessageType.Approve)
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}



EngagementApproveSDK.setApprover = (status, currentUserId, currentUserName) => {
    switch (status) {
        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            EngagementApproveSDK.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.local_approver)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.approver_date).setValue(new Date())

            break;
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            EngagementApproveSDK.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.finance_manager)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.finance_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            EngagementApproveSDK.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.head)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.head_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            EngagementApproveSDK.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.cfo)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.cfo_date).setValue(new Date())
            break;
    }
}

EngagementApproveSDK.fillApprover = (currentUserId, currentUserName, fieldName) => {
    EAAShareSdk.setLookupValue(currentUserId, currentUserName, EAAShareSdk.Tables.systemuser, fieldName)
}

EngagementApproveSDK.getApprovalStage = (status) => {
    switch (status) {
        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            return EAAShareSdk.WorkflowStages.LocalApproverApprove
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            return EAAShareSdk.WorkflowStages.GroupFinanceCoordinatorApprove;
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxApprove
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            return EAAShareSdk.WorkflowStages.RegionalCFOApprove
    }
}

EngagementApproveSDK.getNextRoleName = (formRequestType, status) => {
    switch (status) {
        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            return EAAShareSdk.RolesName.GroupFinanceCoordinator
            break;
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            if (formRequestType === EAAShareSdk.RequestType.PwcNon_Tax) {
                return EAAShareSdk.RolesName.RegionalCFO
            }
            else {
                return EAAShareSdk.RolesName.HeadofGroupTax
            }
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            if (formRequestType === EAAShareSdk.RequestType.Non_PwcTax) {
                return EAAShareSdk.RolesName.Requester
            } else {
                return EAAShareSdk.RolesName.RegionalCFO
            }
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            return EAAShareSdk.RolesName.Requester
    }
}


/**
 * if current user is administrator(final approver)
 * if current user is approver
 * @returns 
 */
EngagementApproveSDK.enableApproveButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    if (status && (status === EAAShareSdk.FormStatus.PendingLocalApproverApproval ||
        status === EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval ||
        status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval ||
        status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval)) {

        var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

        var currentuserid = EAAShareSdk.getCurrentUserId();

        if (status === EAAShareSdk.FormStatus.PendingLocalApproverApproval) {
            //check the localapprovel

            return ownerid === currentuserid
        }
        else {

            if (EAAShareSdk.isUserInTeam(ownerid, currentuserid)) {
                return true
            }

            //if current user is administrator, on-behalf of
            if (EAAShareSdk.IsAdmin()) {
                return status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval || status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval
            }

            return false
        }

    } else {
        return false;
    }

}

