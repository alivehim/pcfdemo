using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Services.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.DataExtractDescriptors
{
    [MessageOwner(MessageOwner.MediaGet)]
    public class MediaGetExtractDescription : BaseSimpleExtractDescriptor<MediaDataDescription>
    {
        private readonly IMediaPageDescriptorFactory mediaPageDescriptorFactory;
        private readonly IMediaSymbolService mediaSymbolService;

        public MediaGetExtractDescription(IMediaPageDescriptorFactory mediaPageDescriptorFactory, IMediaSymbolService mediaSymbolService)
        {
            this.mediaPageDescriptorFactory = mediaPageDescriptorFactory;
            this.mediaSymbolService = mediaSymbolService;
        }

        //public override IExtractResult<MediaDataDescription> Run(ITaskContext taskContext)
        //{
        //    var symbol = mediaSymbolService.GetSymbol(taskContext.Key);
        //    var descripor = mediaPageDescriptorFactory.GetPageDescriptor(symbol);

        //    var extractDescriptor = descripor as IMediaPageDescriptor<MediaDataDescription>;
        //    return extractDescriptor.Run(new MediaGetContext(taskContext, symbol));
        //}

        public async override Task RunAsync(ITaskContext taskContext)
        {
            var symbol = mediaSymbolService.GetSymbol(taskContext.Key);
            var descripor = mediaPageDescriptorFactory.GetPageDescriptor(symbol);

            var extractDescriptor = descripor as IMediaPageDescriptor<MediaDataDescription>;
            ExtractResult = await extractDescriptor.RunAsync(new MediaGetContext(taskContext, symbol));

            
        }
    }
}
