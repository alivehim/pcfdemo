using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.TaskSchedule
{
    public class StreamFileExtractScheduler : IStreamFileExtractScheduler
    {
        private readonly StreamFileAnalyzerFactory streamFileAnalyzerFactory;
        private readonly IMessageStreamProvider<IExtractResult<BaseResourceMetadata>> messageStreamProvider;
        private readonly IMessageStreamProvider<IUXMessage> logProvider;

        public StreamFileExtractScheduler(StreamFileAnalyzerFactory streamFileAnalyzerFactory, IMessageStreamProvider<IExtractResult<BaseResourceMetadata>> messageStreamProvider, IMessageStreamProvider<IUXMessage> logProvider)
        {
            this.streamFileAnalyzerFactory = streamFileAnalyzerFactory;
            this.messageStreamProvider = messageStreamProvider;
            this.logProvider = logProvider;
        }

        public async Task<string> Work(IStreamUXItemDescription streamUXItemDescription)
        {

            try
            {
                var streamAnalyzer = streamFileAnalyzerFactory.GetStreamAnalyzer(streamUXItemDescription.SymbolType);
                if (streamAnalyzer != null)
                {
                    return await streamAnalyzer.GetStreamFile(streamUXItemDescription);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                logProvider.Error(ex.ToString());
                streamUXItemDescription.Error(ex.Message);
                return string.Empty;
            }
            finally
            {
                streamUXItemDescription.IsWaiting = false;
            }
        }

    }
}
