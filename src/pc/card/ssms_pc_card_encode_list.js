var PROXCardEncodeListSDK = window.PROXCardEncodeListSDK || {};


PROXCardEncodeListSDK.onAction = async (ids) => {

    var cardid = ids[0]


    var pageInput = {
        entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
        pageType: "entityrecord",
        formId: window.parent.ShareSdk.Forms.Card_EncodeList,
        entityId: ids[0],
        data: {
        }
    }

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Encode List"
        }).then(function success() {
        }, function error() {

        })
}


PROXCardEncodeListSDK.enableButton = () => {

    return window.parent.ShareSdk.IsinInventory()
}