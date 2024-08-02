var PROXCardReturnSDK = window.PROXCardReturnSDK || {};


PROXCardReturnSDK.onAction = async (ids, newCardStatus,primaryControl) => {


    var cardid = ids[0]
    try {

        const card = await window.parent.ShareSdk.getPROXCardByIdAsync(cardid)
        // if (card.clp_card_status != window.parent.ShareSdk.ProximityCardStatus.Returned &&
        //     card.clp_card_status != window.parent.ShareSdk.ProximityCardStatus.Damaged &&
        //     card.clp_card_status != window.parent.ShareSdk.ProximityCardStatus.Lost
        // ) {
        //     throw new Error('Card Return Type is not specified')
        // }

        if (!await window.parent.ShareSdk.isCurrentAdminHasDeptAuthorityAsync(card.clp_issuedepartment)) {
            throw new Error('The Proximity Card does not belong to your department')
        }

        if (card.clp_card_type != window.parent.ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            throw new Error('Card Type must be ISMS')
        }

        if (card.clp_card_status != window.parent.ShareSdk.ProximityCardStatus.Allocated) {
            throw new Error('Card Status must be Allocated')
        }

        var replaceCards = await window.parent.ShareSdk.getReplaceCardsAsync(cardid)
        if (replaceCards.length > 0) {

            if (!replaceCards[0]._clp_holder_value) {
                throw new Error(`${card.clp_name} will be replaced by ${replaceCards[0].clp_name}. Please ask Security to encode the replacement before returning the original card.`)
            }
        }

        var pageInput = {
            entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
            pageType: "entityrecord",
            formId: window.parent.ShareSdk.Forms.Card_Return,
            entityId: ids[0],
            data: {
                newCardStatus: newCardStatus
            }
        }

        if (newCardStatus === window.parent.ShareSdk.ProximityCardStatus.Returned) {
            title = "Card Return 歸還磁咭"
        }
        else if (newCardStatus === window.parent.ShareSdk.ProximityCardStatus.Damaged) {
            title = "Card Damaged 損壞磁咭"
        }
        else {
            title = "Card Lost 遺失磁咭"
        }
        Xrm.Navigation.navigateTo(pageInput,
            {
                target: 2,
                position: 1,
                height: { value: 80, unit: "%" },
                width: { value: 70, unit: "%" },
                title: title
            }).then(function success() {
                primaryControl.refresh()
            }, function error() {

            })
    }
    catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
    }


}


PROXCardReturnSDK.enableButton = (ids) => {

    // var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    // if (areaid === 'viewMapping_pc_updatecardprefix_I' || areaid === 'viewMapping_pc_updatecardprefix_C') {
    //     return false
    // }

    return window.parent.ShareSdk.isProximityCardAdmin() && window.parent.ShareSdk.IsinInventory()

}