var PROXCardCollectFormSDK = window.PROXCardCollectFormSDK || {};


PROXCardCollectFormSDK.onFormLoaded = () => {
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.collectiondate).setValue(new Date())
}

PROXCardCollectFormSDK.onCancelCollectionChanged = () => {

    var cancelcollection = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.cancelcollection).getValue()

    if (cancelcollection) {
        ShareSdk.setRequiredControl(ShareSdk.PROXCardInventoryFields.collectionremarks)
    } else {

        ShareSdk.setRequiredControl(ShareSdk.PROXCardInventoryFields.collectionremarks, false)
    }
}