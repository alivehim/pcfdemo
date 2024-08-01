using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.DataExtractDescriptors
{
    [MessageOwner(MessageOwner.AutomateFlow)]
    public class AutoflowExtractDescriptor : BaseSimpleExtractDescriptor<FlowDescription>
    {
        private readonly IEntityDefinitionService entityDefinitionService;

        public AutoflowExtractDescriptor(IEntityDefinitionService entityDefinitionService)
        {
            this.entityDefinitionService = entityDefinitionService;
        }

        public override async Task RunAsync(ITaskContext taskContext)
        {
            var data = new List<FlowDescription>();

            var list = await this.entityDefinitionService.GetFlowsAsync(taskContext.Key);

            foreach (var item in list.value)
            {
                data.Add(new FlowDescription
                {
                    Name = item.msdyn_name,
                    DisplayName = item.msdyn_displayname,
                    ObjectId = item.msdyn_objectid,
                    WorkflowId = item.msdyn_workflowidunique
                });

            }

            Result(data);
        }
    }
}
