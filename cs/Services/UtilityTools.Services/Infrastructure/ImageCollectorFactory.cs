﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;

namespace UtilityTools.Services.Infrastructure
{
    public class ImageCollectorFactory : IImageCollectorFactory
    {
        private readonly IMediaPageDescriptorFactory mediaPageDescriptorFactory;
        private readonly IMediaSymbolService mediaSymbolService;

        public ImageCollectorFactory(IMediaPageDescriptorFactory mediaPageDescriptorFactory, IMediaSymbolService mediaSymbolService)
        {
            this.mediaPageDescriptorFactory = mediaPageDescriptorFactory;
            this.mediaSymbolService = mediaSymbolService;
        }

        public IImageCollector GetImageCollector(string key)
        {
            var symbol = mediaSymbolService.GetSymbol(key);
            var descripor = mediaPageDescriptorFactory.GetPageDescriptor(symbol);

            return descripor as IImageCollector;
        }
    }
}
