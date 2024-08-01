using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DependencyInjection;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.MediaGet
{
    public class MediaPageDescriptorFactory : IMediaPageDescriptorFactory
    {
        private readonly IEnumerable<MediaGetDescriptionNode> mediaGetDescriptionNodes;
        private readonly IContainerProvider containerProvider;

        public MediaPageDescriptorFactory(IEnumerable<MediaGetDescriptionNode> mediaGetDescriptionNodes, IContainerProvider containerProvider)
        {
            this.mediaGetDescriptionNodes = mediaGetDescriptionNodes;
            this.containerProvider = containerProvider;
        }
        public IList<MediaGetDescriptionNode> GetAllDescriptor() => mediaGetDescriptionNodes.ToList();

        public IDataDescriptor GetPageDescriptor(MediaSymbolType symbol)
        {

            var endpoint = mediaGetDescriptionNodes.Where(p => p.Symbol == symbol).FirstOrDefault();
            if (endpoint == null)
            {
                return mediaGetDescriptionNodes.First(p => p.GetType().Name.Contains("LocalFilesPageDescriptor")) as IDataDescriptor;
            }

            return Resovle(endpoint);

        }

        private IDataDescriptor Resovle(MediaGetDescriptionNode extractDescriptionNode)
        {
            var handler = containerProvider.Resolve(extractDescriptionNode.Handler) as IDataDescriptor;
            return handler;
        }

    }
}
