var PROXCardApplicantNewSDK = window.PROXCardApplicantNewSDK || {};


PROXCardApplicantNewSDK.onAction = () => {
    var pageInput = {
        entityName: ShareSdk.Tables.proximitycardapplicant,
        pageType: "entityrecord",
        formId: ShareSdk.Forms.Applicant_Main,
        createFromEntity: {
            entityType: ShareSdk.Tables.proximitycardrequest,
            id: Xrm.Page.data.entity.getId(),
            name: Xrm.Page.data.entity.attributes.getByName("clp_name").getValue()
        },
        data: {

        }
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Add Applicant"
        }).then(function success() {
            PROXCardApplicantNewSDK.refreshGrid()
        }, function error() {

        })
}

PROXCardApplicantNewSDK.refreshGrid = () => {
    formContext = Xrm.Page.ui.formContext
    var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.Applicant);
    subgrid.refresh();
}

PROXCardApplicantNewSDK.enableButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        return true
    }
    return false
}