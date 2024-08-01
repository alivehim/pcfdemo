var PROXCardDecodeFormSumbitSDK = window.PROXCardDecodeFormSumbitSDK || {};

PROXCardDecodeFormSumbitSDK.onAction = async () => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {
        formContext = Xrm.Page.ui.formContext

        const card_status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).getValue()
        const card_type = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_type).getValue()
        const card_no = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue()

        if (card_type != ShareSdk.ProximityCardCardType.ISMSProximityCard) {
            throw new Error('Card Type must be ISMS')
        }

        if (card_status != ShareSdk.ProximityCardStatus.Returned &&
            card_status != ShareSdk.ProximityCardStatus.Damaged &&
            card_status != ShareSdk.ProximityCardStatus.Lost
        ) {
            throw new Error('Card Status must be Returned/Damaged/Lost')
        }


        const formid = ShareSdk.getCurrentFormId()
        var currentCLPUser = await ShareSdk.fillLatestModifiedByAsync()
        const details = await PROXCardDecodeFormSumbitSDK.getPROXCardREQDetailsAsync(formid)

        let syncDetails = []
        for (var detail of details) {
            if (detail.clp_collect_cancel) {
                newDtlStatus = ShareSdk.PROXCardIndividualStatus.CollectionCancelled
            } else {
                newDtlStatus = ShareSdk.PROXCardIndividualStatus.Completed
            }

            syncDetails.push({
                "appno": detail["request.clp_name"],
                "appid": detail["request.clp_proximitycardrequestid"],
                "cardno": detail["card.clp_name"],
                "cardid": detail["card.clp_proximitycardinventoryid"],
                "detailstatus": ShareSdk.PROXCardIndividualStatusMapping[newDtlStatus]
            })

            await ShareSdk.updatePROXCardREQDTLStatusAsync(detail.clp_proximity_card_request_detailid, newDtlStatus)

        }

        //Insert Decode List
        const encodeList = await ShareSdk.getPROXCardEncodeListAsync(formid)

        for (var item of encodeList) {
            await ShareSdk.addPROXCardDecodeAsync(item._clp_proximity_card_request_value, item._clp_card_value, item.clp_ss_code, currentCLPUser.clp_user)
        }
        //Delete Encode List

        for (var item of encodeList) {
            await ShareSdk.DeleteEncodeAsync(item.clp_proximity_card_encodeid)
        }

        var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).getValue()
        if (status === ShareSdk.ProximityCardStatus.Returned) {

            Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.card_status).setValue(ShareSdk.ProximityCardStatus.Available)
        }

        //await PROXCardDecodeFormSumbitSDK.syncDataAsync(formid, syncDetails, card_no)

        await Xrm.Page.data.save()


        await ShareSdk.openAlertDialog('Card is decoded successfully').then(res => {

            // if(window.location.href.includes("formid=34dd04b9-4249-ed11-bba2-000d3a0871cb")){
            //     var pageInput = {
            //         pageType: "entitylist",
            //         entityName: ShareSdk.Tables.proximitycardinventory,
            //     }

            //     Xrm.Navigation.navigateTo(pageInput)
            // }

        })

        formContext.ui.close()


    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardDecodeFormSumbitSDK.syncDataAsync = async (cardid, syncDetails, card_no) => {
    //api
    await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.DeleteEncodes, null, cardid,
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardInventoryFields.name).getValue(), '')

    for (var item of syncDetails) {
        await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.UpdateDeatilStatus,
            item.appid, item.cardid, item.appno, item.cardno, item.detailstatus, '')
    }

    // await ShareSdk.addSyncDataAsync(ShareSdk.DataSyncType.DeleteEncodes,
    //     null, cardid, card_no, '', '', '')
}

PROXCardDecodeFormSumbitSDK.getPROXCardREQDetailsAsync = (cardid) => {

    var fetchXml = `<fetch>
  <entity name="clp_proximity_card_request_detail">
    <attribute name="clp_collect_cancelled" />
    <attribute name="clp_proximity_card_request_detailid" />
    <filter>
      <condition attribute="clp_detail_status" operator="eq" value="768230003" />
    </filter>
    <link-entity name="clp_proximitycardinventory" from="clp_proximitycardinventoryid" to="clp_proximity_card" alias="card">
      <attribute name="clp_name" />
      <attribute name="clp_proximitycardinventoryid" />
      <filter>
        <condition attribute="clp_proximitycardinventoryid" operator="eq" value="${cardid}" />
      </filter>
    </link-entity>
    <link-entity name="clp_proximitycardrequest" from="clp_proximitycardrequestid" to="clp_proximity_card_request" alias="request">
      <attribute name="clp_name" />
      <attribute name="clp_proximitycardrequestid" />
      <filter>
        <condition attribute="clp_workflow_status" operator="eq" value="768230005" />
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

    // var statusFilter = [ShareSdk.PROXCardIndividualStatus.WaitingforCardDecoding]

    // var condition = `_clp_proximity_card_value eq '${carid}' and ` + statusFilter.map(p => `clp_detail_status ne ${p}`).join(' and ')
    // return fetch(`/api/data/v9.0/clp_proximity_card_request_details?$filter=${condition}&$expand=clp_proximity_card_request($filter=clp_workflow_status eq ${ShareSdk.PROXCardReqStatus.Completed})`,
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


PROXCardDecodeFormSumbitSDK.enableButton = () => {
    return ShareSdk.isProximityCardSecurity() && ShareSdk.IsProxCardDecodeForm()
}