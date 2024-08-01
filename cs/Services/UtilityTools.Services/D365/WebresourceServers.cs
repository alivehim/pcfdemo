using Prism.Events;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.D365;
using UtilityTools.Services.Aspects;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.D365
{
    public class WebresourceServers : IWebresourceServers
    {

        private readonly IMessageStreamProvider<IUXMessage> messageStreamProvider;
        private readonly IUtilityToolsSettingService utilityToolsSettingService;
        private readonly IEventAggregator eventAggregator;

        public WebresourceServers(IMessageStreamProvider<IUXMessage> messageStreamProvider, IUtilityToolsSettingService utilityToolsSettingService, IEventAggregator eventAggregator)
        {
            this.messageStreamProvider = messageStreamProvider;
            this.utilityToolsSettingService = utilityToolsSettingService;
            this.eventAggregator = eventAggregator;
        }

        public async Task<SolutionCollectoin> GetSolutionAsync(string authorization)
        {
            //https://aia-dev.crm5.dynamics.com/api/data/v9.0/solutions?%24expand=publisherid(%24select%3Duniquename%2Cpublisherid%2Cfriendlyname%2Ccustomizationprefix%2Ccustomizationoptionvalueprefix)&%24filter=isvisible%20eq%20true%20or%20endswith(uniquename%2C%20%27_PowerAppsChecker%27)%20or%20uniquename%20eq%20%27msdynce_ServiceAnchor%27
            //var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/solutions?%24expand=publisherid(%24select%3Duniquename%2Cpublisherid%2Cfriendlyname%2Ccustomizationprefix%2Ccustomizationoptionvalueprefix)&%24filter=isvisible%20eq%20true%20or%20endswith(uniquename%2C%20%27_PowerAppsChecker%27)%20or%20uniquename%20eq%20%27msdynce_ServiceAnchor%27";
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/solutions?%24expand=publisherid&%24filter=(isvisible%20eq%20true)%20and%20ismanaged%20eq%20false&%24orderby=createdon%20desc";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<SolutionCollectoin>(content);
            return definition;
        }


        [LogAspect("GetWebresources")]
        public async Task<WebResourceMetaDefinition> GetWebresourcesAsync(string authorization)
        {
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/msdyn_solutioncomponentsummaries?%24filter=(msdyn_solutionid%20eq%20{Settings.Current.D365SolutionId})%20and%20((msdyn_componenttype%20eq%2061))&%24orderby=msdyn_displayname%20asc&api-version=9.1";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<WebResourceMetaDefinition>(content);
            return definition;
        }

        /// <summary>
        /// upload local file to solution
        /// </summary>
        /// <param name="message"></param>
        /// <param name="id">STRING of web resource record you want to upload</param>
        /// <param name="authorization">authorization code to call api</param>
        /// <param name="filePath">the path of file</param>
        /// <returns></returns>
        [LogAspect("UploadFile")]
        public async Task<string> UploadFileAsync(IMessage message, string id, string authorization, string filePath)
        {

            var fileContent = File.ReadAllText(filePath);
            var patchUrl = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/webresourceset({id})";

            var jsonObj = new
            {
                content = Base64.Encode(Encoding.UTF8, fileContent)
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj);
            var result = await HttpHelper.Patch(patchUrl, authorization, json);

            messageStreamProvider.Info(message, result);
            return result;

        }


        class PublishContent
        {
            public string ParameterXml { get; set; }
        }

        [LogAspect("Publish")]
        public async Task PublishAsync(IMessage message, string id, string authorization)
        {
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/PublishXml";
            var publishContent = new PublishContent();
            publishContent.ParameterXml = $"<importexportxml><webresources><webresource>{{{id}}}</webresource></webresources></importexportxml>";

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(publishContent);
            var result = await HttpHelper.PostDataToUrl(url, json, authorization);

            messageStreamProvider.Info(message, result);
        }


        public class TokenResponse
        {
            public string token_type { get; set; }
            public string access_token { get; set; }
            public string refresh_token { get; set; }
            public string client_info { get; set; }
        }

        [LogAspect("Get Token")]
        public virtual async Task<string> GetTokenAsync()
        {

            var url = "https://login.microsoftonline.com/common/oauth2/token";

            var datamaps = new Dictionary<string, string>
            {
                {"client_id","51f81489-12ee-4a9e-aaae-a2591f45987d" },
                //{"client_id","55e68f14-899d-4e01-8a0a-9fe1098f55ee" },
                {"scope","openid" },
                {"grant_type","password" },
                {"client_info","1" },
                {"username", Settings.Current.D365UserName },
                {"password", Settings.Current.D365Password },
                {"resource",$"{Settings.Current.D365ResourceUrl}" },
                //{"resource",$"https://service.flow.microsoft.com" },
            };


            var result = await HttpHelper.PostFormData(url, datamaps);

            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(result);

            //utilityToolsSettingService.SetAccessToken(jsonResult.access_token);
            Settings.Current.D365AccessToken = jsonResult.access_token;
            Settings.Current.Save(nameof(Settings.Current.D365AccessToken));

            await GetFlowTokenAsync();

            //update token value from other workspace

            eventAggregator.GetEvent<TokenEvent>().Publish(jsonResult.access_token);
            return jsonResult.access_token;

        }

        [LogAspect("Get Flow Token")]
        public virtual async Task<string> GetFlowTokenAsync()
        {

            var url = "https://login.microsoftonline.com/common/oauth2/token";

            var datamaps = new Dictionary<string, string>
            {
                {"client_id","51f81489-12ee-4a9e-aaae-a2591f45987d" },
                //{"client_id","55e68f14-899d-4e01-8a0a-9fe1098f55ee" },
                {"scope","openid" },
                {"grant_type","password" },
                {"client_info","1" },
                {"username", Settings.Current.D365UserName },
                {"password", Settings.Current.D365Password },
                //{"resource",$"{Settings.Current.D365ResourceUrl}" },
                {"resource",$"https://service.flow.microsoft.com" },
            };


            var result = await HttpHelper.PostFormData(url, datamaps);

            var jsonResult = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenResponse>(result);

            //utilityToolsSettingService.SetAccessToken(jsonResult.access_token);
            Settings.Current.FlowToken = jsonResult.access_token;
            Settings.Current.Save(nameof(Settings.Current.FlowToken));

            //update token value from other workspace

            eventAggregator.GetEvent<TokenEvent>().Publish(jsonResult.access_token);
            return jsonResult.access_token;

        }
    }
}
