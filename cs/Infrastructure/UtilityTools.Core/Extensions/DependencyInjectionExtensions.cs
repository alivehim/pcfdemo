using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.D365;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.DependencyInjection;
using UtilityTools.Core.Models.Engine;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.D365;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;

namespace UtilityTools.Core.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static T ResolveService<T>(this IContainerProvider containerProvider)
        {
            var result = containerProvider.Resolve(typeof(T));
            if (result != null)
                return (T)result;

            throw new Exception("DI encounter Error");
        }

        public static IContainerRegistry AddDescription<T,To>(this IContainerRegistry services, MessageOwner  messageOwner)
           where T : new()
           where To: ISimpleExtractDescriptor<T>
        {
            //here code can be optimized
            services.Register<ISimpleExtractDescriptor<T>,To>();
            services.RegisterInstance(new ExtractDescriptionNode(messageOwner, typeof(To)));

            return services;
        }

        public static IContainerRegistry AddMediaDescription<T,To>(this IContainerRegistry services, MediaSymbolType symbol)
           where T : new()
           where To : IMediaPageDescriptor<T>
        {
            services.Register<IMediaPageDescriptor<T>, To>();
            services.RegisterInstance(new MediaGetDescriptionNode(symbol, typeof(To)));

            return services;
        }

        public static IContainerRegistry AddMagnetService<T>(this IContainerRegistry services, MagnetSearchSource symbol)
          where T : IMagnetService
        {
            services.Register<IMagnetService, T>();
            services.RegisterInstance(new MagnetDescriptionNode(symbol, typeof(T)));

            return services;
        }

        public static IContainerRegistry AddDownloadEngineProvider<T>(this IContainerRegistry services, LivingStreamDownloadProvider  downloadProvider)
         where T : ILivingStreamMediaDownloadEngine
        {
            services.Register<ILivingStreamMediaDownloadEngine, T>();
            services.RegisterInstance(new LivingStreamDownloadProviderDescriptionNode(downloadProvider, typeof(T)));

            return services;
        }


        //public static IContainerRegistry AddOnenoteService<T>(this IContainerRegistry services, OnenoteSource symbol)
        //where T : IGraphOnenoteService
        //{
        //    services.Register<IGraphOnenoteService, T>().InterceptAsync<IViolaService, UtilityToolInterceptor>(); ;

        //    services.RegisterInstance(new OnenoteDescriptionNode(symbol, typeof(T)));

        //    return services;
        //}

    }
}
