var PCREQDTLExportSDK = window.PCREQDTLExportSDK || {};

PCREQDTLExportSDK.onAction = async (primaryControl) => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

       await window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximtiy_Card_Application_Details, "Proximtiy_Card_Application_Details")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}


PCREQDTLExportSDK.enableButton = async () => {
     
    return window.parent.ShareSdk.IsApplicationDetail()
}