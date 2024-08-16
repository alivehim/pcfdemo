
window.EngagementReopenSDK = window.EngagementReopenSDK || {}

EngagementReopenSDK.onReopen = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requestType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()

    if (EAAShareSdk.IsAdmin() && (status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval || status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval)) {
        EAAShareSdk.onBehalfOfApproval(status, requestType, EAAShareSdk.ApprovalStatus.ResubmissionPending)
    } else {
        var confirmationMsg = `Confirm to send back to Requester?`
        var confirmStrings = { confirmButtonLabel: "Confirm", text: confirmationMsg, title: "Confirmation" };
        var confirmOptions = { height: 200, width: 450 };
        Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {
                    EngagementReopenSDK.reopenProcess(status)
                }
            }
        )
    }
}

EngagementReopenSDK.reopenProcess = async (status) => {
    //change the status
    var requestGuid = EAAShareSdk.getFormId()
    var currentUserId = EAAShareSdk.getCurrentUserId()
    var currentUserName = EAAShareSdk.getCurrentUserName()
    var stage = EAAShareSdk.getReopenStage(status)
    Xrm.Utility.showProgressIndicator("Loading")

    //set aia_sys_processlock with "true"
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_sys_processlock).setValue(true)
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.latest_modified_on).setValue(new Date())
    EngagementReopenSDK.setApprover(status, currentUserId, currentUserName)

    try {
        await Xrm.Page.data.save()
        await EAAShareSdk.Reopen(requestGuid, currentUserId, stage)
        await EngagementReopenSDK.shareAccess(requestGuid, status)
        var title = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_name).getValue()
        await EAAShareSdk.popupSuccess(title, EAAShareSdk.MessageType.Reopen)
    }
    catch (err) {
        EAAShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

EngagementReopenSDK.shareAccess = async (requestGuid, status) => {
    try {
        if (status === EAAShareSdk.FormStatus.Unpublished) {
            let localapprover = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.local_approver).getValue()
            if (localapprover) {
                let localapproverid = localapprover[0].id.replace(/[{}]*/g, "").toLowerCase()
                EAAShareSdk.share(requestGuid, localapproverid)
            }

            //localviewer
            const localviewer = EAAShareSdk.getLocalViewers(requestGuid)
            localviewer.forEach(p => {
                EAAShareSdk.share(requestGuid, p)
            })

            //country admin
            let country = EAAShareSdk.getCountries()
            if (country) {
                let teamid = await EAAShareSdk.getTeamIdByCountry(country.id)
                if (teamid) {
                    await EAAShareSdk.shareToTeam(requestGuid, teamid)
                }
            }

            //gfc
            EngagementReopenSDK.shareTeamIfapproverExists(requestGuid, [EAAShareSdk.FormFields.finance_manager],
                EAAShareSdk.Stages.GroupFinanceCoordinator)

            //head
            EngagementReopenSDK.shareTeamIfapproverExists(requestGuid, [EAAShareSdk.FormFields.head, EAAShareSdk.FormFields.on_behalf_head],
                EAAShareSdk.Stages.HeadofGroupTax)

            //cfo
            EngagementReopenSDK.shareTeamIfapproverExists(requestGuid, [EAAShareSdk.FormFields.cfo, EAAShareSdk.FormFields.on_behalf_rcfo],
                EAAShareSdk.Stages.RegionalCFO)

        }
    }
    catch (err) {
        console.error(err.message)
    }

}

EngagementReopenSDK.shareTeamIfapproverExists = (requestGuid, fieldNames, stage) => {

    let exists = false
    fieldNames.forEach(element => {

        let ctl = Xrm.Page.data.entity.attributes.getByName(element).getValue()
        if (ctl) {
            exists = true
        }
    });

    if (!exists) {
        return;
    }
    let teamid = EAAShareSdk.getTeamIdByStage(stage)
    EAAShareSdk.shareToTeam(requestGuid, teamid)

}



EngagementReopenSDK.setApprover = (status, currentUserId, currentUserName) => {
    switch (status) {
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.finance_manager)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.finance_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.head)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.head_date).setValue(new Date())
            break;
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            EAAShareSdk.fillApprover(currentUserId, currentUserName, EAAShareSdk.FormFields.cfo)
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.cfo_date).setValue(new Date())
            break;
    }
}


EngagementReopenSDK.enableReOpenButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    if (status && (
        status === EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval ||
        status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval ||
        status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval ||
        status === EAAShareSdk.FormStatus.Unpublished)) {

        var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()
        var currentuserid = EAAShareSdk.getCurrentUserId();

        if (EAAShareSdk.isUserInTeam(ownerid, currentuserid)) {
            return true
        }

        if (EAAShareSdk.IsAdmin()) {
            return status === EAAShareSdk.FormStatus.Unpublished || status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval || status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval
        }

        return false

    } else {
        return false;
    }
}