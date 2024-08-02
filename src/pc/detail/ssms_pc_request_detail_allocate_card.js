var PCREQDTLAllocateCardSDK = window.PCREQDTLAllocateCardSDK || {};

PCREQDTLAllocateCardSDK.onAction = (ids) => {
    var pageInput = {
        entityName: ShareSdk.Tables.proximity_card_request_detail,
        pageType: "entityrecord",
        formId: ShareSdk.Forms.Detail_Allocated,
        entityId: ids[0],
        data: {
            status: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue(),
            action: ShareSdk.ProximityCardAction.AllocateCard,
            individualStatus: ShareSdk.PROXCardIndividualStatus.WaitingforCardEncoding
        }
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Allocate Card"
        }).then(function success() {
            PCREQDTLRejectSDK.refreshGrid()
        }, function error() {

        })
}


PCREQDTLAllocateCardSDK.enableButton = async () => {

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation || status === ShareSdk.PROXCardReqStatus.RejectedbySecurity) {
        var currentuserid = ShareSdk.getCurrentUserId()
        var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

        return await ShareSdk.isUserInTeamAsync(ownerid, currentuserid)

    }
    return false
}