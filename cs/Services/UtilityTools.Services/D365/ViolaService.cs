using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.D365;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class ViolaService : IViolaService
    {

        public async Task<eaa_form_collection> GetEaaForms(string authorization)
        {
            var fetchXml = @"<fetch version='1.0' mapping='logical' distinct='true' returntotalrecordcount='true' page='1' count='50' no-lock='false'>
	<entity name='aia_eaa_form'>
		<all-attributes />
		<filter type='and'>
			<condition attribute='aia_sys_active' operator='eq' value='1'/>
			<condition attribute='statecode' operator='eq' value='0'/>
			<condition attribute='aia_sys_processlock' operator='eq' value='0'/>
			<condition attribute='aia_eaa_form_status' operator='ne' value='589460001'/>
			<filter type='or'>
				<filter type='and'>
					<condition attribute='aia_eaa_requester_date' operator='this-year'/>
					<condition attribute='aia_eaa_approval_status' operator='ne' value='589450001'/>
				</filter>
				<filter type='and'>
					<condition attribute='aia_eaa_approval_status' operator='in'>
						<value>589450000</value>
						<value>589450003</value>
						<value>589450004</value>
					</condition>
				</filter>
				<filter type='and'>
					<condition attribute='aia_eaa_requester_date' operator='null'/>
				</filter>
			</filter>
		</filter>
	</entity>
</fetch>";
            string url = $"https://aia-dev.crm5.dynamics.com/api/data/v9.0/aia_eaa_forms?fetchXml=" + fetchXml;

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<eaa_form_collection>(content);
            return definition;

        }

        public async Task<RequestFormCollection> GetRequestFormsAsync(string authorization)
        {

            //https://aia-sit.crm5.dynamics.com/api/data/v9.0/aia_apv_request_forms?fetchXml=<fetch version="1.0" mapping="logical" distinct="true" returntotalrecordcount="true" page="1" count="50" no-lock="false"><entity name="aia_apv_request_form"><attribute name="statecode"/><attribute name="aia_apv_request_formid"/><attribute name="aia_sys_requesttype"/><attribute name="aia_com_priorityname"/><attribute name="aia_com_companycode"/><attribute name="aia_sys_status"/><attribute name="aia_com_priority"/><attribute name="createdby"/><attribute name="createdon"/><attribute name="aia_name"/><attribute name="aia_sys_wf_request" /><attribute name="aia_com_effectivedate"/><order attribute="aia_name" descending="true"/><order attribute="aia_com_effectivedate" descending="true"/><order attribute="createdon" descending="true"/><attribute name="aia_sys_sla_level"/><filter type="and"><condition attribute="statecode" operator="eq" value="0"/><condition attribute="aia_sys_status" operator="ne" value="143140000"/><condition attribute="aia_sys_active" operator="ne" value="0"/></filter></entity></fetch>


            var fetXml = @"<fetch version='1.0' mapping='logical' distinct='true' returntotalrecordcount='true' page='1' count='250' no-lock='false'>
    <entity name='aia_apv_request_form'>
        <attribute name='statecode' />
        <attribute name='aia_apv_request_formid' />
        <attribute name='aia_sys_requesttype' />
        <attribute name='aia_com_companycode' />
        <attribute name='aia_sys_status' />
        <attribute name='aia_com_priority' />
        <attribute name='aia_sys_leadtime_reminder' />
        <attribute name='createdby' />
        <attribute name='aia_sys_wf_request' />
        <attribute name='createdon' />
        <attribute name='aia_name' />
        <attribute name='aia_com_effectivedate' />
        <order attribute='aia_name' descending='true' />
        <order attribute='aia_com_effectivedate' descending='true' />
        <order attribute='createdon' descending='true' />
        <attribute name='aia_sys_sla_level' />
        <filter type='and'>
            <condition attribute='statecode' operator='eq' value='0' />
            <condition attribute='aia_sys_status' operator='ne' value='143140000' />
            <condition attribute='aia_sys_active' operator='ne' value='0' />
        </filter>
        <attribute name='aia_sys_submissiondate' />
    </entity>
</fetch>";

            //string url = $"https://aia-test.crm5.dynamics.com/api/data/v9.0/aia_apv_request_forms?fetchXml=<fetch version=\"1.0\" mapping=\"logical\" distinct=\"true\" returntotalrecordcount=\"true\" page=\"1\" count=\"50\" no-lock=\"false\"><entity name=\"aia_apv_request_form\"><attribute name=\"statecode\"/><attribute name=\"aia_apv_request_formid\"/><attribute name=\"aia_sys_requesttype\"/><attribute name=\"aia_com_priorityname\"/><attribute name=\"aia_com_companycode\"/><attribute name=\"aia_sys_status\"/><attribute name=\"aia_com_priority\"/><attribute name=\"createdby\"/><attribute name=\"createdon\"/><attribute name=\"aia_name\"/><attribute name=\"aia_com_effectivedate\"/><order attribute=\"aia_name\" descending=\"true\"/><attribute name=\"aia_sys_wf_request\" /><order attribute=\"aia_com_effectivedate\" descending=\"true\"/><order attribute=\"createdon\" descending=\"true\"/><attribute name=\"aia_sys_sla_level\"/><filter type=\"and\"><condition attribute=\"aia_sys_status\" operator=\"ne\" value=\"143140000\"/><condition attribute=\"aia_sys_active\" operator=\"ne\" value=\"0\"/></filter></entity></fetch>";
            string url = $"https://aia-sit.crm5.dynamics.com/api/data/v9.0/aia_apv_request_forms?fetchXml=" + fetXml;

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization,true);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestFormCollection>(content);
            return definition;
        }

        public async Task UpdateSubmissDate(string formId,string authorization)
        {
            string url = $"https://aia-sit.crm5.dynamics.com/api/data/v9.0/aia_apv_request_forms({formId})";

            var data = new { aia_sys_submissiondate = new DateTime(2022,6,23) };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);


            //await HttpHelper.PostDataToUrl(url, json, authorization);

            await HttpHelper.Patch(url, authorization, json);
        }

        public async Task MockTimeLineDate(string aia_wf_requestid, string authorization)
        {

            string requestTimeLineRequest = $"https://aia-sit.crm5.dynamics.com/api/data/v9.0/aia_wf_requesttimelines?fetchXml="
                + "<fetch version=\"1.0\" mapping=\"logical\" page=\"1\" count=\"10\" no-lock=\"false\"><entity name=\"aia_wf_requesttimeline\"><attribute name=\"aia_wf_requesttimelineid\" /><attribute name=\"aia_wf_request\" /><attribute name=\"modifiedon\" /><attribute name=\"aia_actionon\" />"
        + "<order attribute=\"modifiedon\" descending=\"true\" />"
        + "<attribute name=\"aia_actionreason\" />"
        + "<link-entity name=\"aia_wf_request\" from=\"aia_wf_requestid\" to=\"aia_wf_request\" alias=\"bb\">"
            + "<filter type=\"and\">"
                + $"<condition attribute=\"aia_wf_requestid\" operator=\"eq\" uitype=\"aia_wf_request\" value=\"{aia_wf_requestid}\" />"
            + "</filter>"
            + "</link-entity>"
            + "</entity></fetch>";

           
            var content = await HttpHelper.GetUrlContentAsync(requestTimeLineRequest, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeLineCollection>(content);


            foreach (var item in definition.value)
            {
                var dt = DateTime.Parse(item.aia_actionon);

                var newdt = dt.AddDays(-5);

                string url = $"https://aia-sit.crm5.dynamics.com/api/data/v9.0/aia_wf_requesttimelines({item.aia_wf_requesttimelineid})";

                var data = new { aia_actionon = newdt };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);


                //await HttpHelper.PostDataToUrl(url, json, authorization);

                await HttpHelper.Patch(url, authorization, json);

            }


        }
    }
}
