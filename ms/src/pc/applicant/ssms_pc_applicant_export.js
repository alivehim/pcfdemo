var PROXCardApplicantExportSDK = window.PROXCardApplicantExportSDK || {};

PROXCardApplicantExportSDK.onAction = async (primaryControl) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

       await window.parent.ShareSdk.Export(primaryControl,window.parent.ShareSdk.Views.Proximity_Card_Applicant_Profile, "Proximity_Card_Applicant_Profile")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}


PROXCardApplicantExportSDK.enableButton = () => {
    return true
}