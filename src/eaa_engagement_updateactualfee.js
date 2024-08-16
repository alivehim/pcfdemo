
window.EngagementUpdateActualFeeSDK = window.EngagementUpdateActualFeeSDK || {}

EngagementUpdateActualFeeSDK.onUpdate = () => {


    var confirmStrings = { confirmButtonLabel: "Submit", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.UpdateActualFee, title: "Confirmation" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                EngagementUpdateActualFeeSDK.updateActualFeeProcess()
            }
        }
    );
}

EngagementUpdateActualFeeSDK.updateActualFeeProcess = async () => {

    var isCompleted = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.completed).getValue()
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())
    if (isCompleted === false) {
        // clear the reminder count
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sla_reminder).setValue()
        Xrm.Page.data.save().then(() => {
            EAAShareSdk.navigateBack()
        })
        return
    }

    await EngagementUpdateActualFeeSDK.updateActualFee()
}


EngagementUpdateActualFeeSDK.updateActualFee = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        var currentUserId = EAAShareSdk.getCurrentUserId()
        var requestGuid = EAAShareSdk.getFormId();
        await EngagementUpdateActualFeeSDK.engagementValidation()


        await Xrm.Page.data.save()
        await EAAShareSdk.UpdateActualFee(requestGuid, currentUserId)
        var title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()
        await EAAShareSdk.popupSuccess(title, EAAShareSdk.MessageType.Sumbit)

    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

/**
 * system should check whether “AFS number” has been input
 * @returns 
 */
EngagementUpdateActualFeeSDK.engagementValidation = () => {

    const service_number = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.service_number).getValue()
    if (!service_number) {

    }
    return Promise.resolve()
}
EngagementUpdateActualFeeSDK.enableUpdateButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
    if (status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending &&
        EAAShareSdk.IsCurrentUserCanUpdateActualFee() &&
        stage === EAAShareSdk.Stages.ActualFeeUpdate) {
        return true
    }

    return false
}