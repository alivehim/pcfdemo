var PROXCardReqRejectSDK = window.PROXCardReqRejectSDK || {};

PROXCardReqRejectSDK.onAction = async (buttonText) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {


        var formid = ShareSdk.getCurrentFormId()
        var pageInput = {
            entityName: ShareSdk.Tables.proximitycardrequest,
            pageType: "entityrecord",
            formId: ShareSdk.Forms.Request_Approval,
            entityId: formid,
            data: {
                action: ShareSdk.ProximityCardAction.Reject
            }
        }

        Xrm.Navigation.navigateTo(pageInput,
            {
                target: 2,
                position: 1,
                height: { value: 80, unit: "%" },
                width: { value: 70, unit: "%" },
                title: buttonText === "Reject" ? "Reject" : "Reject to Admin"
            }).then(function success() {
                Xrm.Page.ui.refresh()
            }, function error() {

            })

        // var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
        // if (status === ShareSdk.PROXCardReqStatus.WaitingforApproval || status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation) {

        //     Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.RequestRejected)
        // }
        // else if (status === ShareSdk.PROXCardReqStatus.WaitingforCardEncoding) {
        //     Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.RejectedbySecurity)
        // }
        // await Xrm.Page.data.save()

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}
PROXCardReqRejectSDK.enableButton = async (buttonText) => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()
    if (!locked) {
        if (buttonText === "Reject") {
            if (status === ShareSdk.PROXCardReqStatus.WaitingforApproval) {
                const ownerid = ShareSdk.getOwnerId()
                const currentUserId = ShareSdk.getCurrentUserId()
                return ownerid === currentUserId || await ShareSdk.isDelegateeforApprovalAsync()
            }
            else if (status === ShareSdk.PROXCardReqStatus.WaitingforReview) {
                return ShareSdk.isProximityCardAdmin()
            }
            else if (status === ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval) {
                const ownerid = ShareSdk.getOwnerId()
                const currentUserId = ShareSdk.getCurrentUserId()
                return ownerid === currentUserId || await ShareSdk.isDelegateeforPrincipleManagerAsync()
            }
            else if (status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation || status === ShareSdk.PROXCardReqStatus.RejectedbySecurity) {
                var currentuserid = ShareSdk.getCurrentUserId()
                var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
                var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

                return await ShareSdk.isUserInTeamAsync(ownerid, currentuserid)
            }

        } else if (buttonText === "RejecttoAdmin") {
            if (status === ShareSdk.PROXCardReqStatus.WaitingforCardEncoding) {
                return ShareSdk.isProximityCardSecurity()
            }
        }

    }

    return false
}