var PROXCardReturnFormSumbitSDK = window.PROXCardReturnFormSumbitSDK || {};

PROXCardReturnFormSumbitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        formContext = Xrm.Page.ui.formContext


        const inputData = Xrm.Utility.getPageContext().input.data

        const returndate = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.returndate).getValue()

        //api
        let syncDetails = []

        if (inputData?.newCardStatus && Xrm.Page.data.isValid()) {

            await ShareSdk.fillLatestModifiedByAsync()
            
            var formid = ShareSdk.getCurrentFormId()
            const holer = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).getValue()
            //Update SSMS_PC_EXPIRY_LOG
            const expirylogs = await ShareSdk.getExpityLogAsync(formid, holer ? ShareSdk.getLookupId(holer) : null)
            if (expirylogs.length != 0) {
                await ShareSdk.updateExpiryLogAsync(expirylogs[0].clp_proximity_card_expiry_logid, returndate, new Date())
            }

            //	'Check if all the SS were decoded
            newCardStatus = inputData.newCardStatus
            const isDecodeAll = await ShareSdk.isPROXCardDecodeAllAsync(formid)
            if (isDecodeAll && newCardStatus === ShareSdk.ProximityCardStatus.Returned) {
                newCardStatus = ShareSdk.ProximityCardStatus.Available
            }
            //Update SSMS_PC_DTL

            var details = await ShareSdk.getValidPROXCardREQDetailsAsync(formid)
            var currentCLPUser = await ShareSdk.getCurrentClpUserAsync()
            for (var detail of details) {
                var isREQDecodeAll = await ShareSdk.isPROXCardREQDecodeAllAsync(formid, detail["request.clp_proximitycardrequestid"])
                if (isREQDecodeAll) {
                    newDtlStatus = ShareSdk.PROXCardIndividualStatus.Completed
                } else {
                    newDtlStatus = ShareSdk.PROXCardIndividualStatus.WaitingforCardDecoding
                }

                syncDetails.push({
                    "appno": detail["request.clp_name"],
                    "appid": detail["request.clp_proximitycardrequestid"],
                    "cardno": detail["card.clp_name"],
                    "cardid": detail._clp_proximity_card_value,
                    "detailstatus": ShareSdk.PROXCardIndividualStatusMapping[newDtlStatus]
                })

                await ShareSdk.updatePROXCardREQDTLStatusForReturnAsync(detail.clp_proximity_card_request_detailid, newDtlStatus, currentCLPUser.clp_userid, returndate, new Date())


            }

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).setValue(newCardStatus)
            //clear holder,re,expity date

            const holder = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).getValue()
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).setValue()
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.responsible_engineer).setValue()
            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.expirydate).setValue()

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.previous_card_holder).setValue(holder)

            if (!isDecodeAll) {

                await ShareSdk.addPROXCardNotificationAsync(formid, ShareSdk.CardNotificaitonType.Decode)
            }

            //await PROXCardReturnFormSumbitSDK.syncDataAsync(syncDetails)

            await Xrm.Page.data.save()


            await ShareSdk.openAlertDialog('Card is returned successfully').then(res => {
                // var pageInput = {
                //     pageType: "entitylist",
                //     entityName: ShareSdk.Tables.proximitycardinventory,
                // }

                // Xrm.Navigation.navigateTo(pageInput)
            })
            formContext.ui.close()
        }

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardReturnFormSumbitSDK.syncDataAsync = async (syncDetails) => {
    //api
    for (var item of syncDetails) {
        await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateDeatilStatus,
            item.appid, item.cardid, item.appno, item.cardno, item.detailstatus, '')
    }
}
// PROXCardReturnFormSumbitSDK.getEncodesByCardAsync = (cardid) => {
//     return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$select=clp_name&$filter=_clp_card_value eq '${cardid}'&$count=true`,
//         {
//             headers: {
//                 "Content-Type": "application/json",
//                 'Prefer': 'odata.include-annotations="*"',
//             }
//         }
//     ).then(res => res.json()).then(res => {
//         if (res.error) {
//             throw new Error(res.error.message)
//         }

//         return Promise.resolve(res.value)


//     }).catch(err => {
//         console.error(err)
//         throw err;
//     })
// }

// PROXCardReturnFormSumbitSDK.getEncodesByCardandReqAsync = (cardid, reqid) => {
//     return fetch(`/api/data/v9.0/clp_proximity_card_encodes?$select=clp_name&$filter=_clp_card_value eq '${cardid}' and _clp_proximity_card_request_value eq '${reqid}'&$count=true`,
//         {
//             headers: {
//                 "Content-Type": "application/json",
//                 'Prefer': 'odata.include-annotations="*"',
//             }
//         }
//     ).then(res => res.json()).then(res => {

//         if (res.error) {
//             throw new Error(res.error.message)
//         }

//         return Promise.resolve(res.value)


//     }).catch(err => {
//         console.error(err)
//         throw err;
//     })
// }

// PROXCardReturnFormSumbitSDK.getPROXCardREQDetailsAsync = (cardid) => {

//     var fetchXml = `<fetch version="1.0" output-format="xml-platform" mapping="logical">
//   <entity name="clp_proximity_card_request_detail">
//     <filter>
//       <condition attribute="clp_proximity_card" operator="eq" value="${cardid}" />
//       <condition attribute="clp_detail_status" operator="not-in" value="">
//         <value>768230004</value>
//         <value>768230005</value>
//         <value>768230006</value>
//       </condition>
//     </filter>
//     <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" alias="request">
//       <attribute name="clp_proximitycardrequestid" />
//       <filter>
//         <condition attribute="clp_workflow_status" operator="ne" value="768230006" />
//       </filter>
//     </link-entity>
//   </entity>
// </fetch>`
//     return fetch(`/api/data/v9.0/clp_proximity_card_request_details?fetchXml=${encodeURI(fetchXml)}`,
//         {
//             headers: {
//                 "Content-Type": "application/json",
//                 'Prefer': 'odata.include-annotations="*"',
//             }
//         }
//     ).then(res => res.json()).then(res => {

//         if (res.error) {
//             throw new Error(res.error.message);
//         }

//         return Promise.resolve(res.value)


//     }).catch(err => {
//         console.error(err)
//         throw err;
//     })
// }

PROXCardReturnFormSumbitSDK.enableButton = () => {
    return ShareSdk.isProximityCardAdmin() && ShareSdk.IsProxCardReturnForm()
}