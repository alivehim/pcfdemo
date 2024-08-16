var PROXCardApplicantExcelSDK = window.PROXCardApplicantExcelSDK || {};

PROXCardApplicantExcelSDK.onAction = () => {

}

PROXCardApplicantExcelSDK.enableButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        return true
    }
    return false
}