window.EngagementCommentSDK = window.EngagementCommentSDK || {}

EngagementCommentSDK.FormId = "4475101c-46d7-ec11-a7b5-000d3a806eb6"


EngagementCommentSDK.onFormLoad = () => {
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false);
}
EngagementCommentSDK.addComment = () => {

    var currentUserId = EAAShareSdk.getCurrentUserId()
    var formId = Xrm.Page.data.entity.getId()

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    let data = {
        "aia_user": currentUserId,
        "aia_sys_eaa_form": formId,
        "aia_name": EAAShareSdk.getCurrentUserName(),
    }

    if (status === EAAShareSdk.FormStatus.Unpublished && EAAShareSdk.IsAdmin()) {
        data.aia_action_by_admin = true
    }

    EngagementCommentSDK.popup(data)


}



EngagementCommentSDK.onSubmitPost = () => {
    formContext = Xrm.Page.ui.formContext
    //fill action on field
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.CommentFields.eaa_action_on).setValue(new Date());

    formContext.data.save()
        .then(() => {
            formContext.ui.close();
        }, () => { });

}

EngagementCommentSDK.addReply = () => {

    var currentUserId = EAAShareSdk.getCurrentUserId()
    var rows = Xrm.Page.getControl(EAAShareSdk.SubGrids.Comment).getGrid().getSelectedRows();
    var replyto = null
    var formId = Xrm.Page.data.entity.getId()
    rows.forEach(function (row, i) {
        // replyto = row.getData().getEntity().attributes.get(EAAShareSdk.CommentFields.user).getValue();
        replyto = row.getData().getEntity().getId()
    });

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    let data = {
        aia_user: currentUserId,
        aia_reply_to: replyto,
        "aia_sys_eaa_form": formId,
        "aia_name": EAAShareSdk.getCurrentUserName(),
    }

    if (status === EAAShareSdk.FormStatus.Unpublished && EAAShareSdk.IsAdmin()) {
        data.aia_action_by_admin = true
    }

    EngagementCommentSDK.popup(data);
}

EngagementCommentSDK.onDelete = (selectedItems) => {
    var confirmStrings = { confirmButtonLabel: "Confirm to delete", cancelButtonLabel: "Cancel", text: EAAShareSdk.ConfirmationMessage.DeleteComment, title: "Confirmation" };
    var confirmOptions = { height: 200, width: 450 };
    Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
        function (success) {
            if (success.confirmed) {
                EngagementCommentSDK.deleteComments(selectedItems)
            }
        }
    );
}
EngagementCommentSDK.popup = (data) => {
    var pageInput = {
        entityName: EAAShareSdk.Tables.comment,
        pageType: "entityrecord",
        formId: EngagementCommentSDK.FormId,
        data: data
    };

    Xrm.Navigation.navigateTo(pageInput,
        {
            target: 2,
            position: 1,
            height: { value: 80, unit: "%" },
            width: { value: 70, unit: "%" },
            title: "Add Data"
        }).then(function success() {

            EngagementCommentSDK.refresh()

        }, function error() {

        })
}

EngagementCommentSDK.deleteComments = (selectedItems) => {

    Xrm.Utility.showProgressIndicator("Loading")
    Promise.all(selectedItems.map(item => {
        return Xrm.WebApi.deleteRecord(EAAShareSdk.Tables.comment, item.Id).then(
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
        EngagementCommentSDK.refresh()
        Xrm.Utility.closeProgressIndicator()
    })
}

EngagementCommentSDK.refresh = () => {
    formContext = Xrm.Page.ui.formContext
    var subgrid = formContext.ui.controls.get(EAAShareSdk.SubGrids.Comment);
    subgrid.refresh();
}
/**
 * 1 .Anyone in the approval flow can leave and reply comments  after Requester’s submission, excluding the “Actual Fee Update - Completed” stage.
 * 2. If users can view the request, they can view the “Comments” page.
 * @returns 
 */
EngagementCommentSDK.enableAddButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    if (status && !EngagementCommentSDK.statusCheck(status)) {

        if (status != EAAShareSdk.FormStatus.Unpublished) {
            var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
            var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()
            var formId = EAAShareSdk.getFormId()
            var currentuserid = EAAShareSdk.getCurrentUserId()
            var localViewers = EAAShareSdk.getLocalViewers(formId)


            var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
            var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

            return currentuserid == requesterid || EAAShareSdk.IsApprover(currentuserid) 
            || ownerid === currentuserid || localViewers.includes(currentuserid) || EAAShareSdk.isUserInTeam(ownerid, currentuserid) || EAAShareSdk.IsAdmin()
        } else {
            return EAAShareSdk.IsAdmin()
        }
    } else {
        return false;
    }
}

EngagementCommentSDK.statusCheck = (status) => {
    return [EAAShareSdk.FormStatus.Initiator_Pending,
    EAAShareSdk.FormStatus.LocalApprover_Rejected,
    EAAShareSdk.FormStatus.GroupFinanceCoordinator_Rejected,
    EAAShareSdk.FormStatus.HeadOfGroupTax_Rejected,
    EAAShareSdk.FormStatus.RegionalCFO_Rejected,
    EAAShareSdk.FormStatus.ActualFeeUpdate_Completed].includes(status)
}
EngagementCommentSDK.enableReplyButton = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    if (status && !EngagementCommentSDK.statusCheck(status)) {

        if (status != EAAShareSdk.FormStatus.Unpublished) {

            var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
            var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()
            var formId = EAAShareSdk.getFormId()
            var currentuserid = EAAShareSdk.getCurrentUserId()
            var localViewers = EAAShareSdk.getLocalViewers(formId)


            var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
            var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

            return currentuserid == requesterid || EAAShareSdk.IsApprover(currentuserid) 
            || ownerid === currentuserid || localViewers.includes(currentuserid) || EAAShareSdk.isUserInTeam(ownerid, currentuserid) || EAAShareSdk.IsAdmin()

        } else {
            return EAAShareSdk.IsAdmin()
        }

    } else {
        return false;
    }
}

EngagementCommentSDK.enableDeleteButton = (selectedItems) => {

    // const formId = EAAShareSdk.getFormId()
    // const currentUserId = EAAShareSdk.getCurrentUserId()
    // // selectedItems.some(p=>)
    // const comments = EAAShareSdk.getCommentsByUser(formId, currentUserId)

    // let result = true
    // selectedItems.forEach(x => {
    //     if (!comments.some(y => y === x.Id)) {
    //         result = false
    //     }
    // })
    // return result
    return false


}