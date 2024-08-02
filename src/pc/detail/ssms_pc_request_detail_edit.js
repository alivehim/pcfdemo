var PCREQDTLEditSDK = window.PCREQDTLEditSDK || {};

PCREQDTLEditSDK.onAction = (ids) => {
    var pageInput = {
        entityName: ShareSdk.Tables.proximity_card_request_detail,
        pageType: "entityrecord",
        formId: ShareSdk.Forms.Detail_Main,
        entityId: ids[0],
        data: {

        }
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Edit Applicant"
        }).then(function success() {
            ShareSdk.refreshPROXCardREQDTLGrid()
        }, function error() {

        })
}


PCREQDTLEditSDK.enableButton = (count) => {
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        return ShareSdk.isPROXCardRequestor()
    }
    return false
}