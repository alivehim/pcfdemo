var PROXCardUpdateFormSDK = window.PROXCardUpdateFormSDK || {};


PROXCardUpdateFormSDK.onFormLoaded = () => {
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Card_Update) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Card_Update);
        formItem.navigate();
    }
    PROXCardUpdateFormSDK.unlockFields()
    PROXCardUpdateFormSDK.initCardStatus()
}

PROXCardUpdateFormSDK.unlockFields = () => {

    var cardtype = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_type).getValue()
    var cardstatus = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).getValue()
    let unlockFields = []
    unlockFields.push(ShareSdk.PROXCardInventoryFields.name)
    if (cardstatus == ShareSdk.ProximityCardStatus.Available) {
        unlockFields.push(ShareSdk.PROXCardInventoryFields.card_status)
    }

    // if (cardstatus != ShareSdk.ProximityCardStatus.Lost) {
    //     unlockFields.push(ShareSdk.PROXCardInventoryFields.name)
    //     if (cardtype === ShareSdk.ProximityCardCardType.ISMSProximityCard) {
    //         unlockFields.push(ShareSdk.PROXCardInventoryFields.card_status)

    //     }
    // } else {
    //     unlockFields.push(ShareSdk.PROXCardInventoryFields.chequenoissuingbank)
    //     unlockFields.push(ShareSdk.PROXCardInventoryFields.receiptno)
    //     unlockFields.push(ShareSdk.PROXCardInventoryFields.remarks)
    //     unlockFields.push(ShareSdk.PROXCardInventoryFields.sendthereceiptbacktothecontractororvisitor)
    // }
    if (cardstatus == ShareSdk.ProximityCardStatus.Lost) {
        unlockFields.push(ShareSdk.PROXCardInventoryFields.chequenoissuingbank)
        unlockFields.push(ShareSdk.PROXCardInventoryFields.receiptno)
        unlockFields.push(ShareSdk.PROXCardInventoryFields.lostremarks)
        unlockFields.push(ShareSdk.PROXCardInventoryFields.sendthereceiptbacktothecontractororvisitor)
    }

    for (let attr of unlockFields) {
        ShareSdk.setDisableControl(attr, false)
    }
}

PROXCardUpdateFormSDK.initCardStatus = () => {
    var cardstatus = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).getValue()
    if (cardstatus === ShareSdk.ProximityCardStatus.Available) {
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardInventoryFields.card_status).clearOptions();
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardInventoryFields.card_status).addOption({ text: 'Available', value: 768230000 });
        Xrm.Page.ui.controls.get(ShareSdk.PROXCardInventoryFields.card_status).addOption({ text: 'Scrapped', value: 768230005 });

    }

}


PROXCardUpdateFormSDK.onCardNoChanged = ()=>{
    var cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue()
    var card_type = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_type).getValue()
    formContext = Xrm.Page.ui.formContext
    formContext.getControl(ShareSdk.PROXCardInventoryFields.name).clearNotification(ShareSdk.PROXCardInventoryErrors.ContractorConsultantCardNoFormat.id);
    formContext.getControl(ShareSdk.PROXCardInventoryFields.name).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id);
    if (cardno) {

        if (card_type === ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            const cardNoReg = /^[A-Za-z0-9]{2}[0-9]{5}$/;

            if (!cardNoReg.test(cardno)) {
                formContext.getControl(ShareSdk.PROXCardInventoryFields.name).addNotification({
                    notificationLevel: 'ERROR',
                    messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.message],
                    uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id
                });
                return
            }

        }
        else if (card_type === ShareSdk.ProximityCardCardType.ContractorConsultantPass) {
            const cardNoReg = /^C[0-9]{4}-[0-9]{5}$/;
            const cardNoReg1 = /^C[0-9]{5}-[0-9]{5}$/;
            const cardNoReg2 = /^[0-9]{2}C[0-9]{4}-[0-9]{5}$/

            if (!cardNoReg.test(cardno) && !cardNoReg1.test(cardno) && !cardNoReg2.test(cardno)) {
                formContext.getControl(ShareSdk.PROXCardInventoryFields.name).addNotification({
                    notificationLevel: 'ERROR',
                    messages: [ShareSdk.PROXCardInventoryErrors.ContractorConsultantCardNoFormat.message],
                    uniqueId: ShareSdk.PROXCardInventoryErrors.ContractorConsultantCardNoFormat.id
                });
                return
            }
        }
    }
}