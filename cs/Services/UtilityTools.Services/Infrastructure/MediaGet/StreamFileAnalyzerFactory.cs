using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.MediaGet
{
    public class StreamFileAnalyzerFactory : IStreamFileAnalyzerFactory
    {
        private readonly MediaPageDescriptorFactory mediaPageDescriptorFactory;
        private readonly IMediaSymbolService mediaSymbolService;

        public StreamFileAnalyzerFactory(MediaPageDescriptorFactory mediaPageDescriptorFactory, IMediaSymbolService mediaSymbolService)
        {
            this.mediaPageDescriptorFactory = mediaPageDescriptorFactory;
            this.mediaSymbolService = mediaSymbolService;
        }

        public IStreamAnalyzer GetStreamAnalyzer(MediaSymbolType mediaSymbolType)
        {
            var descripor = mediaPageDescriptorFactory.GetPageDescriptor(mediaSymbolType);

            return descripor as IStreamAnalyzer;
        }
    }
}
