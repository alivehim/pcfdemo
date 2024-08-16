var AppSdk = window.AppSdk || {};

AppSdk.notificationId = null

AppSdk.loadGlobalCss = () => {
	console.log('loadGlobalCss')

	// add global css
	const linkLabel = document.createElement("link")
	linkLabel.setAttribute("id", "globalHideCss")
	linkLabel.setAttribute("rel", "stylesheet")
	let cssText = ``

	// hide default buttons in the command bar
	cssText += `
	li[id="ShowChartPane"],
	li[id*="OverflowButton_button"],
	div[title="Open in new window"],
	div[title="Open Record Set"],
	ul[id*="MenuSectionItemsOverflowButton"] > li:not(li[id*="systemuser|NoRelationship|SubGridStandard|Mscrm.SubGrid.systemuser.ExportToExcel"]),
	button[id*="ViewSelector_"] i,
	ul[data-id="CommandBar"]:not(ul[data-lp-id*="commandbar-Global"]) > li:not(li[id*="OverflowButton"]) {
		background: rgba(0,0,0,0.3);
		display: none;
	}
	button[id*="ViewSelector_"]{
		pointer-events: none
	}
	`

	// make textarea higher
	cssText += `
	textarea[id*='fieldControl-text-box-text'],
	textarea[id*='fieldControl-text-box-text']:hover{
		height: 10em	
	}
	`

	// hide "saved", "unsave" status text
	cssText += `
	h1[id*="formHeaderTitle"] span[data-id*="header_saveStatus"] {
		display: none;
	}`

	// hide form select control
	cssText += hidetab()

	cssText += hideFormHeader()

	cssText += showButtons()

	cssText += changeColor()

	cssText += hideUnpublishedDialog()

	if (!AppSdk.IsAdmin()) {
		cssText += hideAdminArea()
	}

	linkLabel.href = URL.createObjectURL(new Blob([cssText], { type: "text/css" }))
	window.parent.document.body.appendChild(linkLabel)
	AppSdk._globalHideStyle = linkLabel
}

const hidetab = () => {
	return `
	li[data-lp-id*="form-tab-related_tab"],
	span[data-id="entity_name_span"] {
		display: none;
	}`
}

const hideUnpublishedDialog = () => {
	return `
	div[id*="dialogPageContainer"] li[id*="HomePageGrid|aia.aia_eaa_form.UnpublishedRequests.Command"],
	div[id*="dialogPageContainer"] li[id*="HomePageGrid|aia.aia_eaa_form.Unpublish.Command"],
	div[id*="dialogPageContainer"] li[id*="HomePageGrid|aia.aia_eaa_form.AdminSendBack.Command"] {
		display: none;
	}`
}

const hideAdminArea = () => {
	return `
	li[aria-label*="AdminConfigurations"] {
		display: none;
	}`
}

const hideFormHeader = () => {
	return `
	h2[id*="formHeaderTitle"],
	div[data-lp-id="form-header-title"] > div {
		display: none;
	}`
}

/**
 * 
 show require buttons in the command bar(custom action buttons total 10, faq button total 2, export to excel, refresh button)
 */
const showButtons = () => {
	return `
	li[id*="refreshCommand"],
	li[id*="OverflowButton_button"],
	li[id*="HomePageGrid|aia.aia_eaa_form.AddNewEAA.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.RawReport.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.ChangeLogRpt.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.TaxReport.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.Export.Command"],
	li[id*="HomePageGrid|Mscrm.HomepageGrid.aia_eaa_form.ExportToExcel"],
	li[id*="HomePageGrid|aia.aia_eaa_form.Guide.Command"],
	div[id*="mainContent"] li[id*="HomePageGrid|aia.aia_eaa_form.Unpublish.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.ChangeInitator.Command"],
	div[id*="mainContent"] li[id*="HomePageGrid|aia.aia_eaa_form.UnpublishedRequests.Command"],
	li[id*="Form|aia.aia_eaa_form.Cancel.Command"],
	li[id*="Form|aia.aia_eaa_form.Save.Command"],
	li[id*="Form|aia.aia_eaa_form.Delete.Command"],
	li[id*="Form|aia.aia_eaa_form.Submit.Command"],
	li[id*="Form|aia.aia_eaa_form.Approve.Command"],
	li[id*="Form|aia.aia_eaa_form.Reopen.Command"],
	li[id*="Form|aia.aia_eaa_form.Reject.Command"],
	li[id*="Form|aia.aia_eaa_form.LeaveComment.Command"],
	li[id*="Form|aia.aia_eaa_form.ChangeLog.Command"],
	li[id*="Form|aia.aia_eaa_form.UpdateActualFee.Command"],
	li[id*="Form|aia.aia_eaa_form.FormGuid.Command"],
	li[id*="Form|aia.aia_eaa_form.ChangeInitatorForm.Command"],
	li[id*="Form|aia.aia_eaa_form.UnpublishForm.Command"],
	li[id*="Form|aia.aia_eaa_comment.SubmitPost.Command"],
	li[id*="Form|aia.aia_eaa_approval.Submit.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_local_viewer.New.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_local_viewer.Edit.Command"],
	li[id*="aia_eaa_local_viewer|NoRelationship|SubGridStandard|Mscrm.DeleteSelectedRecord"],
	li[id*="aia_eaa_local_viewer|NoRelationship|SubGridStandard|aia.aia_eaa_local_viewer.DeleteSelectedRecord.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_contact.New.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_contact.Edit.Command"],
	li[id*="aia_eaa_contact|NoRelationship|SubGridStandard|Mscrm.DeleteSelectedRecord"],
	li[id*="aia_eaa_contact|NoRelationship|SubGridStandard|aia.aia_eaa_contact.DeleteSelectedRecord.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_comment.New.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_comment.Reply.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_comment.Delete.Command"],
	li[id*="aia_eaa_local_viewer|NoRelationship|Form|aia.aia_eaa_local_viewer.Submit.Command"],
	li[id*="aia_eaa_contact|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="aia_eaa_country|NoRelationship|HomePageGrid|Mscrm.NewRecordFromGrid"],
	li[id*="aia_eaa_country|NoRelationship|HomePageGrid|Mscrm.HomepageGrid.aia_eaa_country.DeleteMenu"],
	li[id*="aia_eaa_country|NoRelationship|Form|Mscrm.SavePrimary"],
	li[id*="aia_eaa_country|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="aia_eaa_entity|NoRelationship|HomePageGrid|Mscrm.NewRecordFromGrid"],
	li[id*="aia_eaa_entity|NoRelationship|HomePageGrid|Mscrm.HomepageGrid.aia_eaa_entity.DeleteMenu"],
	li[id*="aia_eaa_entity|NoRelationship|Form|Mscrm.SavePrimary"],
	li[id*="aia_eaa_entity|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="aia_eaa_subcategory|NoRelationship|HomePageGrid|Mscrm.NewRecordFromGrid"],
	li[id*="aia_eaa_subcategory|NoRelationship|HomePageGrid|Mscrm.HomepageGrid.aia_eaa_subcategory.DeleteMenu"],
	li[id*="aia_eaa_subcategory|NoRelationship|Form|Mscrm.SavePrimary"],
	li[id*="aia_eaa_subcategory|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="aia_eaa_report_quarter|NoRelationship|Form|Mscrm.SavePrimary"],
	li[id*="aia_eaa_report_quarter|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="transactioncurrency|NoRelationship|HomePageGrid|Mscrm.NewRecordFromGrid"],
	li[id*="transactioncurrency|NoRelationship|HomePageGrid|Mscrm.HomepageGrid.transactioncurrency.DeleteMenu"],
	li[id*="transactioncurrency|NoRelationship|Form|Mscrm.SavePrimary"],
	li[id*="transactioncurrency|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="team|NoRelationship|HomePageGrid|Mscrm.NewRecordFromGrid"],
	li[id*="team|NoRelationship|HomePageGrid|Mscrm.HomepageGrid.team.DeleteMenu"],
	li[id*="team|NoRelationship|Form|Mscrm.SavePrimary"],
	li[id*="team|NoRelationship|Form|Mscrm.SaveAndClosePrimary"],
	li[id*="systemuser|NoRelationship|SubGridStandard|Mscrm.AddExistingRecordFromSubGridAssociated"],
	li[id*="systemuser|NoRelationship|SubGridStandard|Mscrm.RemoveSelectedRecord"]
	{
		display: list-item !important;
		background: white !important;
	}
	`
}

const changeColor = () => {
	// Change the color in list of request.
	return `
    div[col-id="aia_eaa_approval_status"] label[aria-label="Pending"]{
        color: #1F78AD;
        font-weight: 700;
    }
	div[col-id="aia_eaa_approval_status"] label[aria-label="Resubmission Pending"]{
        color: #1F78AD;
        font-weight: 700;
    }
    div[col-id="aia_eaa_approval_status"] label[aria-label="Rejected"] {
        color: #D31145;
        font-weight: 700;
    }
    div[col-id="aia_eaa_approval_status"] label[aria-label="Completed"] {
        color: #3DA758;
        font-weight: 700;
    }
    div[col-id="aia_eaa_approval_status"] label[aria-label="Approved"] {
        color: #3DA758;
        font-weight: 700;
    }
    `
}


AppSdk.Views = {
	eaa_all_request: "7e7ac2ed-c1d1-ec11-a7b5-000d3a80b291",
	eaa_my_request: "973ca551-84d5-ec11-a7b5-000d3a806eb6",
	eaa_awaiting_my_response: "a0bb4621-8fd5-ec11-a7b5-000d3a806eb6",
	eaa_change_initator: "b7cedccc-f1d7-ec11-a7b5-002248177dde",
	eaa_unpublished_content: "2e222fae-f1d7-ec11-a7b5-002248177dde",
	eaa_countries: "9450d053-4df4-4a50-a3ad-f2d9dc7feedf",
	eaa_role_manage: "468e4d68-eed7-ec11-a7b5-002248177dde"
}

/// addBeforeOnLoad: set specific view before loading
AppSdk.addBeforeOnLoad = () => {
	Xrm.Navigation.addBeforeOnLoad(function (e, a) {
		var areaId = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId

		if (AppSdk.Views[areaId] && e.getEventSource().getViewSelector) {
			viewSelector = e.getEventSource().getViewSelector()
			const view = AppSdk.Views[areaId].toUpperCase()
			if (viewSelector.getCurrentView().id != `{${view}}`) {

				if (viewSelector.getCurrentView().id != `{70bd4d76-afeb-ec11-bb3d-002248177d91}`.toUpperCase()) {

					e.getEventSource().getViewSelector().setCurrentView({ id: view })
				}
			}
			else {
				if (areaId === "eaa_my_request") {
					AppSdk.showAFUPendingBanner();
				}
			}
		}
	})
}

/**
 * On "My Request" page top should show banner "Please update actual fee information when the stage is 'Actual Fee Update - Pending'."
 */
AppSdk.showAFUPendingBanner = async () => {

	const pendingUpdateActualFeeFetxml = `
        <fetch version="1.0" output-format="xml-platform" mapping="logical" distinct="true" returntotalrecordcount="true" no-lock="false">
            <entity name="aia_eaa_form">
                <attribute name="aia_name"/>
				<attribute name="aia_eaa_form_status"/>
                <filter type="and">
                    <condition attribute="aia_eaa_requester" operator="eq-userid"/>
                    <condition attribute="statecode" operator="eq" value="0"/>
                    <condition attribute="aia_eaa_approval_status" operator="eq" value="589450003"/>
                    <condition attribute="aia_sys_processlock" operator="eq" value="0"/>
                    <condition attribute='aia_stage' operator="eq" value="589450007"/>
					<condition attribute='aia_eaa_form_status' operator="eq" value="589450013"/>
                </filter>
            </entity>
        </fetch>
        `

	const requests = await fetch(
		`/api/data/v9.0/aia_eaa_forms?fetchXml=${encodeURI(pendingUpdateActualFeeFetxml)}`,
		{
			headers: {
				"Content-Type": "application/json",
				'Prefer': 'odata.include-annotations="*"',
			}
		}
	).then(res => res.json())

	if (!requests.error && requests.value.length > 0) {

		if (AppSdk.notificationId) {
			Xrm.App.clearGlobalNotification(AppSdk.notificationId);
		}
		//show banner
		var notification =
		{
			showCloseButton: true,
			type: 2,
			level: 4, //information
			message: `Please update actual fee information when the stage is 'Actual Fee Update - Pending'.`,
		}

		Xrm.App.addGlobalNotification(notification).then(
			function success(result) {
				AppSdk.notificationId = result
				// Wait for 10 seconds and then clear the notification
				window.setTimeout(function () {
					Xrm.App.clearGlobalNotification(AppSdk.notificationId);
				}, 10000);
			},
			function (error) {
				console.log(error.message);
				// handle error conditions
			}
		);

	}

}

AppSdk.SecurityRoles = {
	Admin: "EAA_Admin",
	BasicUser: "EAA_Basic_User",
	EAA_Business_Admin: "EAA_Business_Admin"
}

AppSdk.IsAdmin = () => {
	var securityRoles = Xrm.Page.context.userSettings.roles.getAll().map(r => r.name);
	return securityRoles.includes(AppSdk.SecurityRoles.EAA_Business_Admin)
}

AppSdk.siteMapAreaList = ['eaa_all_request', 'eaa_my_request', 'eaa_awaiting_my_response']

AppSdk.autoRefresh = () => {
	var areaId = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
	if (AppSdk.siteMapAreaList.includes(areaId)) {
		var refreshBtn = window.parent.document.querySelector("button[id*='aia_eaa_form|NoRelationship|HomePageGrid|Mscrm.Modern.refreshCommand']")
		if (refreshBtn) {
			refreshBtn.click()
		}
	}
}


AppSdk.helloEAA = function (executionContext) {
	console.log('Hello EAA!!!')

	AppSdk.loadGlobalCss()
	AppSdk.addBeforeOnLoad()
	setInterval(() => {
		AppSdk.autoRefresh()
	}, 5 * 60 * 1000);
}