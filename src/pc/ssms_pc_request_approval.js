var PROXCardReqApprovalSDK = window.PROXCardReqApprovalSDK || {};

PROXCardReqApprovalSDK.onFormLoaded = () => {

    PROXCardReqApprovalSDK.changeFormType()

    formContext = Xrm.Page.ui.formContext
    const inputData = Xrm.Utility.getPageContext().input.data
    if (inputData.action === ShareSdk.ProximityCardAction.Reject) {

        const remarksCtls = [ShareSdk.PROXCardReqFields.appr_remarks,
        ShareSdk.PROXCardReqFields.reviewer_remarks,
        ShareSdk.PROXCardReqFields.mm_remarks,
        ShareSdk.PROXCardReqFields.admin_remarks,
        ShareSdk.PROXCardReqFields.security_remarks]

        remarksCtls.forEach(p => {
            if (formContext.getControl(p).getVisible()) {
                Xrm.Page.data.entity.attributes.getByName(p).setRequiredLevel("required")
            }
        })

    }
}

PROXCardReqApprovalSDK.changeFormType = () => {
    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Request_Approval) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Request_Approval);
        formItem.navigate();
    }
}

PROXCardReqApprovalSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        if (Xrm.Page.data.isValid()) {

            formContext = Xrm.Page.ui.formContext
            var formid = ShareSdk.getCurrentFormId()
            var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()

            var currentUserId = ShareSdk.getCurrentUserId()

            var user = await ShareSdk.getUserBySystemUserAsync(currentUserId)
            // var departmentBranch = (user[ShareSdk.CLPUserFields.dept_name] ?? '') + ' / ' + (user[ShareSdk.CLPUserFields.branch_name] ?? '')
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.latest_modified_on).setValue(new Date())
            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.latest_modified_by)

            const inputData = Xrm.Utility.getPageContext().input.data

            if (inputData?.action) {
                if (inputData.action === ShareSdk.ProximityCardAction.Approve) {
                    switch (status) {
                        case ShareSdk.PROXCardReqStatus.WaitingforApproval:

                            // var stations = await ShareSdk.getPROXCardSubstationsAsync(formid)

                            const wecnt = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_we).getValue()
                            const northcnt = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_nr).getValue()

                            if ((wecnt + northcnt) > 20) {
                                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforReview)
                            }
                            else {
                                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforCardAllocation)
                            }
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.appr_date).setValue(new Date())
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.approver)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.approver_deptbranch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforReview:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.reviewed_date).setValue(new Date())
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval)
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.reviewer)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.reviewer_dept_branch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_date).setValue(new Date())
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforCardAllocation)
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.mm)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.mm_dept_branch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforCardAllocation:
                        case ShareSdk.PROXCardReqStatus.RejectedbySecurity:
                            await window.parent.PROXCardReqValidatinoSDK.validateAlcCardsAsync()
                            await PROXCardReqApprovalSDK.updateStatusforAllocationAsync(formid, user)
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.card_allocation_date).setValue(new Date())
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforCardEncoding)
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.admin)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.admin_dept_branch, departmentBranch)

                            //await PROXCardReqApprovalSDK.syncRequestAsync(formid)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforCardEncoding:
                            await PROXCardReqApprovalSDK.updateStatusforEncodingAsync(formid)
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.card_encoding_date).setValue(new Date())
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.RequestCompleted)
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.security)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.security_dept_branch, departmentBranch)

                            PROXCardReqApprovalSDK.fillCollectionReminderDate()

                            break;
                        default:
                            return
                    }
                }
                else if (inputData.action === ShareSdk.ProximityCardAction.Reject) {

                    //update status
                    if (status != ShareSdk.PROXCardReqStatus.WaitingforCardEncoding) {


                        await PROXCardReqApprovalSDK.updateDetailforRejectionAsync(formid, user)
                        //Update Reserved -> Available for New ISMS

                        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.RequestRejected)
                    }
                    else {

                        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.RejectedbySecurity)
                    }

                    //update date
                    switch (status) {
                        case ShareSdk.PROXCardReqStatus.WaitingforApproval:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.appr_date).setValue(new Date())
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.approver)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.reviewer_dept_branch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforReview:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.reviewed_date).setValue(new Date())
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.reviewer)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.reviewer_dept_branch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforPrincipleManager_CivilApproval:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.mm_date).setValue(new Date())
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.mm)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.mm_dept_branch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforCardAllocation:
                        case ShareSdk.PROXCardReqStatus.RejectedbySecurity:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.card_allocation_date).setValue(new Date())
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.admin)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.admin_dept_branch, departmentBranch)
                            break;
                        case ShareSdk.PROXCardReqStatus.WaitingforCardEncoding:
                            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.card_encoding_date).setValue(new Date())
                            PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.security)
                            // PROXCardReqApprovalSDK.fillDetpBranch(ShareSdk.PROXCardReqFields.security_dept_branch, departmentBranch)
                            break;
                        default:
                            return
                    }

                    if (status != ShareSdk.PROXCardReqStatus.WaitingforCardEncoding) {
                        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.rejected_date).setValue(new Date())
                        PROXCardReqApprovalSDK.fillUser(user, ShareSdk.PROXCardReqFields.rejectedby)
                    }
                }

                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).setValue(true)
                await Xrm.Page.data.save()

                formContext.ui.close()
            }
        }

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

/**
 * Update Card Status to Reserved for accepted New ISMS
 * @param {*} requestId 
 */
PROXCardReqApprovalSDK.updateStatusforAllocationAsync = async (requestId, user) => {
    var details = await ShareSdk.getPROXCardRequestDetailsAsync(requestId)

    // var filterDetails = details.filter(p => p.clp_detail_status != ShareSdk.PROXCardIndividualStatus.Rejected)
    //Update Card Status to Reserved for accepted New ISMS
    // await Promise.all(cards.map(card => ShareSdk.updatePROXCardStatusAsync(card._clp_proximity_card_value, ShareSdk.ProximityCardStatus.Reserved)))

    for (var item of details) {

        if (!item.clp_is_rejected) {

            await PROXCardReqApprovalSDK.updatePROXCardforAllocationAsync(item)

            //Check whether the card is still valid for allocation
            if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS || item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
                var result = await PROXCardReqApprovalSDK.getPROXCardIdAsync(item._clp_proximity_card_applicant_value)
                if (result.length === 0) {
                    tmpAlcCardId = item._clp_proximity_card_value
                } else {
                    tmpAlcCardId = result[0]["detail.clp_proximity_card"]
                }

                if ((tmpAlcCardId === item._clp_proximity_card_value
                    && !((item.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS
                        && item.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Available)
                        || item.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Reserved
                        || item.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.PendingforReplacement
                        || item.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Allocated))
                    || (tmpAlcCardId != item._clp_proximity_card_value && item.clp_proximity_card.clp_card_status != ShareSdk.ProximityCardStatus.Available)
                ) {
                    throw new Error(`ISMS card ${item.clp_proximity_card.clp_name} is no longer available for allocation`)
                }


            }

            //Update Card Status to Reserved for accepted New ISMS
            if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS) {
                if (item.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Available) {
                    await ShareSdk.updatePROXCardStatusAsync(item._clp_proximity_card_value, ShareSdk.ProximityCardStatus.Reserved, user?.clp_userid)
                }

                //Update other application if there is other New ISMS which is rejected by security for this applicant
                var otherDetails = await PROXCardReqApprovalSDK.getOtherApplicationDetailsAsync(item._clp_proximity_card_applicant_value, item._clp_proximity_card_value, item.clp_proximity_card_request_detailid)
                for (var otherdetail of otherDetails) {
                    await ShareSdk.updatePROXCardREQDTLCardAsync(otherdetail["detail.clp_proximity_card_request_detailid"], item._clp_proximity_card_value)
                }
            }
        } else {
            //reject detail
            await ShareSdk.rejectPROXCardREQDTLAsync(item.clp_proximity_card_request_detailid)

        }


        //Reset Card Status from Reserved to Available if Allocated Card has been changed

    }
}

PROXCardReqApprovalSDK.getOtherApplicationDetailsAsync = (applicantid, cardid, detailid) => {
    var fetchXml = `<fetch>
  <entity name="clp_proximitycardrequest">
    <filter>
      <condition attribute="clp_workflow_status" operator="eq" value="768230008" />
    </filter>
    <link-entity name="clp_proximity_card_request_detail" from="clp_proximity_card_request" to="clp_proximitycardrequestid" alias="detail">
      <attribute name="clp_proximity_card" />
      <filter>
        <condition attribute="clp_detail_status" operator="eq" value="768230000" />
        <condition attribute="clp_app_type" operator="eq" value="768230000" />
        <condition attribute="clp_proximity_card_applicant" operator="eq" value="${applicantid}" />
        <condition attribute="clp_proximity_card" operator="ne" value="${cardid}" />
        <condition attribute="clp_proximity_card_request_detailid" operator="ne" value="${detailid}" />
      </filter>
    </link-entity>
  </entity>
</fetch>`

    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests?fetchXml=${encodeURI(fetchXml)}`,
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
PROXCardReqApprovalSDK.getPROXCardIdAsync = (applicantid) => {
    var fetchXml = `<fetch>
  <entity name="clp_proximitycardrequest">
    <filter>
      <condition attribute="clp_workflow_status" operator="not-in" value="">
        <value>768230006</value>
        <value>768230005</value>
      </condition>
    </filter>
    <link-entity name="clp_proximity_card_request_detail" from="clp_proximity_card_request" to="clp_proximitycardrequestid" alias="detail">
      <attribute name="clp_proximity_card" />
      <filter>
        <condition attribute="clp_detail_status" operator="eq" value="768230000" />
        <condition attribute="clp_app_type" operator="eq" value="768230000" />
        <condition attribute="clp_proximity_card_applicant" operator="eq" value="${applicantid}" />
        <condition attribute="clp_proximity_card" operator="not-null" />
      </filter>
    </link-entity>
  </entity>
</fetch>`

    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests?fetchXml=${encodeURI(fetchXml)}`,
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

PROXCardReqApprovalSDK.updatePROXCardforAllocationAsync = async (detail) => {
    if (!detail.clp_detail_status ||
        (detail.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS && detail.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Available)) {
        iDtlStatus = ShareSdk.PROXCardIndividualStatus.WaitingforCardEncoding

        await ShareSdk.updatePROXCardREQDTLStatusAsync(detail.clp_proximity_card_request_detailid, iDtlStatus)

    }
}

PROXCardReqApprovalSDK.updateStatusforEncodingAsync = async (requestId) => {
    var details = await ShareSdk.getPROXCardRequestDetailsAsync(requestId)

    var availableDetails = details.filter(p => p.clp_detail_status === ShareSdk.PROXCardIndividualStatus.WaitingforCardEncoding)
    var stations = await ShareSdk.getPROXCardSubstationsAsync(requestId)
    var user = await ShareSdk.getCurrentClpUserAsync()
    for (var detail of availableDetails) {
        await PROXCardReqApprovalSDK.updateDetailforEncodingAsync(detail, requestId, stations, user)
    }

}

PROXCardReqApprovalSDK.updateDetailforEncodingAsync = async (detail, requestId, stations, user) => {
    if (detail.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
        newStatus = ShareSdk.PROXCardIndividualStatus.WaitingforCardDecoding
    }
    else if (detail.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS) {
        if (detail.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Allocated) {
            newStatus = ShareSdk.PROXCardIndividualStatus.Issued
        } else {
            newStatus = ShareSdk.PROXCardIndividualStatus.WaitingforCardCollection
        }
    }
    else if (detail.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
        if (detail.clp_proximity_card.clp_card_status === ShareSdk.ProximityCardStatus.Reserved) {
            newStatus = ShareSdk.PROXCardIndividualStatus.WaitingforCardCollection
        } else {
            newStatus = ShareSdk.PROXCardIndividualStatus.Issued
        }
    }
    //Update Detail record
    await ShareSdk.updatePROXCardREQDTLStatusAsync(detail.clp_proximity_card_request_detailid, newStatus)


    // var requester = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.requester).getValue()
    var re = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.responsibleperson).getValue()
    var accessperiodto = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()
    // requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    reid = re[0].id.replace(/[{}]*/g, "").toLowerCase()

    let expirydate = detail.clp_proximity_card?.clp_expirydate

    if (!expirydate) {
        expirydate = new Date();
        expirydate.setDate(expirydate.getDate() - 1000)
    } else {
        expirydate = new Date(expirydate)
    }

    let is_latest = expirydate < accessperiodto ? true : false
    if ((detail.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeISMS || detail.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS) && is_latest) {
        //Update Card Info
        await ShareSdk.updatePROXCardInfoAsync(detail._clp_proximity_card_value,
            detail._clp_proximity_card_applicant_value, requestId, reid, accessperiodto, user?.clp_userid)
    }


    //Insert Card Encoding
    // await ShareSdk.addPROXCardEncodeAsync(requestId, detail._clp_proximity_card_value)

    // await Promise.all(stations.map(p => ShareSdk.addPROXCardEncodeDataAsync(requestId, detail._clp_proximity_card_value, p.clp_scadacode)))
}

/**
 * Insert Card Encoding
 * @param {*} REQ_ID 
 * @param {*} CARD_ID 
 * @param {*} SS_CODE 
 * @returns 
 */
PROXCardReqApprovalSDK.addPROXCardDecodeAsync = (REQ_ID, CARD_ID, SS_CODE, DECODE_BY) => {
    var formData = {
        "clp_proximity_card_request@odata.bind": `/clp_proximitycardrequests(${REQ_ID})`,
        "clp_card@odata.bind": `/clp_proximitycardinventories(${CARD_ID})`,
        "clp_ss_code": SS_CODE,
        "clp_encode_date": new Date()
    }

    if (DECODE_BY) {
        formData["clp_decoded_by@odata.bind"] = `/clp_proximitycardrequests(${DECODE_BY})`
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


PROXCardReqApprovalSDK.checkCardIsReservedAsync = (applicantid, cardid) => {
    var fetchXml = `<fetch>
  <entity name="clp_proximity_card_request_detail">
    <filter>
      <condition attribute="clp_detail_status" operator="in" value="">
        <value>768230000</value>
        <value>768230001</value>
      </condition>
      <condition attribute="clp_app_type" operator="eq" value="768230000" />
      <condition attribute="clp_proximity_card_applicant" operator="eq" value="${applicantid}" />
      <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
    </filter>
  </entity>
</fetch>`

    return fetch(
        `/api/data/v9.0/clp_proximity_card_request_details?fetchXml=${encodeURI(fetchXml)}`,
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
 * Update Reserved -> Available for New ISMS
 * @param {*} requestId 
 */
PROXCardReqApprovalSDK.updateDetailforRejectionAsync = async (requestId, user) => {
    var details = await ShareSdk.getPROXCardRequestDetailsAsync(requestId)

    // await Promise.all(details.map(detail => ShareSdk.resetPROXCardREQDTLAsync(detail.clp_proximity_card_request_detailid)))
    for (var detail of details) {


        await ShareSdk.resetPROXCardREQDTLAsync(detail.clp_proximity_card_request_detailid, detail.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS)


        //Update Reserved -> Available for New ISMS
        if (detail.clp_app_type === ShareSdk.PROXCardReqAPPType.NewISMS
            && detail.clp_proximity_card?.clp_card_status === ShareSdk.ProximityCardStatus.Reserved
            && detail.clp_detail_status === ShareSdk.PROXCardIndividualStatus.WaitingforCardEncoding) {

            const result = await PROXCardReqApprovalSDK.checkCardIsReservedAsync(detail._clp_proximity_card_applicant_value, detail._clp_proximity_card_value)
            if (result.length === 0) {
                //Reset Card Status from Reserved to Available
                await ShareSdk.resetPROXCardAvailableAsync(detail.clp_proximity_card.clp_proximitycardinventoryid, user?.clp_userid)
            }
        }
    }
}


PROXCardReqApprovalSDK.fillUser = (user, field, fi) => {

    ShareSdk.setLookupValue(user.clp_userid, user.clp_name, ShareSdk.Tables.user, field)
}

PROXCardReqApprovalSDK.fillDetpBranch = (field, value) => {

    Xrm.Page.data.entity.attributes.getByName(field).setValue(value)
}

PROXCardReqApprovalSDK.fillCollectionReminderDate = () => {
    const today = new Date();
    const reminderDate = new Date()

    reminderDate.setDate(today.getDate() + 30)
    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.collection_reminder_date).setValue(reminderDate)

}


PROXCardReqApprovalSDK.syncRequestAsync = async (requestId) => {
    //MST

    let accessperiodfrom = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodfrom).getValue()
    let accessperiodto = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.accessperiodto).getValue()
    const jsonObj = {

        SSMS_PC_MSTS: [
            {
                SSMS_PC_MST: {
                    SS_DISPLAY: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_display).getValue(),
                    SS_LIST: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_list).getValue(),
                    APP_NO: Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.name).getValue(),
                    ACCESS_TO: ShareSdk.formateDateTime(accessperiodto, 'yyyy-MM-dd'),
                    ACCESS_FROM: ShareSdk.formateDateTime(accessperiodfrom, 'yyyy-MM-dd'),
                    WF_STATUS: ShareSdk.PROXCardReqStatusMapping[Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()]
                }
            }
        ]
    }
    const mstXML = ShareSdk.JSONtoXML(jsonObj)
    console.log(mstXML)

    //DTL
    let detailsJsonObj = {
        SSMS_PC_DTLS: {
            SSMS_PC_DTL: []
        }
    }
    var details = await ShareSdk.getPROXCardRequestDetailsAsync(requestId)
    // var encodes = await ShareSdk.getPROXCardEncodesByReqAsync(requestId)

    for (var item of details) {
        if (item.clp_detail_status != ShareSdk.PROXCardIndividualStatus.Rejected) {
            detailsJsonObj.SSMS_PC_DTLS.SSMS_PC_DTL.push(
                {
                    "APP_TYPE": ShareSdk.PROXCardReqAPPTypeMapping[item.clp_app_type],
                    "REQ_ID": Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.name).getValue(),
                    "DTL_STATUS": ShareSdk.PROXCardIndividualStatusMapping[item.clp_detail_status],
                    'CTR_ID': item.clp_contractor_id,
                    "CARD_ID": item.clp_card_no
                }
            )
        }

    }

    const detailsXML = ShareSdk.JSONtoXML(detailsJsonObj)
    console.log(detailsXML)

    //CARD

    let cardJsonObj = {
        SSMS_PC_CARDS: {
            SSMS_PC_CARD: []
        }

    }

    // var encodeCCards = details.filter(p => p.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card)

    for (var item of details) {
        cardJsonObj.SSMS_PC_CARDS.SSMS_PC_CARD.push(
            {
                "CARD_ID": item.clp_proximity_card.clp_name,
                "CARD_NO": item.clp_proximity_card.clp_name,
                "CARD_TYPE": ShareSdk.ProximityCardCardTypeMapping[item.clp_proximity_card.clp_card_type],
                "ISSUE_DEPT": ShareSdk.getLegacyIssueDepartment(item.clp_proximity_card.clp_issuedepartment),
                "CARD_STATUS": ShareSdk.ProximityCardStatusMapping[item.clp_proximity_card.clp_card_status],
                "EXPIRY_DATE": item.clp_proximity_card.clp_expirydate || '',
                "CARD_EXPIRY_DATE": item.clp_proximity_card.clp_card_expiry_date || '',
                "REPLACE_CARD_ID": "",
                "CARD_PREFIX": item.clp_proximity_card.clp_cardprefix || ''
            }
        )
    }

    const cardsXML = ShareSdk.JSONtoXML(cardJsonObj)
    console.log(cardsXML)


    // let encodedJsonObj = {
    //     SSMS_PC_ENCODES: [
    //     ]
    // }

    // for (var item of encodes) {
    //     encodedJsonObj.SSMS_PC_ENCODES.push(
    //         {
    //             SSMS_PC_ENCODE: {
    //                 "CARD_ID": item["_clp_card_value@OData.Community.Display.V1.FormattedValue"],
    //                 "REQ_ID": item["_clp_proximity_card_request_value@OData.Community.Display.V1.FormattedValue"],
    //                 "SS_CODE": item.clp_ss_code,
    //                 "ENCODE_DATE": item.clp_encoded_date,
    //             }
    //         }
    //     )
    // }

    // const encodesXML = ShareSdk.JSONtoXML(encodedJsonObj)
    // console.log(encodesXML)

    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.Encode, requestId, null, mstXML, cardsXML, detailsXML, null)

}


PROXCardReqApprovalSDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestApprovalForm()) {
        return false
    }



    return true
}