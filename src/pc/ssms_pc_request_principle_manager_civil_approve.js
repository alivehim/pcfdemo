var PROXCardReqPMCApproveSDK = window.PROXCardReqPMCApproveSDK || {};

PROXCardReqPMCApproveSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        var formid = ShareSdk.getCurrentFormId()
        var pageInput = {
            entityName: ShareSdk.Tables.proximitycardrequest,
            pageType: "entityrecord",
            formId: ShareSdk.Forms.Request_Approval,
            entityId: formid,
            data: {
                action: ShareSdk.ProximityCardAction.Approve
            }
        }

        Xrm.Navigation.navigateTo(pageInput,
            {
                target: 2,
                position: 1,
                height: { value: 80, unit: "%" },
                width: { value: 70, unit: "%" },
                title: "Principle Manager - Civil Approve"
            }).then(function success() {
                Xrm.Page.ui.refresh()
            }, function error() {

            })

        // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforCardAllocation)
        // await Xrm.Page.data.save()

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}


PROXCardReqPMCApproveSDK.enableButton = async () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()
    if (!locked && status === ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval) {
        const ownerid = ShareSdk.getOwnerId()
        const currentUserId = ShareSdk.getCurrentUserId()
        return ownerid === currentUserId || await ShareSdk.isDelegateeforPrincipleManagerAsync()
    }
    return false
}