using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models.D365;
using UtilityTools.Services.Aspects;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class EntityDefinitionService : IEntityDefinitionService
    {
        [LogAspect("GetEntities")]
        public async Task<WebResourceMetaDefinition> GetEntitiesAsync(string authorization)
        {
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/msdyn_solutioncomponentsummaries?%24filter=(msdyn_solutionid%20eq%20{Settings.Current.D365SolutionId})%20and%20((msdyn_componenttype%20eq%201))&api-version=9.1";

            var content = await HttpHelper.GetUrlContentAsync(url,  Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<WebResourceMetaDefinition>(content);
            return definition;
        }

        [LogAspect("GetEntity")]
        public async Task<EntityDefinition> GetEntityAsync(string entityName, string authorization)
        {
            //var url= "{Settings.Current.D365ResourceUrl}/api/data/v9.0/EntityDefinitions(LogicalName = 'aia_apv_businessarea')?$expand=Attributes,OneToManyRelationships,ManyToOneRelationships,ManyToManyRelationships,Keys&api-version=9.1"
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/EntityDefinitions(LogicalName = '{entityName}')?$expand=Attributes,Keys&api-version=9.1";

            var content = await HttpHelper.GetUrlContentAsync(url,  Encoding.UTF8,authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<EntityDefinition>(content);

            return definition;
        }

        [LogAspect("GetChoices")]
        public async Task<WebResourceMetaDefinition> GetChoicesAsync(string authorization)
        {
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/msdyn_solutioncomponentsummaries?$filter=(msdyn_solutionid eq {Settings.Current.D365SolutionId}) and ((msdyn_componenttype eq 9))&$orderby=msdyn_displayname asc&api-version=9.1";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<WebResourceMetaDefinition>(content);
            return definition;
        }

        [LogAspect("GetChoice")]
        public async Task<ChoiceDefinition> GetChoiceAsync(string objectId,string authorization)
        {
            
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/GlobalOptionSetDefinitions({objectId})/Microsoft.Dynamics.CRM.OptionSetMetadata?$select=IsCustomizable,MetadataId,IsCustomOptionSet,Options";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8,authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<ChoiceDefinition>(content);
            return definition;

        }

        [LogAspect("GetEntityChoice")]
        public async Task<EntityChioceDefinition> GetEntityChoiceAsync(string entityName,string columnName, string authorization)
        {

            //https://clp-dev.crm5.dynamics.com/api/data/v9.0/EntityDefinitions(LogicalName='clp_proximitycardinventory')/Attributes(LogicalName='clp_cardtype')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$expand=OptionSet($select=Options),GlobalOptionSet($select=Options)
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/EntityDefinitions(LogicalName='{entityName}')/Attributes(LogicalName='{columnName}')/Microsoft.Dynamics.CRM.PicklistAttributeMetadata?$select=LogicalName&$expand=OptionSet($select=Options),GlobalOptionSet($select=Options)";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<EntityChioceDefinition>(content);
            return definition;

        }


        [LogAspect("GetFlows")]
        public async Task<FlowDefinitionCollection> GetFlowsAsync(string authorization)
        {

            //https://clp-dev.crm5.dynamics.com/api/data/v9.0/msdyn_solutioncomponentsummaries?%24filter=(msdyn_solutionid%20eq%209486d209-c82c-ed11-9db2-000d3a0871cb)%20and%20((msdyn_componenttype%20eq%2029%20and%20msdyn_workflowcategory%20eq%20%275%27))&%24orderby=msdyn_displayname%20asc&api-version=9.1
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/msdyn_solutioncomponentsummaries?%24filter=(msdyn_solutionid%20eq%20{Settings.Current.D365SolutionId})%20and%20((msdyn_componenttype%20eq%2029%20and%20msdyn_workflowcategory%20eq%20%275%27))&%24orderby=msdyn_displayname%20asc&api-version=9.1";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<FlowDefinitionCollection>(content);
            return definition;

        }

        public async Task<DependencyMetadataResponse> GetFlowRequiredComponentsAsync(string objectid, string authorization)
        {
            //https://clp-dev.crm5.dynamics.com/api/data/v9.0/RetrieveRequiredComponentsWithMetadata(ObjectId%3Da1624519-ebdf-ed11-a7c7-000d3aa0f86c%2CComponentType%3D29)

            //var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/msdyn_solutioncomponentsummaries?%24filter=(msdyn_solutionid%20eq%20{Settings.Current.D365SolutionId})%20and%20((msdyn_componenttype%20eq%2029%20and%20msdyn_workflowcategory%20eq%20%275%27))&%24orderby=msdyn_displayname%20asc&api-version=9.1";
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/RetrieveRequiredComponentsWithMetadata(ObjectId%3D{objectid}%2CComponentType%3D29)";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<DependencyMetadataResponse>(content);
            return definition;
        }

        public async Task<DependencyMetadataResponse> GetFlowDependentComponentsAsync(string objectid, string authorization)
        {
            //https://clp-dev.crm5.dynamics.com/api/data/v9.0/RetrieveDependentComponentsWithMetadata(ObjectId%3D46c5e7df-e9ef-ed11-8849-000d3aa0f86c%2CComponentType%3D29)

            //var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/msdyn_solutioncomponentsummaries?%24filter=(msdyn_solutionid%20eq%20{Settings.Current.D365SolutionId})%20and%20((msdyn_componenttype%20eq%2029%20and%20msdyn_workflowcategory%20eq%20%275%27))&%24orderby=msdyn_displayname%20asc&api-version=9.1";
            var url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/RetrieveDependentComponentsWithMetadata(ObjectId%3D{objectid}%2CComponentType%3D29)";

            var content = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8, authorization);

            var definition = Newtonsoft.Json.JsonConvert.DeserializeObject<DependencyMetadataResponse>(content);
            return definition;
        }
    }
}
