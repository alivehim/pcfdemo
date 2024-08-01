using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IEntityDefinitionService
    {
        Task<WebResourceMetaDefinition> GetEntitiesAsync(string authorization);

        Task<EntityDefinition> GetEntityAsync(string entityName, string authorization);

        Task<WebResourceMetaDefinition> GetChoicesAsync(string authorization);

        Task<ChoiceDefinition> GetChoiceAsync(string objectId, string authorization);

        Task<EntityChioceDefinition> GetEntityChoiceAsync(string entityName, string columnName, string authorization);

        Task<FlowDefinitionCollection> GetFlowsAsync(string authorization);

        Task<DependencyMetadataResponse> GetFlowRequiredComponentsAsync(string objectid, string authorization);

        Task<DependencyMetadataResponse> GetFlowDependentComponentsAsync(string objectid, string authorization);
    }
}
