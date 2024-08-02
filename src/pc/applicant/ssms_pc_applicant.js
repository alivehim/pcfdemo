var PROXCardApplicantSDK = window.PROXCardApplicantSDK || {};


PROXCardApplicantSDK.onFormLoad = (context) => {

    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Applicant_Update) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Applicant_Update);
        formItem.navigate();
    }

    PROXCardApplicantSDK.unlockFields()
    PROXCardApplicantSDK.installCardFilter()
}

PROXCardApplicantSDK.unlockFields = () => {

    const lockfields = (lockFields) => {
        for (let attr in Xrm.Page.data.entity.attributes._collection) {
            if (lockFields.some(p => p == attr)) {
                ShareSdk.setDisableControl(attr, false)
            }
        }
    }



    lockfields([ShareSdk.ApplicantFields.phoneno, ShareSdk.ApplicantFields.associated_card])
}

PROXCardApplicantSDK.installCardFilter = () => {

    const formid= ShareSdk.getCurrentFormId()
    formContext = Xrm.Page.ui.formContext
    formContext.getControl(ShareSdk.ApplicantFields.associated_card).addPreSearch(function (executionContext) {

        var filterxml = `<filter type='and'>
        <condition attribute="clp_holder" operator="eq" value="${formid}" />
        </filter>`;

        executionContext.getFormContext().getControl(ShareSdk.ApplicantFields.associated_card).addCustomFilter(filterxml, ShareSdk.Tables.proximitycardinventory);

    });
}

