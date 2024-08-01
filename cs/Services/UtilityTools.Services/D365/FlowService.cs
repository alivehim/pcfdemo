using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.D365;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class FlowService: IFlowService
    {
        //https://asia.api.flow.microsoft.com/providers/Microsoft.ProcessSimple/environments/6f93d7ef-77e7-4491-a5d8-a0d4b4d5114f/flows/32af331d-2eb6-4e23-9fac-6827caccb260/runs?api-version=2016-11-01
        public async Task<FlowHistoryCollection> GetFlowHistoryAsync(string environmentid,string flowid, string token="")
        {

            string url = $"https://asia.api.flow.microsoft.com/providers/Microsoft.ProcessSimple/environments/{environmentid}/flows/{flowid}/runs?api-version=2016-11-01";
            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, token.Contains("Bearer")? token : $"Bearer {token}");

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<FlowHistoryCollection>(content);
            return definition;
        }

        public async Task<FlowHistoryCollection> GetFlowHistoryAsync(string nextUrl, string token = "")
        {

            var content = await HttpHelper.GetUrlContentAsync(nextUrl, Encoding.UTF8, token.Contains("Bearer") ? token : $"Bearer {token}");

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<FlowHistoryCollection>(content);
            return definition;
        }

        public  async Task<TriggerDetail> GetFlowTriggerOutDetail(string url)
        {
            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<TriggerDetail>(content);
            return definition;
        }
    }
}
