var PROXCardEncodeReplacementFormSumbitSDK = window.PROXCardEncodeReplacementFormSumbitSDK || {};

PROXCardEncodeReplacementFormSumbitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        formContext = Xrm.Page.ui.formContext

        const user = await ShareSdk.fillLatestModifiedByAsync()

        const formid = ShareSdk.getCurrentFormId()
        //get replace card 
        const cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue()
        const rpcard = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.replace_card).getValue()
        const replacecardid = ShareSdk.getLookupId(rpcard)
        const replacecard = await ShareSdk.getPROXCardByIdAsync(replacecardid)

        //'Update Card Holder, RE, Expiry Date of Replace By card
        await ShareSdk.updatePROXCardInfoAsync(formid, replacecard._clp_holder_value, null, replacecard._clp_responsible_engineer_value, replacecard.clp_expirydate, user?.clp_userid)

        //'Retrieve Request from Original Card

        const details = await PROXCardEncodeReplacementFormSumbitSDK.getPROXCardREQDTLWithEncodeAsync(replacecardid)

        let syncAddDetails = []
        for (var item of details) {
            let detail = await ShareSdk.getPROXCardREQDTLByIdAsync(item.clp_proximity_card_request_detailid)
            let orgDtlRemarks = detail.clp_remarks
            if (!orgDtlRemarks) {
                newDtlRemarks = `Replaced by ${cardno}`
            } else {
                newDtlRemarks = newDtlRemarks + `. Replaced by ${cardno}`
            }

            await ShareSdk.addNewPROXCardRequestDetailAsync(detail._clp_proximity_card_request_value,
                ShareSdk.PROXCardIndividualStatus.WaitingforCardCollection, detail.clp_app_type, detail._clp_proximity_card_applicant_value,
                detail.clp_applicant_name, detail._clp_contractor_value, detail.clp_contractor_id, cardno, formid,
                detail.clp_phone_no)

            await ShareSdk.updatePROXCardREQDTLForEncodeReplacementAsync(item.clp_proximity_card_request_detailid, formid, newDtlRemarks)

            syncAddDetails.push({
                apptype: detail.clp_app_type,
                requestno: detail.clp_proximity_card_request.clp_name,
                detailstatus: ShareSdk.PROXCardIndividualStatus.WaitingforCardCollection,
                contractorid: detail.clp_contractor_id,
                cardno: cardno
            })
        }

        //Update Card ID from Original Card to Replace By Card if the application is in progress

        var originalDetails = await PROXCardEncodeReplacementFormSumbitSDK.getPROXCardREQDTLAsync(replacecardid)
        var syncUpdateDetails = []
        for (var item of originalDetails) {

            await ShareSdk.updatePROXCardREQDTLCardAsync(item.clp_proximity_card_request_detailid, formid)

            syncUpdateDetails.push({
                appno: item.request.clp_name,
                cardno: item.card.clp_name,
                newcardno: cardno
            })
        }


        var syncEncodes = []
        //'Insert Encode Table
        var encodelist = await ShareSdk.getPROXCardEncodeListAsync(replacecardid)
        for (var item of encodelist) {
            await ShareSdk.addPROXCardEncodeAsync(item._clp_proximity_card_request_value, formid, item.clp_ss_code)
            syncEncodes.push({
                APPNO: item.clp_proximity_card_request.clp_name,
                CARDNO: item.clp_card.clp_name,
                SS_CODE: item.clp_ss_code,
                Encode_Date: ShareSdk.formateDateTime(new Date(), 'yyyy-MM-dd')
            })
        }

        await ShareSdk.addPROXCardNotificationAsync(formid, ShareSdk.CardNotificaitonType.ReplaceCollect)


        // await Xrm.Page.data.save()

        //await PROXCardEncodeReplacementFormSumbitSDK.asyncProccessAsync(formid, syncAddDetails, syncUpdateDetails, syncEncodes)


        await ShareSdk.openAlertDialog('Replace card is encoded successfully').then(res => {
            // var pageInput = {
            //     pageType: "entitylist",
            //     entityName: ShareSdk.Tables.proximitycardinventory,
            // }

            // Xrm.Navigation.navigateTo(pageInput)
        })
        formContext.ui.close()


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardEncodeReplacementFormSumbitSDK.asyncProccessAsync = async (cardid, syncAddDetails, syncUpdateDetails, syncEncodes) => {

    const card = await ShareSdk.getPROXCardByIdAsync(cardid)

    let cardJsonObj = {
        SSMS_PC_CARDS: {
            SSMS_PC_CARD: []
        }

    }

    cardJsonObj.SSMS_PC_CARDS.SSMS_PC_CARD.push(
        {
            "CARD_ID": card.clp_name,
            "CARD_NO": card.clp_name,
            "CARD_TYPE": ShareSdk.ProximityCardCardTypeMapping[card.clp_card_type],
            "ISSUE_DEPT": ShareSdk.getLegacyIssueDepartment(card.clp_issuedepartment),
            "CARD_STATUS": ShareSdk.ProximityCardStatusMapping[card.clp_card_status],
            "EXPIRY_DATE": card.clp_expirydate || '',
            "CARD_EXPIRY_DATE": card.clp_card_expiry_date || '',
            "REPLACE_CARD_ID": "",
            "CARD_PREFIX": card.clp_cardprefix || ''
        }
    )

    const cardsXML = ShareSdk.JSONtoXML(cardJsonObj)

    console.log(cardsXML)

    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateCard, null, cardid, cardsXML)

    if (syncAddDetails.length > 0) {
        let detailsJsonObj = {
            SSMS_PC_DTLS: {
                SSMS_PC_DTL: []
            }
        }
        for (var item of syncAddDetails) {
            detailsJsonObj.SSMS_PC_DTLS.SSMS_PC_DTL.push(
                {
                    "APP_TYPE": ShareSdk.PROXCardReqAPPTypeMapping[item.apptype],
                    "REQ_ID": item.requestno,
                    "DTL_STATUS": ShareSdk.PROXCardIndividualStatusMapping[item.detailstatus],
                    'CTR_ID': item.contractorid,
                    "CARD_ID": item.cardno
                }
            )
        }

        const detailsXML = ShareSdk.JSONtoXML(detailsJsonObj)

        console.log(detailsXML)

        await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.AddDetails, null, cardid, detailsXML)
    }




    if (syncEncodes.length > 0) {
        let encodedJsonObj = {
            SSMS_PC_ENCODES: {
                SSMS_PC_ENCODE: []
            }
        }

        for (var item of syncEncodes) {
            encodedJsonObj.SSMS_PC_ENCODES.SSMS_PC_ENCODE.push(
                {
                    "CARD_ID": item.CARDNO,
                    "REQ_ID": item.APPNO,
                    "SS_CODE": item.SS_CODE,
                    "ENCODE_DATE": item.Encode_Date,
                }
            )
        }

        const encodesXML = ShareSdk.JSONtoXML(encodedJsonObj)
        console.log(encodesXML)

        await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.AddEncodes, null, cardid, encodesXML)

    }


    for (var item of syncUpdateDetails) {
        await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateDeatilStatus, null, cardid, item.appno, item.cardno, '', item.newcardno)
    }

}

// PROXCardEncodeReplacementFormSumbitSDK.getPROXCardREQDTLWithEncodeAsync = async (cardid) => {
//     const result = await ShareSdk.getPROXCardREQDTLAsync(cardid)
//     let detailids = []
//     for (var item of result) {
//         const encodes = await ShareSdk.getPROXCardEncodesByCardandReqAsync(cardid, item._clp_proximity_card_request_value)
//         if (encodes.length > 0) {
//             detailids.push(item.clp_proximity_card_request_detailid)
//         }
//     }

//     return Array.from(new Set(detailids))
// }

PROXCardEncodeReplacementFormSumbitSDK.getPROXCardREQDTLWithEncodeAsync = (cardid) => {
    var fetchXml = `<fetch>
  <entity name="clp_proximity_card_request_detail">
    <filter>
      <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
      <filter>
        <condition entityname="encode" attribute="clp_proximity_card_encodeid" operator="not-null" />
      </filter>
    </filter>
    <link-entity name="clp_proximity_card_encode" from="clp_proximity_card_request" to="clp_proximity_card_request" link-type="outer" alias="encode">
      <filter>
        <condition attribute="clp_card" operator="eq" value="${cardid}" />
      </filter>
    </link-entity>
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


PROXCardEncodeReplacementFormSumbitSDK.getPROXCardREQDTLAsync = (cardid) => {

    var fetchXml = `<fetch>
    <entity name="clp_proximity_card_request_detail" >
        <filter type="and" >
            <filter type="or" >
                <condition attribute="clp_detail_status" operator="in" value="" >
                    <value>
                        768230000
                    </value>
                    <value>
                        768230005
                    </value>
                </condition>
                <condition attribute="clp_detail_status" operator="null" />
            </filter>
            <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
        </filter>
        <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" >
            <filter>
                <condition attribute="clp_workflow_status" operator="not-in" value="" >
                    <value>
                        768230005
                    </value>
                    <value>
                        768230006
                    </value>
                </condition>
            </filter>
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

    // wfstatusfilter = [ShareSdk.PROXCardReqStatus.RequestCompleted, ShareSdk.PROXCardReqStatus.RequestCancelled]

    // statusfilter = [ShareSdk.PROXCardIndividualStatus.WaitingforCardEncoding, ShareSdk.PROXCardIndividualStatus.Rejected]

    // const statusfilterstr = statusfilter.map(p => ` clp_detail_status eq ${p} `).join(' or ')

    // const requestfilterstr = wfstatusfilter.map(p => ` clp_workflow_status ne ${p} `).join(' and ')
    // const condition = `clp_proximity_card_request_detailid eq '${cardid}' and (${statusfilterstr}) `
    // return fetch(`/api/data/v9.0/clp_proximity_card_request_details?$filter=${condition}&$expand=clp_proximity_card_request($filter=${requestfilterstr}) `,
    //     {
    //         headers: {
    //             "Content-Type": "application/json",
    //             'Prefer': 'odata.include-annotations="*"',
    //         }
    //     }
    // ).then(res => res.json()).then(res => {
    //     if (res.error) {
    //         throw new Error(res.error.message)
    //     }
    //     return Promise.resolve(res.value)
    // }).catch(err => {
    //     console.error(err)
    //     throw err;
    // })
}


PROXCardEncodeReplacementFormSumbitSDK.enableButton = () => {
    return ShareSdk.isProximityCardSecurity() && ShareSdk.IsProxCardEncodeReplacementForm()
}