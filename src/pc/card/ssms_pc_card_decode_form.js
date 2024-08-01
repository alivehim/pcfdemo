var PROXCardDecodeFormSDK = window.PROXCardDecodeFormSDK || {};


PROXCardDecodeFormSDK.onFormLoaded = () => {
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Card_Decode) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Card_Decode);
        formItem.navigate();
    }
}

