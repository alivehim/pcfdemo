var PROXCardSSRemoveSDK = window.PROXCardSSRemoveSDK || {};

PROXCardSSRemoveSDK.onAction = async (ids) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {


        var formid = ShareSdk.getCurrentFormId()
        for (var item of ids) {
            await PROXCardSSRemoveSDK.removeSSAsync(formid, item)
        }

        formContext = Xrm.Page.ui.formContext
        var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.Subgrid_station);
        subgrid.refresh();

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardSSRemoveSDK.removeSSAsync = (formid, ssid) => {
    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${formid})/clp_proximitycardrequest_substation_list(${ssid})/$ref`,
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

PROXCardSSRemoveSDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false

}

