var PROXCardAuditExportSDK = window.PROXCardAuditExportSDK || {};

PROXCardAuditExportSDK.onAction = async (primaryControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

       await window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximity_Card_Audit, "Proximity_Card_Audit")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardAuditExportSDK.onDetailAction = async (primaryControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

       await  window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximity_Card_Audit_Details, "Proximity_Card_Audit_Details")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}


PROXCardAuditExportSDK.enableButton = () => {
    return true
}

PROXCardAuditExportSDK.enableDetailButton = () => {
    return true
}