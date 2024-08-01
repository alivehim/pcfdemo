var PROXCardReqCopySDK = window.PROXCardReqCopySDK || {};

PROXCardReqCopySDK.onAction = () => {
    // Xrm.Utility.showProgressIndicator("Loading")

    async function getRequestDetail(data) {

        Xrm.Utility.showProgressIndicator("Loading")
        try {

            if (data.length === 0) {
                return false;
            }
            let id = data[0].id;
            let r_id = id.slice(1, id.length - 1);

            const result = await ShareSdk.getPROXCardRequestAsync(r_id)


            console.log("Retrieved values: ", result);
            copyDataIntoForm(result);
            await copySubstation(r_id);
            // copyRequestDetail(r_id);
            // perform operations on record retrieval


        } catch (err) {
            ShareSdk.formError(err)
        }
        finally {
            Xrm.Utility.closeProgressIndicator()
        }
    }

    const propertyKey = (attr) => {
        let key = attr + "@Microsoft.Dynamics.CRM.associatednavigationproperty";
        return key;
    }
    const lookupEntityNameKey = (attr) => {
        let key = attr + "@Microsoft.Dynamics.CRM.lookuplogicalname";
        return key;
    }
    const formatValueKey = (attr) => {
        let key = attr + "@OData.Community.Display.V1.FormattedValue";
        return key;
    }

    function copyDataIntoForm(result) {
        let date_keys = ["clp_accessperiodfrom", "clp_accessperiodto"];
        let abandon_keys = ["clp_name",
            "clp_workflow_status",
            "clp_mm",
            "clp_mm_dept_branch",
            "clp_reviewer",
            "clp_reviewed_date",
            "clp_chosen_mm",
            "clp_approver",
            "clp_appr_date",
            "clp_security",
            "clp_sla_count",
            "clp_collection_reminder_date",
            "clp_expiry_3_wd",
            "clp_expiry_5_wd",
            "clp_expiry_10_wd",
            "clp_is_locked",
            "clp_requestdate",
            "clp_is_reminder_email_sended",
            "clp_rejected_date",
            "clp_approver_deptbranch",
            "clp_nosofss",
            "clp_admin",
            "clp_appr_remarks",
            "clp_admin_remarks",
            "clp_mm_remarks",
            "clp_security_remarks",
            "clp_remarks",
            "clp_extend_from_request",
            "clp_readunderstoodandagreed",
            "clp_is_migration_data",
            "clp_card_allocation_date"
        ]
        let lookup_keys = ["_clp_lut_ctrcomp_value", "_clp_chosenapprover_value", "_clp_responsible_cp_value"];
        let keys = Object.keys(result)
            .filter(key => key.indexOf("@") === -1 && date_keys.indexOf(key) === -1 && lookup_keys.indexOf(key) === -1)
            .filter(key => abandon_keys.indexOf(key) === -1);
        console.log("result: ", result);
        let ctx = Xrm.Page.ui.formContext;

        // 对日期特殊处理 https://learn.microsoft.com/en-us/power-apps/developer/model-driven-apps/clientapi/reference/attributes/setvalue
        for (let key of date_keys) {
            // "2022-12-27T00:00:00Z"
            try {
                let date = new Date(result[key]);
                let control = ctx.getAttribute(key);
                // console.log(date, control);
                control.setValue(date);
                control.setSubmitMode("always")
            } catch (e) {
                // console.log("no such key control")
            } finally {

            }
        }
        // 对lookup特殊处理
        for (let key of lookup_keys) {
            // "2022-12-27T00:00:00Z"
            try {
                let lookupValue = [];
                let lookupObject = new Object()
                lookupObject.id = result[key];

                lookupObject.name = result[formatValueKey(key)];

                lookupObject.entityType = result[lookupEntityNameKey(key)];

                lookupValue.push(lookupObject);
                let control = ctx.getAttribute(key.slice(1, key.length - 6)); // 取出form key
                console.log("lookup populate", key.slice(1, key.length - 6));
                control.setValue(lookupValue);
            } catch (e) {
                console.log("lookup failed", e)
            } finally {

            }
        }

        // 其他相关字段
        for (let key of keys) {
            try {
                let control = ctx.getAttribute(key);
                control.setValue(result[key]);
            } catch (e) {
                // console.log("no such key control")
            } finally {

            }
        }
    }

    async function copySubstation(id) {
        const formid = ShareSdk.getCurrentFormId()

        const currentSubstation = await ShareSdk.getPROXCardSubstationsAsync(formid)
        for (var item of currentSubstation) {
            await ShareSdk.removeSSAsync(formid, item.clp_ssms_lut_substationid)
        }

        const selectRequest = await ShareSdk.getPROXCardRequestAsync(id)

        if (selectRequest.clp_is_migration_data === true) {

            await copySubstationsFromMigratedRequestAsync(formid, selectRequest.clp_ss_display)

        } else {
            const result = await ShareSdk.getPROXCardSubstationsAsync(id)
            await ShareSdk.addSubstionsAsync(formid, result.map(p => p.clp_ssms_lut_substationid))
        }

        formContext = Xrm.Page.ui.formContext
        var subgrid = formContext.ui.controls.get(ShareSdk.ProximityCardRequestSubGrids.Subgrid_station);
        subgrid.refresh();
    }

    const copySubstationsFromMigratedRequestAsync = async (formid, ss_display) => {
        const substations = ss_display.split(",")

        for (var item of substations) {
            const sub = await ShareSdk.getSubstationByScadaCodeAsync(item.trim())

            if (sub) {
                await ShareSdk.addSubstionAsync(formid, sub.clp_ssms_lut_substationid)
            }
        }
    }

    var lookupOptions = {
        defaultEntityType: "clp_proximitycardrequest",
        entityTypes: ["clp_proximitycardrequest"],
        allowMultiSelect: false,
        defaultViewId: "38e544f4-2436-48e1-ad73-c4dd0b257bdc",
        viewIds: ["38e544f4-2436-48e1-ad73-c4dd0b257bdc"],
        disableMru: true,
        // searchText: "Allison",
        filters: [{
            filterXml: "<filter type='and'><condition attribute='clp_workflow_status' operator='not-null'/></filter>",
            entityLogicalName: "clp_proximitycardrequest"
        }]
    };

    let r_pickup = Xrm.Utility.lookupObjects(lookupOptions).then(
        function (success) {
            // Xrm.Utility.closeProgressIndicator();
            console.log("success callback: ", success);
            return success

        },
        function (error) {
            console.log(error);
        });
    r_pickup.then(getRequestDetail);

}


PROXCardReqCopySDK.enableButton = () => {

    if (!ShareSdk.IsProxRequestForm()) {
        return false
    }

    var status = Xrm.Page.data.entity.attributes.getByName(ShareSdk.PROXCardReqFields.workflow_status).getValue()
    if (status === null) {
        return ShareSdk.isPROXCardRequestor()
    }

    return false
}