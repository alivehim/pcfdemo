window.EngagementContactSDK = window.EngagementContactSDK || {}


EngagementContactSDK.FormId = "eb79f38d-3fd7-ec11-a7b5-000d3a806eb6"

EngagementContactSDK.addContact = () => {
    var pageInput = {
        entityName: EAAShareSdk.Tables.contact,
        pageType: "entityrecord",
        formId: EngagementContactSDK.FormId,
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
            title: "Add Data"
        }).then(function success() {
            EngagementContactSDK.refreshGrid()
        }, function error() {

        })
}

EngagementContactSDK.onDelete = (selectedIds) => {

    var confirmStrings = { confirmButtonLabel: "Delete", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.DeleteContact, title: "Confirm Deletion" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                EngagementContactSDK.deleteRecords(selectedIds)
            } else {

            }
        }
    );

}

EngagementContactSDK.onEdit = (selectedIds) => {

    var pageInput = {
        entityName: EAAShareSdk.Tables.contact,
        pageType: "entityrecord",
        formId: EngagementContactSDK.FormId,
        entityId: selectedIds[0]
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Edit Data"
        }).then(function success() {
            EngagementContactSDK.refreshGrid()
        }, function error() {

        })

}

EngagementContactSDK.onFormLoad = () => {
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false)

    EngagementContactSDK.unlock()
}

EngagementContactSDK.unlock = () => {
    const unlockfileds = () => {
        for (let attr in Xrm.Page.data.entity.attributes._collection) {
            try {
                Xrm.Page.data.entity.attributes.getByName(attr).controls.getAll()[0].setDisabled(false);
            }
            catch {
            }
        }
    }

    if (EAAShareSdk.IsAdmin()) {
        unlockfileds()
    }
}

EngagementContactSDK.onFirstNameChanged = () => {
    var first_name = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ContactFields.first_name).getValue()
    if (first_name) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.ContactFields.aia_name).setValue(first_name)
    }
}

EngagementContactSDK.deleteRecords = (selectedIds) => {
    Xrm.Utility.showProgressIndicator();
    Promise.all(selectedIds.map(item => {
        return Xrm.WebApi.deleteRecord(EAAShareSdk.Tables.contact, item).then(
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
    })).then(() => {

    }).finally(() => {
        // refresh
        EngagementContactSDK.refreshGrid()
        Xrm.Utility.closeProgressIndicator()
    })
}

EngagementContactSDK.refreshGrid = () => {
    formContext = Xrm.Page.ui.formContext
    var subgrid = formContext.ui.controls.get(EAAShareSdk.SubGrids.Contact);
    subgrid.refresh();
}

EngagementContactSDK.enableAddButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

    return ((status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending)
        && EAAShareSdk.getCurrentUserId() === requesterid) ||
        (EAAShareSdk.IsAdmin())
}

EngagementContactSDK.enableDeleteButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    return ((status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending) && EAAShareSdk.getCurrentUserId() === requesterid) ||
        (EAAShareSdk.IsAdmin())

}

EngagementContactSDK.enableEditButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

    return ((status === EAAShareSdk.FormStatus.Initiator_Pending || status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending)
        && EAAShareSdk.getCurrentUserId() === requesterid) ||
        (EAAShareSdk.IsAdmin())
}