var PROXCardReqResubmitSDK = window.PROXCardReqResubmitSDK || {};

PROXCardReqResubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        if (Xrm.Page.data.isValid()) {
            await window.parent.PROXCardReqValidatinoSDK.submissionValidateAsync()

            // const sslist = await window.parent.PROXCardReqValidatinoSDK.getSSListAsync()
            // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_list).setValue(sslist)

            var errormsgs = await window.parent.PROXCardReqValidatinoSDK.checkErrorDetailsAsync()
            if (errormsgs.length > 0) {

                errormsgs= Array.from(new Set(errormsgs))
                for (var item of errormsgs) {
                    ShareSdk.formError(new Error(item))
                    return
                }
            }

            var warnningMessages = await window.parent.PROXCardReqValidatinoSDK.checkWarnningDetailsAsync()

            if (warnningMessages.length > 0) {

                let message = warnningMessages.join(',')
                var confirmStrings = { confirmButtonLabel: "Ok", cancelButtonLabel: "Cancel", text: message, title: "Message" };
                var confirmOptions = { height: 200, width: 450 };
                Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                    function (success) {
                        if (success.confirmed) {

                            PROXCardReqResubmitSDK.resubmitAsync()
                        }
                    }
                );

            } else {
                await PROXCardReqResubmitSDK.resubmitAsync()
            }


        }

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}
PROXCardReqResubmitSDK.resubmitAsync = async () => {

    await window.parent.PROXCardReqValidatinoSDK.substationProcessAsync()

    await window.parent.PROXCardReqValidatinoSDK.detailsDataProcessAsync()

    window.parent.PROXCardReqValidatinoSDK.generateExpiryWeekdayDate()
    
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforApproval)
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).setValue(true)
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.requestdate).setValue(new Date())
    PROXCardReqResubmitSDK.reset()
    await Xrm.Page.data.save()

    Xrm.Page.ui.refresh()
}

PROXCardReqResubmitSDK.reset = () => {

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.approver).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.appr_date).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.appr_remarks).setValue()

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.reviewer).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.reviewed_date).setValue()
    // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.reviewer_dept_branch).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.reviewer_remarks).setValue()

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_date).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_dept_branch).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_remarks).setValue()

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.admin).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.card_allocation_date).setValue()
    // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.admin_dept_branch).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.admin_remarks).setValue()

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.security).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.card_encoding_date).setValue()
    // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.security_dept_branch).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.security_remarks).setValue()

    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.rejectedby).setValue()
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.rejected_date).setValue()

}
PROXCardReqResubmitSDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var locked = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).getValue()
    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (!locked && status === ShareSdk.PROXCardReqStatus.RequestRejected) {
        return ShareSdk.isPROXCardRequestor()
    }
    return false
}