var PROXCardCollectSDK = window.PROXCardCollectSDK || {};


PROXCardCollectSDK.onAction = async (ids,primaryControl) => {

    var cardid = ids[0]
    try {

        const card = await window.parent.ShareSdk.getPROXCardByIdAsync(cardid)

        if (!await window.parent.ShareSdk.isCurrentAdminHasDeptAuthorityAsync(card.clp_issuedepartment)) {
            throw new Error('The Proximity Card does not belong to your department')
        }

        if (card.clp_card_type != window.parent.ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            throw new Error('Card Type must be ISMS')
        }

        if (!(card.clp_card_status === window.parent.ShareSdk.ProximityCardStatus.Reserved || card.clp_card_status === window.parent.ShareSdk.ProximityCardStatus.PendingforReplacement)) {
            throw new Error('Card Status must be Reserved/ Pending for Replacement')
        }

        if (!card._clp_holder_value) {
            throw new Error('The Proximity Card must be encoded by Security before collection')
        }


        // if (card.clp_card_type === ShareSdk.ProximityCardCardType.ISMSProximityCard &&
        //     (card.clp_card_status === ShareSdk.ProximityCardStatus.Reserved || card.clp_card_status === ShareSdk.ProximityCardStatus.PendingforReplacement) &&
        //     card._clp_holder_value) {
        //     return true
        // }

        var pageInput = {
            entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
            pageType: "entityrecord",
            formId: window.parent.ShareSdk.Forms.Card_Collect,
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
                title: "Collect Card 領取磁咭"
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


PROXCardCollectSDK.enableButton = (ids) => {

    // var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    // if (areaid === 'viewMapping_pc_updatecardprefix_I' || areaid === 'viewMapping_pc_updatecardprefix_C') {
    //     return false
    // }

    return window.parent.ShareSdk.isProximityCardAdmin() && window.parent.ShareSdk.IsinInventory()
}