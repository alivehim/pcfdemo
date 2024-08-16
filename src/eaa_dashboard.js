
window.EAADashboardSDK = window.EAADashboardSDK || {}

EAADashboardSDK.Tables = {
    approval: "aia_eaa_approval",
    comment: "aia_eaa_comment",
    config: "aia_eaa_config",
    contact: "aia_eaa_contact",
    country: "aia_eaa_country",
    entity: "aia_eaa_entity",
    form: "aia_eaa_form",
    form_attachment: "aia_eaa_form_attachment",
    local_viewer: "aia_eaa_local_viewer",
    notification: "aia_eaa_notification",
    sla_calculation: "aia_eaa_sla_calculation",
    sla_priority: "aia_eaa_sla_priority",
    subcategory: "aia_eaa_subcategory",
    team_role: "aia_eaa_team_role",
    timeline: "aia_eaa_timeline",
    wf_template: "aia_eaa_wf_template",
}

EAADashboardSDK.Forms = {
    "EAA_Engagement": "7e7ac2ed-c1d1-ec11-a7b5-000d3a80b291",
    "ChangeInitator": "25de5984-f9e6-ec11-bb3d-002248177d91",
    Unpublish: "4fecef90-f3ea-ec11-bb3d-002248177d91"
}

EAADashboardSDK.Views = {
    MyRequest: "973ca551-84d5-ec11-a7b5-000d3a806eb6",
    All: "7e7ac2ed-c1d1-ec11-a7b5-000d3a80b291",
    AwaitingMyResponse: "a0bb4621-8fd5-ec11-a7b5-000d3a806eb6",
    UnpublishedRequests: "70bd4d76-afeb-ec11-bb3d-002248177d91"
}

EAADashboardSDK.onAddNewEAA = () => {
    Xrm.Navigation.navigateTo({
        pageType: "webresource",
        webresourceName: "aia_eaa_new_engagement.html",
    }, {
        target: 2,
        position: 1,
        width: {
            value: 480,
            unit: 'px'
        },
        height: {
            value: 280,
            unit: 'px'
        },
        title: "Add New EAA"
    })
}

EAADashboardSDK.onRawReport = async (primaryControl) => {
    EAADashboardSDK.ExportRawReport(primaryControl)
}

EAADashboardSDK.onTaxReport = (primaryControl) => {
    EAADashboardSDK.ExportReport({
        title: "TaxReport",
        getFetchXmlFunc: () => { return EAADashboardSDK.getTaxReportFetchXml(primaryControl) },
        dataProcessFunc: EAADashboardSDK.taxReportProcess
    })
}

EAADashboardSDK.onExport = (primaryControl) => {
    EAADashboardSDK.ExportReport({
        title: "GeneralReport",
        getFetchXmlFunc: () => { return EAADashboardSDK.getGeneralReportFetchXml(primaryControl) },
        dataProcessFunc: EAADashboardSDK.generalReportProcess
    })
}

EAADashboardSDK.onGuide = () => {
    Xrm.Navigation.navigateTo({
        pageType: "webresource",
        webresourceName: "aia_eaa_guide.html",
    }, {
        target: 2,
        position: 1,
        width: {
            value: 480,
            unit: 'px'
        },
        height: {
            value: 320,
            unit: 'px'
        },
        title: "Policy & Ref Guide"
    })
}

EAADashboardSDK.unpublishSessionKey = 'unpublished'
EAADashboardSDK.RationaleSessionKey = 'rationale'

EAADashboardSDK.onUnpublish = (selectids) => {
    sessionStorage.removeItem(EAADashboardSDK.unpublishSessionKey);
    Xrm.Navigation.navigateTo({
        pageType: "webresource",
        webresourceName: "aia_eaa_engagement_unpublish.html",
        data: `selectedid=${selectids.join()}`
    }, {
        target: 2,
        position: 1,
        height: { value: 50, unit: "%" },
        width: { value: 66, unit: "%" },
        title: "Unpublish content"
    }).then(
        function success() {
            EAAShareSdk.navigateBack()
        },
        function error() {
        }
    );

}

EAADashboardSDK.onUnpublishedRequests = () => {

    Xrm.Navigation.navigateTo({
        pageType: 'entitylist',
        entityName: EAADashboardSDK.Tables.form,
        viewId: EAADashboardSDK.Views.UnpublishedRequests,
    }, {
        target: 2,
        position: 1,
        height: { value: 80, unit: "%" },
        width: { value: 70, unit: "%" },
        title: "Unpublished Requests"
    })
}

EAADashboardSDK.onSendback = async (seletedids) => {
    let sdk = window.parent.EAAShareSdk
    var currentUserId = sdk.getCurrentUserId()
    Xrm.Utility.showProgressIndicator("Loading");
    try {
        for (let id of seletedids) {
            let field = sdk.FormFields.stage
            await fetch(`/api/data/v9.0/aia_eaa_forms(${id})?$select=${field}`)
                .then(r => r.json()).then(json => {
                    let status = sdk.getReopenStageByFormStage(json[field])
                    return sdk.Reopen(id, currentUserId, status).then(res => {
                        return Promise.resolve(res)
                    })
                })
        }
    } catch (e) {
        throw e
    } finally {
        Xrm.Utility.closeProgressIndicator();
        Xrm.Navigation.navigateBack()
    }
}


EAADashboardSDK.onChangeInitator = (selectids) => {
    Xrm.Navigation.navigateTo({
        pageType: 'entityrecord',
        entityName: EAADashboardSDK.Tables.form,
        entityId: selectids[0],
        formId: EAADashboardSDK.Forms.ChangeInitator,
    }, {
        target: 2,
        position: 1,
        height: { value: 80, unit: "%" },
        width: { value: 70, unit: "%" },
        title: "Change Requester"
    })
}


EAADashboardSDK.unpublishForms = async (selectedids, rationale) => {

    Xrm.Utility.showProgressIndicator("Loading");

    try {

        const currentUserId = EAAShareSdk.getCurrentUserId()
        //deactive requests
        await Promise.all(selectedids.map(item => {
            return EAADashboardSDK.unpublish(item, currentUserId, rationale)
        }))
    }
    catch (err) {
        EAADashboardSDK.error(err)
    }
    finally {

        Xrm.Utility.closeProgressIndicator();
    }
}

EAADashboardSDK.unpublish = (formId, currentUserId, rationale) => {
    return fetch(
        `/api/data/v9.0/${EAADashboardSDK.Tables.form}s(${formId})`,
        {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                'Prefer': 'return=representation',
            },
            body: JSON.stringify({
                "aia_sys_form_type": 589460001,
                "aia_eaa_unpublish_user@odata.bind": `/systemusers(${currentUserId})`,
                "aia_eaa_unpublish_date": new Date(),
                "aia_eaa_unpublish_rationale": rationale
            })
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

EAADashboardSDK.ExportReport = async ({
    getFetchXmlFunc = () => { },
    dataProcessFunc = (data) => { },
    title = ""
}) => {

    Xrm.Utility.showProgressIndicator("Loading");
    try {

        await EAADashboardSDK.initExportJs();
        //fetch data
        let fetchXml = getFetchXmlFunc()

        // filter = "?fetchXml=" + encodeURIComponent(fetchXml);
        if (fetchXml.indexOf('$filter') == -1) {
            filter = "?fetchXml=" + encodeURIComponent(fetchXml);
        } else {
            filter = fetchXml
        }

        fetch(`/api/data/v9.0/aia_eaa_forms${filter}`,
            {
                headers: {
                    "Content-Type": "application/json",
                    'Prefer': 'odata.include-annotations="*"',
                }
            }
        ).then(res => res.json()).then(res => {
            if (res.value.length == 0) {
                window.parent.EAAShareSdk.addGlobalMessage(`No record available`, 3)
                return
            }

            let exportMap = dataProcessFunc(res.value)
            EAADashboardSDK.export(exportMap, title).then(() => {
                Xrm.Utility.closeProgressIndicator();
            })
        })
    }
    catch (err) {
        EAADashboardSDK.error(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator();
    }

}

EAADashboardSDK.ExportRawReport = async (primaryControl) => {

    Xrm.Utility.showProgressIndicator("Loading");
    try {

        await EAADashboardSDK.initExportJs();
        //fetch data
        let fetchXml = EAADashboardSDK.getRawReportFetchXml(primaryControl)

        // filter = "?fetchXml=" + encodeURIComponent(fetchXml);
        if (fetchXml.indexOf('$filter') == -1) {
            filter = "?fetchXml=" + encodeURIComponent(fetchXml);
        } else {
            filter = fetchXml
        }

        let formData = await fetch(`/api/data/v9.0/aia_eaa_forms${filter}`,
            {
                headers: {
                    "Content-Type": "application/json",
                    'Prefer': 'odata.include-annotations="*"',
                }
            }
        ).then(res => res.json())

        if (formData.value.length == 0) {
            window.parent.EAAShareSdk.addGlobalMessage(`No record available`, 3)
            return
        }

        let odataFilter = formData.value.map(x => `${x.aia_eaa_formid}`).join(" or aia_eaa_formid eq ")

        let subgrids = await fetch(`/api/data/v9.0/aia_eaa_forms?$select=aia_name&$filter=aia_eaa_formid eq ${odataFilter}&$expand=aia_eaa_contact_sys_form_eaa_form,aia_eaa_local_viewer_sys_form_eaa_form`,
            {
                headers: {
                    "Content-Type": "application/json",
                    'Prefer': 'odata.include-annotations="*"',
                }
            }
        ).then(res => res.json())

        let exportMap = EAADashboardSDK.rawReportProcess(formData.value, subgrids.value)
        EAADashboardSDK.export(exportMap, 'RawReport').then(() => {
            Xrm.Utility.closeProgressIndicator();
        })
    }
    catch (err) {
        EAADashboardSDK.error(err)
    }
    finally {
        Xrm.Utility.closeProgressIndicator();
    }

}

EAADashboardSDK.getRawReportFetchXml = (primaryControl) => {

    var fetchXML = String(primaryControl.getFetchXml());
    let result = fetchXML.match(/<filter type="and">[\w\W]*<\/filter>/)
    if (result) {
        return `<fetch xmlns:generator="MarkMpn.SQL4CDS" version="1.0" output-format="xml-platform" mapping="logical" distinct="true" returntotalrecordcount="true" no-lock="false" >
    <entity name="aia_eaa_form" >
        <all-attributes/>
        ${result[0]}
    </entity>
</fetch>`
    }

    throw new Error('get condition error')
}

EAADashboardSDK.getGeneralReportFetchXml = (primaryControl) => {
    var fetchXML = String(primaryControl.getFetchXml());
    let result = fetchXML.match(/<filter type="and">[\w\W]*<\/filter>/)
    if (result) {
        return `<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="true" returntotalrecordcount="true" no-lock="false">
            <entity name="aia_eaa_form">
                <all-attributes />
                ${result[0]}
            </entity>
        </fetch>`
    }

    throw new Error('get condition error')
}

/**
 * only for both PwC and non-PwC engagements which have been approved at the final level
 * @returns 
 */
EAADashboardSDK.getTaxReportFetchXml = (primaryControl) => {

    var fetchXML = String(primaryControl.getFetchXml());

    let result = fetchXML.match(/<filter type="and">[\w\W]*<\/filter>/)
    if (result) {

        var filter = result[0]

        let taxfilter = filter.replace('<filter type="and">',
            `<filter type="and">
            <condition attribute="aia_sys_form_type" operator="in">
                <value>589450000</value>
                <value>589450002</value>
            </condition>
            <condition attribute="aia_eaa_form_status" operator="in">
                <value>589450013</value>
                <value>589450014</value>
            </condition>`)

        return `<fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="true" returntotalrecordcount="true" no-lock="false">
            <entity name="aia_eaa_form">
                <all-attributes />
                ${taxfilter}
            </entity>
        </fetch>`
    }

    throw new Error('get condition error')


}

EAADashboardSDK.usd = (value) => {
    if (value) {
        return `$${value}`
    }
    return null
}

EAADashboardSDK.getSubcategory = (item) => {

    if (item.aia_sys_form_type === window.parent.EAAShareSdk.RequestType.PwCTax) {
        return item["_aia_eaa_sub_cate_tax_value@OData.Community.Display.V1.FormattedValue"]
    } else {
        return item["_aia_eaa_sub_cate_nontax_value@OData.Community.Display.V1.FormattedValue"]
    }
}

EAADashboardSDK.reportingperiod = (year, quarter) => {
    let x = year
    let y = quarter

    if (!year && !quarter) {
        return 'Pending'
    }

    if (!year) {
        x = 'Pending'
    }

    if (!quarter) {
        y = 'Pending'
    }
    return x + ' ' + y
}

EAADashboardSDK.generalReportProcess = (data) => {

    let exportMap = [];




    // format the data
    data.forEach(item => {



        let rowData = {
            "Reference Number": item.aia_name,
            "Country": item["_aia_eaa_countries_value@OData.Community.Display.V1.FormattedValue"],
            "Entity Name": item["_aia_eaa_entity_sign_value@OData.Community.Display.V1.FormattedValue"],
            "AFS Number": item.aia_eaa_service_number,
            "Requested by": item["_aia_eaa_requester_value@OData.Community.Display.V1.FormattedValue"],
            "Requested on": EAADashboardSDK.dateConverter(item["aia_eaa_requester_date"]),
            "Requested Amount (USD)": EAADashboardSDK.usd(item["aia_eaa_usd_est_cost@OData.Community.Display.V1.FormattedValue"]),
            "Approved by": item["_aia_eaa_final_approver_value@OData.Community.Display.V1.FormattedValue"],
            "Approved on": EAADashboardSDK.dateConverter(item["aia_eaa_final_date"]),
            "Short Scope": item.aia_eaa_description_short,
            "Long Scope": item.aia_eaa_description_long,
            "Type of service": item["aia_eaa_category@OData.Community.Display.V1.FormattedValue"],
            "Subcategory": EAADashboardSDK.getSubcategory(item),
            "Reporting period": EAADashboardSDK.reportingperiod(item.aia_eaa_report_year, item["aia_eaa_quarter@OData.Community.Display.V1.FormattedValue"]),
            "Recurring or not?": item["aia_eaa_recurring@OData.Community.Display.V1.FormattedValue"],
            "Actual Amount (USD)": EAADashboardSDK.usd(item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"]),
            "Final Amount for Reporting (USD)": item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"] ? EAADashboardSDK.usd(item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"]) : EAADashboardSDK.usd(item["aia_eaa_usd_est_cost@OData.Community.Display.V1.FormattedValue"]),
            "Status": `${item["aia_stage@OData.Community.Display.V1.FormattedValue"]} - ${item["aia_eaa_approval_status@OData.Community.Display.V1.FormattedValue"]}`
        }


        var pendingGroup = ['Approved by', 'Approved on']
        for (let field in rowData) {
            EAADashboardSDK.mandatoryList.forEach(x => {
                if (x === field) {
                    if (!rowData[field]) {
                        rowData[field] = "Pending"
                    }

                }
            })

            if (!rowData[field]) {
                rowData[field] = "N/A"
            }

            pendingGroup.forEach(x => {
                if (!rowData[x]) {
                    rowData[x] = "Pending"
                }
            })
        }

        exportMap.push(rowData)
    })

    return exportMap;

}

EAADashboardSDK.taxReportProcess = (data) => {

    let exportMap = [];

    // format the data
    data.forEach(item => {

        let rowData = {
            "Reference Number": item.aia_name,
            "AIA Entity": item["_aia_eaa_entity_sign_value@OData.Community.Display.V1.FormattedValue"],
            "Professional Firm": item.aia_sys_form_type === window.parent.EAAShareSdk.RequestType.Non_PwcTax ? item.aia_eaa_name_of_pro : "PwC",
            "AFS Number": item.aia_eaa_service_number,
            "Description of Services": item.aia_eaa_description_long,
            "Estimate Amount (USD)": EAADashboardSDK.usd(item["aia_eaa_usd_est_cost@OData.Community.Display.V1.FormattedValue"]),
            "Actual Amount (USD)": EAADashboardSDK.usd(item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"]),
            "Final Amount (USD)": item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"] ? EAADashboardSDK.usd(item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"]) : EAADashboardSDK.usd(item["aia_eaa_usd_est_cost@OData.Community.Display.V1.FormattedValue"]),
            "Status": `${item["aia_stage@OData.Community.Display.V1.FormattedValue"]} - ${item["aia_eaa_approval_status@OData.Community.Display.V1.FormattedValue"]}`
        }

        for (let field in rowData) {

            EAADashboardSDK.mandatoryList.forEach(x => {
                if (x === field) {

                    if ((field === 'AFS Number')
                        && item.aia_sys_form_type === window.parent.EAAShareSdk.RequestType.Non_PwcTax) {
                        rowData[field] = "N/A"
                    } else {

                        if (!rowData[field]) {
                            rowData[field] = "Pending"
                        }
                    }

                }
            })

            if (!rowData[field]) {
                rowData[field] = "N/A"
            }

        }


        exportMap.push(rowData)
    })

    return exportMap;
}



EAADashboardSDK.rawReportProcess = (data, subgrids) => {

    let exportMap = [];

    const isNon_PwC_Tax = (item) => {
        return item.aia_sys_form_type === window.parent.EAAShareSdk.RequestType.Non_PwcTax
    }

    const isPwc_Non_Tax = (item) => {
        return item.aia_sys_form_type == window.parent.EAAShareSdk.RequestType.PwcNon_Tax
    }
    // format the data
    data.forEach(item => {

        let rowData = {
            'Reference No.': item.aia_name,
            'Is the service provider PricewaterhouseCoopers Limited ("PwC") or one of its member firms? (whichever the name is)?': item["aia_eaa_define_pwc@OData.Community.Display.V1.FormattedValue"],
            'Name of professional firm': item.aia_sys_form_type === window.parent.EAAShareSdk.RequestType.Non_PwcTax ? item.aia_eaa_name_of_pro : "PwC",
            'Reason for choosing this professional firm': item.aia_eaa_reason_for_pro_firm,
            "Have you read the AIA Group's Policy": isNon_PwC_Tax(item) ? 'N/A' : item["aia_eaa_policy_read@OData.Community.Display.V1.FormattedValue"],
            "Countries": item["_aia_eaa_countries_value@OData.Community.Display.V1.FormattedValue"],
            'Which AIA Entity will bear the fee?': item["_aia_eaa_entity_fee_value@OData.Community.Display.V1.FormattedValue"],
            'Which AIA Entity will sign the engagement letter / statement of work?': item["_aia_eaa_entity_sign_value@OData.Community.Display.V1.FormattedValue"],
            'Department': item.aia_eaa_department,

            'Local Viewer': EAADashboardSDK.generateLocalViewerReport(item.aia_eaa_formid, subgrids),
            'Contact Detail of Engagement Team in Service Provider': EAADashboardSDK.generateContactReport(item.aia_eaa_formid, subgrids),

            'Description of Services (In one line)': item.aia_eaa_description_short,
            'Description of Services (More details)': item.aia_eaa_description_long,
            'State which category of service': item["aia_eaa_category@OData.Community.Display.V1.FormattedValue"],
            'Is this a pre-approved service as defined in Appendix A to D of the Policy?': isNon_PwC_Tax(item) ? 'N/A' : item["aia_eaa_defined_policy@OData.Community.Display.V1.FormattedValue"],
            'State which sub-category of service': EAADashboardSDK.getSubcategory(item),
            'PwC Authorization for Services (AFS) Number': item.aia_eaa_service_number,
            'Rationale of Engaging PwC': item.aia_reason_for_engaging_pwc,

            'State why it should be permissible': item.aia_eaa_state_why_permissible,

            'Estimated Start Date': item["aia_eaa_est_start_date@OData.Community.Display.V1.FormattedValue"],
            'Estimated End Date': item["aia_eaa_est_end_date@OData.Community.Display.V1.FormattedValue"],
            'Estimated duration of engagement': item.aia_eaa_est_duration,
            'Please choose the currency in which the service provider will issue the invoice later': item["_transactioncurrencyid_value@OData.Community.Display.V1.FormattedValue"],

            //Please choose the currency in which the service provider will issue the invoice later
            'Base fee': item["aia_eaa_base_fee@OData.Community.Display.V1.FormattedValue"],
            'Indirect Taxes (e.g. VAT/GST, etc.)': item["aia_eaa_indrect_taxes@OData.Community.Display.V1.FormattedValue"],
            'Estimated Out of pocket expenses': item["aia_eaa_pocket@OData.Community.Display.V1.FormattedValue"],
            'Total estimated cost of engagement': item["aia_eaa_est_cost@OData.Community.Display.V1.FormattedValue"],
            'Exchange rate to convert to USD': item.aia_eaa_est_usd_rate,
            'Base fee (USD)': item["aia_eaa_usd_est_base_fee@OData.Community.Display.V1.FormattedValue"],
            'Indirect Taxes (e.g. VAT/GST, etc.) (USD)': item["aia_eaa_usd_est_indrect_taxes@OData.Community.Display.V1.FormattedValue"],
            'Estimated out-of-pocket expenses (USD)': item["aia_eaa_usd_est_pocket@OData.Community.Display.V1.FormattedValue"],
            'Total estimated cost of engagement (USD)': item["aia_eaa_usd_est_cost@OData.Community.Display.V1.FormattedValue"],

            'Fully budgeted?': item["aia_eaa_fully_budgeted@OData.Community.Display.V1.FormattedValue"],
            'Details of cost arrangement (e.g. fixed fee, time-based using agreed rates, etc.)': item.aia_eaa_cost_arrangement_detail,
            'Other remarks about the fee': item.aia_eaa_fee_other_remarks,
            'Is this a recurring service?': item["aia_eaa_recurring@OData.Community.Display.V1.FormattedValue"],
            'Is the fee included in the latest Audit Fee template?': !isPwc_Non_Tax(item) ? "N/A" : item["aia_eaa_fee_included_audit_fee_template@OData.Community.Display.V1.FormattedValue"],
            'State why not Included in the Audit Fee template': !isPwc_Non_Tax(item) ? "N/A" : item.aia_eaa_why_not_included_audit_fee_template,
            'Is the fee on a contingent fee basis?': item["item.aia_eaa_fee_basis@OData.Community.Display.V1.FormattedValue"],
            'Has this engagement been completed?': item["aia_eaa_completed@OData.Community.Display.V1.FormattedValue"],

            'Actual Base fee': item["aia_eaa_act_fee@OData.Community.Display.V1.FormattedValue"],
            'Actual Indirect Taxes (e.g. VAT/GST, etc.)': item["aia_eaa_act_taxes@OData.Community.Display.V1.FormattedValue"],
            'Actual Out of pocket expenses': item["aia_eaa_act_pocket@OData.Community.Display.V1.FormattedValue"],
            'Actual Total cost of engagement': item["aia_eaa_act_cost@OData.Community.Display.V1.FormattedValue"],


            'Actual Base fee (USD)': item["aia_eaa_usd_act_fee@OData.Community.Display.V1.FormattedValue"],
            'Actual Indirect Taxes (e.g. VAT/GST, etc.) (USD)': item["aia_eaa_usd_act_taxes@OData.Community.Display.V1.FormattedValue"],
            'Actual Out-of-pocket expenses (USD)': item["aia_eaa_usd_act_pocket@OData.Community.Display.V1.FormattedValue"],
            'Actual Total cost of engagement (USD)': item["aia_eaa_usd_act_cost@OData.Community.Display.V1.FormattedValue"],

            'Revised Estimated End Date': item["aia_eaa_revised_date@OData.Community.Display.V1.FormattedValue"],
            'EAA Status': `${item["aia_stage@OData.Community.Display.V1.FormattedValue"]} - ${item["aia_eaa_approval_status@OData.Community.Display.V1.FormattedValue"]}`,

            // 'EAA Year': item.aia_eaa_report_year,
            // 'EAA Quarter': item["aia_eaa_quarter@OData.Community.Display.V1.FormattedValue"],
            'Reporting Period': EAADashboardSDK.reportingperiod(item.aia_eaa_report_year, item["aia_eaa_quarter@OData.Community.Display.V1.FormattedValue"]),

            'Requester': item["_aia_eaa_requester_value@OData.Community.Display.V1.FormattedValue"],
            'Requester Submit Date': item["aia_eaa_requester_date@OData.Community.Display.V1.FormattedValue"],

            'Local Approver': item["_aia_eaa_local_approver_value@OData.Community.Display.V1.FormattedValue"],
            'Local Approver Submit Date': item["aia_eaa_approver_date@OData.Community.Display.V1.FormattedValue"],

            'Group Finance Coordinator': item["_aia_eaa_finance_manager_value@OData.Community.Display.V1.FormattedValue"],
            'Group Finance Coordinator Submit Date': item["aia_eaa_finance_date@OData.Community.Display.V1.FormattedValue"],

            'Head of Group Tax': item["_aia_eaa_head_value@OData.Community.Display.V1.FormattedValue"],
            // 'Estimated Indirect Taxes (Base)': item.aia_eaa_indrect_taxes_base,
            'Head of Group Tax Submit Date': item["aia_eaa_head_date@OData.Community.Display.V1.FormattedValue"],
            'On Behalf Head Of Group Tax': item["_aia_eaa_on_behalf_head_value@OData.Community.Display.V1.FormattedValue"],
            'On Behalf Head Tax Submit Date': item["aia_eaa_on_behalf_head_submit_date@OData.Community.Display.V1.FormattedValue"],

            'Regional CFO': item["_aia_eaa_cfo_value@OData.Community.Display.V1.FormattedValue"],
            'Regional CFO Submit Date': item["aia_eaa_cfo_date@OData.Community.Display.V1.FormattedValue"],
            'On Behalf Regional CFO': item["_aia_eaa_on_behalf_rcfo_value@OData.Community.Display.V1.FormattedValue"],
            'On Behalf RCFO Submit Date': item["aia_on_behalf_rcfo_submit_date@OData.Community.Display.V1.FormattedValue"],

            'EAA Completion Date': item["aia_eaa_completion_date@OData.Community.Display.V1.FormattedValue"],

            'Final Approver': item["_aia_eaa_final_approver_value@OData.Community.Display.V1.FormattedValue"],
            'Final Approval Date': item["aia_eaa_final_date@OData.Community.Display.V1.FormattedValue"],
            'Unpublished By': item["_aia_eaa_unpublish_user_value@OData.Community.Display.V1.FormattedValue"],
            'Unpublished Date': item["aia_eaa_unpublish_date@OData.Community.Display.V1.FormattedValue"],

            // 'Attachments (optional)': item.aia_eaa_attachments,
            // 'Actual Currency': item.aia_eaa_act_currency,
            //'Exchange rate to convert to USD': item.aia_eaa_usd_rate,
            // 'Estimated Out of pocket expenses (Base)': item.aia_eaa_pocket_base,
            //'Countries': item["_aia_eaa_countries_value@OData.Community.Display.V1.FormattedValue"],
            // 'Total estimated cost of engagement (Base)': item.aia_eaa_est_cost_base,
            //'Sub-Category list of pre-approved services': item.aia_eaa_sub_cate_pre_approved_svc,
            //'SLA Reminder': item.aia_eaa_sla_reminder,
            //'Pre-Approval Services by the External Auditor': item.aia_eaa_pre_approval_svc_by_ext_auditor,
            //'Stage': item["aia_stage@OData.Community.Display.V1.FormattedValue"],
            //'Status': item["aia_eaa_approval_status@OData.Community.Display.V1.FormattedValue"],
            //'Approval Evidence HEAD': item.aia_eaa_approval_evidence_head,
            //'Approval Evidence CFO': item.aia_eaa_approval_evidence_cfo,
            //'Tax Manager Submit Date': item.aia_eaa_tax_date,
            // 'Actual Total cost of engagement (Base)': item.aia_eaa_act_cost_base,
            //'Tax Manager': item.aia_eaa_tax_manager,
            // 'Actual Base fee (Base)': item.aia_eaa_act_fee_base,
            // 'Estimated Base fee (Base)': item.aia_eaa_base_fee_base,
            // 'Actual Out of pocket expenses (Base)': item.aia_eaa_act_pocket_base,
            // 'Actual Indirect Taxes (Base)': item.aia_eaa_act_taxes_base,

        }

        for (let field in rowData) {

            console.log(field)
            EAADashboardSDK.mandatoryList.forEach(x => {
                if (x === field) {

                    if ((field === 'PwC Authorization for Services (AFS) Number' || field === 'Fully budgeted?')
                        && item.aia_sys_form_type === window.parent.EAAShareSdk.RequestType.Non_PwcTax) {
                        rowData[field] = "N/A"
                    } else {
                        if (!rowData[field]) {
                            rowData[field] = "Pending"
                        }
                    }
                }
            })

            if (!rowData[field]) {
                rowData[field] = "N/A"
            }

            if (item.aia_sys_form_type) {
                let list = EAADashboardSDK.workflowRoleList[item.aia_sys_form_type]
                list.forEach(x => {
                    if (!rowData[x]) {
                        rowData[x] = "Pending"
                    }
                })
            }

        }

        if (rowData["On Behalf Head Of Group Tax"] && rowData["On Behalf Head Of Group Tax"] != 'N/A' && rowData["On Behalf Head Of Group Tax"] != 'Pending') {
            rowData["Head of Group Tax"] = 'N/A'
            rowData["Head of Group Tax Submit Date"] = 'N/A'
        }

        if (rowData["On Behalf Regional CFO"] && rowData["On Behalf Regional CFO"] != 'N/A' && rowData["On Behalf Regional CFO"] != 'Pending') {
            rowData["Regional CFO"] = 'N/A'
            rowData["Regional CFO Submit Date"] = 'N/A'
        }

        exportMap.push(rowData)
    })

    return exportMap;
}

EAADashboardSDK.mandatoryList = [
    'AFS Number',
    'Is the service provider PricewaterhouseCoopers Limited ("PwC") or one of its member firms?',
    'Countries',
    'Which AIA Entity will sign the engagement letter / statement of work?',
    'Which AIA Entity will bear the fee?',
    'Description of Services (In one line)',
    'Description of Services (More details)',
    'PwC Authorization for Services (AFS) Number',
    'Is this a recurring service?',
    'Estimated End Date',
    'Contact Detail of Engagement Team in Service Provider',
    'Please choose the currency in which the service provider will issue the invoice later',
    'Base fee',
    'Indirect Taxes (e.g. VAT/GST, etc.)',
    'Estimated out-of-pocket expenses',
    'Total estimated cost of engagement',
    'Exchange rate to convert to USD',
    'Base fee (USD)',
    'Indirect Taxes (e.g. VAT/GST, etc.) (USD)',
    'Estimated out-of-pocket expenses (USD)',
    'Total estimated cost of engagement (USD)',
    'Has this engagement been completed?',

    'Actual Base fee',
    'Actual Base fee (USD)',
    'Actual Indirect Taxes (e.g. VAT/GST, etc.)',
    'Actual Indirect Taxes (e.g. VAT/GST, etc.) (USD)',
    'Actual Out of pocket expenses',
    'Actual Out-of-pocket expenses (USD)',
    'Actual Total cost of engagement',
    'Actual Total cost of engagement (USD)',

    'Reporting Period',
    'Requester',
    'Requester Submit Date',
    'EAA Completion Date',
    'Final Approver',
    'Final Approval Date',
    'Local Viewer',

    'Fully budgeted?',
    'AFS Number',

    'Estimated Out of pocket expenses'

]
EAADashboardSDK.workflowRoleList = {
    '589450000':
        [
            'Local Approver',
            'Local Approver Submit Date',
            'Group Finance Coordinator',
            'Group Finance Coordinator Submit Date',
            'Head of Group Tax',
            'Head of Group Tax Submit Date',
            'Regional CFO',
            'Regional CFO Submit Date',
        ],
    '589450001':
        [
            'Local Approver',
            'Local Approver Submit Date',
            'Group Finance Coordinator',
            'Group Finance Coordinator Submit Date',
            'Regional CFO',
            'Regional CFO Submit Date',
        ],
    '589450002':
        [
            'Head of Group Tax',
            'Head of Group Tax Submit Date',
        ]
}

EAADashboardSDK.generateContactReport = (formid, subgrids) => {

    var rows = subgrids.filter(x => x.aia_eaa_formid === formid)
        .map(x => x.aia_eaa_contact_sys_form_eaa_form)

    const result = []
    rows.forEach(m => {
        m.forEach(p => {
            result.push(`Service Provider - First Name: ${p.aia_eaa_first_name};Service Provider - Last Name: ${p.aia_eaa_last_name}; Service Provider - Job Title: ${p.aia_eaa_job_title}; Service Provider - Phone Number: ${p.aia_eaa_phone}; Service Provider - Email Address: ${p.aia_email_address} `)
        })
    })
    if (result.length > 0) {
        return result.join('|');
    }
    else {
        return ''
    }
}

EAADashboardSDK.generateLocalViewerReport = (formid, subgrids) => {

    var rows = subgrids.filter(x => x.aia_eaa_formid === formid)
        .map(x => x.aia_eaa_local_viewer_sys_form_eaa_form)

    const result = []
    rows.forEach(m => {
        m.forEach(p => {
            result.push(`${p["_aia_eaa_recipient_value@OData.Community.Display.V1.FormattedValue"]}`)
        })
    })
    if (result.length > 0) {
        return result.join('|');
    }
    else {
        return ''
    }
}

EAADashboardSDK.initExportJs = async () => {
    await fetch('/WebResources/aia_excel_utils.js'
    ).then(res => res.text()).then(jsData => eval(jsData));
}

EAADashboardSDK.months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

EAADashboardSDK.padTo2Digits = (num) => {
    return num.toString().padStart(2, '0');
}

EAADashboardSDK.dateConverter = (datestr) => {
    if (!datestr) {
        return null
    }
    const date = new Date(datestr)

    return EAADashboardSDK.padTo2Digits(date.getDate()) + "-"
        + EAADashboardSDK.months[date.getMonth()] + "-"
        + date.getFullYear()
}

EAADashboardSDK.export = async (list, topic) => {
    await ExportExcelSdk._exportExcel({
        // the json you want to export
        xlsxData: JSON.stringify(list),
        // the excel file name
        fileName: EAADashboardSDK.getReportName(topic),
        // excel theme
        theme: "TableStyleLight4",
        // column config
        filterColumn: JSON.stringify([])
    })


}

EAADashboardSDK.getReportName = (topic) => {
    return `EAA ${topic} ${EAADashboardSDK.dateFormat(new Date())}`
}


EAADashboardSDK.dateFormat = (current_datetime) => {
    return `${current_datetime.getFullYear()}${EAADashboardSDK.padTo2Digits(current_datetime.getMonth() + 1)}${EAADashboardSDK.padTo2Digits(current_datetime.getDate())}`
}

EAADashboardSDK.error = (err) => {
    //show banner
    var notification =
    {
        showCloseButton: true,
        type: 2,
        level: 2, //error
        message: err.message,
    }

    Xrm.App.addGlobalNotification(notification).then(
        function success(result) {
            // Wait for 5 seconds and then clear the notification
            window.setTimeout(function () {
                Xrm.App.clearGlobalNotification(result);
            }, 10000);
        },
        function (error) {
            console.log(error.message);
            // handle error conditions
        }
    );
}

EAADashboardSDK.Areas = {
    eaa_all_request: "eaa_all_request",
    eaa_my_request: "eaa_my_request",
    eaa_awaiting_my_response: "eaa_awaiting_my_response",
    eaa_change_initator: "eaa_change_initator",
    eaa_unpublished_content: "eaa_unpublished_content",
    eaa_countries: "eaa_countries"
}

EAADashboardSDK.enableAddNewEAAButton = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid != EAADashboardSDK.Areas.eaa_unpublished_content && areaid != EAADashboardSDK.Areas.eaa_change_initator
}

/**
 * 
-Group Tax Viewer
-Group Finance Coordinator
-EAA admin
-Country Admin
 * @returns 
 */
EAADashboardSDK.enableRawReportButton = () => {
    return EAADashboardSDK.enableReportButton()
}

EAADashboardSDK.enableTaxReportButton = () => {
    return EAADashboardSDK.enableReportButton()
}

EAADashboardSDK.enableExportButton = () => {
    return EAADashboardSDK.enableReportButton()
}

EAADashboardSDK.enableReportButton = () => {
    let roles = ["EAA_Country_Admin", "EAA_Business_Admin", "EAA_Group_Tax_Viewer", "EAA_Group_Finance_Coordinator"]
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);

    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    return roles.some(x => { return securityRoles.includes(x) }) &&
        (areaid != EAADashboardSDK.Areas.eaa_unpublished_content && areaid != EAADashboardSDK.Areas.eaa_change_initator)
}

EAADashboardSDK.enableGuideButton = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid != EAADashboardSDK.Areas.eaa_unpublished_content && areaid != EAADashboardSDK.Areas.eaa_change_initator
}

EAADashboardSDK.enableUnpublishButton = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid === EAADashboardSDK.Areas.eaa_unpublished_content
}

EAADashboardSDK.enableUnpublishedrequestsButton = () => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid === EAADashboardSDK.Areas.eaa_unpublished_content
}

EAADashboardSDK.enableChangeInitatorButton = (selectedCount) => {
    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
    return areaid === EAADashboardSDK.Areas.eaa_change_initator
}

EAADashboardSDK.enableSendBackButton = () => {
    return false
}

EAADashboardSDK.enableChangelogButton = () => {
    let roles = ["System Administrator", "EAA_Business_Admin", "EAA_System_Admin"]
    var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);

    var areaid = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

    return roles.some(x => { return securityRoles.includes(x) }) &&
        (areaid != EAADashboardSDK.Areas.eaa_unpublished_content && areaid != EAADashboardSDK.Areas.eaa_change_initator)
}
EAADashboardSDK.openChangelogReport = () => {
    fetch(`api/data/v9.0/aia_eaa_configs?$filter=aia_name eq 'EAA:SharePoint:ChangelogReport'`).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res.value[0].aia_eaa_value
    }).then(URL => {
        Xrm.Navigation.openUrl(URL)
    })
}

