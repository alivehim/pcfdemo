using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Engine;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;

namespace UtilityTools.Engine
{
    public class MediaDownloadEngineFactory : IMediaDownloadEngineFactory
    {
        private readonly IEnumerable<StreamDownloadProviderDescriptionNode> downloadProviderDescriptionNodes;
        private readonly IContainerProvider containerProvider;

        public MediaDownloadEngineFactory(IEnumerable<StreamDownloadProviderDescriptionNode> downloadProviderDescriptionNodes, IContainerProvider containerProvider)
        {
            this.downloadProviderDescriptionNodes = downloadProviderDescriptionNodes;
            this.containerProvider = containerProvider;
        }

        public IStreamFileDownloadEngine GetHandler(StreamDownloadProvider downloadProvider)
        {
            var endpoint = downloadProviderDescriptionNodes.Where(p => p.DownloadProvider == downloadProvider).FirstOrDefault();
            if (endpoint == null)
            {
                throw new Exception($"can not find ExtractResultHandler provider {downloadProvider}");
            }

            return Resovle(endpoint);
        }

        private IStreamFileDownloadEngine Resovle(StreamDownloadProviderDescriptionNode downloadProviderDescriptionNode)
        {
            var handler = containerProvider.Resolve(downloadProviderDescriptionNode.Handler) as IStreamFileDownloadEngine;
            return handler;
        }

    }

    public class LivingMediaDownloadEngineFactory : ILivingMediaDownloadEngineFactory
    {
        private readonly IEnumerable<LivingStreamDownloadProviderDescriptionNode> downloadProviderDescriptionNodes;
        private readonly IContainerProvider containerProvider;

        public LivingMediaDownloadEngineFactory(IEnumerable<LivingStreamDownloadProviderDescriptionNode> downloadProviderDescriptionNodes, IContainerProvider containerProvider)
        {
            this.downloadProviderDescriptionNodes = downloadProviderDescriptionNodes;
            this.containerProvider = containerProvider;
        }

        public ILivingStreamMediaDownloadEngine GetHandler(LivingStreamDownloadProvider downloadProvider)
        {
            var endpoint = downloadProviderDescriptionNodes.Where(p => p.DownloadProvider == downloadProvider).FirstOrDefault();
            if (endpoint == null)
            {
                throw new Exception($"can not find ExtractResultHandler provider {downloadProvider}");
            }

            return Resovle(endpoint);
        }

        private ILivingStreamMediaDownloadEngine Resovle(LivingStreamDownloadProviderDescriptionNode downloadProviderDescriptionNode)
        {
            var handler = containerProvider.Resolve(downloadProviderDescriptionNode.Handler) as ILivingStreamMediaDownloadEngine;
            return handler;
        }

    }
}
