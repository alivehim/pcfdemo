var PROXCardReturnFormSDK = window.PROXCardReturnFormSDK || {};

PROXCardReturnFormSDK.onFormLoaded = () => {
    const inputData = Xrm.Utility.getPageContext().input.data

    if (inputData?.newCardStatus === ShareSdk.ProximityCardStatus.Returned || inputData.newCardStatus === ShareSdk.ProximityCardStatus.Damaged) {
        ShareSdk.setVisiableControl(ShareSdk.PROXCardInventoryFields.returndate)
        ShareSdk.setRequiredControl(ShareSdk.PROXCardInventoryFields.returndate)
        if(!Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.returndate).getValue())
        {
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.returndate).setValue(new Date())
        }
    }
    else if (inputData?.newCardStatus === ShareSdk.ProximityCardStatus.Lost) {
        ShareSdk.setVisiableControl(ShareSdk.PROXCardInventoryFields.lostcardreportdate)
        ShareSdk.setRequiredControl(ShareSdk.PROXCardInventoryFields.lostcardreportdate)

        ShareSdk.setVisiableControl(ShareSdk.PROXCardInventoryFields.chequenoissuingbank)
        ShareSdk.setVisiableControl(ShareSdk.PROXCardInventoryFields.receiptno)
        ShareSdk.setVisiableControl(ShareSdk.PROXCardInventoryFields.lostremarks)
        ShareSdk.setVisiableControl(ShareSdk.PROXCardInventoryFields.sendthereceiptbacktothecontractororvisitor)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.lostcardreportdate).setValue(new Date())
    }

}