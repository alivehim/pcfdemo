using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;

namespace UtilityTools.Services.Core
{
    public class ContentCrawlingServiceFactory : IContentCrawlingServiceFactory
    {
        private readonly IMediaPageDescriptorFactory mediaPageDescriptorFactory;
        private readonly IMediaSymbolService mediaSymbolService;

        public ContentCrawlingServiceFactory(IMediaPageDescriptorFactory mediaPageDescriptorFactory, IMediaSymbolService mediaSymbolService)
        {
            this.mediaPageDescriptorFactory = mediaPageDescriptorFactory;
            this.mediaSymbolService = mediaSymbolService;
        }


        public IGrabArtcleContent GetContentExtractor(string key)
        {
            var symbol = mediaSymbolService.GetSymbol(key);
            var descripor = mediaPageDescriptorFactory.GetPageDescriptor(symbol);

            return descripor as IGrabArtcleContent;
        }

    }
}
