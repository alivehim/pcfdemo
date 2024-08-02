var PROXCardReqCTAMSDK = window.PROXCardReqCTAMSDK || {};

PROXCardReqCTAMSDK.onAction = async () => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

        var responsible_cp = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsible_cp).getValue()

        if (responsible_cp) {

            var id = ShareSdk.getLookupId(responsible_cp)
            const clpuser = await ShareSdk.getUserAsync(id)

            var userId = ''
            if (clpuser[ShareSdk.CLPUserFields.networkid]) {
                userId = clpuser[ShareSdk.CLPUserFields.networkid]
            }
            else if (clpuser[ShareSdk.CLPUserFields.person_id]) {
                userId = clpuser[ShareSdk.CLPUserFields.person_id]
            }

            var navigationOptions = {
                target: 2,
                width: 500, // value specified in pixel
                height: 400, // value specified in pixel
                position: 1
            };

            const ctamURL = await ShareSdk.getEnvVariableAsync('clp_SSMS_CTAM_GetAuthorizedPersonList')

            localStorage.setItem('reqCTAMURL', ctamURL)


            Xrm.Navigation.openUrl(location.origin + "/WebResources/clp_indexhtml?Data=userId%3D" + userId, navigationOptions)
        }

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }




}

PROXCardReqCTAMSDK.enableButton = async () => {

    if (Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsible_cp).getValue()) {
        return true
    }

    return false
}