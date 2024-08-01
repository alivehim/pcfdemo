var PROXCardDecodeListFormSDK = window.PROXCardDecodeListFormSDK || {};


PROXCardDecodeListFormSDK.onFormLoaded = () => {
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Card_EncodeList) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Card_EncodeList);
        formItem.navigate();
    }
}

