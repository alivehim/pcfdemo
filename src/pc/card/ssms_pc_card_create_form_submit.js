var PROXCardCreateFormSubmitSDK = window.PROXCardCreateFormSubmitSDK || {};

PROXCardCreateFormSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        if (Xrm.Page.data.isValid()) {

            var card_no_from = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardCreationFields.card_no_from).getValue()
            var card_no_to = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardCreationFields.card_no_to).getValue()

            var currentUserId = ShareSdk.getCurrentUserId()
            var user = await ShareSdk.getUserBySystemUserAsync(currentUserId)
            var syncCards = []
            var issuedepartment = await ShareSdk.getCurrentAdminHasDeptAsync()
            if (!card_no_to) {

                const existing = await ShareSdk.getPROXCardByCardNoAsync(card_no_from)
                if (existing.length > 0) {
                    throw new Error(`Duplicated Card No. ${card_no_from} is not allowed`)
                }
                await ShareSdk.addNewCardAsync(card_no_from, ShareSdk.ProximityCardCardType.ISMSProximityCard, ShareSdk.ProximityCardStatus.Available, null, issuedepartment, null, null, user?.clp_userid)

                syncCards.push({ cardno: card_no_from, cardtype: "I", cardstatus: "AVE", expirydate: "", cardexpirydate: "", replacecardno: "", department: issuedepartment })
            } else {
                var prefix = card_no_from.substring(0, 2)
                var from = parseInt(card_no_from.substring(2, 8))
                var to = parseInt(card_no_to.substring(2, 8))
                var cards = []
                for (var index = from; index <= to; index++) {
                    cards.push(`${prefix}${ShareSdk.padDigitToString(index, 5)}`)
                }

                var existings = await ShareSdk.getPROXCardsAsync(cards)
                if (existings.length > 0) {
                    throw new Error(`Duplicated Card No. ${existings[0].clp_name} is not allowed`)
                }

                for (var item of cards) {
                    await ShareSdk.addNewCardAsync(item, ShareSdk.ProximityCardCardType.ISMSProximityCard, ShareSdk.ProximityCardStatus.Available, null, issuedepartment, null, null, user?.clp_userid)

                    syncCards.push({ cardno: item, cardtype: "I", cardstatus: "AVE", expirydate: "", cardexpirydate: "", replacecardno: "", department: issuedepartment })

                }
            }

            //await PROXCardCreateFormSubmitSDK.syncCardsAsync(syncCards)

            await Xrm.Page.data.save()
            formContext.ui.close()

            window.parent.ShareSdk.openAlertDialog('Card is added successfully').then(res => {
                var pageInput = {
                    pageType: "entitylist",
                    entityName: ShareSdk.Tables.proximitycardinventory,
                }

                Xrm.Navigation.navigateTo(pageInput)

                // ShareSdk._doUntil(
                //     () => {
                //         window.parent.document.querySelector(`div[id*="modalDialogRoot"] button[id*="confirmButton_"]`).click()
                //     },
                //     () => window.parent.document.querySelector(`div[id*="modalDialogRoot"] button[id*="confirmButton_"]`)
                // )

            })


        }

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}


PROXCardCreateFormSubmitSDK.syncCardsAsync = async (syncCards) => {
    let cardJsonObj = {
        SSMS_PC_CARDS: {
            SSMS_PC_CARD: []
        }
    }

    for (var item of syncCards) {
        cardJsonObj.SSMS_PC_CARDS.SSMS_PC_CARD.push({
            "CARD_ID": item.cardno,
            "CARD_NO": item.cardno,
            "CARD_TYPE": item.cardtype,
            "ISSUE_DEPT": ShareSdk.getLegacyIssueDepartment(item.department),
            "CARD_STATUS": item.cardstatus,
            "EXPIRY_DATE": item.expirydate || '',
            "CARD_EXPIRY_DATE": item.cardexpirydate || '',
            "REPLACE_CARD_ID": "",
            "CARD_PREFIX": "",
        })
    }
    const cardsXML = ShareSdk.JSONtoXML(cardJsonObj)
    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.AddCards, null, null, cardsXML)
}

PROXCardCreateFormSubmitSDK.enableButton = () => {
    return ShareSdk.isProximityCardAdmin()
}