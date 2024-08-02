var PROXCardReplaceSDK = window.PROXCardReplaceSDK || {};


PROXCardReplaceSDK.onAction = async (ids,primaryControl) => {
    var cardid = ids[0]
    try {

        const card = await window.parent.ShareSdk.getPROXCardByIdAsync(cardid)


        if (!await window.parent.ShareSdk.isCurrentAdminHasDeptAuthorityAsync(card.clp_issuedepartment)) {
            throw new Error('The Proximity Card does not belong to your department')
        }

        if (card.clp_card_type != window.parent.ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            throw new Error('Card Type must be ISMS')
        }

        if (
            card.clp_card_status != window.parent.ShareSdk.ProximityCardStatus.Allocated
        ) {
            throw new Error('Card Status must be Allocated')
        }

        const result = await PROXCardReplaceSDK.getPROXCardByReplaceCardIdAsync(cardid)
        if (result.length > 0) {
            throw new Error(`${card.clp_name} will be replaced by ${result[0].clp_name}. Each card can only be replaced by ONE card at the same time.`)
        }

        var pageInput = {
            entityName: window.parent.ShareSdk.Tables.proximitycardinventory,
            pageType: "entityrecord",
            formId: window.parent.ShareSdk.Forms.Card_Replace,
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
                title: "Card Replacement 更換磁咭"
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

PROXCardReplaceSDK.getPROXCardByReplaceCardIdAsync = (cardid) => {

    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=_clp_replace_card_value eq '${cardid}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(res.value)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

PROXCardReplaceSDK.enableButton = () => {

    // var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    // if (areaid === 'viewMapping_pc_updatecardprefix_I' || areaid === 'viewMapping_pc_updatecardprefix_C') {
    //     return false
    // }

    return window.parent.ShareSdk.isProximityCardAdmin() && window.parent.ShareSdk.IsinInventory()
}