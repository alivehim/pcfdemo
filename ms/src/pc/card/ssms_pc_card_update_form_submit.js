var PROXCardUpdateFormSubmitSDK = window.PROXCardUpdateFormSubmitSDK || {};

PROXCardUpdateFormSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        if (Xrm.Page.data.isValid()) {

            await ShareSdk.fillLatestModifiedByAsync()
            
            const formid = ShareSdk.getCurrentFormId()
            var cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue()

            const cards = await ShareSdk.getPROXCardByCardNoAsync(cardno)
            const currentCard = await ShareSdk.getPROXCardByIdAsync(formid)
            const previousCardNo = currentCard.clp_name
            const cardid = ShareSdk.getCurrentFormId()
            if (cards[0] && cards[0].clp_proximitycardinventoryid.toLowerCase() != cardid.toLowerCase()) {
                throw new Error('Duplicated Card No. is not allowed')
            }
            await Xrm.Page.data.save()

            if (previousCardNo != cardno) {
                var syncCards = []
                syncCards.push({
                    cardid: formid,
                    previousCardNo: previousCardNo, cardno: cardno,
                    cardtype: "I", cardstatus: "AVE",
                    expirydate: currentCard.clp_expirydate,
                    cardexpirydate: currentCard.clp_card_expiry_date,
                    replacecardno: currentCard["_clp_replace_card_value@OData.Community.Display.V1.FormattedValue"],
                    department: currentCard.clp_issuedepartment
                })

                //await PROXCardUpdateFormSubmitSDK.syncCardsAsync(syncCards)
            }

            await ShareSdk.openAlertDialog('Card is updated successfully').then(res => {
                // var pageInput = {
                //     pageType: "entitylist",
                //     entityName: ShareSdk.Tables.proximitycardinventory,
                // }

                // Xrm.Navigation.navigateTo(pageInput)
            })
            
            formContext.ui.close()
            // ShareSdk.showGlobalNotification('Card is updated successfully')
        }

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardUpdateFormSubmitSDK.syncCardsAsync = async (syncCards) => {
    let cardJsonObj = {
        SSMS_PC_CARDS: {
            SSMS_PC_CARD: []
        }
    }

    for (var item of syncCards) {
        cardJsonObj.SSMS_PC_CARDS.SSMS_PC_CARD.push({
            "CARD_ID": item.previousCardNo,
            "CARD_NO": item.cardno,
            "CARD_TYPE": item.cardtype,
            "ISSUE_DEPT": ShareSdk.getLegacyIssueDepartment(item.department),
            "CARD_STATUS": item.cardstatus,
            "EXPIRY_DATE": item.expirydate || '',
            "CARD_EXPIRY_DATE": item.cardexpirydate || '',
            "REPLACE_CARD_ID": item.replacecardno || '',
            "CARD_PREFIX": item.clp_cardprefix,
        })
    }
    const cardsXML = ShareSdk.JSONtoXML(cardJsonObj)
    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.AddCards, null, syncCards[0].cardid, cardsXML)
}

PROXCardUpdateFormSubmitSDK.enableButton = () => {
    return ShareSdk.isProximityCardAdmin() && ShareSdk.IsProxCardUpdateForm()
}