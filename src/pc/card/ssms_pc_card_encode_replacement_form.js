var PROXCardEncodeReplacementFormSDK = window.PROXCardEncodeReplacementFormSDK || {};


PROXCardEncodeReplacementFormSDK.onFormLoaded = () => {
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Card_Encode_Replacement) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Card_Encode_Replacement);
        formItem.navigate();
    }
}

