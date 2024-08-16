
window.EngagementRejectSDK = window.EngagementRejectSDK || {}


EngagementRejectSDK.onReject = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requestType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()

    if (EAAShareSdk.IsAdmin() && (status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval || status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval)) {
        EAAShareSdk.onBehalfOfApproval(status, requestType, EAAShareSdk.ApprovalStatus.Rejected)
    } else {
        var confirmationMsg = `Confirm to reject?`
        var confirmStrings = { confirmButtonLabel: "Confirm", text: confirmationMsg, title: "Confirmation" };
        var confirmOptions = { height: 200, width: 450 };
        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {
                    EngagementRejectSDK.rejectionProcess(status, requestType)
                }
            }
        )
    }
}



EngagementRejectSDK.rejectionProcess = async (status, requestType) => {
    //change the status
    var requestGuid = EAAShareSdk.getFormId()
    var currentUserId = EAAShareSdk.getCurrentUserId()
    var currentUserName = EAAShareSdk.getCurrentUserName()
    var previousStage = EngagementRejectSDK.getPreviousStage(status)
    Xrm.Utility.showProgressIndicator("Loading")

    //set aia_sys_processlock with "true"
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_sys_processlock).setValue(true)
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())
    EngagementRejectSDK.setApprover(status, currentUserId, currentUserName)

    try {
        await Xrm.Page.data.save()
        await EAAShareSdk.Reject(requestGuid, currentUserId, previousStage)
        var title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()
        await EAAShareSdk.popupSuccess(title, EAAShareSdk.MessageType.Reject)
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

EngagementRejectSDK.setApprover = (status, currentUserId, currentUserName) => {
    switch (status) {
        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.local_approver)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.approver_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.finance_manager)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.finance_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.head)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.head_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.cfo)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.cfo_date).setValue(new Date())
            break;
    }
}

EngagementRejectSDK.getPreviousStage = (status) => {
    switch (status) {

        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            return EAAShareSdk.WorkflowStages.LocalApproverReject
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            return EAAShareSdk.WorkflowStages.GroupFinanceCoordinatorReject
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxReject
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            return EAAShareSdk.WorkflowStages.RegionalCFOReject
    }
}
EngagementRejectSDK.getPreviousRoleName = (formRequestType, status) => {
    switch (status) {

        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            return EAAShareSdk.RolesName.Requester
            break;
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:

            return EAAShareSdk.RolesName.LocalApprover

        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            if (formRequestType === EAAShareSdk.RequestType.Non_PwcTax) {
                return EAAShareSdk.RolesName.Requester
            }
            else {
                return EAAShareSdk.RolesName.LocalApprover
            }
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            if (formRequestType === EAAShareSdk.RequestType.PwCTax) {
                return EAAShareSdk.RolesName.HeadofGroupTax
            }
            else {
                return EAAShareSdk.RolesName.LocalApprover
            }
    }
}

EngagementRejectSDK.enableRejectButton = () => {

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

            
            if(EAAShareSdk.isUserInTeam(ownerid, currentuserid)){
                return true
            }

            if (EAAShareSdk.IsAdmin()) {
                return status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval || status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval
            }

            return false
        }

    } else {
        return false;
    }
}