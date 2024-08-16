var PROXCardReqReassignFormSDK = window.PROXCardReqReassignFormSDK || {};

PROXCardReqReassignFormSDK.oldApproverId = null
PROXCardReqReassignFormSDK.onFormLoaded = () => {


    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === ShareSdk.PROXCardReqStatus.WaitingforApproval) {

        const approver = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).getValue()
        PROXCardReqReassignFormSDK.oldApproverId = approver[0].id
    }

    if (status === ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval) {
        const approver = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosen_mm).getValue()
        PROXCardReqReassignFormSDK.oldApproverId = approver[0].id
    }
}

PROXCardReqReassignFormSDK.onApporverChanged = async () => {

    var approver = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).getValue()
    if (approver) {
        var approverid = ShareSdk.getLookupId(approver)
        var departmentBranch = await ShareSdk.getDeptBranchAsync(approverid)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.approver_deptbranch).setValue(departmentBranch || '')
    } else {
        //clear Dept./Branch 部門/支部
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.approver_deptbranch).setValue()
    }

}

PROXCardReqReassignFormSDK.onSeniorCivilMgrChanged = async () => {

    var mm = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosen_mm).getValue()
    if (mm) {
        var mmid = ShareSdk.getLookupId(mm)
        var departmentBranch = await ShareSdk.getDeptBranchAsync(mmid)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_dept_branch).setValue(departmentBranch || '')
    } else {
        //clear Dept./Branch 部門/支部
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_dept_branch).setValue()
    }

}

PROXCardReqReassignFormSDK.submit = async () => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

        var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
        if (status === ShareSdk.PROXCardReqStatus.WaitingforApproval) {

            const approver = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).getValue()
            if (PROXCardReqReassignFormSDK.oldApproverId === approver[0].id) {
                throw new Error('Same as the original assigned staff.  Please select another staff.')
            }

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.approver_reassign_time).setValue(new Date())
        }

        if (status === ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval) {
            const approver = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosen_mm).getValue()
            if (PROXCardReqReassignFormSDK.oldApproverId === approver[0].id) {
                throw new Error('Same as the original assigned staff.  Please select another staff.')
            }

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_reassign_time).setValue(new Date())
        }

        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).setValue(true)
        await Xrm.Page.data.save()

        formContext = Xrm.Page.ui.formContext
        formContext.ui.close()

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }


}

PROXCardReqReassignFormSDK.submitButtonEnable = () => {
    return ShareSdk.IsProxRequestReassginForm()
}