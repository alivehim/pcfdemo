var PROXCardApplicantAloocateCardSDK = window.PROXCardApplicantAloocateCardSDK || {};


PROXCardApplicantAloocateCardSDK.onAction = (ids) => {
    var pageInput = {
        entityName: ShareSdk.Tables.proximitycardapplicant,
        pageType: "entityrecord",
        formId: ShareSdk.Forms.Applicant_Main,
        entityId: ids[0],
        data: {
            status: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
        }
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Allocate Card"
        }).then(function success() {
            PROXCardApplicantNewSDK.refreshGrid()
        }, function error() {

        })
}


PROXCardApplicantAloocateCardSDK.enableButton = (count) => {
    // var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    // if (status === null) {
    //     return true
    // }
    // return false

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation) {
        return ShareSdk.isProximityCardAdmin()
    }
    return false

}