var PROXCardExportSDK = window.PROXCardExportSDK || {};

PROXCardExportSDK.onExpiryRecordExportAction = async (primaryControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

       await window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximity_Card_Expiry_Record, "Proximity_Card_Expiry_Record")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardExportSDK.onInventoryExportAction = async (primaryControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
       await window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximity_Card_Inventory, "Proximity_Card_Inventory")

    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}




PROXCardExportSDK.enableExpiryRecordExportButton = () => {
    return window.parent.ShareSdk.IsinExpirtyRecord()
}

PROXCardExportSDK.enableInventoryExportButton = () => {
    return window.parent.ShareSdk.IsinInventory()
}