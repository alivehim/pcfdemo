var PROXCardUpdateCardPrefixSDK = window.PROXCardUpdateCardPrefixSDK || {};


PROXCardUpdateCardPrefixSDK.onAction = async (ids, type) => {
    try {
        if (type === 'C') {


            detail = await PROXCardUpdateCardPrefixSDK.getPROXCardREQDTLAsync(ids[0])
            cardid = detail._clp_proximity_card_value
        }
        else {
            cardid = ids[0]
        }


        var pageInput = {
            entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
            pageType: "entityrecord",
            formId: window.parent.ShareSdk.Forms.Card_UpdateCardPrefix,
            entityId: cardid,
            data: {
            }
        }

        Xrm.Navigation.navigateTo(pageInput,
            {
                target: 2,
                position: 1,
                height: { value: 80, unit: "%" },
                width: { value: 70, unit: "%" },
                title: "Update Card Prefix"
            }).then(function success() {
            }, function error() {

            })
    }
    catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
    }

}
PROXCardUpdateCardPrefixSDK.getPROXCardREQDTLAsync = (id) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_request_details(${id})`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardUpdateCardPrefixSDK.enableButton = () => {

    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    if (areaid != 'viewMapping_pc_updatecardprefix_I' && areaid != 'viewMapping_pc_updatecardprefix_C') {
        return false
    }

    return window.parent.ShareSdk.isProximityCardSecurity()
}