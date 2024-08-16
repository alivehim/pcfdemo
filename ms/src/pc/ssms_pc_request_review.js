var PROXCardReqReviewSDK = window.PROXCardReqReviewSDK || {};

PROXCardReqReviewSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        await Xrm.Page.data.save()
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
                title: "Review"
            }).then(function success() {
                Xrm.Page.ui.refresh()
            }, function error() {

            })
        // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval)
        // await Xrm.Page.data.save()

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardReqReviewSDK.enableButton = async () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()

    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()
    if (!locked && status === ShareSdk.PROXCardReqStatus.WaitingforReview) {
        var currentuserid = ShareSdk.getCurrentUserId()
        var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

        return await ShareSdk.isUserInTeamAsync(ownerid, currentuserid)
    }
    return false
}