var PROXCardUpdateSDK = window.PROXCardUpdateSDK || {};


PROXCardUpdateSDK.onAction = async (ids,primaryControl) => {
    try {
        cardid = ids[0]
        const card = await window.parent.ShareSdk.getPROXCardByIdAsync(cardid)

        if (!await window.parent.ShareSdk.isCurrentAdminHasDeptAuthorityAsync(card.clp_issuedepartment)) {
            throw new Error('The Proximity Card does not belong to your department')
        }

        var pageInput = {
            entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
            pageType: "entityrecord",
            formId: window.parent.ShareSdk.Forms.Card_Update,
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
                title: "Card Update 更新磁咭"
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


PROXCardUpdateSDK.enableButton = () => {

    // var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    // if (areaid === 'viewMapping_pc_updatecardprefix_I' || areaid === 'viewMapping_pc_updatecardprefix_C') {
    //     return false
    // }

    return window.parent.ShareSdk.isProximityCardAdmin() && window.parent.ShareSdk.IsinInventory()
}