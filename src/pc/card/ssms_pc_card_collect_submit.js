var PROXCardCollectSubmitSDK = window.PROXCardCollectSubmitSDK || {};

PROXCardCollectSubmitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        formContext = Xrm.Page.ui.formContext
        if (Xrm.Page.data.isValid()) {

            await ShareSdk.fillLatestModifiedByAsync()

            const formid = ShareSdk.getCurrentFormId()
            var collectiondate = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.collectiondate).getValue()
            if (ShareSdk.dayDifferent3(collectiondate) > 0) {
                throw new Error(`Collection Date must be before/the same as Today`)
            }

            var cardno = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue()
            var holder = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).getValue()
            var cancelcollection = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.cancelcollection).getValue()

            var clp_holder_value = holder[0].id.replace(/[{}]*/g, "").toLowerCase()
            var clp_holder_name = holder[0].name
            var cards = await PROXCardCollectSubmitSDK.getCardsAsync(ShareSdk.ProximityCardCardType.ISMSProximityCard,
                ShareSdk.ProximityCardStatus.Allocated, clp_holder_value)

            if (cards.length > 0) {
                throw new Error(`${clp_holder_name} is holding another ISMS Proximity Card ${cards[0].clp_name}. Please return that card before collecting ${cardno}`)
            }


            const isDecodeAll = await ShareSdk.isPROXCardDecodeAllAsync(formid)
            if (cancelcollection) {
                const logs = await ShareSdk.getExpityLogAsync(formid, clp_holder_value)
                if (logs.length > 0) {
                    //Update Expiry Event Log
                    await ShareSdk.updateExpiryLogAsync(logs[0].clp_proximity_card_expiry_logid, new Date())
                }

                if (isDecodeAll) {
                    iNewStatus = ShareSdk.ProximityCardStatus.Available
                } else {
                    iNewStatus = ShareSdk.ProximityCardStatus.Returned
                }
                //Update Card Status
                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).setValue(iNewStatus)
                var holder = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).getValue()

                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.holder).setValue()
                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.responsible_engineer).setValue()
                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.expirydate).setValue()

                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.previous_card_holder).setValue(holder)
            } else {
                //Update Card Status
                Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).setValue(ShareSdk.ProximityCardStatus.Allocated)
            }

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.replace_card).setValue()

            const details = await ShareSdk.getValidPROXCardREQDetailsAsync(formid)
            let syncDetails = []

            for (var item of details) {
                if (cancelcollection) {
                    let _isDecodeAll = await ShareSdk.isPROXCardREQDecodeAllAsync(formid, item["request.clp_proximitycardrequestid"])
                    //	If (RsDB3(0) = 0) Then newDtlStatus = PC_DTLSTATUS_CAN Else newDtlStatus = PC_DTLSTATUS_WCD
                    if (_isDecodeAll) {
                        newDtlStatus = ShareSdk.PROXCardIndividualStatus.CollectionCancelled
                    } else {
                        newDtlStatus = ShareSdk.PROXCardIndividualStatus.WaitingforCardDecoding
                    }

                    syncDetails.push({
                        "appno": item["request.clp_name"],
                        "appid": item["request.clp_proximitycardrequestid"],
                        "cardno": item["card.clp_name"],
                        "cardid": item._clp_proximity_card_value,
                        "detailstatus": ShareSdk.PROXCardIndividualStatusMapping[newDtlStatus]
                    })

                    await PROXCardCollectSubmitSDK.updatePROXCardREQDTLwithCancelAsync(item.clp_proximity_card_request_detailid, newDtlStatus, cancelcollection)


                } else {

                    if (item.clp_detail_status === ShareSdk.PROXCardIndividualStatus.WaitingforCardEncoding || !item.clp_detail_status) {
                        newDtlStatus = item.clp_detail_status
                    } else {
                        newDtlStatus = ShareSdk.PROXCardIndividualStatus.Issued
                    }

                    syncDetails.push({
                        "appno": item["request.clp_name"],
                        "appid": item["request.clp_proximitycardrequestid"],
                        "cardno": item["card.clp_name"],
                        "cardid": item._clp_proximity_card_value,
                        "detailstatus": ShareSdk.PROXCardIndividualStatusMapping[newDtlStatus]
                    })

                    await PROXCardCollectSubmitSDK.updatePROXCardREQDTLAsync(item.clp_proximity_card_request_detailid, newDtlStatus, cancelcollection)



                }
            }

            if (cancelcollection && !isDecodeAll) {
                await ShareSdk.addPROXCardNotificationAsync(formid, ShareSdk.CardNotificaitonType.Decode)
            }

            //await PROXCardCollectSubmitSDK.syncDataAsync(syncDetails)

            await Xrm.Page.data.save()

            await ShareSdk.openAlertDialog('Card is collected successfully').then(res => {
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
PROXCardCollectSubmitSDK.syncDataAsync = async (syncDetails) => {
    //api
    for (var item of syncDetails) {
        await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateDeatilStatus,
            item.appid, item.cardid, item.appno, item.cardno, item.detailstatus, '')
    }
}
PROXCardCollectSubmitSDK.updatePROXCardREQDTLwithCancelAsync = (detailid, DTL_STATUS, COLLECT_CANCEL) => {
    var body = {
        "clp_detail_status": DTL_STATUS,
        "clp_collected_date": null,
        "clp_collected_by@odata.bind": null,
        "clp_collect_cancelled": COLLECT_CANCEL
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

PROXCardCollectSubmitSDK.updatePROXCardREQDTLAsync = (detailid, DTL_STATUS, COLLECT_CANCEL) => {
    var body = {
        "clp_detail_status": DTL_STATUS,
        "clp_collect_cancelled": COLLECT_CANCEL,
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

PROXCardCollectSubmitSDK.getCardsAsync = (type, status, holder) => {
    return fetch(`/api/data/v9.0/clp_proximitycardinventories?$filter=clp_card_type eq ${type} and clp_card_status eq ${status} and _clp_holder_value eq '${holder}'  `,
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


PROXCardCollectSubmitSDK.enableButton = () => {
    return ShareSdk.isProximityCardAdmin() && ShareSdk.IsProxCardCollectForm()
}