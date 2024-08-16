var PCREQDTLRemoveSDK = window.PCREQDTLRemoveSDK || {};

PCREQDTLRemoveSDK.onAction = async (ids) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {


        for (var item of ids) {
            await PCREQDTLRemoveSDK.removeDetailAsync(item)
        }
        ShareSdk.refreshPROXCardREQDTLGrid()
    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PCREQDTLRemoveSDK.removeDetailAsync = (detailid) => {
    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${detailid})`,
        {
            method: "DELETE",
        }
    ).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve()
    }).catch(err => {
        throw new Error(err.message)
    })
}

PCREQDTLRemoveSDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false
}