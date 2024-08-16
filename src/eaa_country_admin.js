
window.EAACountryAdminSDK = window.EAACountryAdminSDK || {}


EAACountryAdminSDK.onFormLoad = (executionContext) => {
	formContext = Xrm.Page.ui.formContext
	formContext.ui.headerSection.setTabNavigatorVisible(false);
	// var formContext = executionContext.getFormContext();
	// formContext.getControl("aia_country_admin").addPreSearch(function (executionContext) {

	//     var filterxml = `<link-entity name="teamroles" from="teamid" to="teamid" link-type="inner" intersect="true">
	// 		<link-entity name="role" from="roleid" to="roleid" link-type="inner" intersect="true">
	// 			<filter>
	// 				<condition attribute="name" operator="eq" value="EAA_Country_Admin"/>
	// 			</filter>
	// 		</link-entity>
	// 	</link-entity>`;
	//     executionContext.getFormContext().getControl("aia_country_admin").addCustomFilter(filterxml, "team");

	// });
}