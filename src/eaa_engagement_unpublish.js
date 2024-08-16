
window.EngagementUnpublishSDK = window.EngagementUnpublishSDK || {}

EngagementUnpublishSDK.onFormLoaded = () => {
    
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false);
}

EngagementUnpublishSDK.onUnpublish = () => {
    var confirmStrings = { confirmButtonLabel: "Confirm to unpublish", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.Unpublish, title: "Confirmation" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                //
                sessionStorage.setItem('unpublished', true);
                let rationale =  Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.unpublish_rationale).getValue()
                sessionStorage.setItem('rationale', rationale);

                formContext = Xrm.Page.ui.formContext
                formContext.ui.close()
            }
        }
    );
}

EngagementUnpublishSDK.enableUnpublishButton = () => {
    return EAAShareSdk.IsUnpublishForm() && EAAShareSdk.IsAdmin()
}