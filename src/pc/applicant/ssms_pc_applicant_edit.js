var PROXCardApplicantEditSDK = window.PROXCardApplicantEditSDK || {};

PROXCardApplicantEditSDK.onAction = (ids) => {
    var pageInput = {
        entityName: window.parent.ShareSdk.Tables.proximitycardapplicant,
        pageType: "entityrecord",
        formId: window.parent.ShareSdk.Forms.Applicant_Update,
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
            title: "Update Applicant"
        }).then(function success() {
             
        }, function error() {

        })
}


PROXCardApplicantEditSDK.enableButton = (count) => {
    return window.parent.ShareSdk.isProximityCardAdmin()
}