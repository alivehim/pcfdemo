window.EngagementDeleteSDK = window.EngagementDeleteSDK || {}

EngagementDeleteSDK.onDelete = () => {
    formContext = Xrm.Page.ui.formContext
    if (formContext) {

        var confirmStrings = { confirmButtonLabel: "Confirm to delete", cancelButtonLabel: "Return", text: EAAShareSdk.ConfirmationMessage.Delete, title: "Confirmation" };
        var confirmOptions = { height: 200, width: 450 };
        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {
                    //set activate status with "no"

                    EngagementDeleteSDK.deleteProcess()

                }
            }
        );
    }
}

EngagementDeleteSDK.deleteProcess = async () => {

    Xrm.Utility.showProgressIndicator("Loading")

    try {
        var reqeustGuid = EAAShareSdk.getFormId()
        //deactive
        await EAAShareSdk.deactiveEngagement(reqeustGuid)
        EAAShareSdk.navigateBack()
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

/**
 * enable to display delete button
 * Only visible for the draft request 
 * Only visible for Requester
 * @returns boolean
 */
EngagementDeleteSDK.enableDeleteButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var active = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_active).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    return (status === EAAShareSdk.FormStatus.Initiator_Pending) && active && EAAShareSdk.getCurrentUserId() === requesterid;
}