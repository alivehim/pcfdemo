
window.EngagementEvidenceSDK = window.EngagementEvidenceSDK || {}


EngagementEvidenceSDK.onFormLoaded = () => {
    formContext = Xrm.Page.ui.formContext
    formContext.ui.headerSection.setTabNavigatorVisible(false);
}