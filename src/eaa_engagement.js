/**
 * EngagementSDK for  Engagement Form
 */
window.EngagementSDK = window.EngagementSDK || {}

EngagementSDK.Tabs = {
    Actual_Fee_Update: "tab_Actual_Fee_Update",
    Status_Summary: "tab_Status_Summary",
    Comment: "tab_Comment"
}

EngagementSDK.categoryValue = 0
EngagementSDK.countryid = '00000000-0000-0000-0000-000000000000'
/**
 * triggered when form loaded
 */
EngagementSDK.formOnLoad = (context) => {
    // Xrm.UI._context.Router._preNavigationHandlers = []

    var formContext = context.getFormContext();

    EngagementSDK.hideTab(formContext);

    EngagementSDK.switchTab(formContext)

    EngagementSDK.AddDefaultFilter(context)

    EngagementSDK.filterCategoryService()

    EngagementSDK.showRetionalofengaginePwC()

    EngagementSDK.ShowIscontengentfeebasis()

    EngagementSDK.unlockFieldsBusinessAdmin()

    EngagementSDK.unlockAcutalFeeFields()

    EngagementSDK.unlockAFXNumber()


    // EngagementSDK.showReportingPeriod()

    EngagementSDK.unlockQuarter();

    // EngagementSDK.initControl();

    formContext.getControl(EAAShareSdk.FormFields.sub_cate_tax).addPreSearch(EngagementSDK.subcategoryTaxFilter);
    formContext.getControl(EAAShareSdk.FormFields.sub_cate_nontax).addPreSearch(EngagementSDK.subcategoryNonTaxFilter);
    formContext.getControl(EAAShareSdk.FormFields.entity_fee).addPreSearch(EngagementSDK.entityFeeFilter);
    formContext.getControl(EAAShareSdk.FormFields.entity_sign).addPreSearch(EngagementSDK.entitySignFilter);


    // EngagementSDK.reviseEntityPlaceHolder()

}

EngagementSDK._doUntil = (doFunc, untilFunc, maxMs = 20000, delayMs = 50) => {
    const start = new Date();

    const setTimeoutFunc = () => {
        if (untilFunc()) {
            doFunc()
        } else if (new Date() - start < maxMs) {
            setTimeout(() => {
                setTimeoutFunc()
            }, delayMs)
        }
    }
    setTimeoutFunc()
}

EngagementSDK.reviseErrorMessage = () => {
    const selector = () => {
        return window.parent.document.querySelector(`span[id*=aia_eaa_policy_read-error-message]`)
    }
    EAAShareSdk._doUntil(() => {
        // 
        let msg = `Please make sure you have read the AIA Group's Policy before submitting the request and link to the policy is provided below.`
        try {
            window.parent.document.querySelector(`span[id*=aia_eaa_policy_read-error-message]`).innerText = msg
        }
        catch (e) {
            console.error(e)
        }
    }, selector)
}

EngagementSDK.remarkTooltipOnLoad = () => {
    const fieldName = 'aia_eaa_fee_other_remarks'
    const tooltipId = `${fieldName}-tooltip`
    const selector = () => {
        return window.parent.document.querySelector(`label[id*="${fieldName}-field-label"]`)
    }
    const titleSelector = () => {
        return window.parent.document.querySelector(`div[id*="${fieldName}-FieldSectionItemContainer"] span[title]`)
    }

    EAAShareSdk._doUntil(
        () => {
            if (window.parent.document.querySelector(`#${tooltipId}`)) {
                return
            }

            window.React = window.parent.React
            window.ReactDOM = window.parent.ReactDOM
            window.FluentUIReact = window.parent.FluentUIReact

            const tooltipcontext = React.createElement(FluentUIReact.Stack, {},
                React.createElement("p", {
                    style: {
                        whiteSpace: "pre-line",
                    }
                }, "Below are example of remarks to be included in the EAA request:"),
                React.createElement("p", {
                    style: {
                        whiteSpace: "pre-line",
                    }
                }, "(i) What is the indirect tax rate used in cost estimation above"),
                React.createElement("p", {
                    style: {
                        whiteSpace: "pre-line",
                    }
                }, "(ii) If the fee is time-based using agreed rates, is there a maximum cap?  If so, how much is the capped amount?"),
            )
            EngagementSDK.addRemarkTooltip(selector(), tooltipcontext, tooltipId)
            // remove title attribute
            titleSelector().removeAttribute("title")
        },
        () => selector() && titleSelector() && window.parent.FluentUIReact,
    )
}


EngagementSDK.addRemarkTooltip = (target, tips, tooltipId = new Date().getTime().toString()) => {
    window.React = window.parent.React
    window.ReactDOM = window.parent.ReactDOM
    window.FluentUIReact = window.parent.FluentUIReact

    const cloneTarget = target.cloneNode(true)

    const comp = () => {
        const tooltipProps = {
            onRenderContent: () => tips
        }

        const labelRef = React.useRef()
        React.useEffect(() => {
            labelRef.current.appendChild(cloneTarget)
        }, [])

        return (
            React.createElement(FluentUIReact.TooltipHost, {
                tooltipProps,
                id: tooltipId,
                directionalHint: FluentUIReact.DirectionalHint.bottomLeftEdge
            },
                React.createElement("div", {
                    ref: labelRef,
                }, React.createElement(FluentUIReact.FontIcon, {
                    iconName: "Info",
                    style: {
                        float: 'right',
                        "margin-left": "5px",
                        "margin-top": "2px"
                    }
                }))

            )
        )
    }

    ReactDOM.render(
        React.createElement(React.StrictMode, {},
            React.createElement(comp)
        ),
        target
    )
}

EngagementSDK.professionalFirmTooltipOnLoad = () => {
    var requestType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()
    if (requestType === EAAShareSdk.RequestType.Non_PwcTax) {
        const fieldName = 'aia_eaa_reason_for_pro_firm'
        const tooltipId = `${fieldName}-tooltip`
        const selector = () => {
            return window.parent.document.querySelector(`label[id*="${fieldName}-field-label"]`)
        }
        const titleSelector = () => {
            return window.parent.document.querySelector(`div[id*="${fieldName}-FieldSectionItemContainer"] span[title]`)
        }

        EAAShareSdk._doUntil(
            () => {
                if (window.parent.document.querySelector(`#${tooltipId}`)) {
                    return
                }

                window.React = window.parent.React
                window.ReactDOM = window.parent.ReactDOM
                window.FluentUIReact = window.parent.FluentUIReact

                const tooltipcontext = React.createElement(FluentUIReact.Stack, {},
                    React.createElement("p", {
                        style: {
                            whiteSpace: "pre-line",
                        }
                    }, "If the professional firm is NOT EY, KPMG or Deloitte (or one of their member firms), please advise the reason for choosing this professional firm"),
                    React.createElement("p", {
                        style: {
                            whiteSpace: "pre-line",
                        }
                    }, "(Please fill in “N/A” or “Not applicable” as appropriate)"),
                )
                EngagementSDK.addRemarkTooltip(selector(), tooltipcontext, tooltipId)
                // remove title attribute
                titleSelector().removeAttribute("title")
            },
            () => selector() && titleSelector() && window.parent.FluentUIReact,
        )
    }
}

EngagementSDK.addExTooltip = (target, tips, tooltipId = new Date().getTime().toString()) => {
    window.React = window.parent.React
    window.ReactDOM = window.parent.ReactDOM
    window.FluentUIReact = window.parent.FluentUIReact

    const cloneTarget = target.cloneNode(true)

    const comp = () => {
        const tooltipProps = {
            onRenderContent: () => tips
        }

        const labelRef = React.useRef()
        React.useEffect(() => {
            labelRef.current.appendChild(cloneTarget)
        }, [])

        return (
            React.createElement(FluentUIReact.TooltipHost, {
                tooltipProps,
                id: tooltipId,
                directionalHint: FluentUIReact.DirectionalHint.bottomLeftEdge
            },
                React.createElement("div", {
                    ref: labelRef,
                    style: {
                        display: "flex",
                        "flex-direction": "row-reverse",
                        "justify-content": "start"
                    }
                }, React.createElement(FluentUIReact.FontIcon, {
                    iconName: "Info",
                    style: {

                        "margin-left": "5px",
                        "margin-top": "2px"
                    }
                }))

            )
        )
    }

    ReactDOM.render(
        React.createElement(React.StrictMode, {},
            React.createElement(comp)
        ),
        target
    )
}

EngagementSDK.approverTooltipOnLoad = () => {
    var requestType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()
    if (requestType != EAAShareSdk.RequestType.Non_PwcTax) {
        const fieldName = 'aia_eaa_local_approver'
        const tooltipId = `${fieldName}-tooltip`
        const selector = () => {
            return window.parent.document.querySelector(`label[id*="${fieldName}-field-label"]`)
        }
        const titleSelector = () => {
            return window.parent.document.querySelector(`div[id*="${fieldName}-FieldSectionItemContainer"] span[title]`)
        }

        EAAShareSdk._doUntil(
            () => {
                if (window.parent.document.querySelector(`#${tooltipId}`)) {
                    return
                }

                window.React = window.parent.React
                window.ReactDOM = window.parent.ReactDOM
                window.FluentUIReact = window.parent.FluentUIReact

                const tooltipcontext = React.createElement(FluentUIReact.Stack, {},
                    React.createElement("p", {
                        style: {
                            whiteSpace: "pre-line",
                        }
                    }, "1) Local CFO for BU engagements"),
                    React.createElement("p", {
                        style: {
                            whiteSpace: "pre-line",
                        }
                    }, "2) Department Head for Group Office engagements"),
                )
                EngagementSDK.addExTooltip(selector(), tooltipcontext, tooltipId)
                // remove title attribute
                titleSelector().removeAttribute("title")
            },
            () => selector() && titleSelector() && window.parent.FluentUIReact,
        )
    }
}

EngagementSDK.reviseEntityPlaceHolder = () => {
    EngagementSDK.placeHolder('aia_eaa_entity_fee')
    EngagementSDK.placeHolder('aia_eaa_entity_sign')
}
EngagementSDK.placeHolder = (controlid) => {
    const inputControlId = `input[id*="${controlid}.fieldControl-LookupResultsDropdown`
    const selector = () => {
        return window.parent.document.querySelector(inputControlId)
    }

    EAAShareSdk._doUntil(() => {
        window.parent.document.querySelector(inputControlId)?.addEventListener("focus", () => {
            window.parent.document.querySelector(inputControlId)?.setAttribute("placeholder", "Look for Entity")
        }, false);

        window.parent.document.querySelector(inputControlId)?.addEventListener("hover", () => {
            // window.parent.document.querySelector(inputControlId)?.setAttribute("placeholder","Look for Entity")
            console.log('hover')
        }, false);

    }, selector)
}

EngagementSDK.initControl = () => {
    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()

    if (stage === EAAShareSdk.Stages.Initiator) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.completed).setValue()
    }
}

EngagementSDK.AddDefaultFilter = (executionContext) => {
    var formContext = executionContext.getFormContext();
    [EAAShareSdk.FormFields.local_approver]
        .forEach(c => {

            formContext.getControl(c).addPreSearch(function (executionContext) {

                var filterxml = `<filter type='and'>
				<condition attribute="internalemailaddress" operator="not-like" value="%microsoft.com" />
				</filter>`;
                executionContext.getFormContext().getControl(c).addCustomFilter(filterxml, "systemuser");

            });
        })

    let country = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.countries).getValue()
    if (country) {
        EngagementSDK.countryid = country[0].id.replace(/[{}]*/g, "").toLowerCase()
    }
}

/**
 * Triggered when "State which category of service" changed
 * @param {*} executionContext 
 */
EngagementSDK.onCategoryServiceChange = (executionContext) => {
    EngagementSDK.categoryValue = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.category).getValue()

    //clear sub category value
    if (EngagementSDK.categoryValue) {
        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sub_cate_tax).getValue()) {
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sub_cate_tax).setValue()
        }
        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sub_cate_nontax).getValue()) {
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sub_cate_nontax).setValue()
        }
    }

    EngagementSDK.showRetionalofengaginePwC()
    EngagementSDK.ShowIscontengentfeebasis()
}

EngagementSDK.onCountryChange = (executionContext) => {
    let country = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.countries).getValue()

    if (country) {
        EngagementSDK.countryid = country[0].id.replace(/[{}]*/g, "").toLowerCase()

    } else {
        EngagementSDK.countryid = EAAShareSdk.EmptyLookupId
    }

    if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.entity_sign).getValue()) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.entity_sign).setValue()
    }
    if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.entity_fee).getValue()) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.entity_fee).setValue()
    }
}

EngagementSDK.subcategoryTaxFilter = function (executionContext) {
    EngagementSDK.categoryValue = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.category).getValue()
    var filterxml = `<filter type='and'>
				<condition attribute="aia_eaa_category" operator="eq" value="${EngagementSDK.categoryValue}" />
				</filter>`;
    executionContext.getFormContext().getControl(EAAShareSdk.FormFields.sub_cate_tax).addCustomFilter(filterxml, "aia_eaa_subcategory");
}
EngagementSDK.subcategoryNonTaxFilter = function (executionContext) {
    EngagementSDK.categoryValue = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.category).getValue()
    var filterxml = `<filter type='and'>
				<condition attribute="aia_eaa_category" operator="eq" value="${EngagementSDK.categoryValue}" />
				</filter>`;
    executionContext.getFormContext().getControl(EAAShareSdk.FormFields.sub_cate_nontax).addCustomFilter(filterxml, "aia_eaa_subcategory");
}

EngagementSDK.entitySignFilter = function (executionContext) {
    var filterxml = `<filter type='and'>
				<condition attribute="aia_eaa_country" operator="eq" value="${EngagementSDK.countryid}" />
				</filter>`;
    executionContext.getFormContext().getControl(EAAShareSdk.FormFields.entity_sign).addCustomFilter(filterxml, "aia_eaa_entity");
}

EngagementSDK.entityFeeFilter = function (executionContext) {
    var filterxml = `<filter type='and'>
				<condition attribute="aia_eaa_country" operator="eq" value="${EngagementSDK.countryid}" />
				</filter>`;
    executionContext.getFormContext().getControl(EAAShareSdk.FormFields.entity_fee).addCustomFilter(filterxml, "aia_eaa_entity");
}

EngagementSDK.filterCategoryService = () => {
    var formType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()
    let dropdown = Xrm.Page.ui.controls.get(EAAShareSdk.FormFields.category)

    // dropdown.getAttribute().getOptions();
    dropdown.clearOptions();
    if (formType === EAAShareSdk.RequestType.PwcNon_Tax) {
        dropdown.addOption({ text: 'PwC Audit Services', value: EAAShareSdk.Category.PwCAuditServices });
        dropdown.addOption({ text: 'PwC Audit-related Services', value: EAAShareSdk.Category.PwCAudit_relatedServices });
        dropdown.addOption({ text: 'PwC All Other Services', value: EAAShareSdk.Category.PwCAllOtherServices });
    }
    else if (formType === EAAShareSdk.RequestType.PwCTax) {
        dropdown.addOption({ text: 'PwC Tax Services', value: EAAShareSdk.Category.PwCTaxServices });

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.category).setValue(EAAShareSdk.Category.PwCTaxServices)
    }
}

EngagementSDK.setControlVisiable = (field, visibile) => {
    // Xrm.Page.data.entity.attributes.getByName(field).controls.getAll()[0].setVisible(visibile)
    Xrm.Page.getControl(field).setVisible(visibile)

    if (visibile) {
        Xrm.Page.data.entity.attributes.getByName(field).setRequiredLevel("required")
    }
    else {
        Xrm.Page.data.entity.attributes.getByName(field).setRequiredLevel("none")
    }
}

EngagementSDK.showRetionalofengaginePwC = () => {
    let formType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()
    let category = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.category).getValue()
    if (formType === EAAShareSdk.RequestType.PwCTax) {
        if (category === EAAShareSdk.Category.PwCTaxServices) {
            EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.rationale_of_engaging_pwc, true)
        } else {
            EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.rationale_of_engaging_pwc, false)
        }
    }
    else if (formType === EAAShareSdk.RequestType.PwcNon_Tax) {
        if (category === EAAShareSdk.Category.PwCAllOtherServices) {
            EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.rationale_of_engaging_pwc, true)
        } else {
            EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.rationale_of_engaging_pwc, false)
        }
    } else {
        EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.rationale_of_engaging_pwc, false)
    }
}

EngagementSDK.ShowIscontengentfeebasis = () => {

    let formType = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.sys_form_type).getValue()
    let category = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.category).getValue()
    if (formType === EAAShareSdk.RequestType.PwCTax && category === EAAShareSdk.Category.PwCTaxServices) {
        EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.fee_basis, true)
    } else {
        EngagementSDK.setControlVisiable(EAAShareSdk.FormFields.fee_basis, false)
    }
}

/**
 * hide the tabs
 * @param {*} formContext 
 */
EngagementSDK.hideTab = (formContext) => {

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    if (!!!status || status === EAAShareSdk.FormStatus.Initiator_Pending) {

        [
            EngagementSDK.Tabs.Status_Summary,
            EngagementSDK.Tabs.Comment].forEach(c => {

                var tabObj = formContext.ui.tabs.get(c);
                if (tabObj) {
                    tabObj.setVisible(false)
                }
            })
    }

}

EngagementSDK.switchTab = (formContext) => {

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
    if (status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending &&
        EAAShareSdk.IsCurrentUserCanUpdateActualFee() &&
        stage === EAAShareSdk.Stages.ActualFeeUpdate) {

        var tabObj = formContext.ui.tabs.get(EngagementSDK.Tabs.Actual_Fee_Update);
        tabObj.setFocus()
    }
}

EngagementSDK.unlockAcutalFeeFields = () => {

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()

    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
    if (status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending &&
        EAAShareSdk.IsCurrentUserCanUpdateActualFee() &&
        stage === EAAShareSdk.Stages.ActualFeeUpdate) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.completed).controls.getAll()[0].setDisabled(false);
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_fee).controls.getAll()[0].setDisabled(false);
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_taxes).controls.getAll()[0].setDisabled(false);
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_pocket).controls.getAll()[0].setDisabled(false);

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.revised_date).controls.getAll()[0].setDisabled(false);
    }
}

EngagementSDK.statusCheck = (status) => {
    return [
        EAAShareSdk.FormStatus.LocalApprover_Rejected,
        EAAShareSdk.FormStatus.GroupFinanceCoordinator_Rejected,
        EAAShareSdk.FormStatus.HeadOfGroupTax_Rejected,
        EAAShareSdk.FormStatus.RegionalCFO_Rejected,
        EAAShareSdk.FormStatus.ActualFeeUpdate_Completed].includes(status)
}

EngagementSDK.unlockAFXNumber = () => {
    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    if (stage > EAAShareSdk.Stages.Initiator && !EngagementSDK.statusCheck(status) && EAAShareSdk.IsCurrentUserCanUpdateActualFee()) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.service_number).controls.getAll()[0].setDisabled(false);
    }
}

/**
 *  got final approval , editable to Group Finance Coordinator, EAA Admin and Country Admin.
 */
EngagementSDK.unlockQuarter = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    if (status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending || status === EAAShareSdk.FormStatus.ActualFeeUpdate_Completed) {
        if (EAAShareSdk.IsAdmin() || EAAShareSdk.IsCountryAdmin() || EAAShareSdk.isUserInTeamByStage(EAAShareSdk.Stages.GroupFinanceCoordinator, EAAShareSdk.getCurrentUserId())) {
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.report_year).controls.getAll()[0].setDisabled(false);
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.quarter).controls.getAll()[0].setDisabled(false);
        }
    }
}

/// Unlock all fields if security role is 'EAA Business Admin'
EngagementSDK.unlockFieldsBusinessAdmin = () => {

    const unlockfileds = () => {
        for (let attr in Xrm.Page.data.entity.attributes._collection) {
            if (!EngagementSDK.calculatedFields.includes(attr)) {

                if (attr === EAAShareSdk.FormFields.est_end_date) {

                    let stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
                    if (stage != EAAShareSdk.Stages.ActualFeeUpdate) {
                        Xrm.Page.data.entity.attributes.getByName(attr).controls.getAll()[0].setDisabled(false);
                    }
                } else {
                    Xrm.Page.data.entity.attributes.getByName(attr).controls.getAll()[0].setDisabled(false);
                }
            }
        }
    }

    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    var requester = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.requester).getValue()
    var requesterid = requester[0].id.replace(/[{}]*/g, "").toLowerCase()
    if (EAAShareSdk.IsAdmin()) {
        unlockfileds()
    }
    else if (status === EAAShareSdk.FormStatus.Initiator_ResubmissionPending && requesterid == EAAShareSdk.getCurrentUserId()) {
        unlockfileds()
    }
}

EngagementSDK.calculatedFields = [
    'aia_eaa_define_pwc',
    'aia_eaa_pre_approval_svc_by_ext_auditor',
    'aia_eaa_sub_cate_pre_approved_svc',
    'aia_eaa_est_duration',
    'aia_eaa_est_usd_rate',
    'aia_eaa_usd_est_base_fee',
    'aia_eaa_usd_est_indrect_taxes',
    'aia_eaa_usd_est_pocket',
    'aia_eaa_usd_est_cost',
    'aia_eaa_est_cost',
    'aia_eaa_usd_rate',
    'aia_eaa_usd_act_fee',
    'aia_eaa_usd_act_taxes',
    'aia_eaa_usd_act_pocket',
    'aia_eaa_usd_act_cost',
    'aia_eaa_act_currency',
    'aia_eaa_act_cost',
    'aia_eaa_completed',
    'aia_eaa_approval_status',
    'aia_stage'
]

/**
 * Display:
By X years X months X days
 
if it is 0-31 days = show X days only;
If it is more than 32 days = show as X months X days;
If its >365 days = show X years X months X days.

 */
EngagementSDK.calculateEstimatedDuration = () => {
    var est_end_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_end_date).getValue()
    var est_start_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_start_date).getValue()
    if (est_start_date && est_end_date) {

        var difference_In_Time = est_end_date.getTime() - est_start_date.getTime()

        // To calculate the no. of days between two dates
        var difference_In_Days = difference_In_Time / (1000 * 3600 * 24)
        if (difference_In_Days >= 0 && difference_In_Days < 32) {
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_duration).setValue(`${difference_In_Days} days`)
        }
        else if (difference_In_Days < 366) {
            var months = parseInt(difference_In_Days / 31)
            var days = difference_In_Days % 31

            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_duration).setValue(`${months} months ${days} days`)

        }
        else {
            var years = parseInt(difference_In_Days / 365)
            var leftdays = difference_In_Days % 365

            const getYearToken = (years) => {
                return years > 1 ? 'years' : 'year'
            }

            if (leftdays > 31) {

                var months = parseInt(leftdays / 31)
                var days = leftdays % 31
                Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_duration).setValue(`${years} ${getYearToken(years)} ${months} months ${days} days`)
            }
            else {
                Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_duration).setValue(`${years} ${getYearToken(years)} 0 months ${leftdays} days`)
            }

        }
    }
    else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_duration).setValue()
    }
}


/**
 * EAA Admin / Country Admin / Group Finance Coordinator & request has been final approved
 */
EngagementSDK.showReportingPeriod = () => {
    var status = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.form_status).getValue()
    if (EAAShareSdk.IsInRoles([EAAShareSdk.SecurityRoles.EAA_Business_Admin,
    EAAShareSdk.SecurityRoles.EAA_Country_Admin,
    EAAShareSdk.SecurityRoles.EAA_Group_Finance_Coordinator
    ]) &&
        (status === EAAShareSdk.FormStatus.ActualFeeUpdate_Pending || status === EAAShareSdk.FormStatus.ActualFeeUpdate_Completed)) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.report_year).controls.getAll()[0].setVisible(true);
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.quarter).controls.getAll()[0].setVisible(true);

        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.report_year).controls.getAll()[0].setDisabled(false);
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.quarter).controls.getAll()[0].setDisabled(false);
    }
}


EngagementSDK.actualFeeUpdateTipOnLoad = (info = {}) => {

    // console.log('notificationTipOnLoad: ', context, info)

    var stage = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.stage).getValue()
    if (stage != EAAShareSdk.Stages.ActualFeeUpdate) {
        const selector = () => window.parent.document.querySelector(`div[data-id*="tabpanel-tab_Actual_Fee_Update"]>section`)
        const notificationId = "questionNotification"

        EAAShareSdk._doUntil(
            () => {
                if (window.parent.document.querySelector(`#${notificationId}`)) {
                    return
                }
                const parentEle = selector()
                const ele = document.createElement('div')
                ele.setAttribute("id", "questionNotification")
                ele.style.padding = '5px 20px'
                ele.style.margin = "10px 0"
                ele.style.border = 'solid 1px #5C646C'
                ele.innerHTML = `Please update actual fee information upon <span style="color: #1F78AD;">estimated end date</span> after final approval`
                ele.innerHTML = `Please update actual fee information upon <span style="color: #1F78AD;">estimated end date</span> after final approval`
                parentEle.insertBefore(ele, parentEle.childNodes[0])
            },
            selector,
        )
    }

}

EngagementSDK.onEstimatedStartDateChanged = () => {

    var est_start_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_start_date).getValue()
    var est_end_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_end_date).getValue()
    formContext = Xrm.Page.ui.formContext
    formContext.getControl(EAAShareSdk.FormFields.est_end_date).clearNotification(EAAShareSdk.MessageUniqueIdMap.EstimatedDateComparison);
    EngagementSDK.onEstimatedDateValidation(est_start_date, est_end_date)

}

EngagementSDK.onEstimatedEndDateChanged = () => {
    var est_end_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_end_date).getValue()
    var est_start_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_start_date).getValue()
    formContext = Xrm.Page.ui.formContext
    if (est_end_date) {

        formContext.getControl(EAAShareSdk.FormFields.est_end_date).clearNotification(EAAShareSdk.MessageUniqueIdMap.EstimatedEndDate);
        formContext.getControl(EAAShareSdk.FormFields.est_end_date).clearNotification(EAAShareSdk.MessageUniqueIdMap.EstimatedDateComparison);

        var today = new Date()
        today.setHours(0, 0, 0, 0);
        est_end_date.setHours(0, 0, 0, 0);
        if (est_end_date < today) {
            formContext.getControl(EAAShareSdk.FormFields.est_end_date).addNotification({
                notificationLevel: 'ERROR',
                messages: [EAAShareSdk.ErrorMessages.EstimatedEndDate],
                uniqueId: EAAShareSdk.MessageUniqueIdMap.EstimatedEndDate
            });
        }
    }

    EngagementSDK.onEstimatedDateValidation(est_start_date, est_end_date)
}

EngagementSDK.onRevisedDateChanged = () => {

    var revised_date = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.revised_date).getValue()
    if (revised_date) {
        formContext = Xrm.Page.ui.formContext
        formContext.getControl(EAAShareSdk.FormFields.revised_date).clearNotification(EAAShareSdk.MessageUniqueIdMap.RevisedEstimatedEndDate);
        var today = new Date()
        today.setHours(0, 0, 0, 0);
        revised_date.setHours(0, 0, 0, 0);
        if (revised_date < today) {
            formContext.getControl(EAAShareSdk.FormFields.revised_date).addNotification({
                notificationLevel: 'ERROR',
                messages: [EAAShareSdk.ErrorMessages.RevisedEstimatedEndDate],
                uniqueId: EAAShareSdk.MessageUniqueIdMap.RevisedEstimatedEndDate
            });
        }
    }
}

EngagementSDK.onEstimatedDateValidation = (startdate, endate) => {
    if (startdate && endate && startdate > endate) {
        formContext = Xrm.Page.ui.formContext
        formContext.getControl(EAAShareSdk.FormFields.est_end_date).addNotification({
            notificationLevel: 'ERROR',
            messages: [EAAShareSdk.ErrorMessages.EstimatedDateComparison],
            uniqueId: EAAShareSdk.MessageUniqueIdMap.EstimatedDateComparison
        });
    } else {

        EngagementSDK.calculateEstimatedDuration()
    }
}

EngagementSDK.onActualBaseFeeChanged = () => {
    var base_fee = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_fee).getValue()
    formContext = Xrm.Page.ui.formContext
    formContext.getControl(EAAShareSdk.FormFields.act_fee).clearNotification(EAAShareSdk.MessageUniqueIdMap.BaseFeeLimitation);
    if (base_fee == 0) {
        formContext.getControl(EAAShareSdk.FormFields.act_fee).addNotification({
            notificationLevel: 'ERROR',
            messages: [EAAShareSdk.ErrorMessages.BaseFeeLimitation],
            uniqueId: EAAShareSdk.MessageUniqueIdMap.BaseFeeLimitation
        });
    } else {
        EngagementSDK.onActualFeeChanged()
    }
}

/**
 * triggered when actual base-fee/taxes/expenses changed
 */
EngagementSDK.onActualFeeChanged = () => {

    var currency = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.currency).getValue()
    if (currency) {
        let currencyName = currency[0].name;
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_currency).setValue(currencyName)

        const currencyId = currency[0].id.replace(/[{}]*/g, "").toLowerCase()
        EngagementSDK.changeAcutalExchangeRate(currencyId)

    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_currency).setValue('')
    }
}

EngagementSDK.onEstimatedBaseFeeChanged = () => {
    var base_fee = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.base_fee).getValue()
    formContext = Xrm.Page.ui.formContext
    formContext.getControl(EAAShareSdk.FormFields.base_fee).clearNotification(EAAShareSdk.MessageUniqueIdMap.BaseFeeLimitation);
    if (base_fee == 0) {
        formContext.getControl(EAAShareSdk.FormFields.base_fee).addNotification({
            notificationLevel: 'ERROR',
            messages: [EAAShareSdk.ErrorMessages.BaseFeeLimitation],
            uniqueId: EAAShareSdk.MessageUniqueIdMap.BaseFeeLimitation
        });
    } else {
        EngagementSDK.onCurrencyChanged()
    }
}


/**
 * triggered when currency/base-fee/taxes/expenses changed
 */
EngagementSDK.onCurrencyChanged = () => {

    var currency = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.currency).getValue()
    if (currency) {
        let currencyName = currency[0].name;
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_currency).setValue(currencyName)

        const currencyId = currency[0].id.replace(/[{}]*/g, "").toLowerCase()
        EngagementSDK.changeExchangeRate(currencyId)



    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_currency).setValue('')
    }

}

EngagementSDK.changeExchangeRate = async (currencyId) => {
    //Xrm.Utility.showProgressIndicator("Loading")
    try {
        const currency = await EngagementSDK.getCurrency(currencyId);
        // const rate = await EngagementSDK.getAFXExchangeRate(currency.isocurrencycode);
        const rate = await EngagementSDK.getAFXExchangeRateByApi(currency.isocurrencycode);
        const exchangeRate = parseFloat(rate)
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_usd_rate).setValue(exchangeRate)

        EngagementSDK.calculateEstimatedCost(exchangeRate);

        //if admin change the currenty,the autual fee should be synchronously updated  
        if (Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_cost).getValue()) {
            Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_rate).setValue(exchangeRate)
            EngagementSDK.calculateActualCost(exchangeRate);
        }
    }
    catch (err) {
        window.parent.EAAShareSdk.setFormError(`${err}`)
    }
    finally {
        //Xrm.Utility.closeProgressIndicator()
    }
}

EngagementSDK.changeAcutalExchangeRate = async (currencyId) => {
    //Xrm.Utility.showProgressIndicator("Loading")
    try {
        const currency = await EngagementSDK.getCurrency(currencyId);
        // const rate = await EngagementSDK.getAFXExchangeRate(currency.isocurrencycode);
        const rate = await EngagementSDK.getAFXExchangeRateByApi(currency.isocurrencycode);
        const exchangeRate = parseFloat(rate)
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_rate).setValue(exchangeRate)
        EngagementSDK.calculateActualCost(exchangeRate);
    }
    catch (err) {
        window.parent.EAAShareSdk.setFormError(`${err}`)
    }
    finally {
        //Xrm.Utility.closeProgressIndicator()
    }
}

EngagementSDK.getCurrency = (currencyId) => {
    return fetch(
        `/api/data/v9.0/transactioncurrencies(${currencyId})`,
        {
            method: "GET",
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

EngagementSDK.getAFXExchangeRateByApi = async (currencyCode) => {

    let fxurl = await EAAShareSdk.getConfiguration(EAAShareSdk.ConfigurationKeys.FXURL)

    fxurl = fxurl.replace('{FromCurrency}', currencyCode).replace('{ToCurrency}', 'USD').replace('{Amount}', 1)

    return fetch(
        fxurl,
        {
            method: "GET",
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }
        return res
    })
}

EngagementSDK.getAFXExchangeRate = (currencyCode) => {

    const fetXml = `
    <fetch mapping='logical'>
        <entity name='aia_rate'>
            <attribute name='aia_directrate'/>
            <attribute name='aia_indirectrate'/>
            <link-entity name='aia_currency' to='aia_currency' from='aia_currencyid'>
            <filter type='and'>   
                    <condition attribute='aia_isocode' operator='eq' value='${currencyCode}' />   
            </filter> 
            </link-entity>
        </entity>
    </fetch>
    `
    return fetch(
        `/api/data/v9.0/aia_rates?fetchXml=${encodeURI(fetXml)}`,
        {
            method: "GET",
        }
    ).then(res => res.json()).then(res => {
        if (res.error) {
            throw new Error(res.error.message)
        }

        if (res.value.length < 1) {
            throw new Error('can not find specific Currency.')
        }
        return res.value[0].aia_directrate
    })
}

EngagementSDK.onAFXNumberChanged = () => {

    formContext = Xrm.Page.ui.formContext
    formContext.getControl(EAAShareSdk.FormFields.service_number).clearNotification(EAAShareSdk.MessageUniqueIdMap.AFXNumberDigitalFormat);
    formContext.getControl(EAAShareSdk.FormFields.service_number).clearNotification(EAAShareSdk.MessageUniqueIdMap.AFXNumberLengthCheck);
    const service_number = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.service_number).getValue()
    if (service_number) {
        const isnum = /^\d{6}$/.test(service_number);
        if (!isnum) {
            formContext.getControl(EAAShareSdk.FormFields.service_number).addNotification({
                notificationLevel: 'ERROR',
                messages: [EAAShareSdk.ErrorMessages.AFXNumberDigitalFormat],
                uniqueId: EAAShareSdk.MessageUniqueIdMap.AFXNumberDigitalFormat
            });
        }

    }
}


EngagementSDK.calculateEstimatedCost = (exchangeRate) => {
    formContext = Xrm.Page.ui.formContext
    var fee = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.base_fee).getValue()
    var taxes = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.indrect_taxes).getValue()
    var outofpockets = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.pocket).getValue()

    const totalCost = (fee ?? 0) + (taxes ?? 0) + (outofpockets ?? 0)
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.est_cost).setValue(totalCost)

    if (fee) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_base_fee).setValue(exchangeRate * fee)
    } else if (fee == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_base_fee).setValue(0)
    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_base_fee).setValue()
    }


    if (taxes) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_indrect_taxes).setValue(exchangeRate * taxes)
    } else if (taxes == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_indrect_taxes).setValue(0)
    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_indrect_taxes).setValue()
    }

    if (outofpockets) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_pocket).setValue(exchangeRate * outofpockets)
    } else if (outofpockets == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_pocket).setValue(0)
    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_pocket).setValue()
    }

    if (totalCost) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_cost).setValue(totalCost * exchangeRate)
    } else if (totalCost == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_est_cost).setValue(0)
    }
}

EngagementSDK.calculateActualCost = (exchangeRate) => {
    formContext = Xrm.Page.ui.formContext
    var fee = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_fee).getValue()
    var taxes = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_taxes).getValue()
    var outofpockets = Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_pocket).getValue()

    const totalCost = (fee ?? 0) + (taxes ?? 0) + (outofpockets ?? 0)
    Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.act_cost).setValue(totalCost)

    if (fee) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_fee).setValue(exchangeRate * fee)
    } else if (fee == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_fee).setValue(0)
    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_fee).setValue()
    }

    if (taxes) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_taxes).setValue(exchangeRate * taxes)
    } else if (taxes == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_taxes).setValue(0)
    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_taxes).setValue()
    }

    if (outofpockets) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_pocket).setValue(exchangeRate * outofpockets)
    } else if (outofpockets == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_pocket).setValue(0)
    } else {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_pocket).setValue()
    }

    if (totalCost) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_cost).setValue(totalCost * exchangeRate)
    } else if (totalCost == 0) {
        Xrm.Page.data.entity.attributes.getByName(EAAShareSdk.FormFields.usd_act_cost).setValue(0)
    }
}



EngagementSDK.onGuide = () => {

}