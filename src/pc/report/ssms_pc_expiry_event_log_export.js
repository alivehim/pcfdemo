var PROXCardExpiryEventLogExportSDK = window.PROXCardExpiryEventLogExportSDK || {};

PROXCardExpiryEventLogExportSDK.onAction = async (primaryControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

       await window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximity_Card_Expiry_Event_Log, "Proximity_Card_Expiry_Event_Log")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}


PROXCardExpiryEventLogExportSDK.enableButton = () => {
    return true
}