window.EngagementChangeInitatorSDK = window.EngagementApproveSDK || {}

EngagementChangeInitatorSDK.requesterid = null

EngagementChangeInitatorSDK.onFormLoaded = () => {
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false);

    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    EngagementChangeInitatorSDK.requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

}

EngagementChangeInitatorSDK.onSave = () => {
    //
    var confirmStrings = { confirmButtonLabel: "Confirm to change", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.ChangeInitator, title: "Confirmation" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                EngagementChangeInitatorSDK.changeInitator()
            }
        }
    );
}

EngagementChangeInitatorSDK.changeInitator = async () => {
    Xrm.Utility.showProgressIndicator("Loading")

    try {
        formContext = Xrm.Page.ui.formContext

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())

        var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
        var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
        let owner = EAAShareSdk.getOwner()


        if (owner.id === EngagementChangeInitatorSDK.requesterid) {
            // await EAAShareSdk.assign(formId, requesterid)
            Xrm.Page.data.entity.attributes.getByName("ownerid").setValue(Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue())
        } else {
            //share request to new requester
            var formId = EAAShareSdk.getFormId()
            await EAAShareSdk.share(formId, requesterid)
            //revoke old requester access
            if (EngagementChangeInitatorSDK.requesterid) {
                await EAAShareSdk.revoke(formId, EngagementChangeInitatorSDK.requesterid)
            }
        }
        await Xrm.Page.data.save()
        formContext.ui.close()
        let title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()
        await EAAShareSdk.popupSuccess(title, EAAShareSdk.MessageType.ChangeRequester)
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

EngagementChangeInitatorSDK.enableChangeInitatorButton = () => {
    if (!EAAShareSdk.IsChangeInitatorForm()) {
        return false
    }
    var formType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var activeStatus = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.statecode).getValue()
    if (EAAShareSdk.IsAdmin() && activeStatus === EAAShareSdk.ActiveStatus.Active) {
        if (formType != EAAShareSdk.RequestType.Non_PwcTax) {
            return status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending
                || status === EAAShareSdk.FormStatus.ActualFeeUpdate_Completed
                || status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval
                || status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval
        }
        else {
            return status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending
                || status === EAAShareSdk.FormStatus.ActualFeeUpdate_Completed
        }
    }

    return false

}

EngagementChangeInitatorSDK.onSaveNewRequester = () => {
    let requesterName = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()[0].name
    return fetch(
        `/api/data/v9.0/aia_eaa_forms(${window.parent.EAAShareSdk.getFormId()})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify({ "aia_requester_name": requesterName })
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}