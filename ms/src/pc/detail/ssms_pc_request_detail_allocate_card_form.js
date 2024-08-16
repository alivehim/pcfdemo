var PROXCardDetailAllocateFormSDK = window.PROXCardDetailAllocateFormSDK || {};


PROXCardDetailAllocateFormSDK.onFormLoaded = async (context) => {

    formContext = Xrm.Page.ui.formContext
    var currentForm = formContext.ui.formSelector.getCurrentItem();

    if (currentForm.getId() != ShareSdk.Forms.Detail_Allocated) {
        formItem = formContext.ui.formSelector.items.get(ShareSdk.Forms.Detail_Allocated);
        formItem.navigate();
    }

    PROXCardDetailAllocateFormSDK.unlockFields()

    await PROXCardDetailAllocateFormSDK.initCards()

}

PROXCardDetailAllocateFormSDK.unlockFields = () => {
    const inputData = Xrm.Utility.getPageContext().input.data

    const lockfields = (lockFields) => {
        for (let attr in Xrm.Page.data.entity.attributes._collection) {
            if (!lockFields.some(p => p == attr)) {
                ShareSdk.setDisableControl(attr)
            } else {
                ShareSdk.setDisableControl(attr, false)
            }
        }
    }



    if (inputData.action != ShareSdk.ProximityCardAction.Reject) {
        ShareSdk.setVisiableControl(ShareSdk.PROXCardRequetDetailFields.proximity_card)
        ShareSdk.setRequiredControl(ShareSdk.PROXCardRequetDetailFields.proximity_card)
        ShareSdk.setVisiableControl(ShareSdk.PROXCardRequetDetailFields.card_no, false)
        lockfields([ShareSdk.PROXCardRequetDetailFields.proximity_card])
    }
    else {
        //set remarks mandatory
        ShareSdk.setVisiableControl(ShareSdk.PROXCardRequetDetailFields.remarks)
        ShareSdk.setRequiredControl(ShareSdk.PROXCardRequetDetailFields.remarks)
        lockfields([ShareSdk.PROXCardRequetDetailFields.remarks])
    }

}

PROXCardDetailAllocateFormSDK.initCards = async () => {
    const inputData = Xrm.Utility.getPageContext().input.data


    if (inputData.action != ShareSdk.ProximityCardAction.Reject) {

        const apptype = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.app_type).getValue()
        // const proxcard = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).getValue()

        if (apptype === ShareSdk.PROXCardReqAPPType.NewISMS) {
            const applicant = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card_applicant).getValue()
            const result = await PROXCardDetailAllocateFormSDK.getCardsAsync(ShareSdk.getLookupId(applicant))
            if (result.length > 0) {
                const cardid = result[0]["card.clp_proximitycardinventoryid"]
                PROXCardDetailAllocateFormSDK.addCustomViewToProxCard(cardid)
            } else {
                var issuedepartment = await ShareSdk.getCurrentAdminHasDeptAsync()
                PROXCardDetailAllocateFormSDK.installCardFilter(issuedepartment)
            }
        }

    }

}

PROXCardDetailAllocateFormSDK.installCardFilter = (issuedepartment) => {


    formContext = Xrm.Page.ui.formContext
    formContext.getControl(ShareSdk.PROXCardRequetDetailFields.proximity_card).addPreSearch(function (executionContext) {

        var filterxml = `<filter type='and'>
        <condition attribute="clp_issuedepartment" operator="eq" value="${issuedepartment}" />
        </filter>`;

        executionContext.getFormContext().getControl(ShareSdk.PROXCardRequetDetailFields.proximity_card).addCustomFilter(filterxml, ShareSdk.Tables.proximitycardinventory);

    });
}

PROXCardDetailAllocateFormSDK.getCardsAsync = (applicantid) => {
    var fetchXml = `<fetch>
  <entity name="clp_proximitycardrequest">
    <filter>
      <condition attribute="clp_workflow_status" operator="ne" value="768230006" />
    </filter>
    <link-entity name="clp_proximity_card_request_detail" from="clp_proximity_card_request" to="clp_proximitycardrequestid">
      <filter>
        <condition attribute="clp_detail_status" operator="in" value="">
          <value>768230000</value>
          <value>768230001</value>
          <value>768230002</value>
        </condition>
        <condition attribute="clp_app_type" operator="in" value="">
          <value>768230000</value>
          <value>768230001</value>
        </condition>
        <condition attribute="clp_replaced_by" operator="null" />
        <condition attribute="clp_proximity_card_applicant" operator="eq" value="${applicantid}" />
        <condition attribute="clp_proximity_card" operator="not-null" />
      </filter>
      <link-entity name="clp_proximitycardinventory" from="clp_proximitycardinventoryid" to="clp_proximity_card" alias="card">
        <attribute name="clp_proximitycardinventoryid" />
      </link-entity>
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


PROXCardDetailAllocateFormSDK.onPROXCardChanged = () => {
    var card = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.proximity_card).getValue()
    if (card) {
        var name = card[0].name
        Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardRequetDetailFields.card_no).setValue(name)
    }
}



PROXCardDetailAllocateFormSDK.addCustomViewToProxCard = (cardid) => {
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
