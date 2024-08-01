using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Services.Interfaces.Core;

namespace UtilityTools.Services.Core
{
    public class MagnetServieFactory : IMagnetServieFactory
    {
        private readonly IEnumerable<MagnetDescriptionNode> magnetDescriptionNodes;
        private readonly IContainerProvider containerProvider;

        public MagnetServieFactory(IEnumerable<MagnetDescriptionNode> magnetDescriptionNodes, IContainerProvider containerProvider)
        {
            this.magnetDescriptionNodes = magnetDescriptionNodes;
            this.containerProvider = containerProvider;
        }

        public IMagnetService GetHandler(MagnetSearchSource symbol)
        {
            var endpoint = magnetDescriptionNodes.Where(p => p.SearchSource == symbol).FirstOrDefault();
            if (endpoint == null)
            {
                throw new Exception($"can not find ExtractResultHandler symbol {symbol}");
            }

            return Resovle(endpoint);
        }

        private IMagnetService Resovle(MagnetDescriptionNode magnetDescriptionNode)
        {
            var handler = containerProvider.Resolve(magnetDescriptionNode.Handler) as IMagnetService;
            return handler;
        }
    }
}
