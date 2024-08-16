var PROXCardDeleteSDK = window.PROXCardDeleteSDK || {};


PROXCardDeleteSDK.onAction = async (ids, primaryControl) => {

    var cardid = ids[0]
    try {

        const card = await window.parent.ShareSdk.getPROXCardByIdAsync(cardid)
        if (!await window.parent.ShareSdk.isCurrentAdminHasDeptAuthorityAsync(card.clp_issuedepartment)) {
            throw new Error('The Proximity Card does not belong to your department')
        }

        const result = await window.parent.ShareSdk.getPROXCardREQDTLAsync(cardid)



        if (result.length > 0) {
            throw new Error('Card cannot be deleted as it has been used in the system')
        }

        let message = `Are you sure to delete the selected Proximity Card?`
        var confirmStrings = { confirmButtonLabel: "Ok", cancelButtonLabel: "Cancel", text: message, title: "Message" };
        var confirmOptions = { height: 200, width: 450 };
        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {

                    Xrm.Utility.showProgressIndicator("Loading")
                    PROXCardDeleteSDK.deleteCardAsync(ids[0]).then(res => {

                        PROXCardDeleteSDK.syncDeleteCardAsync(card.clp_name).then(res => {

                            Xrm.Utility.closeProgressIndicator()


                            window.parent.ShareSdk.openAlertDialog('Card is deleted successfully').then(res => {
                                // var pageInput = {
                                //     pageType: "entitylist",
                                //     entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
                                // }
                                // Xrm.Navigation.navigateTo(pageInput).then(res => {

                                // })

                                primaryControl.refresh()
                            })



                        })


                    }).catch(err => {

                        Xrm.UI.addGlobalNotification({
                            level: 2,
                            message: err.message,
                            type: 2,
                            showCloseButton: true
                        })

                        Xrm.Utility.closeProgressIndicator()
                    })
                }
            }
        );
    }
    catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
    }



}

PROXCardDeleteSDK.deleteCardAsync = (id) => {
    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories(${id})`,
        {
            method: "DELETE",
            headers: {
                "Prefer": "return=representation"
            }
        }
    ).then(async res => {
        if (res.status !== 204) {
            const { error } = await res.json()
            throw new Error(error.message)
        }
        return Promise.resolve()
    }).catch(err => {
        Xrm.UI.addGlobalNotification({
            level: 2,
            message: err.message,
            type: 2,
            showCloseButton: true
        })
    })
}

PROXCardDeleteSDK.syncDeleteCardAsync = async (cardno) => {
    await window.parent.ShareSdk.addSyncDataAsync(window.parent.ShareSdk.DataSyncType.DeleteCard, null, null, cardno)
}
PROXCardDeleteSDK.enableButton = async (ids) => {

    // var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    // if (areaid === 'viewMapping_pc_updatecardprefix_I' || areaid === 'viewMapping_pc_updatecardprefix_C') {
    //     return false
    // }

    return window.parent.ShareSdk.isProximityCardAdmin() && window.parent.ShareSdk.IsinInventory()
}