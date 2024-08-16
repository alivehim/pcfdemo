var PROXCardReqSubmitSDK = window.PROXCardReqSubmitSDK || {};

PROXCardReqSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")

    try {

        if (Xrm.Page.data.isValid()) {

            await window.parent.PROXCardReqValidatinoSDK.submissionValidateAsync()

            var errormsgs = await window.parent.PROXCardReqValidatinoSDK.checkErrorDetailsAsync()
            if (errormsgs.length > 0) {

                errormsgs= Array.from(new Set(errormsgs))
                for (var item of errormsgs) {
                    ShareSdk.formError(new Error(item))
                }
                return
            }

            var warnningMessages = await window.parent.PROXCardReqValidatinoSDK.checkWarnningDetailsAsync()

            if (warnningMessages.length > 0) {

                let message = warnningMessages.join('\n')
                var confirmStrings = { confirmButtonLabel: "Ok", cancelButtonLabel: "Cancel", text: `${message}\n\nContinue to proceed?`, title: "Message" };
                var confirmOptions = { height: 200, width: 450 };
                Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
                    function (success) {
                        if (success.confirmed) {

                            PROXCardReqSubmitSDK.submitAsync()
                        }
                    }
                );

            } else {
                await PROXCardReqSubmitSDK.submitAsync()
            }

            // const sslist = await window.parent.PROXCardReqValidatinoSDK.getSSListAsync()
            // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.ss_list).setValue(sslist)
            //generate applicant no

        } else {
            throw new Error('Please fill in mandatory fields')
        }
    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardReqSubmitSDK.submitAsync = async () => {
    Xrm.Utility.showProgressIndicator("Loading")

    try {

        await window.parent.PROXCardReqValidatinoSDK.substationProcessAsync()

        await window.parent.PROXCardReqValidatinoSDK.detailsDataProcessAsync()

        window.parent.PROXCardReqValidatinoSDK.generateExpiryWeekdayDate()

        // await Xrm.Page.data.save()
        // let applicantNo = await PROXCardReqSubmitSDK.generateApplitionNoAsync()
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).setValue(ShareSdk.PROXCardReqStatus.WaitingforApproval)
        // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.name).setValue(applicantNo)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.requestdate).setValue(new Date())
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.is_locked).setValue(true)
        await Xrm.Page.data.save()

        // await PROXCardReqSubmitSDK.notificationAsync(applicantNo)
        await PROXCardReqSubmitSDK.notificationAsync(Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.name).getValue())

        // await ShareSdk.delay(3000)
        
        Xrm.Page.ui.refresh()
    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

// PROXCardReqSubmitSDK.detailsDataProcessAsync = async () => {
//     var formid = ShareSdk.getCurrentFormId()
//     var details = await ShareSdk.getAllPROXCardREQDTLsAsync(formid)

//     for (var item of details) {
//         var cards = await ShareSdk.getPROXCardByCardNoAsync(item.clp_card_no)

//         card = cards[0]


//         if (card) {

//             //should save data after user submitting the request
//             //Update C-Card Expiry Date
//             if (card.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {

//                 if (card.clp_card_expiry_date != card.clp_card_expiry_date) {

//                     await ShareSdk.updateCardExpiryDateAsync(card.clp_proximitycardinventoryid, card.clp_card_expiry_date)
//                 }
//             }
//         } else {

//             let applicantid = ''
//             if (!item.clp_proximity_card_applicant) {
//                 //Insert Applicant if it does not exist
//                 applicant = await ShareSdk.addApplicantAsync(item.clp_applicant_name, item.clp_contractor_id, item.clp_phone_no)
//                 applicantid = applicant.clp_proximitycardapplicantid
//                 await ShareSdk.updatePROXCardREQDTLAsync(item.clp_proximity_card_request_detailid, applicantid)
//             } else {
//                 applicantid = item.clp_proximity_card_applicant.clp_proximity_card_applicantid
//             }


//             if (item.clp_app_type === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {

//                 //should save data after user submitting the request


//                 //Insert C-Card to inventory if it does not exist
//                 var newCard = await ShareSdk.addNewCardAsync(item.clp_card_no, ShareSdk.ProximityCardCardType.ContractorConsultantPass, ShareSdk.ProximityCardStatus.Allocated,
//                     item.clp_card_expiry_date, ShareSdk.IssueDepartmentDefinition.NA, applicantid)

//                 await ShareSdk.updatePROXCardREQDTLAsync(item.clp_proximity_card_request_detailid, applicantid, newCard.clp_proximitycardinventoryid)
//             }
//         }


//     }
// }

PROXCardReqSubmitSDK.getAbbreviation = (issueDept) => {
    var vals = Object.values(ShareSdk.IssueDepartmentMapping)

    for (var item of vals) {
        if (item.val === issueDept) {
            return item.name
        }
    }
}
/**
 * ZZYYMM-XXX
 * ZZ is the two digit abbreviation of Issue Department
 * YY indicates the year when the application was submitted
 * MM indicates the month when the application was submitted
 * XXX is the sequential number
 * @returns 
 */
PROXCardReqSubmitSDK.generateApplitionNoAsync = async () => {
    const issueDept = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.issuedepartment).getValue()

    var abbr = PROXCardReqSubmitSDK.getAbbreviation(issueDept)
    var dateobj = new Date()
    var year = dateobj.getFullYear()
    var yy = `${year}`.substring(2)
    var month = dateobj.getMonth() + 1
    var mm = ShareSdk.padDigitToString(month, 2)
    var sequentialNumber = await PROXCardReqSubmitSDK.getSequentialNumberAsync(issueDept)
    return `${abbr}${yy}${mm}-${ShareSdk.padDigitToString(sequentialNumber, 3)}`
}

PROXCardReqSubmitSDK.getSequentialNumberAsync = async (issueDept) => {


    res = await ShareSdk.getPROXCardRequestsCountAsync(issueDept)

    return res.length + 1;

}

PROXCardReqSubmitSDK.notificationAsync = async (applicationNo) => {
    formContext = Xrm.Page.ui.formContext
    if (formContext) {

        let message = `Request ${applicationNo} is added successfully.  Would you like to export the Request Form now?`
        var confirmStrings = { confirmButtonLabel: "Export", cancelButtonLabel: "Cancel", text: message, title: "Message" };
        var confirmOptions = { height: 200, width: 450 };
        await Xrm.Navigation.openConfirmDialog(confirmStrings, confirmOptions).then(
            function (success) {
                if (success.confirmed) {
                    window.parent.PROXCardReqExportUtilitiesSDK.export()
                }
            }
        );
    }

    // const formid = ShareSdk.getCurrentFormId()
    // var option = {
    //     pageType: "webresource",
    //     webresourceName: "clp_ssms_pc_request_submit_success.html",
    // }

    // option.data = `applitionNo=${applitionNo}&requestid=${formid}`

    // await Xrm.Navigation.navigateTo(option, {
    //     target: 2,
    //     position: 1,
    //     width: {
    //         value: 450,
    //         unit: 'px'
    //     },
    //     height: {
    //         value: 200,
    //         unit: 'px'
    //     },
    //     title: "Message"
    // }).then(
    //     function success() {
    //         // Run code on success
    //     },
    //     function error() {
    //         // Handle errors
    //     }
    // );

}

PROXCardReqSubmitSDK.enableButton = () => {
    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false
}