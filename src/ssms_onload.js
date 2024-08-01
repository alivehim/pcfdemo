var SSMS = window.SSMS || {}

SSMS.loadGlobalCSS = () => {

    // add global css
    const linkLabel = document.createElement("link")
    linkLabel.setAttribute("id", "globalHideCss")
    linkLabel.setAttribute("rel", "stylesheet")
    let cssText = ``

    // hide default buttons in the command bar
    cssText += `
	ul[data-lp-id="commandbar-Form:clp_proximitycardrequest"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_proximitycardrequest"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-Form:clp_proximity_card_creation_event"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-Form:clp_proximitycardinventory"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_proximitycardinventory"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-SubGridStandard:clp_proximity_card_request_detail"] > li,
    ul[data-lp-id="commandbar-Form:clp_proximity_card_request_detail"] > li,
    ul[data-lp-id="commandbar-SubGridStandard:clp_ssms_lut_substation"] > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_proximitycardapplicant"] > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_proximity_card_audit"] > li,
    ul[data-lp-id="commandbar-Form:clp_proximitycardapplicant"] > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_proximity_card_request_detail"] > li,
    ul[data-lp-id="commandbar-Form:clp_proximity_card_audit"] > li,
    ul[data-lp-id="commandbar-SubGridStandard:clp_proximity_card_audit_detail"] > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_proximity_card_expiry_log"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-Form:clp_masterkeyinventory2"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_masterkeyinventory2"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_masterkeyaudit"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-Form:clp_masterkeyaudit"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_masterkeyexpirylog"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="commandbar-HomePageGrid:clp_masterkeymovementrecord"]:not(ul[data-lp-id*="commandbar-Global"]) > li,
    ul[data-lp-id="Form:clp_masterkeyauditdtl-clp_masterkeyauditdtl|NoRelationship|Form|Mscrm.Form.clp_masterkeyauditdtl.Flows.RefreshCommandBar"] > li,
    ul[data-lp-id="Form:clp_masterkeyauditdtl-clp_masterkeyauditdtl|NoRelationship|Form|Mscrm.Form.clp_masterkeyauditdtl.NewRecord"] > li,
    ul[data-lp-id="commandbar-Form:clp_user"] > li,
    ul[data-lp-id="commandbar-SubGridStandard:clp_masterkeyauditdtl"] > li,
    li[id*="QuickPowerBI"],
    li[id*="Send.Menu"],
    li[id*="Flows.RefreshCommandBar"],
    li[id*="RunReport"],
    li[id*="ShowChartPane"],
    li[id*="changeVisualization"],
    button[id*="addNewBtnContainer"],
    div[data-lp-id="form-header-title"] > div
    {
		background: rgba(0,0,0,0.3);
		display: none;
	}
	`
    cssText += showButtons()

    //cssText += disableNavigationLink()

    cssText += showDetailButton()

    cssText += showSubstationButton()

    cssText += showOtherAreaButton()

    cssText += disableSubgridNavigationLink()

    linkLabel.href = URL.createObjectURL(new Blob([cssText], { type: "text/css" }))
    window.parent.document.body.appendChild(linkLabel)
    SSMS._globalHideStyle = linkLabel
}

const disableNavigationLink = () => {
    return `
    div[id*="selected_tag"]
    {
        pointer-events: none !important;
    }
    `
}

const disableSubgridNavigationLink = () => {
    return `
    div[data-lp-id*="clp_masterkeyaudit"] a,div[data-lp-id*="clp_masterkeyaudit"] button[class*="personaRootLinkStyle"] {
        pointer-events: none !important;
        color: #000 !important;
        text-decoration: none !important;
    }
    `
}




const showButtons = () => {
    return `
    li[id*="refreshCommand"]:not(li[id*="clp_masterkeyauditdtl|NoRelationship|SubGridStandard|Mscrm.Modern.refreshCommand"]):not(li[id*="clp_proximitycardrequest|NoRelationship|Form|Mscrm.Modern.refreshCommand"]),
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Submit.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Approve"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.ApprovalAction.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Reassign.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.PrincipleManagerCivilApprove.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Reject.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.RejecttoAdmin.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.AllocatedCards.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.EncodeCards.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.CTAM.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Resubmit.Command"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Cancel"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Review"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Copy"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Extend"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.Export"],
    li[id*="clp_proximitycardrequest|NoRelationship|Form|clp.clp_proximitycardrequest.ReassginSubmit"],
    li[id*="clp_proximitycardrequest|NoRelationship|HomePageGrid|clp.clp_proximitycardrequest.New"],
    li[id*="clp_proximitycardrequest|NoRelationship|HomePageGrid|clp.clp_proximitycardrequest.HomeExport"],
    li[id*="clp_proximity_card_creation_event|NoRelationship|Form|clp.clp_proximity_card_creation_event.Submit"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Create"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.InventoryExport.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.ExpiryRecordExport.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Update"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Delete"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Collect"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.Verification.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Return"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.ReturnDamagedCard"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Replace"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Lost"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.EncodeReplacement"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.Decode"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.UpdateSubmit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.RepalceSubmit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.ReturnSubmit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.PrintAcknowledgement.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.UpdateCardPrefixSumbit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.EncodeReplacementSubmit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.EncodeReplacementExport.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.DecodeSubmit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.DecodeExport.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|Form|clp.clp_proximitycardinventory.CollectSubmit.Command"],
    li[id*="clp_proximitycardinventory|NoRelationship|HomePageGrid|clp.clp_proximitycardinventory.EncodeList.Command"],
    li[id*="clp_proximitycardapplicant|NoRelationship|HomePageGrid|clp.clp_proximitycardapplicant.Update.Command"],
    li[id*="clp_proximitycardapplicant|NoRelationship|HomePageGrid|clp.clp_proximitycardapplicant.Export.Command"],
    li[id*="clp_proximitycardapplicant|NoRelationship|Form|Mscrm.SaveAndClose"],
    li[id*="clp_masterkeyinventory2|NoRelationship|Form|Mscrm.SaveAndClose"],
    li[id*="clp_masterkeyinventory2|NoRelationship|Form|Mscrm.Save"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.InventoryCreation"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.KeyTypeMaintenance"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.KeyUpdate"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.KeyCollection"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.KeyLost"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.KeyReturn"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.KeyDamaged"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.Export"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|Mscrm.ExportSelectedToExcel"],
    li[id*="clp_masterkeyrequest|NoRelationship|HomePageGrid|clp.clp_masterkeyrequest.Export"],
    li[id*="clp_masterkeyrequest|NoRelationship|HomePageGrid|clp.clp_masterkeyrequest.ExportAll"],
    li[id*="clp_masterkeyaudit|NoRelationship|HomePageGrid|clp.clp_masterkeyaudit.Export"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.AuditExport"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.InventoryExport"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.IssueExport"],
    li[id*="clp_masterkeyinventory2|NoRelationship|HomePageGrid|clp.clp_masterkeyinventory2.ExpiryExport"],
    li[id*="clp_masterkeyaudit|NoRelationship|HomePageGrid|Mscrm.ExportSelectedToExcel"],
    li[id*="clp_masterkeyaudit|NoRelationship|Form|Mscrm.SaveAndClose"],
    li[id*="clp_masterkeyaudit|NoRelationship|Form|Mscrm.Save"],
    li[id*="clp_masterkeyexpirylog|NoRelationship|HomePageGrid|Mscrm.ExportSelectedToExcel"],
    li[id*="clp_masterkeyexpirylog|NoRelationship|HomePageGrid|clp.clp_masterkeyexpirylog.Export"],
    li[id*="clp_masterkeymovementrecord|NoRelationship|HomePageGrid|Mscrm.ExportSelectedToExcel"],
    li[id*="clp_masterkeymovementrecord|NoRelationship|HomePageGrid|Mscrm.NewRecordFrom"],
    li[id*="clp_masterkeymovementrecord|NoRelationship|HomePageGrid|clp.clp_masterkeymovementrecord.Export"],
    li[id*="clp_user|NoRelationship|Form|Mscrm.Save"],
    li[id*="clp_user|NoRelationship|Form|Mscrm.SaveAndClose"],
    li[id*="clp_masterkeyauditdtl|NoRelationship|SubGridStandard|clp.clp_masterkeyauditdtl.Audit.Command"],
    li[id*="clp_masterkeyauditdtl|NoRelationship|SubGridStandard|clp.clp_masterkeyauditdtl.Uncheck.Command"],
    li[id*="clp_masterkeyauditdtl|NoRelationship|SubGridStandard|clp.clp_masterkeyauditdtl.export.Command"],
    li[id*="clp_masterkeyauditdtl|NoRelationship|SubGridStandard|clp.clp_masterkeyauditdtl.Reassign.Command"]
    {
		display: list-item !important;
		background: white !important;
	}
	`
}

const showDetailButton = () => {
    return `
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.SelectApplicant.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.Remove.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.Reject.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.Edit.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.AllocateCard.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.AllocateCard.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|Form|clp.clp_proximity_card_request_detail.SelectApplicantSubmit.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_request_detail.ExportEncodeList.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|Form|clp.clp_proximity_card_request_detail.AllocateCardSubmit.Command"],
	li[id*="clp_proximity_card_request_detail|NoRelationship|HomePageGrid|clp.clp_proximity_card_request_detail.Export.Command"]
    {
		display: list-item !important;
		background: white !important;
	}
	`
}

const showSubstationButton = () => {
    return `
	li[id*="clp_ssms_lut_substation|NoRelationship|SubGridStandard|clp.clp_ssms_lut_substation.AddExistingSS.Command"],
	li[id*="clp_ssms_lut_substation|NoRelationship|SubGridStandard|clp.clp_ssms_lut_substation.Remove.Command"]
    {
		display: list-item !important;
		background: white !important;
	}
	`
}

const showOtherAreaButton = () => {
    return `
	li[id*="clp_proximity_card_audit_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_audit_detail.Audit.Command"],
	li[id*="clp_proximity_card_expiry_log|NoRelationship|HomePageGrid|clp.clp_proximity_card_expiry_log.Export.Command"],
	li[id*="clp_proximity_card_audit_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_audit_detail.export.Command"],
	li[id*="clp_proximity_card_audit_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_audit_detail.UnAudit.Command"],
	li[id*="clp_proximity_card_audit_detail|NoRelationship|SubGridStandard|clp.clp_proximity_card_audit_detail.Reassign.Command"],
	li[id*="clp_proximity_card_audit|NoRelationship|HomePageGrid|clp.clp_proximity_card_audit.Export.Command"]
    {
		display: list-item !important;
		background: white !important;
	}
	`
}


SSMS.RequestViewMapping = {
    viewMapping_pc_applicant_profile: "11d46d09-2787-ed11-81ac-000d3a07ca3c",
    viewMapping_pc_expiry_record: "eb87ff23-2c87-ed11-81ac-000d3a07ca3c",
    viewMapping_pc_application: "10605cb2-3b87-ed11-81ac-000d3a07ca3c",
    viewMapping_pc_updatecardprefix_C: "6ba888cd-098c-ed11-81ac-000d3a07ca3c",
    viewMapping_pc_updatecardprefix_I: "19d6b747-fc8b-ed11-81ac-000d3a07ca3c",
    viewMapping_pc_tm_application: "31e062b6-e292-ed11-aad0-000d3a07ca3c",
    viewMapping_pc_request_detail: "aea4a60e-36a1-ed11-aad0-000d3a07ca3c",
    viewMapping_pc_inventory: "f7c43146-8aa3-ed11-aad0-000d3a07ca3c"
}

SSMS.addBeforeOnLoad = () => {

    Xrm.Navigation.addBeforeOnLoad(function (e, a) {
        var areaId = Xrm.Navigation._context.RecentItemsSyncManager._store.getState().appShellState.sitemapState.selectedSubAreaId
        console.log(areaId)


        if (SSMS.RequestViewMapping[areaId] && e.getEventSource().getViewSelector) {
            viewSelector = e.getEventSource().getViewSelector()
            var view = SSMS.RequestViewMapping[areaId].toUpperCase(); //all request, my_action	
            var id = e.getEventSource().getViewSelector().getCurrentView().id


            if (id != `{${view}}`) {

                e.getEventSource().getViewSelector().setCurrentView({ id: view })

            }
            window.parent.ViewID = view;
        }
    })
}

SSMS.helloSSMS = function (executionContext) {
    console.log('Hello SSMS!!!')
    //for jspdf    
    let script = window.document.createElement('script')
    script.src = '/WebResources/clp_jspdf_umd_min.js'
    window.document.body.appendChild(script)
    script.onload = () => {
        console.log('import jsPDF')
        window.parent.jspdf = window.jspdf
        //jspdf - autotable
        script = window.document.createElement('script')
        script.src = '/WebResources/clp_jspdf_plugin_autotable.js'
        window.document.body.appendChild(script)
        script.onload = () => {
            console.log('import jsPDF-autotable')
        }
    }
    SSMS.addBeforeOnLoad()
    SSMS.loadGlobalCSS()
}
