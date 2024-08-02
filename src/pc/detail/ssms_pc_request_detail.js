var PROXCardDetailSDK = window.PROXCardDetailSDK || {};


PROXCardDetailSDK.onFormLoaded = async (context) => {

    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Detail_Main) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Detail_Main);
        formItem.navigate();
    }

    //PROXCardDetailSDK.unlockFields()

    // await PROXCardDetailSDK.initCards()

    await PROXCardDetailSDK.unlockFieldsAsync()
}

PROXCardDetailSDK.unlockFieldsAsync = async () => {

    const lockfields = () => {
        for (let attr in Xrm.Page.data.entity.attributes._collection) {
            ShareSdk.setDisableControl(attr, true)
        }
    }


    const request = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_request).getValue()

    if (request) {
        const requestid = ShareSdk.getLookupId(request)
        const requestForm = await ShareSdk.getPROXCardRequestAsync(requestid)
        if (!(!requestForm.clp_workflow_status
            || requestForm.clp_workflow_status === ShareSdk.PROXCardReqStatus.RequestRejected)) {
            lockfields()
        }
    }
}



PROXCardDetailSDK.onContractorChanged = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        var contractor = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractor).getValue()
        if (contractor) {

            var userid = contractor[0].id.replace(/[{}]*/g, "").toLowerCase()

            var contractor = await ShareSdk.getUserAsync(userid)

            applicantName = contractor[ShareSdk.CLPUserFields.name]
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.applicant_name).setValue(applicantName)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.name).setValue(applicantName)
            ctrid = contractor[ShareSdk.CLPUserFields.person_id]
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractor_id).setValue(ctrid)
            // var existingApplicants = await ShareSdk.getPROXCardApplicantsAsync(userid)
            // if (existingApplicants.length > 0) {
            //     Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.phone_no).setValue(existingApplicants[0].clp_phoneno)
            // }

            // // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractorid).setValue(contractor[ShareSdk.CLPUserFields.person_id])

            // //set cartype to 'Encode C-Card' if contractor already has a C-Card

            // //set cartype to 'Encode ISMS' if contractor already has a ISMS card
            // var inventoryCards = await ShareSdk.getPROXCardsByContractorIdAsync(contractor[ShareSdk.CLPUserFields.person_id])


            // if (inventoryCards.length > 0) {

            // }
            // else {
            //     Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.NewISMS)
            //     ShareSdk.setDisableControl(ShareSdk.PROXCardRequetDetailFields.app_type, false)
            //     ShareSdk.setDisableControl(ShareSdk.PROXCardRequetDetailFields.card_no)
            //     ShareSdk.setDisableControl(ShareSdk.PROXCardRequetDetailFields.card_expiry_date)
            // }

            var apptype = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).getValue()
            var cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).getValue()

            var phone_no = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.phone_no).getValue();

            var result = await ShareSdk.getPROXCardApplicantsInfoAsync(ctrid, cardno, applicantName)
            if (result.length === 0) {
                //if (apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.NewISMS);
                //}
                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).setValue(null);

            } else {

                //set appicant

                let firstResult = result[0]
                if(firstResult.clp_proximitycardapplicantid){

                    ShareSdk.setLookupValue(firstResult.clp_proximitycardapplicantid, firstResult.clp_name, ShareSdk.Tables.proximitycardapplicant, ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant)
                }


                if (!phone_no) {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.phone_no).setValue(firstResult.clp_phoneno??"");
                }

                // Card Expiry Date
                const CardExpiryDate = await ShareSdk.getPROXCardExpiryDateAsync(ctrid, cardno)
                let expirydate = firstResult["pc.clp_card_expiry_date"]
                if (CardExpiryDate) {
                    expirydate = CardExpiryDate
                }



                //https://neilparkhurst.com/2021/10/19/javascript-and-business-rules/
                if (firstResult["pc.clp_card_type"] === ShareSdk.ProximityCardCardType.ISMSProximityCard) {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.EncodeISMS)
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).fireOnChange()

                } else if (firstResult["pc.clp_card_type"] === ShareSdk.ProximityCardCardType.ContractorConsultantPass) {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.EncodeC_Card)
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).fireOnChange()
                }

                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).setValue(firstResult["pc.clp_name"])
                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).fireOnChange()
                if (firstResult["pc.clp_proximitycardinventoryid"]) {

                    ShareSdk.setLookupValue(firstResult["pc.clp_proximitycardinventoryid"], firstResult["pc.clp_name"], ShareSdk.Tables.proximitycardinventory, ShareSdk.PROXCardRequetDetailFields.proximity_card)

                } else {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(null)
                }

                if (expirydate) {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_expiry_date).setValue(new Date(expirydate))
                }
                else {
                    //change color
                }

            }

            if (cardno) {
                // attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_no).controls.get(0).setNotification(ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.message,
                //     ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.id);
            }


        } else {
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).setValue(null);
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(null)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.applicant_name).setValue(null)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractor_id).setValue(null)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).setValue(null)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.phone_no).setValue(null)
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_expiry_date).setValue(null)
        }
    }
    catch (err) {
        ShareSdk.formError(err)

        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).setValue(null);
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(null)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.applicant_name).setValue(null)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractor_id).setValue(null)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.contractor).setValue(null)

    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

PROXCardDetailSDK.onAPPTypeChanged = () => {
    var apptype = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).getValue()
    if (apptype === ShareSdk.PROXCardReqAPPType.NewISMS) {
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(null)
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).setValue(null)
    }
}
PROXCardDetailSDK.onCardNoChanged = async () => {

    Xrm.Utility.showProgressIndicator("Loading")
    try {

        var cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).getValue()
        var apptype = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).getValue()
        formContext = Xrm.Page.ui.formContext
        formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).clearNotification(ShareSdk.PROXCardInventoryErrors.ContractorConsultantCardNoFormat.id);
        formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).clearNotification(ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id);
        formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).clearNotification(ShareSdk.PROXCardReqDTLErrors.CardNotExist.id);
        formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).clearNotification(ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.id);
        if (cardno) {

            if (apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
                const cardNoReg = /^[A-Za-z0-9]{2}[0-9]{5}$/;

                if (!cardNoReg.test(cardno)) {
                    formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).addNotification({
                        notificationLevel: 'ERROR',
                        messages: [ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.message],
                        uniqueId: ShareSdk.PROXCardInventoryErrors.ISMSProximityCardNoFormat.id
                    });
                    return
                }

            }
            else if (apptype === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
                const cardNoReg = /^C[0-9]{4}-[0-9]{5}$/;
                const cardNoReg1 = /^C[0-9]{5}-[0-9]{5}$/;
                const cardNoReg2 = /^[0-9]{2}C[0-9]{4}-[0-9]{5}$/

                if (!cardNoReg.test(cardno) && !cardNoReg1.test(cardno) && !cardNoReg2.test(cardno)) {
                    formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).addNotification({
                        notificationLevel: 'ERROR',
                        messages: [ShareSdk.PROXCardInventoryErrors.ContractorConsultantCardNoFormat.message],
                        uniqueId: ShareSdk.PROXCardInventoryErrors.ContractorConsultantCardNoFormat.id
                    });
                    return
                }
            }


            var cards = await ShareSdk.getPROXCardByCardNoAsync(cardno)
            const expirydate = await ShareSdk.getPROXCardExpiryDateByCardNoAsync(cardno)
            let card = cards[0]
            if (!card) {


                if (apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
                    formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).addNotification({
                        notificationLevel: 'ERROR',
                        messages: [ShareSdk.PROXCardReqDTLErrors.CardNotExist.message],
                        uniqueId: ShareSdk.PROXCardReqDTLErrors.CardNotExist.id
                    });

                    return
                }

                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(null);

                // Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_expiry_date).setValue(new Date(expirydate))

            } else {


                const selectApplicant = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).getValue()
                if (selectApplicant) {

                    if (selectApplicant[0].id.replace(/[{}]*/g, "").toLowerCase() != card._clp_holder_value.toLowerCase()) {

                        formContext.getControl(ShareSdk.PROXCardRequetDetailFields.card_no).addNotification({
                            notificationLevel: 'ERROR',
                            messages: [ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.message],
                            uniqueId: ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.id
                        });
                        return
                    }
                }

                ShareSdk.setLookupValue(card.clp_proximitycardinventoryid, card.clp_name, ShareSdk.Tables.proximitycardinventory, ShareSdk.PROXCardRequetDetailFields.proximity_card)


            }

            if (apptype === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
                //set expiry date
                if (expirydate) {
                    Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_expiry_date).setValue(new Date(expirydate))
                }
                // attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.NewISMS);
            }

        }

    }
    catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }

}

PROXCardDetailSDK.onPROXCardChanged = () => {
    var card = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).getValue()
    if (card) {
        var name = card[0].name
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).setValue(name)
    }
}




PROXCardDetailSDK.addChange = (context) => {
    const elementPath = `input[id*='fieldControl-LookupResultsDropdown_clp_user']`
    const selector = () => {
        return window.parent.document.querySelector(elementPath)
    }


    ShareSdk._doUntil(() => {
        window.parent.document.querySelector(elementPath)?.addEventListener("input", () => {
            var searchText = window.parent.document.querySelector(elementPath).value
            console.log(searchText)
            PROXCardDetailSDK.addCustomViewToPrimaryContact(context, searchText)

        }, false);
    }, selector)


}

/**
 * https://www.praxiis.co.uk/post/filtering-dynamics365-lookups#:~:text=Filtering%20Dynamics%20365%20Lookups%20Four%20Ways%201%20Set,...%204%20Dynamically%20Apply%20a%20Custom%20Filter%20
 * @param {*} context 
 */
PROXCardDetailSDK.addCustomViewToPrimaryContact = (context, searchValue) => {
    // var formContext = context.getFormContext();
    var formContext = Xrm.Page.ui.formContext
    // var searchValue = Xrm.Page.getControl(ShareSdk.PROXCardRequetDetailFields.contractor).getValue()
    // var searchValue = 'PID'
    // var searchValue = document.querySelector(`input[id*='fieldControl-LookupResultsDropdown_clp_user']`)?.value ?? ''
    var viewId = Xrm.Page.getControl(ShareSdk.PROXCardRequetDetailFields.contractor).getDefaultView();

    // var viewId = "{00000000-0000-0000-0000-000000000001}",
    entityName = ShareSdk.Tables.contractor,
        viewDisplayName = "Filtered Lookup View",
        fetchXML = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" +
        "<entity name='clp_user' >" +
        "<attribute name='clp_name' />" +
        "<attribute name='clp_person_id' />" +
        "<filter type='or'>" +
        "<condition attribute='clp_name' operator='like' value='" + searchValue + "%' />" +
        "<condition attribute='clp_person_id' operator='like' value='" + searchValue + "%' />" +
        "</filter>" +
        "</entity>" +
        "</fetch>",
        layoutXML = "<grid name='resultset' object='1' jump='clpuserid' select='1' icon='1' preview='1'>" +
        "<row name='result' id='clpuserid'>" +
        "<cell name='clp_name' width='150' />" +
        "<cell name='clp_person_id' width='150' />" +
        "</row>" +
        "</grid>";

    formContext.getControl(ShareSdk.PROXCardRequetDetailFields.contractor).addCustomView(viewId, entityName, viewDisplayName, fetchXML, layoutXML, true);
}


PROXCardDetailSDK.addCustomViewToProxCard = (cardid) => {
    // var formContext = context.getFormContext();
    var formContext = Xrm.Page.ui.formContext
    // var searchValue = Xrm.Page.getControl(ShareSdk.PROXCardRequetDetailFields.contractor).getValue()
    // var searchValue = 'PID'
    // var searchValue = document.querySelector(`input[id*='fieldControl-LookupResultsDropdown_clp_user']`)?.value ?? ''
    var viewId = Xrm.Page.getControl(ShareSdk.PROXCardRequetDetailFields.proximity_card).getDefaultView();

    // var viewId = "{00000000-0000-0000-0000-000000000001}",
    entityName = ShareSdk.Tables.proximitycardinventory,
        viewDisplayName = "Filtered Lookup View",
        fetchXML = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'>" +
        "<entity name='clp_proximitycardinventory' >" +
        "<attribute name='clp_name' />" +
        "<filter>" +
        `<condition attribute='clp_proximitycardinventoryid' operator='eq' value='${cardid}' />` +
        "</filter>" +
        "</entity>" +
        "</fetch>",
        layoutXML = "<grid name='resultset' object='1' jump='clpuserid' select='1' icon='1' preview='1'>" +
        "<row name='result' id='clp_proximitycardinventoryid'>" +
        "<cell name='clp_name' width='150' />" +
        "</row>" +
        "</grid>";

    formContext.getControl(ShareSdk.PROXCardRequetDetailFields.proximity_card).addCustomView(viewId, entityName, viewDisplayName, fetchXML, layoutXML, true);
}