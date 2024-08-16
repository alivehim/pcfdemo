var PCREQDTLSelectApplicantSDK = window.PCREQDTLSelectApplicantSDK || {};


PCREQDTLSelectApplicantSDK.onAction = () => {

    var count = Xrm.Page.getControl(ShareSdk.ProximityCardRequestSubGrids.SubGridDetail).getGrid().getTotalRecordCount();
    if (count > 20) {
        ShareSdk.formError(new Error('At most 20 applications of new ISMS Proximity Card, encode ISMS Proximity Card and encode Contractor/Consultant Pass can be included in one Requisition Form'))
        return
    }

    var pageInput = {
        entityName: ShareSdk.Tables.proximity_card_request_detail,
        pageType: "entityrecord",
        formId: ShareSdk.Forms.Detail_Main,
        createFromEntity: {
            entityType: ShareSdk.Tables.proximitycardrequest,
            id: Xrm.Page.data.entity.getId(),
            name: Xrm.Page.data.entity.attributes.getByName("clp_name").getValue()
        },
        data: {
            status: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue(),
            accessperiodfrom: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue(),
            accessperiodto: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()
        }
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Select Applicant"
        }).then(function success() {
            ShareSdk.refreshPROXCardREQDTLGrid()
        }, function error() {

        })
}

// PCREQDTLSelectApplicantSDK.refreshGrid = () => {
//     formContext = Xrm.Page.ui.formContext
//     var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.EditableDetail);
//     subgrid.refresh();
// }

PCREQDTLSelectApplicantSDK.enableButton = () => {
    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false
}