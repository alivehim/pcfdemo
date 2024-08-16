var AdminAppSdk = window.AdminAppSdk || {};


function loadGlobalCss() {
	console.log('loadGlobalCss')

	// add global css
	const linkLabel = document.createElement("link")
	linkLabel.setAttribute("id", "globalHideCss")
	linkLabel.setAttribute("rel", "stylesheet")
	let cssText = ``

	cssText += hideButtons()

	cssText += changeColor()

	linkLabel.href = URL.createObjectURL(new Blob([cssText], { type: "text/css" }))
	window.parent.document.body.appendChild(linkLabel)
	AdminAppSdk._globalHideStyle = linkLabel
}


/**
 * 
 show require buttons in the command bar(custom action buttons total 10, faq button total 2, export to excel, refresh button)
 */
const hideButtons = () => {
	return `
	li[id*="HomePageGrid|aia.aia_eaa_form.AddNewEAA.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.RawReport.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.TaxReport.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.ChangeLogRpt.Command"],
	li[id*="HomePageGrid|aia.aia_eaa_form.Export.Command"],
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
	li[id*="aia_eaa_local_viewer|NoRelationship|SubGridStandard|aia.aia_eaa_local_viewer.DeleteSelectedRecord.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_contact.New.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_contact.Edit.Command"],
	li[id*="aia_eaa_contact|NoRelationship|SubGridStandard|aia.aia_eaa_contact.DeleteSelectedRecord.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_comment.New.Command"],
	li[id*="SubGridStandard|aia.aia_eaa_comment.Reply.Command"]
	{
		display: none;
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


AdminAppSdk.helloEAA = function (executionContext) {
	console.log('Hello EAA Admin!!!')

	loadGlobalCss()
}