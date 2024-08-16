var PROXCardReqAllocatedCardsSDK = window.PROXCardReqAllocatedCardsSDK || {};

PROXCardReqAllocatedCardsSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        await PROXCardReqAllocatedCardsSDK.validateCardsAsync()

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
                title: "Allocate Cards"
            }).then(function success() {
                Xrm.Page.ui.refresh()
            }, function error() {

            })

        // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforCardEncoding)
        // await Xrm.Page.data.save()

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardReqAllocatedCardsSDK.validateCardsAsync = async () => {

    var requestId = ShareSdk.getCurrentFormId()
    var details = await ShareSdk.getPROXCardRequestDetailsAsync(requestId)

    var notRejectedDetails = details.filter(p => !p.clp_is_rejected)
    for (var item of notRejectedDetails) {
        if (!item.clp_card_no) {
            throw new Error('Please allocate all the cards')
        }
    }

    var cardsno = notRejectedDetails.map(p => p.clp_card_no).filter(i => i)

    if (Array.from(new Set(cardsno)).length != cardsno.length) {
        throw new Error('Duplicate Card No is not allowed')
    }

}


PROXCardReqAllocatedCardsSDK.enableButton = async () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }


    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()

    if (!locked && (status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation || status === ShareSdk.PROXCardReqStatus.RejectedbySecurity)) {

        var currentuserid = ShareSdk.getCurrentUserId()
        var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

        return await ShareSdk.isUserInTeamAsync(ownerid, currentuserid)
    }
    return false
}