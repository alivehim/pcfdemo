window.EngagementCancelSDK = window.EngagementCancelSDK || {}


EngagementCancelSDK.onCancel = () => {

    formContext = Xrm.Page.ui.formContext
    if (formContext) {

        var confirmStrings = { confirmButtonLabel: "Confirm to cancel", cancelButtonLabel: "Return", text: EAAShareSdk.ConfirmationMessage.Cancel, title: "Confirmation" };
        var confirmOptions = { height: 200, width: 450 };
        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {
                    EngagementCancelSDK.cancelProcess()
                }
            }
        );
    }
}

EngagementCancelSDK.cancelProcess = async () => {
    //delete this date
    const formId = EAAShareSdk.getFormId()
    Xrm.Utility.showProgressIndicator("Loading")

    try {
        await EAAShareSdk.deleteEngagement(formId)

        // EAAShareSdk.navigateToPage(EAAShareSdk.Views.MyRequest)
        EAAShareSdk.navigateToHomePage()

        // auto confirm (auto find the dialog and click yes)
        EAAShareSdk._doUntil(
            () => {
                window.parent.document.querySelector(`div[id*="modalDialogRoot"] button[id*="cancelButton_"]`).click()
            },
            () => window.parent.document.querySelector(`div[id*="modalDialogRoot"] button[id*="cancelButton_"]`)
        )
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

/**
 * enable to display cancel button
 * Only visible to Requester before “Save”
 * @returns 
 */
EngagementCancelSDK.enableCancelButton = () => {
    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var active = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_active).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    var activeStatus = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.statecode).getValue()
    return !active && EAAShareSdk.getCurrentUserId() === requesterid && activeStatus === EAAShareSdk.ActiveStatus.Active;
}