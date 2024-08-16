window.EngagementLocalViewerSDK = window.EngagementLocalViewerSDK || {}

EngagementLocalViewerSDK.FormId = "33560eec-3ed7-ec11-a7b5-000d3a806eb6"


EngagementLocalViewerSDK.orginalLocalViewer = null

EngagementLocalViewerSDK.addLocalViewer = () => {

    var pageInput = {
        entityName: EAAShareSdk.Tables.local_viewer,
        pageType: "entityrecord",
        formId: EngagementLocalViewerSDK.FormId,
        createFromEntity: {
            entityType: EAAShareSdk.Tables.form,
            id: Xrm.Page.data.entity.getId(),
            name: Xrm.Page.data.entity.attributes.getByName("aia_name").getValue()
        },
        data: {

        }
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Add Local Viewer"
        }).then(function success() {

            EngagementLocalViewerSDK.refreshGrid()

        }, function error() {

        })
}

EngagementLocalViewerSDK.onFormLoad = () => {
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false)

    EngagementLocalViewerSDK.unlock()
}

EngagementLocalViewerSDK.onSubmit = () => {
    EngagementLocalViewerSDK.submitProcess()
}

EngagementLocalViewerSDK.submitProcess = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        formContext = Xrm.Page.ui.formContext

        if (EAAShareSdk.getFormId() && EAAShareSdk.IsAdmin()) {
            let localviewformId = EAAShareSdk.getFormId()
            let localviewer = await EngagementLocalViewerSDK.getLocalViewer(localviewformId)
            EngagementLocalViewerSDK.orginalLocalViewer = localviewer["_aia_eaa_recipient_value"]
        }

        await formContext.data.save()

        if (EAAShareSdk.IsAdmin()) {
            await EngagementLocalViewerSDK.changeAccess()
        }

        formContext.ui.close();
    }
    catch (err) {

        EAAShareSdk.formError(err)
    }
    finally {

        Xrm.Utility.closeProgressIndicator()
    }
}

EngagementLocalViewerSDK.unlock = () => {

    const unlockfileds = () => {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.LocalViewerFields.recipient).controls.getAll()[0].setDisabled(false);
    }

    if (EAAShareSdk.IsAdmin()) {
        unlockfileds()
    }
}

EngagementLocalViewerSDK.onLocalViewerChanged = () => {
    var localviewer = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.LocalViewerFields.recipient).getValue()
    if (localviewer) {
        var name = localviewer[0].name

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.LocalViewerFields.aia_name).setValue(name)


    }
}



EngagementLocalViewerSDK.changeAccess = async () => {

    let formid = EngagementLocalViewerSDK.getFormID()
    let form = await EAAShareSdk.getFormData(formid)
    let newLocalViewerId = EngagementLocalViewerSDK.getLocalViewerId()
    let status = form.aia_eaa_form_status
    if (status != EAAShareSdk.FormStatus.Initiator_Pending
        && status != EAAShareSdk.FormStatus.Initiator_ResubmissionPending
        && status != EAAShareSdk.FormStatus.Unpublished) {

        if (EngagementLocalViewerSDK.orginalLocalViewer != newLocalViewerId) {
            if (EngagementLocalViewerSDK.orginalLocalViewer) {
                await EAAShareSdk.revoke(formid, EngagementLocalViewerSDK.orginalLocalViewer)
            }
            await EAAShareSdk.share(formid, newLocalViewerId)
        }

    }

}

EngagementLocalViewerSDK.onDelete = (selectedIds) => {
    var confirmStrings = { confirmButtonLabel: "Delete", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.DeleteLocalViewer, title: "Confirm Deletion" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                EngagementLocalViewerSDK.deleteRecords(selectedIds)
            } else {

            }
        }
    );
}

EngagementLocalViewerSDK.onEdit = (selectedIds) => {
    var pageInput = {
        entityName: EAAShareSdk.Tables.local_viewer,
        pageType: "entityrecord",
        formId: EngagementLocalViewerSDK.FormId,
        entityId: selectedIds[0]
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Edit Local Viewer"
        }).then(function success() {

            EngagementLocalViewerSDK.refreshGrid()

        }, function error() {

        })
}

EngagementLocalViewerSDK.deleteRecords = (selectedIds) => {
    Xrm.Utility.showProgressIndicator();

    try {
        let formid = EAAShareSdk.getFormId()

        Promise.all(selectedIds.map(item => {

            return EngagementLocalViewerSDK.getLocalViewer(item).then(localviewer => {
                //revoke access 
                return EAAShareSdk.revoke(formid, localviewer["_aia_eaa_recipient_value"]).then(res => {

                    return Xrm.WebApi.deleteRecord(EAAShareSdk.Tables.local_viewer, item).then(
                        function success(result) {
                            return Promise.resolve()
                        },
                        function (error) {
                            console.log(error.message);
                            // handle error conditions
                            Xrm.UI.addGlobalNotification({
                                level: 2,
                                message: error.message,
                                type: 2,
                                showCloseButton: true
                            })
                        }
                    )

                })

            })

        }))
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
        EngagementLocalViewerSDK.refreshGrid()
    }

}
EngagementLocalViewerSDK.getLocalViewer = (localviewerid) => {
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.local_viewer}s?$filter=aia_eaa_local_viewerid eq '${localviewerid}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        localviewer = res.value[0];

        return Promise.resolve(localviewer)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EngagementLocalViewerSDK.getLocalViewerId = () => {
    let recipient = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.LocalViewerFields.recipient).getValue()
    if (recipient) {
        var replaceSymbol = /(\w*){(.*)}(.*)/g
        let localviewerid = recipient[0].id.replace(replaceSymbol, "$1$2")
        return localviewerid
    }
    return null
}

EngagementLocalViewerSDK.getFormID = () => {
    let form = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.LocalViewerFields.eaa_form).getValue()
    var replaceSymbol = /(\w*){(.*)}(.*)/g
    let formid = form[0].id.replace(replaceSymbol, "$1$2")
    return formid
}

EngagementLocalViewerSDK.refreshGrid = () => {
    formContext = Xrm.Page.ui.formContext
    var subgrid = formContext.ui.controls.get(EAAShareSdk.SubGrids.LocalViewer);
    subgrid.refresh();
}

EngagementLocalViewerSDK.enableSubmitButton = () => {
    return true
}

EngagementLocalViewerSDK.enableAddButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

    return ((status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending)
        && EAAShareSdk.getCurrentUserId() === requesterid) ||
        (EAAShareSdk.IsAdmin())
}

EngagementLocalViewerSDK.enableDeleteButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    return ((status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending) && EAAShareSdk.getCurrentUserId() === requesterid) ||
        (EAAShareSdk.IsAdmin())
}

EngagementLocalViewerSDK.enableEditButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

    return ((status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending)
        && EAAShareSdk.getCurrentUserId() === requesterid) ||
        (EAAShareSdk.IsAdmin())
}