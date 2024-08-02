var PROXCardReplaceFormSDK = window.PROXCardReplaceFormSDK || {};

// PROXCardReplaceFormSDK.issuedepartment = 0
PROXCardReplaceFormSDK.onFormLoaded = async () => {

    Xrm.UI._context.Router._preNavigationHandlers = []
    
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Card_Replace) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Card_Replace);
        formItem.navigate();
    }

    var issuedepartment = await ShareSdk.getCurrentAdminHasDeptAsync()
    PROXCardReplaceFormSDK.installCardFilter(issuedepartment)

}

PROXCardReplaceFormSDK.installCardFilter = (issuedepartment) => {


    formContext = Xrm.Page.ui.formContext
    formContext.getControl(ShareSdk.PROXCardInventoryFields.replace_card).addPreSearch(function (executionContext) {

        var filterxml = `<filter type='and'>
        <condition attribute="clp_issuedepartment" operator="eq" value="${issuedepartment}" />
        </filter>`;

        executionContext.getFormContext().getControl(ShareSdk.PROXCardInventoryFields.replace_card).addCustomFilter(filterxml, ShareSdk.Tables.proximitycardinventory);

    });
}