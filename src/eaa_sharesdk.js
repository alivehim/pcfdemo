window.EAAShareSdk = window.EAAShareSdk || {};
window.parent.EAAShareSdk = window.EAAShareSdk || {};

EAAShareSdk.import = () => { console.log('import EAAShareSdk') }

EAAShareSdk.Tables = {
    approval: "aia_eaa_approval",
    comment: "aia_eaa_comment",
    config: "aia_eaa_config",
    contact: "aia_eaa_contact",
    country: "aia_eaa_country",
    entity: "aia_eaa_entity",
    form: "aia_eaa_form",
    form_attachment: "aia_eaa_form_attachment",
    local_viewer: "aia_eaa_local_viewer",
    notification: "aia_eaa_notification",
    sla_calculation: "aia_eaa_sla_calculation",
    sla_priority: "aia_eaa_sla_priority",
    subcategory: "aia_eaa_subcategory",
    team_role: "aia_eaa_team_role",
    timeline: "aia_eaa_timeline",
    wf_template: "aia_eaa_wf_template",
    systemuser: "systemuser",
    team: "team",
    template: "template",
    transactioncurrency: "transactioncurrency",
}

EAAShareSdk.ActiveStatus = {
    Active: 0,
    Inactive: 1
}

EAAShareSdk.RequestType = {
    PwCTax: 589450000,
    PwcNon_Tax: 589450001,
    Non_PwcTax: 589450002,
}

EAAShareSdk.FormStatus = {
    Initiator_Pending: 589450000,
    PendingLocalApproverApproval: 589450001,
    PendingGroupFinanceCoordinatorApproval: 589450002,
    PendingHeadOfGroupTaxApproval: 589450004,
    PendingRegionalCFOApproval: 589450005,
    Initiator_ResubmissionPending: 589450006,
    LocalApprover_Rejected: 589450008,
    GroupFinanceCoordinator_Rejected: 589450009,
    HeadOfGroupTax_Rejected: 589450011,
    RegionalCFO_Rejected: 589450012,
    ActualFeeUpdate_Pending: 589450013,
    ActualFeeUpdate_Completed: 589450014,
    Unpublished: 589460001
}

EAAShareSdk.ApprovalStatus = {
    Approved: 589450000,
    Rejected: 589450001,
    Completed: 589450002,
    Pending: 589450003,
    ResubmissionPending: 589450004,
}

EAAShareSdk.Stages = {
    Initiator: 589450000,
    LocalApprover: 589450001,
    GroupFinanceCoordinator: 589450003,
    RegionalCFO: 589450005,
    HeadofGroupTax: 589450006,
    ActualFeeUpdate: 589450007,
}

EAAShareSdk.Category = {
    PwCAuditServices: 589450000,
    PwCAudit_relatedServices: 589450001,
    PwCTaxServices: 589450002,
    PwCAllOtherServices: 589450003,
}

EAAShareSdk.WorkflowStages = {
    New: 589450000,
    RequesterSubmit: 589450001,
    LocalApproverApprove: 589450002,
    GroupFinanceCoordinatorApprove: 589450003,
    HeadOfGroupTaxApprove: 589450005,
    RegionalCFOApprove: 589450006,
    LocalApproverReject: 589450007,
    GroupFinanceCoordinatorReject: 589450008,
    HeadOfGroupTaxReject: 589450010,
    RegionalCFOReject: 589450011,
    LocalApproverReopen: 589450012,
    GroupFinanceCoordinatorReopen: 589450013,
    HeadOfGroupTaxReopen: 589450015,
    RegionalCFOReopen: 589450016,
    UnpublishedReopen: 589450017,
    ActualFeeUpdatePending: 589450100,
    ActualFeeUpdateCompleted: 589450200,

}

EAAShareSdk.FormFields = {
    pocket: "aia_eaa_pocket",
    est_start_date: "aia_eaa_est_start_date",
    approver_date: "aia_eaa_approver_date",
    indrect_taxes_base: "aia_eaa_indrect_taxes_base",
    head_date: "aia_eaa_head_date",
    other_remarks: "aia_eaa_other_remarks",
    description_long: "aia_eaa_description_long",
    attachments: "aia_eaa_attachments",
    sub_cate_tax: "aia_eaa_sub_cate_tax",
    attachments_name: "aia_eaa_attachments_name",
    requester_date: "aia_eaa_requester_date",
    entity_sign: "aia_eaa_entity_sign",
    defined_policy: "aia_eaa_defined_policy",
    requester: "aia_eaa_requester",
    requester_name: "aia_requester_name",
    pocket_base: "aia_eaa_pocket_base",
    countries: "aia_eaa_countries",
    cost_arrangement: "aia_eaa_cost_arrangement",
    final_date: "aia_eaa_final_date",
    entity_fee: "aia_eaa_entity_fee",
    fee_included_audit_fee_template: "aia_eaa_fee_included_audit_fee_template",
    prof_firm: "aia_eaa_prof_firm",
    workflow_stage: "aia_eaa_workflow_stage",
    indrect_taxes: "aia_eaa_indrect_taxes",
    est_cost: "aia_eaa_est_cost",
    est_cost_base: "aia_eaa_est_cost_base",
    service_number: "aia_eaa_service_number",
    revised_date: "aia_eaa_revised_date",
    sla_reminder: "aia_eaa_sla_reminder",
    fee_basis: "aia_eaa_fee_basis",
    form_status: "aia_eaa_form_status",
    local_approver: "aia_eaa_local_approver",
    category: "aia_eaa_category",
    completed: "aia_eaa_completed",
    wf_request_guid: "aia_sys_wf_request_guid",
    est_duration: "aia_eaa_est_duration",
    approval_evidence_head_name: "aia_eaa_approval_evidence_head_name",
    state_not_included_audit_fee_template: "aia_eaa_state_not_included_audit_fee_template",
    aia_sys_processlock: "aia_sys_processlock",
    approval_status: "aia_eaa_approval_status",
    approval_evidence_head: "aia_eaa_approval_evidence_head",
    policy_why: "aia_eaa_policy_why",
    approval_evidence_cfo_name: "aia_eaa_approval_evidence_cfo_name",
    name_of_pro: "aia_eaa_name_of_pro",
    fully_budgeted: "aia_eaa_fully_budgeted",
    head: "aia_eaa_head",
    approval_evidence_cfo: "aia_eaa_approval_evidence_cfo",
    act_taxes: "aia_eaa_act_taxes",
    act_fee: "aia_eaa_act_fee",
    tax_date: "aia_eaa_tax_date",
    base_fee: "aia_eaa_base_fee",
    define_pwc: "aia_eaa_define_pwc",
    tax_manager: "aia_eaa_tax_manager",
    recurring: "aia_eaa_recurring",
    policy_read: "aia_eaa_policy_read",
    completion_date: "aia_eaa_completion_date",
    quarter: "aia_eaa_quarter",
    cfo_date: "aia_eaa_cfo_date",
    unpublish_user: "aia_eaa_unpublish_user",
    description_short: "aia_eaa_description_short",
    rationale_of_engaging_pwc: "aia_reason_for_engaging_pwc",
    act_pocket: "aia_eaa_act_pocket",
    act_fee_base: "aia_eaa_act_fee_base",
    cfo: "aia_eaa_cfo",
    aia_name: "aia_name",
    department: "aia_eaa_department",
    base_fee_base: "aia_eaa_base_fee_base",
    on_behalf_rcfo: "aia_eaa_on_behalf_rcfo",
    final_approver: "aia_eaa_final_approver",
    est_end_date: "aia_eaa_est_end_date",
    year: "aia_eaa_year",
    act_pocket_base: "aia_eaa_act_pocket_base",
    categoryname: "aia_eaa_categoryname",
    sub_cate_nontax: "aia_eaa_sub_cate_nontax",
    finance_date: "aia_eaa_finance_date",
    act_taxes_base: "aia_eaa_act_taxes_base",
    finance_manager: "aia_eaa_finance_manager",
    sys_form_type: "aia_sys_form_type",
    unpublish_date: "aia_eaa_unpublish_date",
    sys_active: "aia_sys_active",

    //Local Approver Submit Date
    approver_date: "aia_eaa_approver_date",
    stage: "aia_stage",

    currency: "transactioncurrencyid",
    act_currency: "aia_eaa_act_currency",
    act_cost: "aia_eaa_act_cost",

    usd_rate: "aia_eaa_usd_rate",
    usd_est_base_fee: "aia_eaa_usd_est_base_fee",
    usd_est_indrect_taxes: "aia_eaa_usd_est_indrect_taxes",
    usd_est_pocket: "aia_eaa_usd_est_pocket",
    usd_est_cost: "aia_eaa_usd_est_cost",

    usd_act_fee: "aia_eaa_usd_act_fee",
    usd_act_taxes: "aia_eaa_usd_act_taxes",
    usd_act_pocket: "aia_eaa_usd_act_pocket",
    usd_act_cost: "aia_eaa_usd_act_cost",

    state_why_permissible: "aia_eaa_state_why_permissible",
    reason_for_engaging_pwc: "aia_reason_for_engaging_pwc",
    cost_arrangement_detail: "aia_eaa_cost_arrangement_detail",
    fee_other_remarks: "aia_eaa_fee_other_remarks",
    why_not_included_audit_fee_template: "aia_eaa_why_not_included_audit_fee_template",
    reason_for_pro_firm: "aia_eaa_reason_for_pro_firm",

    report_year: "aia_eaa_report_year",
    est_usd_rate: "aia_eaa_est_usd_rate",
    statecode: "statecode",

    unpublish_rationale: "aia_eaa_unpublish_rationale",
    reference_createdon: "aia_reference_createdon",
    latest_modified_on: "aia_eaa_latest_modified_on",

    on_behalf_rcfo_submit_date: "aia_on_behalf_rcfo_submit_date",
    on_behalf_head: "aia_eaa_on_behalf_head",
    on_behalf_head_submit_date: "aia_eaa_on_behalf_head_submit_date"

}

EAAShareSdk.ApprovalFields = {
    next_form_status: "aia_eaa_next_form_status",
    approver: "aia_eaa_approver",
    workflow_stage: "aia_eaa_workflow_stage",
    on_behalf: "aia_on_behalf",
    context: "aia_eaa_context",
    return_reason: "aia_eaa_return_reason",
    my_action: "aia_eaa_my_action",
    eaa_form: "aia_sys_eaa_form",
    reject_reason: "aia_eaa_reject_reason",
    action_reason: "aia_eaa_action_reason",
    aia_name: "aia_name",
    action_on: "aia_eaa_action_on",
    comments: "aia_eaa_comments",
    timelineid: "aia_eaa_timelineid"
}

EAAShareSdk.CommentFields = {
    eaa_line_id: "aia_eaa_line_id",
    user: "aia_user",
    reply_to: "aia_reply_to",
    sys_eaa_form: "aia_sys_eaa_form",
    aia_name: "aia_name",
    eaa_action_on: "aia_eaa_action_on",

}

EAAShareSdk.LocalViewerFields = {
    eaa_form: "aia_sys_eaa_form",
    aia_name: "aia_name",
    recipient: "aia_eaa_recipient",
}

EAAShareSdk.ContactFields = {
    first_name: "aia_eaa_first_name",
    email_address: "aia_email_address",
    last_name: "aia_eaa_last_name",
    eaa_form: "aia_sys_eaa_form",
    phone: "aia_eaa_phone",
    job_title: "aia_eaa_job_title",
    aia_name: "aia_name",

}

EAAShareSdk.Category = {
    PwCAuditServices: 589450000,
    PwCAudit_relatedServices: 589450001,
    PwCTaxServices: 589450002,
    PwCAllOtherServices: 589450003,
    Non_PwCTaxServices: 589450004,
}

EAAShareSdk.Teams = {
    EAA_Head_Of_Group_Tax: "EAA_Head_Of_Group_Tax",
    EAA_Regional_CFO: "EAA_Regional_CFO",
    EAA_Group_Finance_Coordinator: "EAA_Group_Finance_Coordinator"
}

EAAShareSdk.AttachmentType = {
    SupportingDoc: 589450000,
    ApprovalEvidence: 589450001,
}

EAAShareSdk.Quarters = {
    "1Q": 589450000,
    "2Q": 589450001,
    "3Q": 589450002,
    "4Q": 589450003,
}

EAAShareSdk.QuarterYears = {
    "2017": 2017,
    "2018": 2018,
    "2019": 2019,
    "2020": 2020,
    "2021": 2021,
    "2022": 2022,
    "2023": 2023,
    "2024": 2024,
    "2025": 2025,
    "2026": 2026,
    "2027": 2027,
    "2028": 2028,
    "2029": 2029,
    "2030": 2030,
    "2031": 2031,
    "2032": 2032,
    "2033": 2033,
    "2034": 2034,
    "2035": 2035,
    "2036": 2036,
    "2037": 2037,
    "2038": 2038,
    "2039": 2039,
    "2040": 2040,
}

EAAShareSdk.Consts = {
    EAA_Workflow_Template: "EAA_Workflow_Template"
}

EAAShareSdk.Forms = {
    "EAA_Engagement": "7e7ac2ed-c1d1-ec11-a7b5-000d3a80b291",
    EAA_Approve_On_Behalf: "9669e741-4fd7-ec11-a7b5-000d3a806eb6",
    ChangeInitator: "25de5984-f9e6-ec11-bb3d-002248177d91",
    Unpublish: "4fecef90-f3ea-ec11-bb3d-002248177d91",
    Evidence: "9cc5efba-d4f2-ec11-bb3d-002248177d91"
}


EAAShareSdk.CommentFormId = "4475101c-46d7-ec11-a7b5-000d3a806eb6"

EAAShareSdk.ConfirmationMessage = {
    Cancel: "Cancel and leave this form? Recent data will not be saved. If data is still needed, please return and save. ",
    Delete: "Confirm to delete?",
    Submit: "Confirm to submit?",
    UpdateActualFee: "Confirm to submit the actual fee information?",
    DeleteContact: "Do you want to delete this contact? You can't undo this action.",
    DeleteLocalViewer: "Do you want to delete this local viewer? You can't undo this action.",
    ChangeInitator: "Confirm to change the Requester?",
    Unpublish: "Confirm to unpublish the request?",
    DeleteComment: "Do you want to delete this comment? You can't undo this action."
}

EAAShareSdk.Views = {
    MyRequest: "973ca551-84d5-ec11-a7b5-000d3a806eb6",
    All: "7e7ac2ed-c1d1-ec11-a7b5-000d3a80b291",
    AwaitingMyResponse: "a0bb4621-8fd5-ec11-a7b5-000d3a806eb6"
}

EAAShareSdk.Areas = {
    eaa_all_request: "eaa_all_request",
    eaa_my_request: "eaa_my_request",
    eaa_awaiting_my_response: "eaa_awaiting_my_response",
    eaa_change_initator: "eaa_change_initator",
    eaa_unpublished_content: "eaa_unpublished_content",
    eaa_countries: "eaa_countries"
}

EAAShareSdk.SecurityRoles = {
    EAA_System_Admin: "EAA_System_Admin",
    BasicUser: "EAA_Basic_User",
    EAA_Business_Admin: "EAA_Business_Admin",
    EAA_Country_Admin: "EAA_Country_Admin",
    EAA_Group_Finance_Coordinator: "EAA_Group_Finance_Coordinator",
    EAA_Group_Tax_Viewer: "EAA_Group_Tax_Viewer"
}

EAAShareSdk.RolesName = {
    Requester: "Requester",
    LocalApprover: "Local Approver",
    GroupFinanceCoordinator: "Group Finance Coordinator",
    HeadofGroupTax: "Head of Group Tax",
    RegionalCFO: "Regional CFO"
}

EAAShareSdk.SubGrids = {
    LocalViewer: "subgrid_localviewer",
    Contact: "subgrid_contact",
    Comment: "subgrid_comment"
}

EAAShareSdk.MessageUniqueIdMap = {
    EstimatedEndDate: "EstimatedEndDateltToday",
    EstimatedDateComparison: "EstimatedDateComparison",
    AFXNumberRequired: "AFXNumberRequired",
    AFXNumberDigitalFormat: "AFXNumberDigitalFormat",
    AFXNumberLengthCheck: "AFXNumberDigitalFormat",
    RevisedEstimatedEndDate: "RevisedEstimatedEndDate",
    SetReminderOn: "SetReminderOn",
    BaseFeeLimitation: "BaseFeeLimitation"
}

EAAShareSdk.ErrorMessages = {
    EstimatedEndDate: "Invalid date. Please enter date that greater than or equal to today",
    EstimatedDateComparison: "Estimated End Date should greater than Estimated Start Date",
    AFXNumberRequired: "Please make sure you have input the “AFS number” under “Service Details” tab. Click to view.",
    AFXNumberDigitalFormat: "6 digits only",
    AFXNumberLengthCheck: "6 digits only",
    RevisedEstimatedEndDate: "Invalid date. Please enter date that greater than or equal to today",
    SetReminderOn: 'Invalid Reminder Date (i.e. Set up 1 day and 15 day will be "1,15")',
    BaseFeeLimitation: "Base fee cannot be zero"
}

EAAShareSdk.RandomLetters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ'
EAAShareSdk.RandomDigits = '0123456789'
EAAShareSdk.EmptyLookupId = '00000000-0000-0000-0000-000000000000'

EAAShareSdk.ConfigurationKeys = {
    FXURL: "EAA:FX:URL",
    FormHelperURL: "EAA:FormHelper:URL"
}

EAAShareSdk.MessageType = {
    Sumbit: "1",
    Approve: "2",
    Reject: "3",
    Reopen: "4",
    ChangeRequester: "5"
}

/**
 * 
 * @param {*} message 
 * @param {*} level  2 error 3 warnning 4 information
 */
EAAShareSdk.addGlobalMessage = (message, level = 4) => {
    //show banner
    var notification =
    {
        showCloseButton: true,
        type: 2,
        level: level, //information
        message: message,
    }

    Xrm.App.addGlobalNotification(notification).then(
        function success(result) {
            // Wait for 10 seconds and then clear the notification
            window.setTimeout(function () {
                Xrm.App.clearGlobalNotification(result);
            }, 10000);
        },
        function (error) {
            console.log(error.message);
        }
    );
}

EAAShareSdk.setFormError = (message, msgId) => {
    let msgBody = {
        hasId: msgId ? true : false,
        msgId: msgId || new Date().getTime().toString(16),
        message: message
    }
    Xrm.Page.ui.formContext.ui.setFormNotification(msgBody.message, 'ERROR', msgBody.msgId);
    if (!msgBody.hasId) {
        setTimeout(() => Xrm.Page.ui.formContext.ui.clearFormNotification(msgBody.msgId), 10000)
    }
}

EAAShareSdk.getReopenStage = (status) => {
    switch (status) {
        case EAAShareSdk.FormStatus.PendingLocalApproverApproval:
            return EAAShareSdk.WorkflowStages.LocalApproverReopen
        case EAAShareSdk.FormStatus.PendingGroupFinanceCoordinatorApproval:
            return EAAShareSdk.WorkflowStages.GroupFinanceCoordinatorReopen
        case EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval:
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxReopen
        case EAAShareSdk.FormStatus.PendingRegionalCFOApproval:
            return EAAShareSdk.WorkflowStages.RegionalCFOReopen
        case EAAShareSdk.FormStatus.Unpublished:
            return EAAShareSdk.WorkflowStages.UnpublishedReopen
    }
}

EAAShareSdk.getReopenStageByFormStage = (stage) => {
    switch (stage) {
        case EAAShareSdk.Stages.LocalApprover:
            return EAAShareSdk.WorkflowStages.LocalApproverReopen
        case EAAShareSdk.Stages.GroupFinanceCoordinator:
            return EAAShareSdk.WorkflowStages.GroupFinanceCoordinatorReopen
        case EAAShareSdk.Stages.HeadofGroupTax:
            return EAAShareSdk.WorkflowStages.HeadOfGroupTaxReopen
        case EAAShareSdk.Stages.RegionalCFO:
            return EAAShareSdk.WorkflowStages.RegionalCFOReopen
    }
}
EAAShareSdk.getFormId = () => {
    var replaceSymbol = /(\w*){(.*)}(.*)/g
    return Xrm.Page.data.entity.getId().replace(replaceSymbol, "$1$2");
}

EAAShareSdk.getCurrentUserId = () => {
    return Xrm._globalContext.userSettings.userId.replace(/[{}]*/g, "").toLowerCase()
}

EAAShareSdk.getCurrentUserName = () => {
    return Xrm._globalContext.userSettings.userName
}

EAAShareSdk.getFormRequestType = () => {
    return Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.aia_sys_form_type).getValue()
}

EAAShareSdk.getOwner = () => {
    let owner = Xrm.Page.data.entity.attributes.getByName("ownerid").getValue()
    let id = owner[0].id.replace(/[{}]*/g, "").toLowerCase()
    return { type: owner[0].entityType, id: id }
}

EAAShareSdk.getFormStatus = () => {
    return Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
}

EAAShareSdk.IsAdmin = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return [EAAShareSdk.SecurityRoles.EAA_Business_Admin, EAAShareSdk.SecurityRoles.EAA_System_Admin].some(p => securityRoles.includes(p))
    // return securityRoles.includes(EAAShareSdk.SecurityRoles.EAA_Business_Admin)
}

EAAShareSdk.IsCountryAdmin = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return securityRoles.includes(EAAShareSdk.SecurityRoles.EAA_Country_Admin)
}


EAAShareSdk.IsEngagementForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId()
    return formId === EAAShareSdk.Forms.EAA_Engagement
}

EAAShareSdk.IsChangeInitatorForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId()
    return formId === EAAShareSdk.Forms.ChangeInitator
}

EAAShareSdk.IsUnpublishForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId()
    return formId === EAAShareSdk.Forms.Unpublish
}

EAAShareSdk.IsInRoles = (roles) => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return roles.some(p => securityRoles.includes(p))
}

EAAShareSdk.IsCurrentUserCanUpdateActualFee = () => {
    var currentuserid = EAAShareSdk.getCurrentUserId();
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    return currentuserid === requesterid || EAAShareSdk.IsLocalViewer() || EAAShareSdk.IsAdmin()
}

EAAShareSdk.IsLocalViewer = () => {
    var currentId = EAAShareSdk.getCurrentUserId()
    var formId = EAAShareSdk.getFormId();
    const xhr = new XMLHttpRequest()
    try {
        xhr.open("GET",
            `/api/data/v9.0/${EAAShareSdk.Tables.local_viewer}s?$filter= _aia_sys_eaa_form_value eq '${formId}' and aia_eaa_recipient/systemuserid eq ${currentId}`,
            false);

        var count = 0
        xhr.send()
        if (xhr.status === 200) {
            const resData = JSON.parse(xhr.responseText)
            count = resData.value.length
        }
        return count > 0
    }
    catch (err) {
        console.log('error in : IsLocalViewer', err);
    }

}

EAAShareSdk.IsFormAtFinalStage = (formRequstType, status) => {
    return (status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval && formRequstType === EAAShareSdk.RequestType.Non_PwcTax) ||
        (status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval)
}

EAAShareSdk.changeFormActiveStatus = (formId, status) => {
    var body = {
        "aia_sys_active": status,
        "aia_eaa_latest_modified_on": new Date()
    }

    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${formId})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    )
}

EAAShareSdk.changeFormProcessLock = (formId, lock) => {
    var body = {
        "aia_sys_processlock": lock,
        "aia_eaa_latest_modified_on": new Date()
    }

    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${formId})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    )
}

EAAShareSdk.changeInitator = (formId, currentUserId) => {
    var body = {
        "aia_eaa_requester@odata.bind": `/systemusers(${currentUserId})`,
        "aia_eaa_latest_modified_on": new Date()
    }

    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${formId})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    )
}

EAAShareSdk.changeFormWorkflowStage = (formId, WorkflowStage) => {
    var body = {
        "aia_eaa_workflow_stage": WorkflowStage,
        "aia_sys_processlock": true,
        "aia_eaa_latest_modified_on": new Date()
    }

    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${formId})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    )
}

EAAShareSdk.Approve = (requstGuid, approverId, nextStage) => {

    let owner = EAAShareSdk.getOwner()

    if (owner.type === 'team') {
        return EAAShareSdk.shareToTeam(requstGuid, owner.id).then(res => {
            return EAAShareSdk.changeFormProcessLock(requstGuid, true).then((res) => {
                return EAAShareSdk.addApprovalData(requstGuid, approverId, nextStage, EAAShareSdk.ApprovalStatus.Approved)
            })
        })
    } else {
        return EAAShareSdk.share(requstGuid, owner.id).then(res => {
            return EAAShareSdk.changeFormProcessLock(requstGuid, true).then((res) => {
                return EAAShareSdk.addApprovalData(requstGuid, approverId, nextStage, EAAShareSdk.ApprovalStatus.Approved)
            })
        })
    }
}



EAAShareSdk.Reject = (requstGuid, approverId, previousStage) => {
    return EAAShareSdk.addApprovalData(requstGuid, approverId, previousStage, EAAShareSdk.ApprovalStatus.Rejected)
}

EAAShareSdk.Reopen = (requstGuid, approverId, reopenStage) => {
    let owner = EAAShareSdk.getOwner()

    if (owner.type === 'team') {
        return EAAShareSdk.shareToTeam(requstGuid, owner.id).then(res => {
            return EAAShareSdk.addApprovalData(requstGuid, approverId, reopenStage, EAAShareSdk.ApprovalStatus.ResubmissionPending)
        })
    }
    else {
        return EAAShareSdk.share(requstGuid, owner.id).then(res => {
            return EAAShareSdk.addApprovalData(requstGuid, approverId, reopenStage, EAAShareSdk.ApprovalStatus.ResubmissionPending)
        })
    }

}

EAAShareSdk.UpdateActualFee = (requstGuid, approverId) => {
    return EAAShareSdk.changeFormProcessLock(requstGuid, true).then((res) => {
        return EAAShareSdk.addApprovalData(requstGuid, approverId, EAAShareSdk.WorkflowStages.ActualFeeUpdateCompleted, EAAShareSdk.ApprovalStatus.Completed)
    })
}

EAAShareSdk.addApprovalData = (requstGuid, approverId, nextStage, approvalStatus) => {
    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.approval}s`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify({
                "aia_eaa_approver@odata.bind": `/systemusers(${approverId})`,
                "aia_sys_eaa_form@odata.bind": `/aia_eaa_forms(${requstGuid})`,
                "aia_eaa_action_on": new Date(),
                "aia_eaa_my_action": approvalStatus,
                "aia_eaa_workflow_stage": nextStage,
            })
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

/**
 * 
 * @param {*} apprvalStatus 
 * @param {*} requstGuid 
 * @param {*} formType 
 * @param {*} status 
 * @param {*} title 
 * @param {*} approverId 
 * @returns 
 */
EAAShareSdk.onBehalfOfApprovalFormProcess = (apprvalStatus, requstGuid, formType, status, title, approverId) => {
    var body = {
        "aia_sys_processlock": true,
        "aia_name": title,
        "aia_eaa_latest_modified_on": new Date()
    }

    if (apprvalStatus === EAAShareSdk.ApprovalStatus.Approved && EAAShareSdk.IsFormAtFinalStage(formType, status)) {
        body["aia_eaa_final_date"] = new Date()
        body["aia_eaa_final_approver@odata.bind"] = `/systemusers(${approverId})`
    }

    //set on-behalf-of approver

    if (status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval) {
        body["aia_eaa_on_behalf_head_submit_date"] = new Date()
        body["aia_eaa_on_behalf_head@odata.bind"] = `/systemusers(${approverId})`
    } else {
        body["aia_on_behalf_rcfo_submit_date"] = new Date()
        body["aia_eaa_on_behalf_rcfo@odata.bind"] = `/systemusers(${approverId})`
    }

    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${requstGuid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    )
}

EAAShareSdk.getTeamIdByStage = (stage) => {
    const xhr = new XMLHttpRequest()
    try {
        xhr.open("GET",
            `/api/data/v9.0/aia_eaa_team_roles?$filter=aia_eaa_stage eq ${stage}`,
            false);

        var count = 0
        xhr.send()
        if (xhr.status === 200) {
            const resData = JSON.parse(xhr.responseText)
            return resData.value[0]._aia_eaa_team_value
        }
        return null
    }
    catch (err) {
        console.log('error in exportToExcel: ', err);
    }
}
EAAShareSdk.isUserInTeamByStage = (stage, userid) => {
    var teamid = EAAShareSdk.getTeamIdByStage(stage)
    return EAAShareSdk.isUserInTeam(teamid, userid)
}

EAAShareSdk.isUserInTeam = (teamId, userid) => {
    const xhr = new XMLHttpRequest()
    try {
        xhr.open("GET",
            `/api/data/v9.0/teammemberships?$filter=teamid eq ${teamId} and systemuserid eq ${userid}`,
            false);

        var count = 0
        xhr.send()
        if (xhr.status === 200) {
            const resData = JSON.parse(xhr.responseText)
            count = resData.value.length
        }
        return count > 0
    }
    catch (err) {
        console.log('error in exportToExcel: ', err);
    }

}

EAAShareSdk.getLocalApproverId = () => {
    var localapprover = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.local_approver)?.getValue()
    if (localapprover) {
        return localapprover[0].id.replace(/[{}]*/g, "").toLowerCase()
    } else {
        return ''
    }
}

EAAShareSdk.getLocalApproverName = () => {
    var localapprover = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.local_approver)?.getValue()
    if (localapprover) {
        return localapprover[0].name
    } else {
        return ''
    }
}

EAAShareSdk.getCountries = () => {
    var countries = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.countries)?.getValue()
    if (countries) {
        return { id: countries[0].id.replace(/[{}]*/g, "").toLowerCase(), name: countries[0].name }
    } else {
        return null
    }
}

EAAShareSdk.IsApprover = (currentUserId) => {

    let approverFields = [EAAShareSdk.FormFields.local_approver,
    EAAShareSdk.FormFields.head, EAAShareSdk.FormFields.cfo,
    EAAShareSdk.FormFields.finance_manager,
    EAAShareSdk.FormFields.head]

    for (let p in approverFields) {
        var approver = Xrm.Page.data.entity.attributes.getByName(approverFields[p])?.getValue()
        if (approver) {
            let approverid = approver[0].id.replace(/[{}]*/g, "").toLowerCase()
            if (approverid === currentUserId) {
                return true
            }
        }
    }

    if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.on_behalf_head).getValue()) {
        let result = EAAShareSdk.isUserInTeam(EAAShareSdk.getTeamIdByStage(EAAShareSdk.Stages.HeadofGroupTax), currentUserId)

        if (result) {
            return result
        }
    }

    if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.on_behalf_rcfo).getValue()) {
        let result = EAAShareSdk.isUserInTeam(EAAShareSdk.getTeamIdByStage(EAAShareSdk.Stages.RegionalCFO), currentUserId)
        if (result) {
            return result
        }
    }

    return false
}

EAAShareSdk.getLocalViewers = (requestGuid) => {
    const xhr = new XMLHttpRequest()
    try {
        xhr.open("GET",
            `/api/data/v9.0/aia_eaa_forms(${requestGuid})?$select=aia_eaa_local_viewer_sys_form_eaa_form &$expand=aia_eaa_local_viewer_sys_form_eaa_form`,
            false);

        xhr.send()
        if (xhr.status === 200) {
            const resData = JSON.parse(xhr.responseText)
            return resData.aia_eaa_local_viewer_sys_form_eaa_form.map(x => x._aia_eaa_recipient_value)
        }
        return []
    }
    catch (err) {
        console.log('error in exportToExcel: ', err);
    }
}

EAAShareSdk.getLocalViewersAsync = (requestGuid) => {
 
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.local_viewer}s?$filter=_aia_sys_eaa_form_value eq '${requestGuid}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        return Promise.resolve(res.value)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EAAShareSdk.getCommentsByUser = (requestGuid, currentUserId) => {
    const xhr = new XMLHttpRequest()
    try {
        xhr.open("GET",
            `/api/data/v9.0/aia_eaa_comments?$select=aia_eaa_commentid&$filter=_aia_sys_eaa_form_value eq '${requestGuid}' and _aia_user_value eq '${currentUserId}'`,
            false);

        xhr.send()
        if (xhr.status === 200) {
            const resData = JSON.parse(xhr.responseText)
            return resData.value.map(x => x.aia_eaa_commentid)
        }
        return []
    }
    catch (err) {
        console.log('error in exportToExcel: ', err);
    }
}

EAAShareSdk.getFormData = (requestGuid) => {
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.form}s?$filter=aia_eaa_formid eq '${requestGuid}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        form = res.value[0];

        return Promise.resolve(form)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EAAShareSdk.getOldestSubmission = (requestGuid) => {
    return fetch(`/api/data/v9.0/aia_eaa_timelines?$filter=_aia_sys_eaa_form_value eq '${requestGuid}' and aia_action_name eq 'Submitted' &$orderby=createdon asc`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.value.length == 0) {
            return null
        }

        form = res.value[0];

        return Promise.resolve({
            name: form["aia_action_by"],
            time: form["createdon"]
        })
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EAAShareSdk.getTeamId = (teamName) => {
    //get team id
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.team}s?$filter=name eq '${teamName}'`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        var teamid = res.value[0].teamid;

        return Promise.resolve(teamid)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EAAShareSdk.getTeamIdByCountry = (countryid) => {
    //get team id
    return fetch(`/api/data/v9.0/aia_eaa_countries(${countryid})`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {
        var teamid = res["_aia_country_admin_value"];

        return Promise.resolve(teamid)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EAAShareSdk.getTeamMembers = (status) => {
    var fetchXml = `<fetch mapping='logical'>
	<entity name='team'>
		<link-entity name='aia_eaa_team_role' from='aia_eaa_team' to='teamid'>
			<filter type='and'>
				<condition attribute='aia_eaa_form_status' operator='eq' value='${status}'/>
			</filter>
		</link-entity>
		<link-entity name='teammembership' from='teamid' to='teamid'>
			<link-entity name='systemuser' from='systemuserid' to='systemuserid'>
				<attribute name='fullname'/>
			</link-entity>
		</link-entity>
	</entity>
</fetch>`

    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.team}s?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        })
}

EAAShareSdk.deleteEngagement = (requestGuid) => {
    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${requestGuid})`,
        {
            method: "DELETE",
        }
    ).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

EAAShareSdk.deactiveEngagement = (requestGuid) => {
    return fetch(
        `/api/data/v9.0/${EAAShareSdk.Tables.form}s(${requestGuid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify({
                statecode: 1,
                statuscode: 2
            })
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

/**
 * share specific request to specific user
 * @param {*} requestGuid 
 * @param {*} userid 
 * @returns 
 */
EAAShareSdk.share = (requestGuid, userid) => {
    return fetch(`api/data/v9.0/GrantAccess`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            },
            body: JSON.stringify({
                "Target": {
                    "aia_eaa_formid": requestGuid,
                    "@odata.type": "Microsoft.Dynamics.CRM.aia_eaa_form"
                },
                "PrincipalAccess": {
                    "Principal": {
                        "systemuserid": userid,
                        "@odata.type": "Microsoft.Dynamics.CRM.systemuser"
                    },
                    "AccessMask": "ReadAccess,WriteAccess,AppendToAccess"
                }
            })
        }
    ).then(res => {
        if (res.error) {
            console.error(res.error || res.error.message)
            return Promise.reject(res)
        } else {
            return Promise.resolve(res)
        }
    })
}

EAAShareSdk.assign = (requestGuid, ownerid) => {
    return fetch(`/api/data/v9.0/Assign`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            },
            body: JSON.stringify({
                "Target": {
                    "@odata.type": "Microsoft.Dynamics.CRM.aia_eaa_form",
                    "aia_eaa_formid": requestGuid
                },
                "Assignee": {
                    "@odata.type": "Microsoft.Dynamics.CRM.systemuser",
                    "systemuserid": ownerid
                }
            })
        }
    ).then(res => {
        if (res.error) {
            console.error(res.error || res.error.message)
            return Promise.reject(res)
        } else {
            return Promise.resolve(res)
        }
    })
}

EAAShareSdk.shareToTeam = (requestGuid, teamid) => {
    return fetch(`api/data/v9.0/GrantAccess`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            },
            body: JSON.stringify({
                "Target": {
                    "aia_eaa_formid": requestGuid,
                    "@odata.type": "Microsoft.Dynamics.CRM.aia_eaa_form"
                },
                "PrincipalAccess": {
                    "Principal": {
                        "teamid": teamid,
                        "@odata.type": "Microsoft.Dynamics.CRM.team"
                    },
                    "AccessMask": "ReadAccess,WriteAccess,AppendToAccess"
                }
            })
        }
    ).then(res => {
        if (res.error) {
            console.error(res.error || res.error.message)
            return Promise.reject(res)
        } else {
            return Promise.resolve(res)
        }
    })
}

EAAShareSdk.revoke = (requestGuid, userid) => {
    return fetch(`api/data/v9.0/RevokeAccess`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            },
            body: JSON.stringify({
                "Target": {
                    "aia_eaa_formid": requestGuid,
                    "@odata.type": "Microsoft.Dynamics.CRM.aia_eaa_form"
                },
                "Revokee": {
                    "systemuserid": userid,
                    "@odata.type": "Microsoft.Dynamics.CRM.systemuser"
                }
            })
        }
    ).then(res => {
        if (res.error) {
            console.error(res.error || res.error.message)
            return Promise.reject(res)
        } else {
            return Promise.resolve(res)
        }
    })
}

EAAShareSdk.revokeTeam = (requestGuid, teamid) => {
    return fetch(`api/data/v9.0/RevokeAccess`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            },
            body: JSON.stringify({
                "Target": {
                    "aia_eaa_formid": requestGuid,
                    "@odata.type": "Microsoft.Dynamics.CRM.aia_eaa_form"
                },
                "Revokee": {
                    "teamid": teamid,
                    "@odata.type": "Microsoft.Dynamics.CRM.team"
                }
            })
        }
    ).then(res => {
        if (res.error) {
            console.error(res.error || res.error.message)
            return Promise.reject(res)
        } else {
            return Promise.resolve(res)
        }
    })
}

EAAShareSdk.getShareRecords = (formId) => {
    var fetchXml = `<fetch version='1.0' mapping='logical' distinct='true' latematerialize='true'>
	<entity name='principalobjectaccess'>
		<attribute name='accessrightsmask'/>
		<attribute name='principaltypecode'/>
		<attribute name='principalid'/>
		<filter type='or' hint='union'>
			<filter type='and'>
				<condition attribute='objectid' operator='eq' value='${formId}'/>
				<condition attribute='principaltypecode' operator='eq' value='8'/>
			</filter>
			<filter type='and'>
				<condition attribute='objectid' operator='eq' value='${formId}'/>
				<condition attribute='principaltypecode' operator='eq' value='9'/>
			</filter>
		</filter>
		<link-entity name='systemuser' link-type='outer' from='systemuserid' to='principalid' alias='systemuser'>
			<attribute name='fullname'/>
			<attribute name='systemuserid'/>
			<order attribute='fullname'/>
		</link-entity>
		<link-entity name='team' link-type='outer' from='teamid' to='principalid' alias='team'>
			<attribute name='name'/>
			<attribute name='teamid'/>
			<attribute name='systemmanaged'/>
			<order attribute='name'/>
		</link-entity>
	</entity>
</fetch>`

    return fetch(`/api/data/v9.0/principalobjectaccessset?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }).then(res => res.json()).then(res => {
            if (res.error) {
                throw new Error(res.error.message)
            }

            let records = res.value.map(x => {
                if (x["principaltypecode"] === 'team') {
                    return { "id": x["team.teamid"], "type": "team" }
                }
                else {
                    return { "id": x["systemuser.systemuserid"], "type": "systemuser" }
                }
            })
            return Promise.resolve(records)
        }).catch(err => {
            console.error(err)
            throw err;
        })
}

/**
 * get mapping value by specific key
 * @param {*} key 
 * @returns 
 */
EAAShareSdk.getConfiguration = (key) => {
    return fetch(`/api/data/v9.0/${EAAShareSdk.Tables.config}s?$filter=aia_name eq '${key}'`,
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

        return Promise.resolve(res.value[0].aia_eaa_value)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

EAAShareSdk.generateReferenceIfNotExists = (title) => {
    if (!!!title || title === 'Draft') {
        return EAAShareSdk.generateReferenceNo().then(res => {
            return Promise.resolve(res)
        })
    } else {
        return Promise.resolve()
    }
}

/**
 * 
EAA-[YYYY]-[MM]-[NUMBER][-CODE]
EAA-2019-08-003

- YYYY:  Year EAA was created
- MM: Month EAA was created
- NUMBER: 3 digit count of the numbe of EAA created for the month. Every month this number starts at 001
- CODE: After Regional CFO or Head of Group Tax approves the EAA, a 2 letter code is appended to the name. The 1st character is a random capital letter A-Z while the 2nd is a random digit from 0-9
EAA-2019-08-003-D7
 */
EAAShareSdk.generateReferenceNo = () => {

    return EAAShareSdk.getConfiguration(EAAShareSdk.ConfigurationKeys.FormHelperURL).then(url => {
        return fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify({
                "action": "refno",
            })
        }).then(res => res.json()).then(res => {

            if (res.error) {
                throw new Error(res.error.message)
            }
            var date = new Date();
            var year = date.getFullYear()
            var month = date.getMonth() + 1
            var count = res.value[0].total + 1;
            var title = `EAA-${year}-${month.toString().padStart(2, '0')}-${count.toString().padStart(3, '0')}`;
            return Promise.resolve(title)
        }).catch(err => {
            console.error(err)
            throw err;
        })
    })
}

EAAShareSdk.onBehalfOfApproval = (status, requestType, action) => {

    if (status === EAAShareSdk.FormStatus.PendingHeadOfGroupTaxApproval || status === EAAShareSdk.FormStatus.PendingRegionalCFOApproval) {
        EAAShareSdk.onBehalfOfApprovalCore(action)
    }
}

EAAShareSdk.onBehalfOfApprovalCore = (action) => {

    Xrm.Utility.showProgressIndicator("Loading")
    Xrm.Page.data.save().then(() => {

        var pageInput = {
            entityName: EAAShareSdk.Tables.approval,
            pageType: "entityrecord",
            formId: EAAShareSdk.Forms.EAA_Approve_On_Behalf,
            createFromEntity: {
                entityType: EAAShareSdk.Tables.form,
                id: Xrm.Page.data.entity.getId(),
                name: Xrm.Page.data.entity.attributes.getByName("aia_name").getValue()
            },
            data: {
                aia_eaa_my_action: action,
                aia_eaa_timelineid: EAAShareSdk.guid()
            }
        }

        Xrm.Navigation.navigateTo(pageInput,
            {
                target: 2,
                position: 1,
                height: { value: 80, unit: "%" },
                width: { value: 70, unit: "%" },
                title: "Approve On Behalf"
            }).then(function success() {

                Xrm.Utility.closeProgressIndicator()
            }, function error() {

                Xrm.Utility.closeProgressIndicator()
            })
    })
}

EAAShareSdk.formError = (err) => {
    const msgKey = new Date().getTime().toString(16)
    Xrm.Page.ui.formContext.ui.setFormNotification(
        err.message,
        'ERROR',
        msgKey);

    window.setTimeout(function () {
        Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
    }, 10000);
}

EAAShareSdk.formInfo = (message, delay = 10000) => {
    const msgKey = new Date().getTime().toString(16)
    Xrm.Page.ui.formContext.ui.setFormNotification(
        message,
        'INFO',
        msgKey);

    window.setTimeout(function () {
        Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
    }, delay);
}

EAAShareSdk.padTo2Digits = (num) => {
    return num.toString().padStart(2, '0');
}

EAAShareSdk.formatDate = (date) => {
    return [
        date.getFullYear(),
        EAAShareSdk.padTo2Digits(date.getMonth() + 1),
        EAAShareSdk.padTo2Digits(date.getDate()),
    ].join('-');
}

EAAShareSdk.randomCode = () => {
    return `${EAAShareSdk.RandomLetters[new Date().getTime() % 26]}${Number.parseInt(Math.random() * 10)}`
}

EAAShareSdk.fillApprover = (currentUserId, currentUserName, fieldName) => {
    EAAShareSdk.setLookupValue(currentUserId, currentUserName, EAAShareSdk.Tables.systemuser, fieldName)
}

EAAShareSdk.setLookupValue = function (id, name, tableName, fieldName) {
    var previousValue = Xrm.Page.data.entity.attributes.getByName(fieldName).getValue()
    if (!previousValue) {
        var lookupData = new Array();
        var lookupItem = new Object();
        lookupItem.id = id;
        lookupItem.name = name;
        lookupItem.entityType = tableName;
        lookupData[0] = lookupItem;
        Xrm.Page.data.entity.attributes.getByName(fieldName).setValue(lookupData);
    }

}

EAAShareSdk.navigateBack = () => {
    var pageInput = {
        pageType: "entitylist",
        entityName: "aia_eaa_form",
    }
    return Xrm.Navigation.navigateTo(pageInput)
}

EAAShareSdk.popupSuccess = (referenceno, type) => {
    var option = {
        pageType: "webresource",
        webresourceName: "aia_eaa_engagement_success.html",
    }

    option.data = `referenceno=${referenceno}&type=${type}`

    return Xrm.Navigation.navigateTo(option, {
        target: 2,
        position: 1,
        width: {
            value: 480,
            unit: 'px'
        },
        height: {
            value: 380,
            unit: 'px'
        },
        title: "Message"
    }).then(
        function success() {
            // Run code on success
            EAAShareSdk.navigateBack()
        },
        function error() {
            // Handle errors
            EAAShareSdk.navigateBack()
        }
    );

}

EAAShareSdk.navigateToPage = (viewid) => {

    const getElementId = (viewid) => {
        if (viewid == EAAShareSdk.Views.All) {
            return "sitemap-entity-eaa_all_request";
        }
        else if (viewid == EAAShareSdk.Views.MyRequest) {
            return "sitemap-entity-eaa_my_request";
        }
        else if (viewid == EAAShareSdk.Views.AwaitingMyResponse) {
            return "sitemap-entity-eaa_awaiting_my_response";
        }
    }
    var elementid = getElementId(viewid)
    const ele = window.parent.document.querySelector(`#${elementid}`)
    if (ele) {
        ele.click()
    } else {
        var pageInput = {
            pageType: "entitylist",
            entityName: "aia_eaa_form",
            viewId: viewid
        }
        Xrm.Navigation.navigateTo(pageInput)
    }

}

EAAShareSdk.navigateToHomePage = () => {
    const viewid = 'sitemap-entity-eaa_home'
    const ele = window.parent.document.querySelector(`#${viewid}`)
    if (ele) {
        ele.click()
    } else {
        var pageInput = {
            pageType: "entitylist",
            entityName: "aia_eaa_form",
            viewId: EAAShareSdk.Views.All
        }
        Xrm.Navigation.navigateTo(pageInput)
    }
}

EAAShareSdk._doUntil = (doFunc, untilFunc, maxMs = 20000, delayMs = 50) => {
    const start = new Date();

    const setTimeoutFunc = () => {
        if (untilFunc()) {
            doFunc()
        } else if (new Date() - start < maxMs) {
            setTimeout(() => {
                setTimeoutFunc()
            }, delayMs)
        }
    }
    setTimeoutFunc()
}

EAAShareSdk.preventAutoSave = (executionContext) => {
    var eventArgs = executionContext.getEventArgs();
    if (eventArgs.getSaveMode() == 70 || eventArgs.getSaveMode() == 2) {
        eventArgs.preventDefault();
    }
}

// Generate four random hex digits.  
const S4 = () => {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
};
// Generate a pseudo-GUID by concatenating random hexadecimal.  
EAAShareSdk.guid = () => {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
};

// EAAShareSdk.createGUID = () => {
//     return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
//         var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
//         return v.toString(16);
//     });
// }
