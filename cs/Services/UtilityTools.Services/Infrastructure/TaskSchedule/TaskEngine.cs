using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DependencyInjection;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.TaskSchedule
{
    public class TaskEngine : ITaskEngine
    {
        private readonly IMessageStreamProvider<IExtractResult<BaseResourceMetadata>> messageStreamProvider;
        private readonly IMessageStreamProvider<IUXMessage> logProvider;

        public TaskEngine(IMessageStreamProvider<IExtractResult<BaseResourceMetadata>> messageStreamProvider, IMessageStreamProvider<IUXMessage> logProvider)
        {
            this.messageStreamProvider = messageStreamProvider;
            this.logProvider = logProvider;
        }

        public async Task Work(ITaskContext context)
        {
            try
            {
                var ExtractDescriptor = context.DataDescriptor as ISimpleExtractDescriptor<BaseResourceMetadata>;
                if (!string.IsNullOrEmpty(context.Key))
                {
                    await ExtractDescriptor.RunAsync(context);
                    if (ExtractDescriptor.ExtractResult != null)
                    {
                        messageStreamProvider.Publisher(ExtractDescriptor.ExtractResult);
                    }
                }
            }
            catch (Exception ex)
            {
                logProvider.Error(ex.ToString());

                messageStreamProvider.Publisher(new ExtractResult<BaseResourceMetadata> { MessageOwner = context.MessageOwner });
            }

        }
    }
}
