var PROXCardReqExportSDK = window.PROXCardReqExportSDK || {};

PROXCardReqExportSDK.onAction = async () => {

    window.parent.PROXCardReqExportUtilitiesSDK.export()


    // const formid = ShareSdk.getCurrentFormId()
    // const appno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.name).getValue()
    // var option = {
    //     pageType: "webresource",
    //     webresourceName: "clp_ssms_pc_request_submit_success.html",
    // }

    // option.data = `applitionNo=${appno}&requestid=${formid}`

    // return Xrm.Navigation.navigateTo(option, {
    //     target: 2,
    //     position: 1,
    //     width: {
    //         value: 450,
    //         unit: 'px'
    //     },
    //     height: {
    //         value: 200,
    //         unit: 'px'
    //     },
    //     title: "Message"
    // }).then(
    //     function success() {
    //         // Run code on success
    //     },
    //     function error() {
    //         // Handle errors
    //     }
    // );
}

PROXCardReqExportSDK.onHomeExportAction = async (primaryControl) => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

        await window.parent.ShareSdk.Export(primaryControl, window.parent.ShareSdk.Views.Proximity_Card_Applications, "Proximity_Card_Applications")


    } catch (err) {
        window.parent.ShareSdk.showGlobalNotification(err.message, 2)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}


PROXCardReqExportSDK.enableButton = () => {

    if (!window.parent.ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    return status != null
}

PROXCardReqExportSDK.enableHomeExportButton = () => {

    return true

}