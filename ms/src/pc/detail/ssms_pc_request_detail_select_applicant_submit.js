var PCREQDTLSelectApplicantSubmitSDK = window.PCREQDTLSelectApplicantSubmitSDK || {};


PCREQDTLSelectApplicantSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        if (Xrm.Page.data.isValid()) {
            formContext = Xrm.Page.ui.formContext

            var user = await ShareSdk.getCurrentClpUserAsync()

            var expirydate = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_expiry_date).getValue()
            var apptype = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).getValue()
            var cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).getValue()
            var applicant_name = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.applicant_name).getValue()
            // add applicant
            var contractor = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractor).getValue()
            var contractorId = contractor[0].id.replace(/[{}]*/g, "").toLowerCase()
            var contractorName = contractor[0].name
            var applicant = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).getValue()
            var applicantid = applicant ? applicant[0].id.replace(/[{}]*/g, "").toLowerCase() : null
            // var clpuser_contractor = await ShareSdk.getCLPUserAsync(contractorId)

            // var applicants = await ShareSdk.getPROXCardApplicantsInfoAsync(clpuser_contractor[ShareSdk.CLPUserFields.person_id], cardno, applicant_name)

            if (apptype != ShareSdk.PROXCardReqAPPType.NewISMS) {


                var cards = await ShareSdk.getPROXCardByCardNoAsync(cardno)

                card = cards[0]

                await PCREQDTLSelectApplicantSubmitSDK.validateCardAsync(apptype, card, cardno, applicantid)

                // if (card) {
                //     if (apptype === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {

                //         if (card.clp_card_expiry_date != expirydate) {

                //             await ShareSdk.updateCardExpiryDateAsync(card.clp_proximitycardinventoryid, expirydate, user?.clp_userid)
                //         }
                //     }
                // }
            }

            // ShareSdk.setLookupValue(applicant.clp_proximitycardapplicantid, applicant.clp_name, ShareSdk.Tables.proximitycardapplicant, ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant)
            // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.applicant_name).setValue(applicant.clp_name)

            await Xrm.Page.data.save()
            formContext.ui.close()
        }


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PCREQDTLSelectApplicantSubmitSDK.validateAsync = () => {
    //'For Encode ISMS, it must not have time gap (Access Period) among applications of the same card

    //End date of Access Period cannot exceed the expiry date
}
PCREQDTLSelectApplicantSubmitSDK.validateCardAsync = async (apptype, card, cardno, applicantid) => {

    if (apptype != ShareSdk.PROXCardReqAPPType.NewISMS) {


        if (!card && apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
            throw new Error(`Encode ISMS Card must exist in the system`)
        }

        if (card && apptype === ShareSdk.PROXCardReqAPPType.EncodeC_Card && card.clp_card_type != ShareSdk.ProximityCardCardType.ContractorConsultantPass) {
            throw new Error(`Card Type must be Contractor/Consultant Pass"`)
        }

        if (card && apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS && card.clp_card_type != ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            throw new Error(`Card Type must be ISMS Card"`)
        }


        if (card && !card._clp_holder_value) {
            throw new Error('Card must have Card Holder')
        }

        if (card && card._clp_holder_value != applicantid) {
            throw new Error('Card Holder must match with Applicant')
        }

        if (apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
            const inputData = Xrm.Utility.getPageContext().input.data

            if (!inputData.accessperiodfrom || !inputData.accessperiodto) {
                throw new Error('Access From/To is missing')
            }

            //'For Encode ISMS, it must not have time gap (Access Period) among applications of the same card
            var result = await ShareSdk.getTimeGapAsync(card.clp_proximitycardinventoryid)

            if (result.accessperiodfrom_min) {

                var min_access_Date = new Date(result.accessperiodfrom_min)
                min_access_Date = new Date(min_access_Date.setDate(min_access_Date.getDate() - 1))

                var max_access_Date = new Date(result.accessperiodto_max)
                max_access_Date = new Date(max_access_Date.setDate(max_access_Date.getDate() + 1))

                if (min_access_Date > inputData.accessperiodto || inputData.accessperiodfrom > max_access_Date) {
                    throw new Error('The Access Period must be continous among Requests')
                }
            }

        }

    }
}





PCREQDTLSelectApplicantSubmitSDK.getApplicantsAsync = (userid) => {
    return fetch(`/api/data/v9.0/clp_proximitycardapplicants?$filter=_clp_user_value eq '${userid}'  `,
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

// PCREQDTLSelectApplicantSubmitSDK.getCardAsync = (CARD_NO) => {
//     return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=clp_name eq '${CARD_NO}'&$expand=clp_holder  `,
//         {
//             headers: {
//                 "Content-Type": "application/json",
//                 'Prefer': 'odata.include-annotations="*"',
//             }
//         }
//     ).then(res => res.json()).then(res => {
//         if (res.value.length > 0) {

//             return Promise.resolve(res.value[0])
//         }
//         else {
//             return Promise.resolve(null)
//         }
//     }).catch(err => {
//         console.error(err)
//         throw err;
//     })
// }

// /**
//  * add proximity card
//  * @param {*} CARD_NO 
//  * @param {*} CARD_TYPE 
//  * @param {*} CARD_STATUS 
//  * @param {*} CARD_EXPIRY_DATE 
//  * @param {*} ISSUE_DEPT 
//  * @param {*} CARD_HOLDER_ID 
//  * @returns 
//  */
// PCREQDTLSelectApplicantSubmitSDK.addCardAsync = (CARD_NO, CARD_TYPE, CARD_STATUS, CARD_EXPIRY_DATE, ISSUE_DEPT, CARD_HOLDER_ID) => {
//     var formData = {
//         "clp_name": CARD_NO,
//         "clp_card_type": CARD_TYPE,
//         "clp_card_status": CARD_STATUS,
//         "clp_expirydate": CARD_EXPIRY_DATE,
//         "clp_user@odata.bind": `/clp_users(${userid})`,
//     }
//     return fetch(
//         `/api/data/v9.0/clp_proximitycardapplicants`,
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

PCREQDTLSelectApplicantSubmitSDK.updateCardExpiryDateAsync = (id, expirtydate) => {
    var body = {
        "clp_expirydate": expirtydate,
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

PCREQDTLSelectApplicantSubmitSDK.enableButton = () => {
    const inputData = Xrm.Utility.getPageContext().input.data
    if (inputData && (!inputData.status || inputData.status === ShareSdk.PROXCardReqStatus.RequestRejected)) {
        return true
    }

    return false
}