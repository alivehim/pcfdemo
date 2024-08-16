window.EngagementChangeLogSDK = window.EngagementChangeLogSDK || {}

EngagementChangeLogSDK.onChangeLog = () => {
    var formId = EAAShareSdk.getFormId()
    Xrm.Navigation.navigateTo({
        pageType: "webresource",
        webresourceName: "aia_eaa_engagement_changelog.html",
        data: `formId=${formId}`
    }, {
        position: 1,
        target: 2,
        height: { value: 80, unit: "%" },
        width: { value: 70, unit: "%" },
        title: "Change Log"
    })
}

EngagementChangeLogSDK.enableChangeLogButton = () => {

    if (!EAAShareSdk.IsEngagementForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    if (status && status != EAAShareSdk.FormStatus.Initiator_Pending) {

        // `var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
        // var ownerid = owner[0].id.replace(/[{}]*/g, "").toLowerCase()

        // var currentuserid = EAAShareSdk.getCurrentUserId();

        // if (status === EAAShareSdk.FormStatus.PendingLocalApproverApproval) {
        //     //check the localapprovel

        //     return ownerid === currentuserid
        // }
        // else {
        //     var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
        //     var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()

        //     return currentuserid == requesterid || EAAShareSdk.isUserInTeam(ownerid, currentuserid)
        // }

        return true;

    } else {
        return false;
    }
}