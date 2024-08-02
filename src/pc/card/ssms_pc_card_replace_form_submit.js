var PROXCardReplaceFormSubmitSDK = window.PROXCardReplaceFormSubmitSDK || {};


PROXCardReplaceFormSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        formContext = Xrm.Page.ui.formContext

        if (Xrm.Page.data.isValid()) {

            var user = await ShareSdk.getCurrentClpUserAsync()
            const formid = ShareSdk.getCurrentFormId()
            const replaceByCard = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.replace_card).getValue()
            const replaceByCardId = ShareSdk.getLookupId(replaceByCard)
            await PROXCardReplaceFormSubmitSDK.updatePROXCardAsync(replaceByCardId, ShareSdk.ProximityCardStatus.PendingforReplacement, formid,user?.clp_userid)
            // await Xrm.Page.data.save()

            await ShareSdk.addPROXCardNotificationAsync(replaceByCardId, ShareSdk.CardNotificaitonType.ReplaceEncode)

            await PROXCardReplaceFormSubmitSDK.syncDataAsync(replaceByCardId, Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue())


            await ShareSdk.openAlertDialog('Card is pended for replacement successfully').then(res => {
                // var pageInput = {
                //     pageType: "entitylist",
                //     entityName: ShareSdk.Tables.proximitycardinventory,
                // }

                // Xrm.Navigation.navigateTo(pageInput)
            })

            formContext.ui.close()

        }



    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardReplaceFormSubmitSDK.updatePROXCardAsync = (id, status, replaceCardId,USERID) => {

    var body = {
        "clp_card_status": status,
        "clp_replace_card@odata.bind": `/clp_proximitycardinventories(${replaceCardId})`,
        "clp_latest_modified_on":new Date()
    }

    if(USERID){
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories(${id})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

PROXCardReplaceFormSubmitSDK.syncDataAsync = async (card_id, replace_card_no) => {
    //api

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
            "CARD_TYPE": 'I',
            "ISSUE_DEPT": ShareSdk.getLegacyIssueDepartment(card.clp_issuedepartment),
            "CARD_STATUS": "PFR",
            "EXPIRY_DATE": card.clp_expirydate,
            "CARD_EXPIRY_DATE": card.clp_card_expiry_date,
            "REPLACE_CARD_ID": replace_card_no,
            "CARD_PREFIX": card.clp_cardprefix,
        }
    )

    const cardsXML = ShareSdk.JSONtoXML(cardJsonObj)
    console.log(cardsXML)

    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateCard, null, card_id, cardsXML)
}


PROXCardReplaceFormSubmitSDK.enableButton = () => {
    return ShareSdk.isProximityCardAdmin() && ShareSdk.IsProxCardReplaceForm()
}