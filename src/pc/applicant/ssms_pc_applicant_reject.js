var PROXCardApplicantRejectSDK = window.PROXCardApplicantRejectSDK || {};


PROXCardApplicantRejectSDK.onAction = (ids) => {
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
            title: "Reject"
        }).then(function success() {
            PROXCardApplicantNewSDK.refreshGrid()
        }, function error() {

        })
}


PROXCardApplicantRejectSDK.enableButton = (count) => {
    // const inputData = Xrm.Utility.getPageContext().input.data
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === ShareSdk.PROXCardReqStatus.WaitingforCardAllocation) {
        return ShareSdk.isProximityCardAdmin()
    }
    return false
}