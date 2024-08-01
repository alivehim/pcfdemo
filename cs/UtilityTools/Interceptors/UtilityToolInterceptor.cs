using Castle.Core.Internal;
using Castle.DynamicProxy;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Aspect;
using UtilityTools.Extensions;

namespace UtilityTools.Interceptors
{
    public class UtilityToolInterceptor : AsyncInterceptorBase
    {
        private readonly IContainerProvider containerProvider;

        public UtilityToolInterceptor(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        private static IList<Aspect> GetAspects(IInvocation invocation)
        {
            return invocation.TargetType.GetAspects()
                .Union(invocation.MethodInvocationTarget.GetAspects())
                .ToList();
        }

        private static CacheAspect GetCacheAspect(IInvocation invocation)
        {
            var aspects = invocation.MethodInvocationTarget.GetAspects();

            foreach(var item in aspects)
            {
                if(item is CacheAspect ca)
                {
                    return ca;
                }
            }

            return null;
        }

        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            var aspects = GetAspects(invocation);
            var args = new AspectArgs()
            {
                ContainerProvider = containerProvider,
                Arguments = invocation.Arguments
            };


            if (!invocation.Arguments.IsNullOrEmpty() && invocation.Arguments[0] is UtilityTools.Services.Interfaces.IMessage messagebody)
            {
                args.messagebody = messagebody;
            }
            Exception ex = null;

            try
            {


                foreach (var aspect in aspects)
                {
                    aspect.OnEntry(args);
                }

                await proceed(invocation, proceedInfo).ConfigureAwait(false);
            }

            catch (Exception exception)
            {
                ex = exception;
            }

            finally
            {
                foreach (var aspect in aspects.Reverse())
                {
                    if (ex != null)
                    {
                        aspect.OnException(args, ex);
                    }

                    else
                    {
                        aspect.OnSucess(args);
                    }

                    aspect.OnExit(args);
                }
            }

        }

        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {

            var aspects = GetAspects(invocation);
            var cacheAspect = GetCacheAspect(invocation);
            var args = new AspectArgs() { ContainerProvider = containerProvider, Arguments = invocation.Arguments };


            if (!invocation.Arguments.IsNullOrEmpty() && invocation.Arguments[0] is UtilityTools.Services.Interfaces.IMessage messagebody)
            {
                args.messagebody = messagebody;
            }
            Exception ex = null;

            try
            {

                foreach (var aspect in aspects)
                {
                    aspect.OnEntry(args);
                }

                if (cacheAspect != null)
                {
                    var result = cacheAspect.GetCache<TResult>(args);

                    if (result != null)
                        return result;
                }

                var data = await proceed(invocation, proceedInfo).ConfigureAwait(false);

                if (cacheAspect != null)
                {
                    cacheAspect.SaveCache(args, data);
                }

                return data;

            }

            catch (Exception exception)
            {
                ex = exception;

                throw ex;
            }

            finally
            {
                foreach (var aspect in aspects.Reverse())
                {
                    if (ex != null)
                    {
                        aspect.OnException(args, ex);
                    }

                    else
                    {
                        aspect.OnSucess(args);
                    }

                    aspect.OnExit(args);
                }
            }
        }
    }
}
