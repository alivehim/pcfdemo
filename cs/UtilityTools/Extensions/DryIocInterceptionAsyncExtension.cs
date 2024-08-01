using Castle.DynamicProxy;
using DryIoc;
using ImTools;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.DryIoc;
using UtilityTools.Core.Extensions;
using Prism.DryIoc;
using UtilityTools.Interceptors;

namespace UtilityTools.Extensions
{
    //https://blog.csdn.net/qq_39652397/article/details/123213219
    public static class DryIocInterceptionAsyncExtension
    {
        private static readonly DefaultProxyBuilder _proxyBuilder = new DefaultProxyBuilder();
        public static void Intercept<TService, TInterceptor>(this IRegistrator registrator, object serviceKey = null)
            where TInterceptor : class, IInterceptor
        {
            //var serviceType = typeof(TService);

            //Type proxyType;
            //if (serviceType.IsInterface)
            //    proxyType = ProxyBuilder.Value.CreateInterfaceProxyTypeWithTargetInterface(
            //        serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            //else if (serviceType.IsClass)
            //    proxyType = ProxyBuilder.Value.CreateClassProxyType(
            //        serviceType, ArrayTools.Empty<Type>(), ProxyOptions.Value);
            //else
            //    throw new ArgumentException(string.Format(
            //        "Intercepted service type {0} is not a supported: nor class nor interface", serviceType));

            //var decoratorSetup = serviceKey == null
            //    ? Setup.Decorator
            //    : Setup.DecoratorWith(r => serviceKey.Equals(r.ServiceKey));

            //registrator.Register(serviceType, proxyType,
            //    made: Made.Of(type => type.GetPublicInstanceConstructors().SingleOrDefault(c => c.GetParameters().Length != 0),
            //        Parameters.Of.Type<IAsyncInterceptor>(typeof(IAsyncInterceptor))),
            //    setup: decoratorSetup);

            var serviceType = typeof(TService);

            Type proxyType;
            if (serviceType.IsInterface())
                proxyType = _proxyBuilder.CreateInterfaceProxyTypeWithTargetInterface(
                    serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else if (serviceType.IsClass())
                proxyType = _proxyBuilder.CreateClassProxyTypeWithTarget(
                    serviceType, ArrayTools.Empty<Type>(), ProxyGenerationOptions.Default);
            else
                throw new ArgumentException(
                    $"{serviceType} 无法被拦截, 只有接口或者类才能被拦截");

            registrator.Register(serviceType, proxyType,
                made: Made.Of(pt => pt.PublicConstructors().FindFirst(ctor => ctor.GetParameters().Length != 0),
                    Parameters.Of.Type<IInterceptor[]>(typeof(TInterceptor[]))),
                setup: Setup.DecoratorOf(useDecorateeReuse: true, decorateeServiceKey: serviceKey));


        }

        public static IContainerRegistry InterceptAsync<TService, TInterceptor>(
            this IContainerRegistry containerRegistry, object serviceKey = null)
        where TInterceptor : class, IAsyncInterceptor
        {
            var container = containerRegistry.GetContainer();
            container.Intercept<TService, AsyncInterceptor<TInterceptor>>(serviceKey);
            return containerRegistry;
        }

        //public static IContainerRegistry InterceptAsync<TService, TInterceptor>(
        //this IContainerRegistry containerRegistry, object serviceKey = null)
        //where TInterceptor : class, IInterceptor
        //{
        //    var container = containerRegistry.GetContainer();
        //    container.Intercept<TService,TInterceptor>(serviceKey);
        //    return containerRegistry;
        //}

        //private static readonly Lazy<DefaultProxyBuilder> ProxyBuilder = new Lazy<DefaultProxyBuilder>(() => new DefaultProxyBuilder());
        //private static readonly Lazy<ProxyGenerationOptions> ProxyOptions = new Lazy<ProxyGenerationOptions>(() => new ProxyGenerationOptions(new ProxyGenerationHook()));

    }
}
