using System.Collections.Generic;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.DataExtractDescriptors
{
    [MessageOwner(MessageOwner.EntityManager)]
    public class D365EntityExtractDescription : BaseSimpleExtractDescriptor<EntityDescription>
    {
        private readonly IEntityDefinitionService entityDefinitionService;

        public D365EntityExtractDescription(IEntityDefinitionService entityDefinitionService)
        {
            this.entityDefinitionService = entityDefinitionService;
        }

        //public override IExtractResult<EntityDescription> Run(ITaskContext taskContext)
        //{
        //    var data = new List<EntityDescription>();

        //    if (taskContext.ExtendKey == $"{(int)MetadataType.Entity}")
        //    {
        //        var list = entityDefinitionService.GetEntitiesAsync(taskContext.Key).GetAwaiter().GetResult();

        //        foreach (var item in list.value)
        //        {
        //            data.Add(new EntityDescription
        //            {
        //                Id = item.msdyn_objectid,
        //                Name = item.msdyn_name,
        //                DisplayName = item.msdyn_displayname,
        //                ObjectId = item.msdyn_objectid
        //            });
        //        }

        //        return Result(data);
        //    }
        //    else
        //    {
        //        var list = entityDefinitionService.GetChoicesAsync(taskContext.Key).GetAwaiter().GetResult();

        //        foreach (var item in list.value)
        //        {
        //            data.Add(new EntityDescription
        //            {
        //                Id = item.msdyn_objectid,
        //                Name = item.msdyn_name,
        //                DisplayName = item.msdyn_displayname,
        //                ObjectId = item.msdyn_objectid
        //            });
        //        }

        //        return Result(data);
        //    }

        //}

        public async override Task RunAsync(ITaskContext taskContext)
        {

            var data = new List<EntityDescription>();

            if (taskContext.ExtendKey == $"{(int)MetadataType.Entity}")
            {
                var list = await entityDefinitionService.GetEntitiesAsync(taskContext.Key);

                foreach (var item in list.value)
                {
                    data.Add(new EntityDescription
                    {
                        Id = item.msdyn_objectid,
                        Name = item.msdyn_name,
                        DisplayName = item.msdyn_displayname,
                        ObjectId = item.msdyn_objectid
                    });
                }

                 Result(data);
            }
            else
            {
                var list = await entityDefinitionService.GetChoicesAsync(taskContext.Key);

                foreach (var item in list.value)
                {
                    data.Add(new EntityDescription
                    {
                        Id = item.msdyn_objectid,
                        Name = item.msdyn_name,
                        DisplayName = item.msdyn_displayname,
                        ObjectId = item.msdyn_objectid
                    });
                }

                Result(data);
            }

        }
    }
}
