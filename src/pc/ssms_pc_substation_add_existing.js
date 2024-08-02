var PROXCardSSAddExistingSDK = window.PROXCardSSAddExistingSDK || {};

PROXCardSSAddExistingSDK.onAction = () => {
    var lookupOptions =
    {
        defaultEntityType: ShareSdk.Tables.ssms_lut_substation,
        entityTypes: [ShareSdk.Tables.ssms_lut_substation],
        allowMultiSelect: true,
        disableMru: true,
        defaultViewId: "7343ed20-4d87-ed11-81ac-000d3a07ca3c",
        viewIds: ["7343ed20-4d87-ed11-81ac-000d3a07ca3c", "a298031a-7b7b-ed11-81ac-000d3a07ca3c", "b0790ca2-7f7b-ed11-81ac-000d3a07ca3c"],
        // filters: [{ filterXml: "<filter type='and'><condition attribute='clp_type' operator='eq' value='768230000' /></filter>", entityLogicalName: ShareSdk.Tables.ssms_lut_substation }]
    };

    Xrm.Utility.lookupObjects(lookupOptions).then(
        function (success) {
            // console.log(success);
            var formid = ShareSdk.getCurrentFormId()
            PROXCardSSAddExistingSDK.addSubstionsAsync(formid, success).then(res => {
                formContext = Xrm.Page.ui.formContext
                var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.Subgrid_station);
                subgrid.refresh();
            })

        },
        function (error) { console.log(error); });
}

PROXCardSSAddExistingSDK.addSubstionsAsync = async (formid, substationlist) => {
    Xrm.Utility.showProgressIndicator("Loading")
    try {

        await Promise.all(substationlist.map(p => PROXCardSSAddExistingSDK.addSubstionAsync(formid, p.id.replace(/[{}]*/g, "").toLowerCase())))

    } catch (err) {
        ShareSdk.formError(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator()
    }
}

PROXCardSSAddExistingSDK.addSubstionAsync = (formid, id) => {
    var formData = {
        "@odata.context": "https://clp-dev.crm5.dynamics.com/api/data/v9.0/$metadata#$ref",
        "@odata.id": `clp_ssms_lut_substations(${id})`
    }
    return fetch(
        `/api/data/v9.0/clp_proximitycardrequests(${formid})/clp_proximitycardrequest_substation_list/$ref`,
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

PROXCardSSAddExistingSDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null || status === ShareSdk.PROXCardReqStatus.RequestRejected) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false

}