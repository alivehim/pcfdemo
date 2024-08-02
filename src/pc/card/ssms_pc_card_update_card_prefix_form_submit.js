var PROXCardUpdateCardPrefixFormSubmitSDK = window.PROXCardUpdateCardPrefixFormSubmitSDK || {};


PROXCardUpdateCardPrefixFormSubmitSDK.onAction = async () => {
    try {
        const formid = ShareSdk.getCurrentFormId()
        await Xrm.Page.data.save()

        formContext.ui.close()
        //await PROXCardUpdateCardPrefixFormSubmitSDK.syncCardsAsync(formid)
        ShareSdk.showGlobalNotification('Card prefix are updated')

    }
    catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
    }

}

PROXCardUpdateCardPrefixFormSubmitSDK.syncCardsAsync = async (card_id) => {
    const card = await ShareSdk.getPROXCardByIdAsync(card_id)
    let cardJsonObj = {
        SSMS_PC_CARDS: {
            SSMS_PC_CARD: []
        }
    }

    cardJsonObj.SSMS_PC_CARDS.SSMS_PC_CARD.push(
        {
            "CARD_ID": card.clp_name,
            "CARD_NO": card.clp_name,
            "CARD_TYPE": ShareSdk.ProximityCardCardTypeMapping[card.clp_card_type],
            "ISSUE_DEPT": ShareSdk.getLegacyIssueDepartment(card.clp_issuedepartment),
            "CARD_STATUS": ShareSdk.ProximityCardStatusMapping[card.clp_card_status],
            "EXPIRY_DATE": card.clp_expirydate,
            "CARD_EXPIRY_DATE": card.clp_card_expiry_date,
            "REPLACE_CARD_ID": card.clp_replace_card?.clp_name || '',
            "CARD_PREFIX": card.clp_cardprefix || '',
        }
    )

    const cardsXML = ShareSdk.JSONtoXML(cardJsonObj)
    console.log(cardsXML)

    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateCard, null, card_id, cardsXML)
}

PROXCardUpdateCardPrefixFormSubmitSDK.enableButton = () => {

    return window.parent.ShareSdk.isProximityCardSecurity() && ShareSdk.IsProxCardUpdateCardPrefixForm()
}