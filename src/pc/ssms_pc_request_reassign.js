var PROXCardReqReassignSDK = window.PROXCardReqReassignSDK || {};

PROXCardReqReassignSDK.onAction = () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        var formid = ShareSdk.getCurrentFormId()
        var pageInput = {
            entityName: ShareSdk.Tables.proximitycardrequest,
            pageType: "entityrecord",
            formId: ShareSdk.Forms.Request_Reassign,
            entityId: formid,
        }

        Xrm.Navigation.navigateTo(pageInput,
            {
                target: 2,
                position: 1,
                height: { value: 80, unit: "%" },
                width: { value: 70, unit: "%" },
                title: "Reassign"
            }).then(function success() {
                Xrm.Page.ui.refresh()
            }, function error() {

            })


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

PROXCardReqReassignSDK.enableButton = async () => {
    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()
    if (!locked && status === ShareSdk.PROXCardReqStatus.WaitingforApproval) {
        return ShareSdk.isPROXCardRequestor()
    }

    if (!locked && status === ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval) {

        var currentuserid = ShareSdk.getCurrentUserId()
        var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

        return await ShareSdk.isUserInTeamAsync(ownerid, currentuserid)
    }

    return false
}