using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class OnenoteServiceFactory : IOnenoteServiceFactory
    {
        private readonly IEnumerable<OnenoteDescriptionNode> magnetDescriptionNodes;
        private readonly IContainerProvider containerProvider;

        public OnenoteServiceFactory(IEnumerable<OnenoteDescriptionNode> magnetDescriptionNodes, IContainerProvider containerProvider)
        {
            this.magnetDescriptionNodes = magnetDescriptionNodes;
            this.containerProvider = containerProvider;
        }

        public IGraphOnenoteService GetHandler(OnenoteSource symbol)
        {
            var endpoint = magnetDescriptionNodes.Where(p => p.SearchSource == symbol).FirstOrDefault();
            if (endpoint == null)
            {
                throw new Exception($"can not find ExtractResultHandler symbol {symbol}");
            }

            return Resovle(endpoint);
        }

        private IGraphOnenoteService Resovle(OnenoteDescriptionNode magnetDescriptionNode)
        {
            var handler = containerProvider.Resolve(magnetDescriptionNode.Handler) as IGraphOnenoteService;
            return handler;
        }
    }
}
