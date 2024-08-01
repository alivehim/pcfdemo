var PROXCardCreateSDK = window.PROXCardCreateSDK || {};

PROXCardCreateSDK.onAction = () => {
    var pageInput = {
        entityName: window.parent.ShareSdk.Tables.proximity_card_creation_event,
        pageType: "entityrecord",
        formId: window.parent.ShareSdk.Forms.Card_Create,
        data: {
        }
    }

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Card Creation 新增磁咭"
        }).then(function success() {
        }, function error() {

        })
}


PROXCardCreateSDK.enableButton = () => {

    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    if(areaid === 'viewMapping_pc_updatecardprefix_I' 
    || areaid === 'viewMapping_pc_updatecardprefix_C' || areaid==='viewMapping_pc_expiry_record'){
        return false
    }
    return window.parent.ShareSdk.isProximityCardAdmin()
}