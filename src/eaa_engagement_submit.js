window.EngagementSubmitSDK = window.EngagementSubmitSDK || {}

EngagementSubmitSDK.onSubmit = async () => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

        //fill requester submit date
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester_date).setValue(new Date());
        //clear approval data
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.head).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.head_date).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.cfo).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.cfo_date).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.finance_manager).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.finance_date).setValue()

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.on_behalf_rcfo).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.on_behalf_rcfo_submit_date).setValue()

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.on_behalf_head).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.on_behalf_head_submit_date).setValue()

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.approver_date).setValue()

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.unpublish_date).setValue()
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.unpublish_user).setValue()

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())

        await EngagementSubmitSDK.engagementValidation()
        await Xrm.Page.data.save()
        EngagementSubmitSDK.submitConfirmation()
    }
    catch (err) {

        const msgKey = new Date().getTime().toString(16)
        Xrm.Page.ui.formContext.ui.setFormNotification(
            err?.message ? err.message : err,
            'ERROR',
            msgKey);

        window.setTimeout(function () {
            Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
        }, 10000);
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}


EngagementSubmitSDK.submitConfirmation = () => {

    var confirmStrings = { confirmButtonLabel: "Yes", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.Submit, title: "Confirmation" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                EngagementSubmitSDK.submit()
            } else {
                Xrm.Utility.closeProgressIndicator()
            }
        }
    );

}

EngagementSubmitSDK.submit = async () => {

    try {
        Xrm.Utility.showProgressIndicator("Loading")
        await EngagementSubmitSDK.deleteShareRecords()
        const title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()
        let newtitle = await EAAShareSdk.generateReferenceIfNotExists(title)
        if (newtitle) {
            await EngagementSubmitSDK.saveTitle(newtitle)
            await EngagementSubmitSDK.notification(newtitle)
            await EngagementSubmitSDK.changeStage(newtitle)

        } else {
            await EngagementSubmitSDK.changeStage(title)
        }
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

EngagementSubmitSDK.changeStage = async (title) => {
    var requestGuid = EAAShareSdk.getFormId()
    await EAAShareSdk.share(requestGuid, EAAShareSdk.getCurrentUserId())
    await EAAShareSdk.changeFormWorkflowStage(requestGuid, EAAShareSdk.WorkflowStages.RequesterSubmit)
    // EAAShareSdk.navigateBack()
    await EAAShareSdk.popupSuccess(title, EAAShareSdk.MessageType.Sumbit)

}

EngagementSubmitSDK.notification = (newTitle) => {
    //update the DOM
    window.parent.document.querySelector('h1[id*=formHeaderTitle]').innerText = newTitle

    notification =
    {
        type: 2,
        level: 1,
        message: `EAA Workflow ${newTitle} has been created.`,
        showCloseButton: true
    };

    Xrm.App.addGlobalNotification(notification).then(
        function success(result) {
            // Wait for 3 seconds and then clear the notification
            window.setTimeout(function () {
                Xrm.App.clearGlobalNotification(result);
            }, 3000);
        },
        function (error) {
            console.log(error.message);
        }
    );
}

EngagementSubmitSDK.saveTitle = (title) => {

    var RequestGUID = EAAShareSdk.getFormId();

    return fetch(
        `/api/data/v9.0/aia_eaa_forms(${RequestGUID})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify({
                "aia_name": title,
                "aia_reference_createdon": new Date(),
                "aia_sys_active": true
            })
        }
    ).then(res => res.json())
        .then(res => {
            if (res.error) {
                return Promise.reject(res.error.message)
            }
            //Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_active).setValue(true)
            //Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).setValue(title)
            return Promise.resolve()

        }).catch(err => {
            const msgKey = new Date().getTime().toString(16)
            Xrm.Page.ui.formContext.ui.setFormNotification(
                err?.message,
                'ERROR',
                msgKey);

            window.setTimeout(function () {
                Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
            }, 10000);
        })
}
/**
 * 
 * @returns 
 */
EngagementSubmitSDK.engagementValidation = async () => {
    //check the contacts

    const contacts = await EngagementSubmitSDK.getContacts()
    if (contacts.length < 1) {
        throw new Error('Contact Detail of Engagement Team in Service Provider in General Information tab: Required fields must be filled in ')
    }

    const localviewers = await EngagementSubmitSDK.getLocalViewers()

    if (localviewers.length < 1) {
        throw new Error('Local Viewer in General Information tab: Required fields must be filled in ')
    }
}

EngagementSubmitSDK.getContacts = () => {
    let requestGuid = EAAShareSdk.getFormId()
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.contact}s?$filter=_aia_sys_eaa_form_value eq '${requestGuid}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message)
        }

        return Promise.resolve(res.value)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EngagementSubmitSDK.getLocalViewers = () => {
    let requestGuid = EAAShareSdk.getFormId()
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.local_viewer}s?$filter=_aia_sys_eaa_form_value eq '${requestGuid}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message)
        }

        return Promise.resolve(res.value)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EngagementSubmitSDK.deleteShareRecords = async () => {
    var formId = EAAShareSdk.getFormId()
    let shareRecords = await EAAShareSdk.getShareRecords(formId)

    if (shareRecords.length > 0) {
        //exclude current user
        let currentUserId = EAAShareSdk.getCurrentUserId()
        let revokeRecords = shareRecords.filter(x => x.id != currentUserId)
        Promise.all(revokeRecords.map(item => {
            if (item.type === "systemuser") {
                return EAAShareSdk.revoke(formId, item.id).then(
                    function success(result) {
                        return Promise.resolve()
                    },
                    function (error) {
                        console.log(error.message);
                        // handle error conditions
                        EAAShareSdk.addGlobalMessage(error.message, 2)
                    }
                )
            } else {
                return EAAShareSdk.revokeTeam(formId, item.id).then(
                    function success(result) {
                        return Promise.resolve()
                    },
                    function (error) {
                        console.log(error.message);
                        EAAShareSdk.addGlobalMessage(error.message, 2)
                    }
                )
            }

        })).then(() => {

        }).finally(() => {

        })

    }
}



EngagementSubmitSDK.enableSumbitButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    var activeStatus = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.statecode).getValue()

    return (status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending) &&
        EAAShareSdk.getCurrentUserId() === requesterid &&
        activeStatus === EAAShareSdk.ActiveStatus.Active
}

