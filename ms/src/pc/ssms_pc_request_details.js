var PROXCardReqDetailsSDK = window.PROXCardReqDetailsSDK || {};

PROXCardReqDetailsSDK.onRecardSelect = (executionContext) => {

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null && ShareSdk.isPROXCardRequestor()) {
        return
    }
    let oFormContext = executionContext.getFormContext();
    if (oFormContext) {
        let arrFields = ["subject", "regardingobjectid", "ownerid"];
        let objEntity = oFormContext.data.entity;
        objEntity.attributes.forEach(function (attribute, i) {
            console.log(attribute)

            let attributeToDisable = attribute.controls.get(0);
            attributeToDisable.setDisabled(true);

        });
    }

}

PROXCardReqDetailsSDK.onSave = (executionContext) => {
    console.log('save....')

    let oFormContext = executionContext.getFormContext();
    if (oFormContext) {

        let objEntity = oFormContext.data.entity;
        objEntity.attributes.forEach(function (attribute, i) {
            console.log(attribute.getValue())



        });
    }

}

//https://debajmecrm.com/dynamics-crm-365-access-your-cell-values-easily-during-on-change-event-of-field-in-dynamics-crm-editable-sub-grid/
/**
 * apptype of editable grid changed
 */
PROXCardReqDetailsSDK.appTypeChanged = (eContext) => {

    // var gridContext = Xrm.Page.ui.formContext.getControl(ShareSdk.ProximityCardRequestSubGrids.EditableDetail)

    // var allRows = gridContext.getGrid().getRows()

    // allRows.forEach(function (row, i) {
    //     var gridColumns = row.data.entity.attributes;
    //     gridColumns.forEach(function (column, j) {
    //         var atrName = column.getName();
    //         var atrValue = column.getValue();
    //         console.log(atrName + ":" + atrValue);
    //     });

    // });

    var nameAttr = eContext.getEventSource();
    // get the container for the attribute.
    var attrParent = nameAttr.getParent();
    // get the value of the name.
    var name = nameAttr.getValue();

    console.log(name)
    // var field1 Attribute
    //var field1 = attrParent.attributes.get("new_field1");
    // field1.setValue("<default value for field 1>");
    // set field as mandatory.
    // field1.setRequiredLevel("required");
    // nameAttr.setRequiredLevel("required");

    // nameAttr.controls.get(0).setDisabled(true);

    // console.log(nameAttr.controls.get(0).getDisabled())

}

PROXCardReqDetailsSDK.onContractIdChanged = async (eContext) => {
    var nameAttr = eContext.getEventSource();
    var contractIdCtrl = eContext.getEventSource();
    var attrParent = nameAttr.getParent();

    var apptype = attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).getValue();
    var ctrid = contractIdCtrl.getValue()
    var selectApplicant = attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).getValue();
    var cardno = attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_no).getValue();

    var phone_no = attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.phone_no).getValue();

    var applicants = await ShareSdk.getPROXCardApplicantsInfoAsync(ctrid, cardno, selectApplicant[0]?.name || '')
    if (applicants.length === 0) {
        if (apptype === ShareSdk.PROXCardReqAPPType.EncodeISMS) {
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.NewISMS);
        }

        attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).setValue(null);
    } else {

        //set appicant

        applicant = applicants[0]

        var lookupData = new Array();
        var lookupItem = new Object();
        if (applicant.clp_proximitycardapplicantid) {
            lookupItem.id = applicant.clp_proximitycardapplicantid;
            lookupItem.name = applicant.clp_name;
            lookupItem.entityType = ShareSdk.Tables.proximitycardapplicant;
            lookupData[0] = lookupItem;
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).setValue(lookupData);

        }

        if (!phone_no) {
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.phone_no).setValue(applicant.clp_phoneno);
        }

        attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_expiry_date).setValue(applicant["pc.clp_card_expiry_date"])
        if (applicant["pc.clp_card_expiry_date"]) {
            //change color
        }

        if (applicant["pc.clp_card_type"] === ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.EncodeISMS)
        } else if (applicant["pc.clp_card_type"] === ShareSdk.ProximityCardCardType.ContractorConsultantPass) {
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.EncodeC_Card)
        }

        attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_no).setValue(applicant["pc.clp_name"])
        if (applicant["pc.clp_proximitycardinventoryid"]) {
            var lookupData = new Array();
            var lookupItem = new Object();
            lookupItem.id = applicant["pc.clp_proximitycardinventoryid"];
            lookupItem.name = applicant["pc.clp_name"];
            lookupItem.entityType = ShareSdk.Tables.proximitycardinventory;
            lookupData[0] = lookupItem;
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(lookupData);

            //show the dropdown
        }


    }

    if (cardno) {
        // attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_no).controls.get(0).setNotification(ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.message,
        //     ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.id);
    }
}

PROXCardReqDetailsSDK.onCardNoChanged = async (eContext) => {
    var nameAttr = eContext.getEventSource();
    var attrParent = nameAttr.getParent();
    var cardnoCtrl = eContext.getEventSource();
    var apptype = attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).getValue();
    var cardno = cardnoCtrl.getValue()

    if (cardno) {
        var card = await ShareSdk.getPROXCardByCardNoAsync(cardno)
        if (!card) {
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_no).controls.get(0).setNotification(ShareSdk.PROXCardReqDTLErrors.CardNotExist.message,
                ShareSdk.PROXCardReqDTLErrors.CardNotExist.id);

            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(null);

        } else {

            var lookupData = new Array();
            var lookupItem = new Object();
            lookupItem.id = card.clp_proximitycardinventoryid
            lookupItem.name = card.pc.clp_name
            lookupItem.entityType = ShareSdk.Tables.proximitycardinventory;
            lookupData[0] = lookupItem;
            attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card).setValue(lookupData);

            const selectApplicant = attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).getValue()
            if (selectApplicant) {

                if (selectApplicant[0].id.replace(/[{}]*/g, "").toLowerCase() != card._clp_holder_value.toLowerCase()) {
                    attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.card_no).controls.get(0).setNotification(ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.message,
                        ShareSdk.PROXCardReqDTLErrors.ApplicantMatch.id);
                }
            }

            if (apptype === ShareSdk.PROXCardReqAPPType.EncodeC_Card) {
                //set expiry date

                // attrParent.attributes.get(ShareSdk.PROXCardRequetDetailFields.app_type).setValue(ShareSdk.PROXCardReqAPPType.NewISMS);
            }
        }
    }
}