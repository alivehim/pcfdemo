window.ShareSdk = window.ShareSdk || {};
window.parent.ShareSdk = window.ShareSdk || {};


ShareSdk.import = () => {
    console.log("SDK coming on.")
}


ShareSdk.SecurityRoles = {
    SSMS_Admin: "SSMS Admin",
    SSMS_Principle_Manager_Civil: "SSMS_Principle_Manager_Civil",
    SSMS_Proximity_Card_Admin: "SSMS_Proximity_Card_Admin",
    SSMS_Proximity_Card_Security: "SSMS_Proximity_Card_Security",
    SSMS_Responsible_Engineer: "SSMS_Responsible_Engineer",
    SSMS_Viewer: "SSMS_Viewer",
    SSMS_User_Representative: "SSMS_User_Representative"
}

ShareSdk.PROXCardReqStatus = {
    WaitingforApproval: 768230000,
    WaitingforReview: 768230001,
    WaitingforPrincipleManager_CivilApproval: 768230002,
    WaitingforCardAllocation: 768230003,
    WaitingforCardEncoding: 768230004,
    RequestCompleted: 768230005,
    RequestCancelled: 768230006,
    RequestRejected: 768230007,
    RejectedbySecurity: 768230008,
}

ShareSdk.PROXCardReqStatusMapping = {
    768230000: 'REG',
    768230001: 'APP',
    768230002: 'OPT',
    768230003: 'AAP',
    768230004: 'ALC',
    768230005: 'CMP',
    768230006: 'CAN',
    768230007: 'REJ',
    768230008: 'RES',
}

ShareSdk.PROXCardReqAPPType = {
    NewISMS: 768230000,
    EncodeISMS: 768230001,
    EncodeC_Card: 768230002,
}

ShareSdk.PROXCardReqAPPTypeMapping = {
    768230000: 'NI',
    768230001: 'EI',
    768230002: 'EC'
}

ShareSdk.ProximityCardCardType = {
    ISMSProximityCard: 768230000,
    ContractorConsultantPass: 768230001,
}

ShareSdk.ProximityCardCardTypeMapping = {
    768230000: "I",
    768230001: "C"
}

ShareSdk.ProximityCardStatus = {
    Available: 768230000,
    Reserved: 768230001,
    Allocated: 768230002,
    Damaged: 768230003,
    Lost: 768230004,
    Scrapped: 768230005,
    Returned: 768230006,
    PendingforReplacement: 768230007,
    AccessExpire: 768230008,
}

ShareSdk.ProximityCardStatusMapping = {
    768230000: "AVE",
    768230001: "REV",
    768230002: "ALD",
    768230003: "DMG",
    768230004: "LOS",

    768230005: "SCP",

    768230006: "RET",
    768230007: "PFR",

}

ShareSdk.PROXCardIndividualStatus = {
    WaitingforCardEncoding: 768230000,
    WaitingforCardCollection: 768230001,
    Issued: 768230002,
    WaitingforCardDecoding: 768230003,
    CollectionCancelled: 768230004,
    Rejected: 768230005,
    Completed: 768230006,
}

ShareSdk.PROXCardIndividualStatusMapping = {
    768230000: "WCE",
    768230001: "WCC",
    768230002: "WCR",
    768230003: "WCD",
    768230004: "CAN",
    768230005: "REJ",
    768230006: "CMP"
}


ShareSdk.IssueDepartmentDefinition = {
    East_WestRegion: 768230000,
    NorthRegion: 768230001,
    TD: 768230002,
    TSD: 768230003,
    AMD: 768230004,
    NA: 768230005,

}
ShareSdk.IssueDepartmentMapping = {
    Transmission: { val: 768230002, name: "TD", subcode: "50316356" },
    East_WestRegion: { val: 768230000, name: "WE", subcode: "50116294" },
    NorthRegion: { val: 768230001, name: "NR", subcode: "50000183" },
    TSD: { val: 768230003, name: "TS", subcode: "50000176" },
    AMD: { val: 768230004, name: "AM", subcode: "50000172" },
}

ShareSdk.IssueDepartmentAbbr = {
    PC_DEPT_WE: "WE",
    PC_DEPT_NR: "NR",
    PC_DEPT_EP: "TD",
    PC_DEPT_TS: "TS",
    PC_DEPT_AM: "AM",
    PC_DEPT_NA: "NA",
}

ShareSdk.IssueDepartmentMappingNoUsed = {
    768230000: "WE",
    768230001: "NR",
    768230002: "TD",
    768230003: "TS",
    768230004: "AM"
}

ShareSdk.ProximityCardAction = {
    Approve: 768230000,
    Reject: 768230002
}

ShareSdk.PROXCardDetailAction = {
    Approve: 768230000,
    Reject: 768230001
}

ShareSdk.SubstationType = {
    Substation: 768230000,
    Group: 768230001,
    All: 768230002,

}

ShareSdk.SubstationCategory = {
    PC: 768230000,
}

ShareSdk.Region = {
    East_West: 768230000,
    North: 768230001,
    All: 768230002,
}

ShareSdk.DataSyncType = {
    Encode: 768230000,
    DeleteEncodes: 768230001,
    UpdateDeatilStatus: 768230002,
    AddCards: 768230003,
    DeleteCard: 768230004,
    AddEncodes: 768230005,
    UpdateCard: 768230007,
    CardEncodeSync: 768230008,
    CardDecodeSync: 768230009,
    CardReplacementSync: 768230010,
    AddDetails: 768230011,
}

ShareSdk.ProximityCardRequestSubGrids = {
    Applicant: "Subgrid_Applicant",
    Detail: "Subgrid_Detail",
    EditableDetail: "EditableSubGrid_Detail",
    SubGridDetail: "SubGrid_Detail",
    Subgrid_station: "Subgrid_station",
    Subgrid_station: "Subgrid_station"
}

ShareSdk.ProximityCardMessage = {
    SumbitSuccessfullyMessage: `Request <%=iAppNo%> is added successfully.  Would you like to export the Request Form now?`,
    UpdateMessage: "Request is updated successfully",

}

ShareSdk.PROXCardReqDTLErrors = {
    ApplicantMatch: { id: "ApplicantMatch", message: "The card holder does not match with the applicant" },
    CardNotExist: { id: "CardExists", message: "The card does not exist in the system!" }
}

ShareSdk.PROXCardErrors = {
    AccessPeriodFrom: { id: "AccessPeriodFrom", message: "Access From Date must be equal/after Today" },
    AccessPeriodTo: { id: "AccessPeriodTo", message: "Access To Date must be equal/after Access From Date" },
}

ShareSdk.PROXCardInventoryErrors = {
    ISMSProximityCardNoFormat: { id: "ISMSProximityCardNoFormat", message: "ISMS Proximity Card No. must be in format XXNNNNN (e.g.ER00001)" },
    ISMSProximityCardDeptConsistant: { id: "ISMSProximityCardDeptConsistant", message: "The first two character of Card No Range must be the same." },
    ISMSProximityCardRange: { id: "ISMSProximityCardRange", message: "Card No.To must be greater than Card No.1" },
    ContractorConsultantCardNoFormat: { id: "ContractorConsultantCardNoFormat", message: "Contractor/Consultant Pass must be in format CNNNN-NNNNN (e.g.C1234-12345) or CNNNNN-NNNNN (e.g.C12345-12345) or NNCNNNN-NNNNN (E.G.92C1234-12345)" }
}

ShareSdk.CardNotificaitonType = {
    Decode: 768230000,
    ReplaceEncode: 768230001,
    ReplaceCollect: 768230002,
}

ShareSdk.Tables = {
    company: "clp_company",
    department: "clp_department",
    firesystemisolation_createpastrecordentry: "clp_firesystemisolation_createpastrecordentry",
    firesystemisolation_createpastrecordworkarea: "clp_firesystemisolation_createpastrecordworkarea",
    firesystemisolationworkarea: "clp_firesystemisolationworkarea",
    keynumberallocation: "clp_keynumberallocation",
    masterkeyapplication: "clp_masterkeyapplication",
    masterkeyauditmonitoring: "clp_masterkeyauditmonitoring",
    masterkeyforselection: "clp_masterkeyforselection",
    masterkeyinventory: "clp_masterkeyinventory",
    masterkeymovementrecord: "clp_masterkeymovementrecord",
    masterkeytypemaintenance: "clp_masterkeytypemaintenance",
    masterkeytypeofrequested: "clp_masterkeytypeofrequested",
    parkingpermitnumber: "clp_parkingpermitnumber",
    parkingpermitreason: "clp_parkingpermitreason",
    pendinglist: "clp_pendinglist",
    personalissue: "clp_personalissue",
    poolissue: "clp_poolissue",
    proximity_card_creation_event: "clp_proximity_card_creation_event",
    proximitycard_event: "clp_proximitycard_event",
    proximitycard_reqeust_sla: "clp_proximitycard_reqeust_sla",
    proximitycardapplicant: "clp_proximitycardapplicant",
    proximitycardapplicantpersonalinformation: "clp_proximitycardapplicantpersonalinformation",
    proximitycardapplicantprofile: "clp_proximitycardapplicantprofile",
    proximitycardauditmonitoring: "clp_proximitycardauditmonitoring",
    proximitycardexpirydecode: "clp_proximitycardexpirydecode",
    proximitycardinventory: "clp_proximitycardinventory",
    proximitycardrequest: "clp_proximitycardrequest",
    proximitycardrequest_workfow_team: "clp_proximitycardrequest_workfow_team",
    proximitycardsubstationlist: "clp_proximitycardsubstationlist",
    region: "clp_region",
    ssms_follow_up: "clp_ssms_follow_up",
    ssms_fs_isolation: "clp_ssms_fs_isolation",
    ssms_lut_area: "clp_ssms_lut_area",
    ssms_lut_ctrcomp: "clp_ssms_lut_ctrcomp",
    ssms_lut_substation: "clp_ssms_lut_substation",
    ssms_mk_pool: "clp_ssms_mk_pool",
    ssms_parking_permit: "clp_ssms_parking_permit",
    ssms_ss_access: "clp_ssms_ss_access",
    ssms_system_configuration: "clp_ssms_system_configuration",
    ssms_tg_storage: "clp_ssms_tg_storage",
    substationaccessrequestno: "clp_substationaccessrequestno",
    substationaccessstandardarea: "clp_substationaccessstandardarea",
    tempgoodsstoragelocation: "clp_tempgoodsstoragelocation",
    temporaryissue: "clp_temporaryissue",
    uam_role_user: "clp_uam_role_user",
    user: "clp_user",
    proximity_card_request_detail: "clp_proximity_card_request_detail",
    proximity_card_encode: "clp_proximity_card_encode"
}

ShareSdk.PROXCardReqFields = {
    security: "clp_security",
    name: "clp_name",
    mm: "clp_mm",
    attachment: "clp_attachment",
    admin_dept_branch: "clp_admin_dept_branch",
    appr_remarks: "clp_appr_remarks",
    rejected_date: "clp_rejected_date",
    contractorphoneno: "clp_contractorphoneno",
    reviewer: "clp_reviewer",
    re_departmentbranch: "clp_re_departmentbranch",
    mm_dept_branch: "clp_mm_dept_branch",
    clpworkorderno: "clp_clpworkorderno",
    ss_nr: "clp_ss_nr",
    reviewed_date: "clp_reviewed_date",
    readunderstoodandagreed: "clp_readunderstoodandagreed",
    accessperiodfrom: "clp_accessperiodfrom",
    expiry_3_wd: "clp_expiry_3_wd",
    approver: "clp_approver",
    reviewer_remarks: "clp_reviewer_remarks",
    card_allocation_date: "clp_card_allocation_date",
    security_dept_branch: "clp_security_dept_branch",
    authorizationcode: "clp_authorizationcode",
    expiry_5_wd: "clp_expiry_5_wd",
    requester: "clp_requester",
    chosenapprover: "clp_chosenapprover",
    sla_count: "clp_sla_count",
    expiry_10_wd: "clp_expiry_10_wd",
    reviewer_dept_branch: "clp_reviewer_dept_branch",
    ss_we: "clp_ss_we",
    requestdate: "clp_requestdate",
    mm_remarks: "clp_mm_remarks",
    is_reminder_email_sended: "clp_is_reminder_email_sended",
    lut_ctrcomp: "clp_lut_ctrcomp",
    issuedepartment: "clp_issuedepartment",
    chosen_mm: "clp_chosen_mm",
    admin_remarks: "clp_admin_remarks",
    mm_date: "clp_mm_date",
    approver_deptbranch: "clp_approver_deptbranch",
    appr_date: "clp_appr_date",
    rejectedby: "clp_rejectedby",
    contractorresponsibleperson: "clp_contractorresponsibleperson",
    responsiblecp: "clp_responsiblecp",
    nosofss: "clp_nosofss",
    security_remarks: "clp_security_remarks",
    responsibleperson: "clp_responsibleperson",
    card_encoding_date: "clp_card_encoding_date",
    admin: "clp_admin",
    ss_display: "clp_ss_display",
    accessperiodto: "clp_accessperiodto",
    workcontent: "clp_workcontent",
    phoneno: "clp_phoneno",
    remarks: "clp_remarks",
    ss_list: "clp_ss_list",
    workflow_status: "clp_workflow_status",
    is_locked: "clp_is_locked",
    raw_requester: "clp_raw_requester",
    collection_reminder_date: "clp_collection_reminder_date",
    extend_from_request: "clp_extend_from_request",
    is_migration_data: "clp_is_migration_data",
    latest_modified_by: "clp_latest_modified_by",
    latest_modified_on: "clp_latest_modified_on",
    attachment_link: "clp_attachment_link",
    responsible_cp: "clp_responsible_cp",
    approver_reassign_time: "clp_approver_reassign_time",
    mm_reassign_time: "clp_mm_reassign_time"
}
ShareSdk.CLPUserFields = {
    bgid: "clp_bgid",
    name: "clp_name",
    section_name: "clp_section_name",
    person_id: "clp_person_id",
    chi_name: "clp_chi_name",
    user: "clp_user",
    companyid: "clp_companyid",
    sectionid: "clp_sectionid",
    departmentbranch: "clp_departmentbranch",
    deptid: "clp_deptid",
    dept_name: "clp_dept_name",
    branchid: "clp_branchid",
    type: "clp_type",
    sub_section_name: "clp_sub_section_name",
    region: "clp_region",
    designation: "clp_designation",
    display_name: "clp_display_name",
    employeeno: "clp_employeeno",
    company_name: "clp_company_name",
    networkid: "clp_networkid",
    ehrs_code: "clp_ehrs_code",
    contr_comp_name: "clp_contr_comp_name",
    ehrs_desc: "clp_ehrs_desc",
    deptname: "clp_deptname",
    bg_name: "clp_bg_name",
    hk_id: "clp_hk_id",
    officetelno: "clp_officetelno",
    branch_name: "clp_branch_name",
    sub_sectionid: "clp_sub_sectionid",
    emailaddress: "clp_emailaddress",
    contr_comp_id: "clp_contr_comp_id",

}

ShareSdk.ApplicantFields = {
    company: "clp_company",
    associated_card: "clp_associated_card",
    contractorid: "clp_contractorid",
    phoneno: "clp_phoneno",
    name: "clp_name",
    user: "clp_user",
}
ShareSdk.PROXCardInventoryFields = {
    name: "clp_name",
    holder: "clp_holder",
    proximity_card_request: "clp_proximity_card_request",
    sendthereceiptbacktothecontractororvisitor: "clp_sendthereceiptbacktothecontractororvisitor",
    card_type: "clp_card_type",
    expirydate: "clp_expirydate",
    hkidnopassportno: "clp_hkidnopassportno",
    lostcardreportdate: "clp_lostcardreportdate",
    collectionremarks: "clp_collectionremarks",
    returndate: "clp_returndate",
    lostremarks: "clp_lostremarks",
    contractorid: "clp_contractorid",
    damagecardreturndate: "clp_damagecardreturndate",
    updateby: "clp_updateby",
    company: "clp_company",
    previouscardholderc: "clp_previouscardholderc",
    collectiondate: "clp_collectiondate",
    cardreplaceby: "clp_cardreplaceby",
    replace_card: "clp_replace_card",
    responsible_engineer: "clp_responsible_engineer",
    cardprefix: "clp_cardprefix",
    updatedate: "clp_updatedate",
    issuedepartment: "clp_issuedepartment",
    cancelcollection: "clp_cancelcollection",
    isaudit: "clp_isaudit",
    returninputdate: "clp_returninputdate",
    receiptno: "clp_receiptno",
    phoneno: "clp_phoneno",
    card_expiry_date: "clp_card_expiry_date",
    remarks: "clp_remarks",
    department: "clp_department",
    chequenoissuingbank: "clp_chequenoissuingbank",
    is_sended: "clp_issended",
    card_status: "clp_card_status",
    previous_card_holder: "clp_previous_card_holder",
    latest_modified_on: "clp_latest_modified_on",
    latest_modified_by: "clp_latest_modified_by"
}

ShareSdk.PROXCardRequetDetailFields = {

    name: "clp_name",
    collected_date: "clp_collected_date",
    proximity_card_request: "clp_proximity_card_request",
    detail_status: "clp_detail_status",
    return_reg_date: "clp_return_reg_date",
    app_type: "clp_app_type",
    contractor_id: "clp_contractor_id",
    applicant_name: "clp_applicant_name",
    contractor: "clp_contractor",
    is_rejected: "clp_is_rejected",
    returned_by: "clp_returned_by",
    returned_date: "clp_returned_date",
    proximity_card: "clp_proximity_card",
    replaced_by: "clp_replaced_by",
    collected_by: "clp_collected_by",
    collect_cancelled: "clp_collect_cancelled",
    card_expiry_date: "clp_card_expiry_date",
    remarks: "clp_remarks",
    proximity_card_applicant: "clp_proximity_card_applicant",
    card_no: "clp_card_no",
    phone_no: "clp_phone_no",


}
ShareSdk.PROXCardDecode = {
    ss_code: "clp_ss_code",
    encode_date: "clp_encode_date",
    card: "clp_card",
    name: "clp_name",
    requester: "clp_requester",
}
ShareSdk.PROXCardCreationFields = {
    card_no_to: "clp_card_no_to",
    name: "clp_name",
    card_no_from: "clp_card_no_from",
}

ShareSdk.Forms = {
    Applicant_Main: "a08c58f1-7cdb-4914-9a31-e46a14b5ccb2",
    Request_Main: "9dfbe4c5-8a56-42a5-947f-a065a5f4ee5f",
    Request_Approval: "d3ea845d-5b80-ed11-81ac-000d3a07ca3c",
    Request_Reassign: "4cb16031-958d-ed11-81ac-000d3a07ca3c",
    Card_Create: "fd7823af-51f0-4e45-b2f9-6537adaa9775",
    Card_Update: "2ef76e72-a56b-ed11-81ab-000d3a07ca3c",
    Card_Collect: "31f390d7-3f49-ed11-bba2-000d3a0871cb",
    Card_Return: "0dd9882e-4049-ed11-bba2-000d3a0871cb",
    Card_Decode: "34dd04b9-4249-ed11-bba2-000d3a0871cb",
    Card_Encode_Replacement: "05d2ef66-4349-ed11-bba2-000d3a0871cb",
    Card_Replace: "1678511c-4349-ed11-bba2-000d3a0871cb",
    Card_EncodeList: "264752b4-2292-ed11-aad0-000d3a07ca3c",
    Card_UpdateCardPrefix: "efb0a0b8-148c-ed11-81ac-000d3a07ca3c",
    Detail_Main: "c377dcfa-d2e0-4635-82d1-ad9e4b82fce8",
    Detail_Allocated: "b6e41f3e-4496-ed11-aad0-000d3a07ca3c",
    Applicant_Update: "adbe4a37-f192-ed11-aad0-000d3a07ca3c",
}

ShareSdk.Views = {
    Card_Available_Cards_for_Replacement: "4e4fd6d6-5886-ed11-81ac-000d3a07ca3c",
    Proximity_Card_Inventory: "f7c43146-8aa3-ed11-aad0-000d3a07ca3c",
    Proximity_Card_Expiry_Record: "eb87ff23-2c87-ed11-81ac-000d3a07ca3c",
    Proximity_Card_Applications: "10605cb2-3b87-ed11-81ac-000d3a07ca3c",
    Proximity_Card_Applicant_Profile: "11d46d09-2787-ed11-81ac-000d3a07ca3c",
    Proximtiy_Card_Application_Details: "aea4a60e-36a1-ed11-aad0-000d3a07ca3c",
    Proximity_Card_Expiry_Event_Log: "2e95320b-bd80-41aa-9c62-5ccff260c314",
    Proximity_Card_Audit_Details: "c272e6d2-8870-4297-a327-39bc12dba4d2",
    Proximity_Card_Audit: "0afc0c78-e3eb-4af7-955b-4472b7cf2b01"
}

ShareSdk.getCurrentFormId = () => {
    var replaceSymbol = /(\w*){(.*)}(.*)/g
    return Xrm.Page.data.entity.getId().replace(replaceSymbol, "$1$2");
}

ShareSdk.getCurrentUserId = () => {
    return Xrm._globalContext.userSettings.userId.replace(/[{}]*/g, "").toLowerCase()
}

ShareSdk.getCurrentUserName = () => {
    return Xrm._globalContext.userSettings.userName
}

ShareSdk.getOwnerId = () => {
    var owner = Xrm.Page.data.entity.attributes.getByName('ownerid').getValue();
    return owner[0].id.replace(/[{}]*/g, "").toLowerCase()
}

ShareSdk.getLookupId = (obj) => {
    return obj[0].id.replace(/[{}]*/g, "").toLowerCase()
}

ShareSdk.getLookupName = (obj) => {
    return obj[0].name
}

ShareSdk.isProximityCardAdmin = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return securityRoles.some(r => r == ShareSdk.SecurityRoles.SSMS_Proximity_Card_Admin)
}

ShareSdk.isPROXCardRequestor = () => {
    const rawRequester = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.raw_requester).getValue()

    if (rawRequester) {
        const rawRequesterId = rawRequester[0].id.replace(/[{}]*/g, "").toLowerCase()

        const currentUserId = ShareSdk.getCurrentUserId()
        return rawRequesterId === currentUserId
    }
    return false
}

ShareSdk.isPROXCardPMC = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return securityRoles.some(r => r == ShareSdk.SecurityRoles.SSMS_Principle_Manager_Civil)
}

ShareSdk.isProximityCardSecurity = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return securityRoles.some(r => r == ShareSdk.SecurityRoles.SSMS_Proximity_Card_Security)
}

ShareSdk.isSSMSUserRepresentative = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return securityRoles.some(r => r == ShareSdk.SecurityRoles.SSMS_User_Representative)
}

ShareSdk.isSSMSViewer = () => {
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
    return securityRoles.some(r => r == ShareSdk.SecurityRoles.SSMS_Viewer)
}

ShareSdk.isDelegateeforApprovalAsync = async () => {
    const currentUserId = ShareSdk.getCurrentUserId()
    var chosenapprover = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosenapprover).getValue()
    var approveruser = await ShareSdk.getCLPUserAsync(ShareSdk.getLookupId(chosenapprover))
    var delegatees = await ShareSdk.getDelegateesAsync(approveruser._clp_user_value)
    return delegatees.some(d => d._clp_delegatee_value === currentUserId)
}

ShareSdk.isDelegateeforPrincipleManagerAsync = async () => {
    const currentUserId = ShareSdk.getCurrentUserId()
    var chosenmm = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.chosen_mm).getValue()
    var approveruser = await ShareSdk.getCLPUserAsync(ShareSdk.getLookupId(chosenmm))
    var delegatees = await ShareSdk.getDelegateesAsync(approveruser._clp_user_value)
    return delegatees.some(d => d._clp_delegatee_value === currentUserId)
}

ShareSdk.IsProxRequestForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Request_Main
}

ShareSdk.IsProxRequestApprovalForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Request_Approval
}

ShareSdk.IsProxRequestReassginForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Request_Reassign
}

ShareSdk.IsProxCardUpdateForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_Update
}

ShareSdk.IsProxCardReplaceForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_Replace
}

ShareSdk.IsProxCardCollectForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_Collect
}

ShareSdk.IsProxCardReturnForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_Return
}

ShareSdk.IsProxCardDecodeForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_Decode
}

ShareSdk.IsProxCardEncodeReplacementForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_Encode_Replacement
}
ShareSdk.IsProxCardUpdateCardPrefixForm = () => {
    const formId = Xrm.Page.ui.formSelector.getCurrentItem().getId().toLowerCase()
    return formId === ShareSdk.Forms.Card_UpdateCardPrefix
}

ShareSdk.IsinInventory = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid === 'viewMapping_pc_inventory'
}

ShareSdk.IsinExpirtyRecord = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid === 'viewMapping_pc_expiry_record'
}

ShareSdk.IsApplicationDetail = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid === 'viewMapping_pc_request_detail'
}

ShareSdk.refreshPROXCardREQDTLGrid = () => {
    formContext = Xrm.Page.ui.formContext
    var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.SubGridDetail);
    subgrid.refresh();
}
ShareSdk.getCurrentClpUserAsync = async () => {
    var currentUserId = ShareSdk.getCurrentUserId()
    return await ShareSdk.getUserBySystemUserAsync(currentUserId)
}

ShareSdk.fillLatestModifiedByAsync = async () => {
    var user = await ShareSdk.getCurrentClpUserAsync()
    if (user) {
        Xrm.Page.data.entity.attributes.getByName("clp_latest_modified_by")?.setValue()
        ShareSdk.setLookupValue(user.clp_userid, user.clp_name, ShareSdk.Tables.user, "clp_latest_modified_by")
    }

    Xrm.Page.data.entity.attributes.getByName("clp_latest_modified_on")?.setValue(new Date())
    return user
}


ShareSdk.getCurrentClpUserDepartmentAsync = async () => {
    var currentUserId = ShareSdk.getCurrentUserId()
    var clpuser = await ShareSdk.getUserBySystemUserAsync(currentUserId)
    return { id: clpuser.clp_deptid, clp_dept_name: clpuser.clp_dept_name }
}

ShareSdk.getLegacyIssueDepartment = (dept) => {
    var vals = Object.values(ShareSdk.IssueDepartmentMapping)

    for (var item of vals) {
        if (item.val === dept) {
            return item.name
        }
    }
}

/**
 * Check if user has Admin Role in specified department
 */
ShareSdk.isCurrentAdminHasDeptAuthorityAsync = async (dept) => {

    // if (!ShareSdk.isProximityCardAdmin()) {
    //     return false
    // }

    if (dept === ShareSdk.IssueDepartmentDefinition.NA) {
        return true
    }

    const deptDB = await ShareSdk.getCurrentClpUserDepartmentAsync()

    var vals = Object.values(ShareSdk.IssueDepartmentMapping)

    for (var item of vals) {
        if (item.val === dept) {
            return item.subcode === deptDB.id
        }
    }

    return false
}


ShareSdk.getCurrentAdminHasDeptAsync = async () => {

    if (!ShareSdk.isProximityCardAdmin()) {
        return false
    }

    const deptDB = await ShareSdk.getCurrentClpUserDepartmentAsync()

    var vals = Object.values(ShareSdk.IssueDepartmentMapping)

    for (var item of vals) {
        if (deptDB.id === item.subcode) {
            return item.val
        }
    }


}

ShareSdk.isUserInTeamAsync = (teamId, userid) => {

    return fetch(`/api/data/v9.0/teammemberships?$filter=teamid eq ${teamId} and systemuserid eq ${userid}`,
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
        count = res.value.length

        return Promise.resolve(count > 0)

    }).catch(err => {
        console.error(err)
        throw err;
    })

}

/**
 * fetch user detail by user id
 * @param {*} userGuid 
 * @returns 
 */
ShareSdk.getUserAsync = (userGuid) => {

    return fetch(`/api/data/v9.0/${ShareSdk.Tables.user}s(${userGuid})`,
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
        return Promise.resolve(res)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getCLPUserAsync = (clpuserid) => {

    return fetch(`/api/data/v9.0/${ShareSdk.Tables.user}s(${clpuserid})`,
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
        return Promise.resolve(res)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getCLPUserByPersionIdAsync = (persionid) => {

    return fetch(`/api/data/v9.0/${ShareSdk.Tables.user}s?$filter= clp_person_id eq '${persionid}' or clp_networkid eq '${persionid}'`,
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

        if (res.value.length > 0) {

            return Promise.resolve(res.value[0])
        }

        return Promise.resolve(null)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getUserBySystemUserAsync = (systemuserid) => {

    return fetch(`/api/data/v9.0/${ShareSdk.Tables.user}s?$filter=_clp_user_value eq '${systemuserid}'`,
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
        return Promise.resolve(res.value[0])
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getDeptBranchAsync = async (clp_userid) => {
    var user = await ShareSdk.getUserAsync(clp_userid)

    if (user) {
        return (user[ShareSdk.CLPUserFields.dept_name] ?? '') + ' / ' + (user[ShareSdk.CLPUserFields.branch_name] ?? '')
    }

}

ShareSdk.BuildExportXMl = (fetchString, layoutxml, vewiId) => {

    var _view = { "@odata.type": "Microsoft.Dynamics.CRM.savedquery", "savedqueryid": vewiId };
    var fetchStr = fetchString.replace('page="1" count="50"', "");
    //var _fetchXml = fetchString;
    //var _layoutXml = "<grid name='resultset'><row name='result'><cell name='aia_name'/><cell name='aia_requesttitle'/><cell name='aia_sys_requester_name'/><cell name='aia_business_unit_team' /><cell name='aia_sys_submission_date' /><cell name='aia_sys_stage'/><cell name='aia_entitycodes'/></row></grid>";
    var retXml = { View: _view, FetchXml: fetchStr, LayoutXml: layoutxml, QueryApi: "", QueryParameters: { Arguments: { Count: 0, IsReadOnly: true, Keys: [], Values: [] } } };
    return retXml;

}

ShareSdk.Export = (primaryControl, viewid, name) => {
    return fetch(`/api/data/v9.0/savedqueries(${viewid})`)
        .then(ret => ret.json()).then(res => {
            var fetchXml = String(primaryControl.getFetchXml());
            var xml = window.parent.ShareSdk.BuildExportXMl(fetchXml, res.layoutxml, viewid);
            var _timeName = (new Date()).format("yyyy-MMM-dd");


            return fetch(
                `/api/data/v9.0/ExportToExcel`,
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Prefer": "return=representation"
                    },
                    body: JSON.stringify(xml)
                }
            ).then(res => res.json()).then(res => {
                if (res.error) {
                    throw new Error(res.error.message)
                }
                if (res) {
                    const a = document.createElement('a')
                    a.href = `data:application/vnd.ms-excel;base64,${res.ExcelFile}`
                    a.download = `${name}_${_timeName}.xlsx`;
                    a.click()
                }
            })

        })
}

/**
 * get substations of specific proximity card
 * @param {*} requestGuid 
 * @returns 
 */
ShareSdk.getPROXCardSubstationsAsync = (requestGuid) => {
    return fetch(`/api/data/v9.0/clp_proximitycardrequests(${requestGuid})?$select=clp_name,clp_is_migration_data,clp_ss_display&$expand=clp_proximitycardrequest_substation_list`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.clp_proximitycardrequest_substation_list)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.removeSSAsync = (formid, ssid) => {
    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${formid})/clp_proximitycardrequest_substation_list(${ssid})/$ref`,
        {
            method: "DELETE",
        }
    ).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve()
    }).catch(err => {
        throw new Error(err.message)
    })
}

ShareSdk.addSubstionsAsync = async (formid, substationlist) => {
    try {

        await Promise.all(substationlist.map(p => ShareSdk.addSubstionAsync(formid, p)))

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
    }
}

ShareSdk.addSubstionAsync = (formid, id) => {
    var formData = {
        "@odata.context": "https://clp-dev.crm5.dynamics.com/api/data/v9.0/$metadata#$ref",
        "@odata.id": `clp_ssms_lut_substations(${id})`
    }
    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${formid})/clp_proximitycardrequest_substation_list/$ref`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}



ShareSdk.getPROXCardRequestAsync = (requestGuid) => {
    return fetch(`/api/data/v9.0/clp_proximitycardrequests(${requestGuid})?$expand=clp_pc_request_detail_pc_request`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.addAttachmentLink = (requestGuid, link) => {

    var body = {
        "clp_attachment_link": link
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${requestGuid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.deleteAttachmentLink = (requestGuid) => {

    var body = {
        "clp_attachment_link": null
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${requestGuid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}


ShareSdk.getPROXCardApplicantsAsync = (userid) => {
    return fetch(`/api/data/v9.0/clp_proximitycardapplicants?$filter=_clp_user_value eq '${userid}'  `,
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


ShareSdk.getPROXCardApplicantByIdAsync = (id) => {
    return fetch(`/api/data/v9.0/clp_proximitycardapplicants(${id}) `,
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
        return Promise.resolve(res)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

//**************************applicants************* */

ShareSdk.addApplicantAsync = (name, ctrid, phoneno, company) => {
    var formData = {
        "clp_name": name,
        "clp_contractorid": ctrid,
        "clp_phoneno": phoneno,
        "clp_company": company
    }
    return fetch(
        `/api/data/v9.0/clp_proximitycardapplicants`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.getApplicantByNameCTRIDANDCompanyAsync = (name, ctrid,phoneno, company) => {
    // var formData = {
    //     "clp_name": name,
    //     "clp_contractorid": ctrid,
    //     "clp_phoneno": phoneno,
    //     "clp_company": company
    // }
    // return fetch(
    //     `/api/data/v9.0/clp_proximitycardapplicants`,
    //     {
    //         method: "POST",
    //         headers: {
    //             "Content-Type": "application/json",
    //             'Prefer': 'return=representation',
    //         },
    //         body: JSON.stringify(formData)
    //     }
    // ).then(res => res.json()).then(res => {
    //     if (res.error) {
    //         throw new Error(res.error.message)
    //     }
    //     return res
    // })

      return fetch(`/api/data/v9.0/clp_proximitycardapplicants?$filter= clp_name eq '${name}' and clp_contractorid eq '${ctrid}' and clp_company eq '${company}' and clp_phoneno eq '${phoneno}' `,
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

/**
 * 
 * @param {*} contractid 
 * @param {*} cardno 
 */
ShareSdk.getPROXCardExpiryDateByCardNoAsync = async (cardno) => {
    //call interface
    /*
    SELECT	@CardExpiryDate = Card_Expiry_Date 
            FROM LAMS.dbo.LAMS_ALL_CARD_INFO 
            WHERE Staff_Contractor_ID+'-'+RIGHT(Contractor_Card_ID,5) = @CardNo AND Card_Type='CONTRACTOR'
            */

    // return new Promise((resolve, reject) => {
    //     return resolve(null)
    // })

    const res = await ShareSdk.getMappingValue("SSMS_PC_Disable_Integration_With_LAMS_LENEL")
    if (res && res === '1') {
        return null
    }
    else {

        const result = await ShareSdk.callLAMSAPIAsync("SP_SSMS_PC_CARDINFO_READ", cardno)
        return result.Action === "1" ? result?.Result1 || null : null
    }
}

ShareSdk.getPROXCardApplicants4Async = async (contractid) => {
    //call interface
    const res = await ShareSdk.getMappingValue("SSMS_PC_Disable_Integration_With_LAMS_LENEL")
    if (res && res === '1') {
        return []
    }
    else {



        const result = await ShareSdk.callLAMSAPIAsync("SP_SSMS_PC_APPLICANT_READ", contractid)
        if (result.Result2) {
            return [{
                "pc.clp_card_expiry_date": "",
                "pc.clp_card_type": ShareSdk.ProximityCardCardType.ContractorConsultantPass,
                "pc.clp_name": result.Result2,
                "pc.clp_card_expiry_date": result.Result1,
            }]
        } else {
            return []
        }

    }
}

/**
 * 
 * @param {*} contractid 
 * @param {*} cardno 
 */
ShareSdk.getPROXCardExpiryDateAsync = async (contractid, cardno) => {
    //call interface
    const res = await ShareSdk.getMappingValue("SSMS_PC_Disable_Integration_With_LAMS_LENEL")
    if (res || res === '1') {
        return null
    }
    else {
        const result = await ShareSdk.callLAMSAPIAsync("SP_SSMS_PC_APPLICANT_READ", contractid, cardno)
        return result.Action === 1 ? result?.Result1 || null : null
    }
}

/**
 * sp_SSMS_PC_CheckCardExpiry 
 */
ShareSdk.PROXCardCheckCardExpiryAsync = async (cardno, ExpiryDate, AccessTo) => {
    const CardExpiryDate = await ShareSdk.getPROXCardExpiryDateByCardNoAsync(cardno)

    if (CardExpiryDate) {
        return AccessTo < CardExpiryDate
    } else {
        return AccessTo < ExpiryDate
    }
}

/**
 * sp_SSMS_PC_GetApplicantInfo
 */
ShareSdk.getPROXCardApplicantsInfoAsync = async (contractid, cardno, applicantname) => {

    

    var applicants1 = await ShareSdk.getPROXCardApplicants1Async(contractid, cardno, applicantname)
    if (applicants1.length > 0) {
        return ShareSdk.orderByLatestModifiedOn(applicants1)
    }

    var applicants2 = await ShareSdk.getPROXCardApplicants2Async(contractid, applicantname)
    if (applicants2.length > 0) {
        return ShareSdk.orderByLatestModifiedOn(applicants2)
    }

    var appliant3 = await ShareSdk.getPROXCardApplicants3Async(contractid)
    if (appliant3.length > 0) {
        return ShareSdk.orderByLatestModifiedOn(appliant3)
    }

    return await ShareSdk.getPROXCardApplicants4Async(contractid)
}

ShareSdk.getPROXCardApplicants1Async = (contractid, cardno, applicantname) => {
    var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical" >
    <entity name="clp_proximitycardapplicant" >
        <attribute name="clp_name" />
        <attribute name="clp_proximitycardapplicantid" />
        <attribute name="clp_contractorid" />
        <attribute name="clp_phoneno" />
        <attribute name="clp_company" />
        <filter type="and" >
            <condition attribute="clp_contractorid" operator="eq" value="${contractid}" />
            <condition attribute="clp_name" operator="eq" value="${applicantname}" />
            <condition entityname="clp_proximitycardinventory" attribute="clp_name" operator="eq" value="${cardno}" />
        </filter>

        <link-entity name="clp_proximitycardinventory" to="clp_proximitycardapplicantid" from="clp_holder" alias="pc"  >
            <attribute name="clp_card_expiry_date"/>
            <attribute name="clp_card_type"/>
            <attribute name="clp_name"/>
            <attribute name="clp_proximitycardinventoryid"/>
            <attribute name="clp_latest_modified_on"/>
        </link-entity>
    </entity>
</fetch>`

    return fetch(
        `/api/data/v9.0/clp_proximitycardapplicants?fetchXml=${encodeURI(fetchXml)}`,
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

ShareSdk.getPROXCardApplicants2Async = (contractid, applicantname) => {
    var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical" >
    <entity name="clp_proximitycardapplicant" >
        <attribute name="clp_name" />
        <attribute name="clp_proximitycardapplicantid" />
        <attribute name="clp_contractorid" />
        <attribute name="clp_phoneno" />
        <attribute name="clp_company" />
        <filter type="and" >
            <condition attribute="clp_contractorid" operator="eq" value="${contractid}" />
            <condition attribute="clp_name" operator="eq" value="${applicantname}" />
        </filter>

        <link-entity name="clp_proximitycardinventory" to="clp_proximitycardapplicantid" from="clp_holder"  alias="pc" >
            <attribute name="clp_card_expiry_date"/>
            <attribute name="clp_card_type"/>
            <attribute name="clp_name"/>
            <attribute name="clp_proximitycardinventoryid"/>
            <attribute name="clp_latest_modified_on"/>
        </link-entity>
    </entity>
</fetch>`

    return fetch(
        `/api/data/v9.0/clp_proximitycardapplicants?fetchXml=${encodeURI(fetchXml)}`,
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

ShareSdk.getPROXCardApplicants3Async = (contractid) => {
    var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical" >
    <entity name="clp_proximitycardapplicant" >
        <attribute name="clp_name" />
        <attribute name="clp_proximitycardapplicantid" />
        <attribute name="clp_contractorid" />
        <attribute name="clp_phoneno" />
        <attribute name="clp_company" />
        <filter type="and" >
            <condition attribute="clp_contractorid" operator="eq" value="${contractid}" />
        </filter>

        <link-entity name="clp_proximitycardinventory" to="clp_proximitycardapplicantid" from="clp_holder"  alias="pc"  >
            <attribute name="clp_card_expiry_date"/>
            <attribute name="clp_card_type"/>
            <attribute name="clp_name"/>
            <attribute name="clp_proximitycardinventoryid"/>
            <attribute name="clp_latest_modified_on"/>
        </link-entity>
    </entity>
</fetch>`

    return fetch(
        `/api/data/v9.0/clp_proximitycardapplicants?fetchXml=${encodeURI(fetchXml)}`,
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

ShareSdk.compareDate = (a, b) => {
    if (a["pc.clp_latest_modified_on"] > b["pc.clp_latest_modified_on"]) return 1;
    if (b["pc.clp_latest_modified_on"] > a["pc.clp_latest_modified_on"]) return -1;

    return 0;
}

ShareSdk.orderByLatestModifiedOn = (array) => {
    return array.sort(ShareSdk.compareDate).reverse()
}

//************************************* */
/**
 *  "SELECT d.REQ_ID, d.DTL_ID, d.DTL_STATUS, d.REMARKS, substring(convert(char,d.COLLECT_DATE,104),1,10) as COLLECT_DATE2, d.COLLECT_BY, d.COLLECT_CANCEL" _
        & " FROM SSMS_PC_DTL d INNER JOIN SSMS_PC_MST m ON m.REQ_ID = d.REQ_ID AND m.WF_STATUS <> '" & PrepareSQLPar(PC_REQSTATUS_CAN) & "'" _
        & " WHERE d.CARD_ID = '" & PrepareSQLPar(iCardId) & "'" _
        & " AND ISNULL(d.DTL_STATUS,'') NOT IN ('" & PrepareSQLPar(PC_DTLSTATUS_CAN) & "','" & PrepareSQLPar(PC_DTLSTATUS_REJ) & "','" & PrepareSQLPar(PC_DTLSTATUS_CMP) & "')"
 * @param {*} cardid 
 * @returns 
 */
ShareSdk.getValidPROXCardREQDetailsAsync = (cardid) => {

    var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical">
  <entity name="clp_proximity_card_request_detail">
    <all-attributes />
    <filter>
      <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
      <condition attribute="clp_detail_status" operator="not-in" value="">
        <value>768230004</value>
        <value>768230005</value>
        <value>768230006</value>
      </condition>
    </filter>
    <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" alias="request">
      <attribute name="clp_proximitycardrequestid" />
      <attribute name="clp_name" />
      <filter>
        <condition attribute="clp_workflow_status" operator="ne" value="768230006" />
      </filter>
    </link-entity>
    <link-entity name="clp_proximitycardinventory" from="clp_proximitycardinventoryid" to="clp_proximity_card" alias="card" >
        <attribute name="clp_name" />
    </link-entity>
  </entity>
</fetch>`
    return fetch(`/api/data/v9.0/clp_proximity_card_request_details?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.deletePROXCardREQDTLAsync = (detailid) => {
    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${detailid})`,
        {
            method: "DELETE",
        }
    ).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve()
    }).catch(err => {
        throw new Error(err.message)
    })
}

ShareSdk.getPROXCardRequestDetailsAsync = (requestGuid) => {
    // return fetch(`/api/data/v9.0/clp_proximitycardrequests(${requestGuid})?$expand=clp_pc_request_detail_pc_request($expand=clp_proximity_card($select=clp_card_status,clp_expirydate))`,
    //     {
    //         headers: {
    //             "Content-Type": "application/json",
    //             'Prefer': 'odata.include-annotations="*"',
    //         }
    //     }
    // ).then(res => res.json()).then(res => {

    //     if (res.error) {
    //         throw new Error(res.error.message);
    //     }

    //     return Promise.resolve(res.clp_pc_request_detail_pc_request)


    // }).catch(err => {
    //     console.error(err)
    //     throw err;
    // })

    return fetch(`/api/data/v9.0/clp_proximity_card_request_details?$filter=_clp_proximity_card_request_value eq '${requestGuid}'&$expand=clp_proximity_card`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getPROXCardRequestDetailsForExtendAsync = (requestGuid) => {
    var fetchXml = `<fetch>
  <entity name="clp_proximity_card_request_detail">
    <attribute name="clp_collected_date" />
    <attribute name="clp_proximity_card_request_detailid" />
    <attribute name="clp_proximity_card_request" />
    <attribute name="clp_detail_status" />
    <attribute name="overriddencreatedon" />
    <attribute name="clp_app_type" />
    <attribute name="clp_applicant_name" />
    <attribute name="clp_contractor" />
    <attribute name="clp_proximity_card" />
    <attribute name="clp_collected_by" />
    <attribute name="clp_collect_cancelled" />
    <attribute name="clp_contractor_id" />
    <attribute name="clp_card_expiry_date" />
    <attribute name="clp_proximity_card_applicant" />
    <attribute name="clp_card_no" />
    <attribute name="clp_phone_no" />
    <filter>
      <condition attribute="clp_returned_by" operator="null" />
      <condition attribute="clp_detail_status" operator="not-in" value="">
        <value>768230004</value>
        <value>768230005</value>
      </condition>
    </filter>
    <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" alias="request">
      <attribute name="clp_is_migration_data" />
      <filter>
        <condition attribute="clp_proximitycardrequestid" operator="eq" value="${requestGuid}" />
      </filter>
    </link-entity>
    <link-entity name="clp_proximitycardinventory" from="clp_proximitycardinventoryid" to="clp_proximity_card" alias="card">
      <attribute name="clp_holder" />
      <attribute name="clp_card_expiry_date" />
      <attribute name="clp_name" />
    </link-entity>
  </entity>
</fetch>`

    return fetch(`/api/data/v9.0/clp_proximity_card_request_details?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.updatePROXCardREQDTLAsync = (detailid, applicantid, cardid, USERID) => {

    var body = {
        "clp_latest_modified_on": new Date()
    }
    if (applicantid) {
        body["clp_proximity_card_applicant@odata.bind"] = `/clp_proximitycardapplicants(${applicantid})`
    }

    if (cardid) {

        body["clp_proximity_card@odata.bind"] = `/clp_proximitycardinventories(${cardid})`
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }


    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${detailid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.rejectPROXCardREQDTLAsync = (detailid) => {

    var body = {
        "clp_detail_status": ShareSdk.PROXCardIndividualStatus.Rejected
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${detailid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}


ShareSdk.getPROXCardByIdAsync = (id) => {

    return fetch(`/api/data/v9.0/clp_proximitycardinventories(${id})?$expand=clp_replace_card`,
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
        return Promise.resolve(res)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

/**
 * get proximity card inventory by contractor id
 * @param {*} contractorid 
 * @returns 
 */
ShareSdk.getPROXCardsByContractorIdAsync = (contractorid) => {

    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=clp_contractorid eq '${contractorid}'`,
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

ShareSdk.getPROXCardByCardNoAsync = (cardno) => {

    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=clp_name eq '${cardno}'&$expand=clp_holder`,
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

ShareSdk.getPROXCardsAsync = (names) => {

    var condtionslist = names.map(p => `clp_name eq '${p}'`)

    const condtions = condtionslist.join(' or ')
    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=${condtions} `,
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

ShareSdk.getReplaceCardsAsync = (cardid) => {
    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=_clp_replace_card_value eq '${cardid}'`,
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


ShareSdk.getTimeGapAsync = (cardid) => {
    //     var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical" aggregate="true" >
    //     <entity name="clp_proximity_card_request_detail" >

    //         <filter type="and" >
    //             <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
    //             <condition attribute="clp_detail_status" operator="in" >
    //                 <value>
    //                     ${ShareSdk.PROXCardIndividualStatus.WaitingforCardCollection}
    //                 </value>
    //                 <value>
    //                     ${ShareSdk.PROXCardIndividualStatus.Issued}
    //                 </value>
    //             </condition>
    //         </filter>
    //         <link-entity name="clp_proximitycardrequest" to="clp_proximity_card_request" from="clp_proximitycardrequestid" link-type="inner" >
    //             <attribute name="clp_accessperiodfrom" aggregate="min" alias="accessperiodfrom_min" />
    //             <attribute name="clp_accessperiodto" aggregate="max" alias="accessperiodto_max" />
    //             <filter type="and" >
    //                 <condition attribute="clp_workflow_status" operator="eq" value="${ShareSdk.PROXCardReqStatus.RequestCompleted}" />
    //             </filter>
    //         </link-entity>
    //     </entity>
    // </fetch>`

    var body = {
        "clp_ssms_pc_card_id": cardid
    }

    return fetch(
        `/api/data/v9.0/clp_ssms_pc_gettimegap`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(JSON.parse(res["clp_ssms_pc_get_time_gap_response_result"]))
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getApplicantDeatilsAsync = (contractorid, accessfrom, accessto, scadacode, currentFormId) => {
    var body = {
        "clp_ssms_pc_contractorid": contractorid,
        "clp_ssms_pc_accessfrom": accessfrom,
        "clp_ssms_pc_accessto": accessto,
        "clp_ssms_pc_scadacode": scadacode,
        "clp_ssms_pc_currentformid": currentFormId
    }

    return fetch(
        `/api/data/v9.0/clp_ssms_pc_getapplicantdetail`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(JSON.parse(res["clp_ssms_pc_get_applicant_detail_response_result"]))
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

// ShareSdk.addNewCardAsync = (CARD_NO, CARD_TYPE, CARD_STATUS, CARD_DEPARTMENT) => {

//     var formData = {
//         "clp_name": CARD_NO,
//         "clp_card_type": CARD_TYPE,
//         "clp_card_status": CARD_STATUS,
//         // "clp_department": `/clp_department(${CARD_DEPARTMENT})`,
//     }
//     return fetch(
//         `/api/data/v9.0/clp_proximitycardinventories`,
//         {
//             method: "POST",
//             headers: {
//                 "Content-Type": "application/json",
//                 'Prefer': 'return=representation',
//             },
//             body: JSON.stringify(formData)
//         }   
//     ).then(res => res.json()).then(res => {
//         if (res.error) {
//             throw new Error(res.error.message)
//         }
//         return res
//     })
// }

ShareSdk.addNewCardAsync = (CARD_NO, CARD_TYPE, CARD_STATUS, CARD_EXPIRY_DATE, ISSUE_DEPT, CARD_HOLDER_ID, CardPrefix, USERID) => {
    var formData = {
        "clp_name": CARD_NO,
        "clp_card_type": CARD_TYPE,
        "clp_card_status": CARD_STATUS,
        "clp_latest_modified_on": new Date()
    }

    if (CARD_EXPIRY_DATE) {
        formData["clp_card_expiry_date"] = CARD_EXPIRY_DATE
    }
    if (ISSUE_DEPT) {
        formData["clp_issuedepartment"] = ISSUE_DEPT
    }

    if (CARD_HOLDER_ID) {
        formData["clp_holder@odata.bind"] = `/clp_proximitycardapplicants(${CARD_HOLDER_ID})`
    }
    if (CardPrefix) {
        formData["clp_cardprefix"] = CardPrefix
    }

    if (USERID) {
        formData["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.updatePROXCardStatusAsync = (id, status, USERID) => {
    var body = {
        "clp_card_status": status,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories(${id})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.resetPROXCardAvailableAsync = (cardid, USERID) => {
    var body = {
        "clp_card_status": ShareSdk.ProximityCardStatus.Available,
        "clp_holder@odata.bind": null,
        "clp_responsible_engineer@odata.bind": null,
        "clp_expirydate": null,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories(${cardid})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
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
 * @param {*} cardid 
 * @param {Applicant} HOLDER 
 * @param {clp_user} REQEUST 
 * @param {clp_user} RE 
 * @param {*} EXPIRY_DATE 
 * @returns 
 */
ShareSdk.updatePROXCardInfoAsync = (cardid, HOLDER, REQEUST, RE, EXPIRY_DATE, USERID) => {
    var body = {
        "clp_holder@odata.bind": `/clp_proximitycardapplicants(${HOLDER})`,
        "clp_responsible_engineer@odata.bind": `/clp_users(${RE})`,
        "clp_expirydate": EXPIRY_DATE,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    if (REQEUST) {
        body["clp_proximity_card_request@odata.bind"] = `/clp_proximitycardrequests(${REQEUST})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories(${cardid})`,
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

ShareSdk.updateCardExpiryDateAsync = (id, expirydate, USERID) => {
    var body = {
        "clp_card_expiry_date": expirydate,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximitycardinventories(${id})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                "Prefer": "return=representation"
            },
            body: JSON.stringify(body)
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message)
        }

    })
}

//***********************PROXIMITY CARD REQUEST DETAIL ************** */

ShareSdk.getPROXCardREQDTLAsync = (carid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_request_details?$filter=_clp_proximity_card_value eq ${carid}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getPROXCardREQDTLByIdAsync = (detailId) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_request_details(${detailId})?$expand=clp_proximity_card_request`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getAllPROXCardREQDTLsAsync = (requestGuid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_request_details?$filter=_clp_proximity_card_request_value eq ${requestGuid}&$expand=clp_proximity_card_applicant`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)

    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.updatePROXCardREQDTLStatusAsync = (id, status, USERID) => {
    var body = {
        "clp_detail_status": status,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${id})`,
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

ShareSdk.updatePROXCardREQDTLStatusForReturnAsync = (id, status, returnedby, RETURN_DATE, RETURN_REG_DATE, USERID) => {
    var body = {
        "clp_detail_status": status,
        "clp_returned_by@odata.bind": `/clp_users(${returnedby})`,
        "clp_return_reg_date": RETURN_REG_DATE,
        "clp_returned_date": RETURN_DATE,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${id})`,
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

ShareSdk.updatePROXCardREQDTLCardAsync = (id, newcardid, USERID) => {
    var body = {
        "clp_card@odata.bind": `/clp_proximitycardinventories(${newcardid})`,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${id})`,
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

ShareSdk.updatePROXCardREQDTLForEncodeReplacementAsync = (id, REPLACE_BY, REMARKS, USERID) => {
    var body = {
        "clp_replaced_by@odata.bind": `/clp_proximitycardinventories(${REPLACE_BY})`,
        "clp_remarks": REMARKS,
        "clp_latest_modified_on": new Date()
    }

    if (USERID) {
        body["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${id})`,
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

ShareSdk.resetPROXCardREQDTLAsync = (id, isresetcardid = false) => {
    var body = {
        "clp_detail_status": null,
        "clp_remarks": null,
        "clp_latest_modified_on": new Date()
    }

    if (isresetcardid) {
        body["clp_proximity_card@odata.bind"] = null
    }



    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details(${id})`,
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

/**
 * 
 * @param {*} REQ_ID 
 * @param {*} DTL_STATUS 
 * @param {*} APP_TYPE 
 * @param {*} APPLICANT_ID 
 * @param {*} APPLICANT_NAME 
 * @param {*} CTR_ID 
 * @param {*} CARD_ID 
 * @param {*} PHONE 
 * @param {*} PHOTO 
 * @returns 
 */
ShareSdk.addNewPROXCardRequestDetailAsync = (REQ_ID, DTL_STATUS, APP_TYPE, APPLICANT_ID,
    APPLICANT_NAME, Contractor, CTR_ID, CardNo, CARD_ID, PHONE, ExpiryDate, USERID) => {
    var formData = {
        "clp_detail_status": DTL_STATUS,
        "clp_app_type": APP_TYPE,
        "clp_applicant_name": APPLICANT_NAME,
        "clp_phone_no": PHONE,
        "clp_card_expiry_date": ExpiryDate,
        "clp_contractor_id": CTR_ID,
        "clp_card_no": CardNo,
        "clp_latest_modified_on": new Date()
    }

    if (REQ_ID) {
        formData["clp_proximity_card_request@odata.bind"] = `/clp_proximitycardrequests(${REQ_ID})`
    }

    if (CARD_ID) {
        formData["clp_proximity_card@odata.bind"] = `/clp_proximitycardinventories(${CARD_ID})`
    }
    if (APPLICANT_ID) {
        formData["clp_proximity_card_applicant@odata.bind"] = `/clp_proximitycardapplicants(${APPLICANT_ID})`
    }

    if (Contractor) {
        formData["clp_contractor@odata.bind"] = `/clp_users(${Contractor})`
    }

    if (USERID) {
        formData["clp_latest_modified_by@odata.bind"] = `/clp_users(${USERID})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.getPROXCardRequestsCountAsync = (issueDept) => {

    // const getFirstandLastDayofCurrentMonth = () => {
    //     let firstDate = new Date();
    //     let startDate = firstDate.getFullYear() + "-" + ((firstDate.getMonth() + 1) < 10 ? "0" : "") + (firstDate.getMonth() + 1) + "-" + "01";

    //     let date = new Date();
    //     let currentMonth = date.getMonth();
    //     let nextMonth = ++currentMonth;
    //     let nextMonthFirstDay = new Date(date.getFullYear(), nextMonth, 1);
    //     let oneDay = 1000 * 60 * 60 * 24;
    //     let lastDate = new Date(nextMonthFirstDay - oneDay);
    //     let endDate = lastDate.getFullYear() + "-" + ((lastDate.getMonth() + 1) < 10 ? "0" : "") + (lastDate.getMonth() + 1) + "-" + (lastDate.getDate() < 10 ? "0" : "") + lastDate.getDate();

    //     return { startDate: startDate, endDate: endDate }
    // }

    // var returnDate = getFirstandLastDayofCurrentMonth()

    var fetchXml = `<fetch>
  <entity name="clp_proximitycardrequest">
    <attribute name="clp_name" />
    <filter>
      <condition attribute="clp_workflow_status" operator="not-null" />
      <condition attribute="clp_issuedepartment" operator="eq" value="${issueDept}" />
      <condition attribute="clp_requestdate" operator="this-month" />
    </filter>
  </entity>
</fetch>`
    return fetch(`/api/data/v9.0/clp_proximitycardrequests?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })

    // var condition = `clp_workflow_status ne null and clp_issuedepartment eq ${issueDept} and createdon gt ${returnDate.startDate} and createdon le ${returnDate.endDate} `
    // return fetch(`/api/data/v9.0/clp_proximitycardrequests?$select=clp_name&$filter=${condition}&$count=true`,
    //     {
    //         headers: {
    //             "Content-Type": "application/json",
    //             'Prefer': 'odata.include-annotations="*"',
    //         }
    //     }
    // ).then(res => res.json()).then(res => {
    //     return Promise.resolve(res.value)


    // }).catch(err => {
    //     console.error(err)
    //     throw err;
    // })
}

/**********************************ENODE & DECODE************************************* */

ShareSdk.getPROXCardSubstationEncodeListAsync = (cardid, ss_code) => {

    condtion = ss_code ? ` and clp_ss_code eq '${ss_code}' ` : ''
    return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$filter=_clp_card_value eq '${cardid}' ${condtion} `,
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

/**
 * Insert Card Encoding
 * @param {*} REQ_ID 
 * @param {*} CARD_ID 
 * @param {*} SS_CODE 
 * @returns 
 */
ShareSdk.addPROXCardDecodeAsync = (REQ_ID, CARD_ID, SS_CODE, DECODE_BY) => {
    var formData = {
        "clp_proximity_card_request@odata.bind": `/clp_proximitycardrequests(${REQ_ID})`,
        "clp_card@odata.bind": `/clp_proximitycardinventories(${CARD_ID})`,
        "clp_ss_code": SS_CODE,
        "clp_decoded_date": new Date()
    }

    if (DECODE_BY) {
        formData["clp_decoded_by@odata.bind"] = `/clp_users(${DECODE_BY})`
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_decodes`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

ShareSdk.getPROXCardEncodeListAsync = (cardid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$filter=_clp_card_value eq '${cardid}'&$expand=clp_proximity_card_request,clp_card`,
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

ShareSdk.isPROXCardDecodeAllAsync = async (cardid) => {
    var result = await ShareSdk.getPROXCardEncodeListAsync(cardid)
    return result.length === 0
}

ShareSdk.getPROXCardEncodesByCardandReqAsync = (cardid, reqid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$filter=_clp_card_value eq '${cardid}' and _clp_proximity_card_request_value eq '${reqid}'&$count=true`,
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

ShareSdk.getPROXCardEncodesByReqAsync = (reqid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$filter=_clp_proximity_card_request_value eq '${reqid}'&$count=true`,
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

/**
 * Check if all the SS in that Request were decoded
 * @param {*} cardid 
 * @param {*} requestid 
 * @returns 
 */
ShareSdk.isPROXCardREQDecodeAllAsync = (cardid, requestid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$filter=_clp_card_value eq '${cardid}' and _clp_proximity_card_request_value eq '${requestid}'`,
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
        return Promise.resolve(res.value.length === 0)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

/**
 * 
 * @param {*} REQ_ID 
 * @param {*} CARD_ID 
 * @param {*} SS_CODE 
 * @returns 
 */
ShareSdk.isPROXCardEncodeExistsAsync = (REQ_ID, CARD_ID, SS_CODE) => {


    return fetch(
        `/api/data/v9.0/clp_proximity_card_encodes?$filter=clp_scadacode eq '${encodeURIComponent(SS_CODE)}' and _clp_proximity_card_request_value eq '${REQ_ID}' and _clp_card_value eq '${CARD_ID}'`,
        {

            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },

        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve(res.value)
    })
}

/**
 * 
 * @param {*} REQ_ID 
 * @param {*} CARD_ID 
 * @param {*} SS_CODE 
 * @returns 
 */
ShareSdk.addPROXCardEncodeAsync = (REQ_ID, CARD_ID, SS_CODE) => {
    var formData = {
        "clp_proximity_card_request@odata.bind": `/clp_proximitycardrequests(${REQ_ID})`,
        "clp_card@odata.bind": `/clp_proximitycardinventories(${CARD_ID})`,
        "clp_ss_code": SS_CODE,
        "clp_encoded_date": new Date()
    }


    return fetch(
        `/api/data/v9.0/clp_proximity_card_encodes`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
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
 * @param {*} REQ_ID 
 * @param {*} CARD_ID 
 * @param {*} SS_CODE 
 * @returns 
 */
ShareSdk.addPROXCardEncodeDataAsync = async (REQ_ID, CARD_ID, SS_CODE) => {
    var data = await ShareSdk.isPROXCardEncodeExistsAsync(REQ_ID, CARD_ID, SS_CODE)
    if (data.length == 0) {
        return await ShareSdk.addPROXCardEncodeAsync(REQ_ID, CARD_ID, SS_CODE)
    }

    return Promise.resolve()
}


ShareSdk.DeleteEncodeAsync = (encodeid) => {
    return fetch(
        `/api/data/v9.0/clp_proximity_card_encodes(${encodeid})`,
        {
            method: "DELETE",
        }
    ).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return Promise.resolve()
    }).catch(err => {
        throw new Error(err.message)
    })
}

/*******************************substation************************************* */

ShareSdk.getSubstationByScadaCodeAsync = (scadaCode) => {
    return fetch(`/api/data/v9.0/clp_ssms_lut_substations?$filter=clp_scadacode eq '${encodeURIComponent(scadaCode)}'`,
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

        return Promise.resolve(res.value[0])


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

/**
 * 
 * @param {*} stations SSLIST
 * @param {*} type  G/S
 * @param {*} category PC
 * @param {*} region NR/WE
 * @returns 
 */
ShareSdk.getSubstationsAsync = (stations, type, category, region) => {
    let fetchXml = ''
    if (!stations.some(p => p === 'ALL')) {
        const conditions = stations.map(p => `<value>${encodeURIComponent(p)}</value>`).join(' ')

        var extraFilter = ''
        if (stations.some(p => p === 'WE')) {
            extraFilter += `<condition attribute="clp_region" operator="eq" value="768230000" />`
        }
        if (stations.some(p => p === 'NR')) {
            extraFilter += `<condition attribute="clp_region" operator="eq" value="768230001" />`
        }

        if (stations.some(p => p === 'NRHV')) {
            extraFilter += `<condition attribute="clp_hv_ehv" operator="eq" value="NRHV" />`
        }

        if (stations.some(p => p === 'WEHV')) {
            extraFilter += `<condition attribute="clp_hv_ehv" operator="eq" value="WEHV" />`
        }

        if (stations.some(p => p === 'WEEHV')) {
            extraFilter += `<condition attribute="clp_hv_ehv" operator="eq" value="WEEHV" />`
        }

        if (stations.some(p => p === 'NREHV')) {
            extraFilter += `<condition attribute="clp_hv_ehv" operator="eq" value="NREHV" />`
        }

        fetchXml = `<fetch distinct="true">
  <entity name="clp_ssms_lut_substation">
    <attribute name="clp_ssms_lut_substationid" />
    <attribute name="clp_name" />
    <attribute name="clp_scadacode" />
    <attribute name="clp_regions" />
    <filter type="and">
      <condition attribute="clp_type" operator="eq" value="${type}" />
      ${region ? `<condition attribute="clp_region" operator="eq" value="${region}" />` : ''}
      <filter type="or">
        <condition attribute="clp_category" operator="null" />
        <condition attribute="clp_category" operator="eq" value="${category}" />
      </filter>
      <filter type="or">
        <condition attribute="clp_scadacode" operator="in">
            ${conditions}
        </condition>
        ${extraFilter}
      </filter>
    </filter>
     
  </entity>
</fetch>`
    } else {
        fetchXml = `<fetch distinct="true">
  <entity name="clp_ssms_lut_substation">
    <attribute name="clp_ssms_lut_substationid" />
    <attribute name="clp_name" />
    <attribute name="clp_scadacode" />
    <attribute name="clp_regions" />
    <filter type="and">
      <condition attribute="clp_type" operator="eq" value="${type}" />
      ${region ? `<condition attribute="clp_region" operator="eq" value="${region}" />` : ''}
      <filter type="or">
        <condition attribute="clp_category" operator="null" />
        <condition attribute="clp_category" operator="eq" value="${category}" />
      </filter>
    </filter>
     
  </entity>
</fetch>`
    }

    return fetch(`/api/data/v9.0/clp_ssms_lut_substations?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })

}

// ShareSdk.getSubstationsAsync = async (requestGuid, category, region) => {

//     var stations = await ShareSdk.getPROXCardSubstationsAsync(requestGuid)
//     stationcodes = stations.map(p => p.clp_scadacode).join()
//     return await ShareSdk.getGroupSubstationsAsync(stationcodes, category, region)

// }

ShareSdk.getGroupSubstationsAsync = (ssid, region) => {
    var fetchXml = `<fetch>
    <entity name="clp_ssms_lut_substation" >
        <attribute name="clp_scadacode" />
        <filter>
            ${region ? `<condition attribute="clp_region" operator="eq" value="${region}" />` : ""}
            <filter type="or" >
                <condition attribute="clp_group_1" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_2" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_3" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_4" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_5" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_6" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_7" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_8" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_9" operator="eq" value="${ssid}" />
                <condition attribute="clp_group_10" operator="eq" value="${ssid}" />
            </filter>
        </filter>
    </entity>
</fetch>`
    return fetch(`/api/data/v9.0/clp_ssms_lut_substations?fetchXml=${encodeURI(fetchXml)}`,
        {
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'odata.include-annotations="*"',
            }
        }
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message);
        }

        return Promise.resolve(res.value)


    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getSubstationsCountAsync = async (requestGuid, category, region) => {

    var stations = await ShareSdk.getPROXCardSubstationsAsync(requestGuid)
    stationcodes = stations.map(p => p.clp_scadacode)

    const groupSubstations = await ShareSdk.getSubstationsAsync(stationcodes,
        ShareSdk.SubstationType.Group, category)

    const Substations = await ShareSdk.getSubstationsAsync(stationcodes,
        ShareSdk.SubstationType.Substation, category, region)

    let scadaCodes = Substations.map(p => p.clp_scadacode)

    for (var item of groupSubstations) {
        let result = await ShareSdk.getGroupSubstationsAsync(item.clp_ssms_lut_substationid, region)
        scadaCodes = [...scadaCodes, ...result.map(p => p.clp_scadacode)]
    }

    return Array.from(new Set(scadaCodes)).length
}

/**
 * SSMS_GetSsFullList
 */
ShareSdk.getStationFullListAsync = async (requestGuid, category) => {

    var stations = await ShareSdk.getPROXCardSubstationsAsync(requestGuid)
    stationcodes = stations.map(p => p.clp_scadacode)

    const groupSubstations = await ShareSdk.getSubstationsAsync(stationcodes,
        ShareSdk.SubstationType.Group, category)

    const Substations = await ShareSdk.getSubstationsAsync(stationcodes,
        ShareSdk.SubstationType.Substation, category)

    let scadaCodes = Substations.map(p => p.clp_scadacode)

    for (var item of groupSubstations) {
        let result = await ShareSdk.getGroupSubstationsAsync(item.clp_ssms_lut_substationid)
        scadaCodes = [...scadaCodes, ...result.map(p => p.clp_scadacode)]
    }

    console.log(Array.from(new Set(scadaCodes)))
    return Array.from(new Set(scadaCodes))
}

/***************************Expiry Log******************************* */

ShareSdk.getExpityLogAsync = (cardid, holderid) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_expiry_logs?$filter=_clp_proximity_card_value eq '${cardid}' and ${holderid ? `_clp_holder_value eq '${holderid}'` : ``}  and clp_returned_date eq null`,
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

/**
 * Update Expiry Event Log
 */
ShareSdk.updateExpiryLogAsync = (logid, RETURN_DATE, RETURN_REG_DATE) => {
    var body = {
        "clp_returned_date": RETURN_DATE,
    }

    if (RETURN_REG_DATE) {
        body["clp_returned_reg_date"] = RETURN_REG_DATE
    }

    return fetch(
        `/api/data/v9.0/clp_proximity_card_expiry_logs(${logid})`,
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

/**************************************Audit*******************************************8 */

ShareSdk.getAduitDetailAsync = (detailId) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_audit_details(${detailId})`,
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
        return Promise.resolve(res)
    }).catch(err => {
        console.error(err)
        throw err;
    })
}


ShareSdk.getAduitAsync = (auditId) => {
    return fetch(`/api/data/v9.0/clp_proximity_card_audits?$filter=clp_proximity_card_auditid eq ${auditId}`,
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

/*****************************DELEGATION************************************************/

ShareSdk.getDelegateesAsync = (userid) => {
    return fetch(`/api/data/v9.0/clp_uam_outofoffices?$filter=_clp_delegator_value eq '${userid}' and clp_enabled eq true`,
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
/*****************************NOTIFICATION********************8 */

ShareSdk.addPROXCardNotificationAsync = (cardid, type) => {
    var formData = {
        "clp_type": type,
        "clp_proximity_card@odata.bind": `/clp_proximitycardinventories(${cardid})`
    }
    return fetch(
        `/api/data/v9.0/clp_proximity_card_notifications`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}
/******************************************************** */

ShareSdk.addSyncDataAsync = (type, requestGuid, cardGuid, parameter1, parameter2, parameter3, parameter4) => {
    var formData = {
        "clp_type": type,
    }
    if (requestGuid) {

        formData["clp_pc_request@odata.bind"] = `/clp_proximitycardrequests(${requestGuid})`
    }

    if (cardGuid) {

        formData["clp_pc_card@odata.bind"] = `/clp_proximitycardinventories(${cardGuid})`
    }

    if (parameter1) {
        formData["clp_parameter1"] = parameter1
    }

    if (parameter2) {
        formData["clp_parameter2"] = parameter2
    }

    if (parameter3) {
        formData["clp_parameter3"] = parameter3
    }

    if (parameter4) {
        formData["clp_parameter4"] = parameter4
    }


    return fetch(
        `/api/data/v9.0/clp_proximity_card_data_sync_events`,
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify(formData)
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

/*********************ENV***************************** */

ShareSdk.getEnvVariableDefinitionAsync = (schemaname) => {
    return fetch(`/api/data/v9.0/environmentvariabledefinitions?$filter=schemaname eq '${schemaname}'`,
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
        return Promise.resolve(res.value[0])
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getEnvVariableValueAsync = (envariabledefinitionid) => {
    return fetch(`/api/data/v9.0/environmentvariablevalues?$filter=_environmentvariabledefinitionid_value eq '${envariabledefinitionid}'`,
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
        return Promise.resolve(res.value[0])
    }).catch(err => {
        console.error(err)
        throw err;
    })
}

ShareSdk.getEnvVariableAsync = async (schemaname) => {
    const definition = await ShareSdk.getEnvVariableDefinitionAsync(schemaname)

    const result = await ShareSdk.getEnvVariableValueAsync(definition.environmentvariabledefinitionid)
    return result.value

}

ShareSdk.getMappingValue = async (key) => {

    return await fetch(
        `/api/data/v9.0/clp_ssms_system_configurations/?$filter=clp_name eq '${key}'`,
    ).then(res => res.json()).then(res => {

        if (res.error) {
            throw new Error(res.error.message)
        }

        if (res.value.length === 0) {
            return Promise.resolve(null)
        }

        return Promise.resolve(res.value[0].clp_value)
    })
}

ShareSdk.callCCRSAPIAsync = (url, apikey, body) => {

    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest()

        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.setRequestHeader("x-api-key", apikey)
        xhr.timeout = 20000;
        xhr.onload = () => {
            if (xhr.status === 200) {
                const resData = JSON.parse(xhr.responseText)

                resolve(resData)
            } else {
                // 请求失败，创建一个错误对象，返回错误文本
                reject(new Error("call api error"))
            }
        }

        xhr.onerror = function () {
            reject(Error("Network Error"));
        };

        xhr.ontimeout = function (e) {
            reject(Error("Time out"));
        };

        xhr.send(JSON.stringify(body))
    })


}

ShareSdk.callLAMSAPIAsync = async (param1, param2, param3, param4) => {

    const apijson = await ShareSdk.getEnvVariableAsync('clp_ssms_clp_api')

    const jsonValue = JSON.parse(apijson)

    const body = {

    }

    if (param1) {
        body["Para1"] = param1
    }

    if (param2) {
        body["Para2"] = param2
    }

    if (param3) {
        body["Para3"] = param3
    }

    if (param4) {
        body["Para4"] = param4
    }

    // var url = await ShareSdk.getMappingValue("SSMS_Internal_API_Address")


    // var url = `https://prod-08.southeastasia.logic.azure.com:443/workflows/c1a9aa71d6724e12ad99ad2a090e927e/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=jXEkabGzvhjL2Tgn_3mRinLSB3pM7h9cpxI9nGIxuuU`
    return await ShareSdk.sendLAMSrequestAsync(jsonValue.url, jsonValue.key, body)
    // return await ShareSdk.sendLAMSrequestAsync(jsonValue.url, body)

}

ShareSdk.sendLAMSrequestAsync = (url, apikey, body) => {
    return new Promise((resolve, reject) => {
        const xhr = new XMLHttpRequest()

        xhr.open("POST", url, true);
        xhr.setRequestHeader("Content-Type", "application/json")
        xhr.setRequestHeader("x-api-key", apikey)
        xhr.timeout = 20000;
        xhr.onload = () => {
            if (xhr.status === 200) {
                const resData = JSON.parse(xhr.responseText)

                resolve(resData)
            } else {
                // 请求失败，创建一个错误对象，返回错误文本
                reject(new Error("call api error"))
            }
        }

        xhr.onerror = function () {
            reject(Error("Network Error"));
        };

        xhr.ontimeout = function (e) {
            reject(Error("Time out"));
        };

        xhr.send(JSON.stringify(body))
    })
}

ShareSdk.formateDateTime = (date, fmt) => {
    var o = {
        "M+": date.getMonth() + 1, //月份
        "d+": date.getDate(), //日
        "H+": date.getHours(), //小时
        "m+": date.getMinutes(), //分
        "s+": date.getSeconds(), //秒
        "q+": Math.floor((date.getMonth() + 3) / 3), //季度
        "S": date.getMilliseconds() //毫秒
    };
    if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

/**
 * 1: Success
2: Error
3: Warning
4: Information
 * @param {*} message 
 * @param {*} timspan 
 */
ShareSdk.showGlobalNotification = (message, level = 4, timspan = 10000) => {

    var notification =
    {
        type: 2,
        level: level,
        message: message,
        showCloseButton: true
    };

    return Xrm.App.addGlobalNotification(notification).then(
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

ShareSdk.openAlertDialog = (message) => {
    var alertStrings = { confirmButtonLabel: "Yes", text: message, title: "Message" };
    var alertOptions = { height: 120, width: 260 };
    return Xrm.Navigation.openAlertDialog(alertStrings, alertOptions)
}

ShareSdk.setRequiredControl = (name, required = true) => {
    Xrm.Page.data.entity.attributes.getByName(name).setRequiredLevel(required ? "required" : "none")
}

ShareSdk.setDisableControl = (name, disabled = true) => {
    Xrm.Page.data.entity.attributes.getByName(name).controls.getAll()[0]?.setDisabled(disabled);
}

ShareSdk.setVisiableControl = (field, visibile = true) => {
    // Xrm.Page.data.entity.attributes.getByName(field).controls.getAll()[0].setVisible(visibile)
    Xrm.Page.getControl(field).setVisible(visibile)
}

ShareSdk.setLookupValue = function (id, name, tableName, fieldName) {
    var previousValue = Xrm.Page.data.entity.attributes.getByName(fieldName)?.getValue()
    if (!previousValue) {
        var lookupData = new Array();
        var lookupItem = new Object();
        lookupItem.id = id;
        lookupItem.name = name;
        lookupItem.entityType = tableName;
        lookupData[0] = lookupItem;
        Xrm.Page.data.entity.attributes.getByName(fieldName)?.setValue(lookupData);
    }

}

ShareSdk.formError = (err) => {
    const msgKey = new Date().getTime().toString(16)
    Xrm.Page.ui.formContext.ui.setFormNotification(
        err.message,
        'ERROR',
        msgKey);

    window.setTimeout(function () {
        Xrm.Page.ui.formContext.ui.clearFormNotification(msgKey);
    }, 10000);
}



ShareSdk.padDigitToString = (num, maxlength = 2) => {
    return num.toString().padStart(maxlength, '0');
}

ShareSdk.delay = (ms) => {
    return new Promise(resolve => setTimeout(resolve, ms));
}

/****************************************************************/

ShareSdk.Mid = (String, Start, Length) => {
    if (String == null)
        return (false);

    if (Start > String.length)
        return '';

    if (Length == null || Length.length == 0)
        return (false);

    return String.substr((Start - 1), Length);
}

ShareSdk.datetimeDifferent = (ToDate, FromDate) => {
    var ToDay = Mid(ToDate, 1, 2);
    var ToMonth = Mid(ToDate, 4, 2);
    var ToYear = Mid(ToDate, 7, 4);
    var ToHour = Mid(ToDate, 12, 2);
    var ToMin = Mid(ToDate, 15, 2);
    var ToSecond = Mid(ToDate, 18, 2);
    var FromDay = Mid(FromDate, 1, 2);
    var FromMonth = Mid(FromDate, 4, 2);
    var FromYear = Mid(FromDate, 7, 4);
    var FromHour = Mid(FromDate, 12, 2);
    var FromMin = Mid(FromDate, 15, 2);
    var FromSecond = Mid(FromDate, 18, 2);

    var FrmDate = new Date(FromYear, FromMonth - 1, FromDay, FromHour, FromMin, FromSecond);
    var TomDate = new Date(ToYear, ToMonth - 1, ToDay, ToHour, ToMin, ToSecond);

    return (TomDate > FrmDate);
}

ShareSdk.dayDifferent2 = (ToDate, FromDate) => {
    var ToDay = Mid(ToDate, 1, 2);
    var ToMonth = Mid(ToDate, 4, 2);
    var ToYear = Mid(ToDate, 7, 4);
    var FromDay = Mid(FromDate, 1, 2);
    var FromMonth = Mid(FromDate, 4, 2);
    var FromYear = Mid(FromDate, 7, 4);
    return dayDifferent(ToDay, ToMonth, ToYear, FromDay, FromMonth, FromYear);
}

ShareSdk.dayDifferent3 = (ToDateValue) => {
    var ToDay = ToDateValue.getDate()
    var ToMonth = ToDateValue.getMonth()
    var ToYear = ToDateValue.getFullYear()
    var ToDate = new Date(ToYear, ToMonth - 1, ToDay, 0, 0, 0);


    var difference = Date.UTC(ToDate.getYear(), ToDate.getMonth(), ToDate.getDate(), 0, 0, 0) - Date.UTC(ToDateValue.getYear(), ToDateValue.getMonth(), ToDateValue.getDate(), 0, 0, 0);

    return difference / (1000 * 60 * 60 * 24);
}

ShareSdk.addlibAsync = async () => {
    var jscontent = await fetch('/WebResources/clp_xlsx.core.min.js', { method: 'GET' })
        .then(r => r.text());
    window.eval(jscontent);
    jscontent = await fetch('/WebResources/clp_xlsx.bundle.js', { method: 'GET' })
        .then(r => r.text());
    window.eval(jscontent);
}

ShareSdk.JSONRefStartElement = (obj, prop) => {
    if (prop === "REQ_ID") {
        return '<' + prop + ` ref_table="SSMS_PC_MST" ref_column="APP_NO" ref_value="${obj[prop]}" column="REQ_ID" ` + '>';
    }
    else if (prop === "CARD_ID") {
        return '<' + prop + ` ref_table="SSMS_PC_CARD" ref_column="CARD_NO" column="CARD_ID" ref_value="${obj[prop]}" ` + '>';
    }
    else if (prop === "REPLACE_CARD_ID") {
        return '<' + prop + ` ref_table="SSMS_PC_CARD" ref_column="CARD_NO" column="REPLACE_CARD_ID" ref_value="${obj[prop]}" ` + '>';
    }
    return '<' + prop + '>';
}
ShareSdk.JSONRefValueElement = (obj, prop) => {
    if (prop === "REQ_ID") {
        return '';
    }
    else if (prop === "CARD_ID") {
        return '';
    }
    else if (prop === "REPLACE_CARD_ID") {
        return '';
    }
    return obj[prop]
}
ShareSdk.JSONtoXML = (obj) => {
    let xml = '';
    for (let prop in obj) {

        xml += obj[prop] instanceof Array ? '' : ShareSdk.JSONRefStartElement(obj, prop);
        if (obj[prop] instanceof Array) {
            for (let array in obj[prop]) {
                xml += '<' + prop + '>';
                xml += ShareSdk.JSONtoXML(new Object(obj[prop][array]));
                xml += '</' + prop + '>';
            }
        } else if (typeof obj[prop] == 'object') {
            xml += ShareSdk.JSONtoXML(new Object(obj[prop]));
        } else {
            xml += ShareSdk.JSONRefValueElement(obj, prop);
        }
        xml += obj[prop] instanceof Array ? '' : '</' + prop + '>';
    }
    xml = xml.replace(/<\/?[0-9]{1,}>/g, '');
    return xml;
}

ShareSdk._doUntil = (doFunc, untilFunc, maxMs = 20000, delayMs = 50) => {
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