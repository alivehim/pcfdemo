using Castle.DynamicProxy;
using DryIoc.Messages;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Aspect;
using UtilityTools.Extensions;
using UtilityTools.Services.Extensions;

namespace UtilityTools.Interceptors
{
    //https://stackoverflow.com/questions/28099669/intercept-async-method-that-returns-generic-task-via-dynamicproxy/43272955#43272955
    public class AsyncExceptionHandlingInterceptor : IInterceptor
    {
        private static readonly MethodInfo handleAsyncMethodInfo = typeof(AsyncExceptionHandlingInterceptor).GetMethod("HandleAsyncWithResult", BindingFlags.Instance | BindingFlags.NonPublic);
        //private readonly IExceptionHandler _handler;
        private readonly IContainerProvider containerProvider;
        public AsyncExceptionHandlingInterceptor(/*IExceptionHandler handler*/ IContainerProvider containerProvider)
        {
            //_handler = handler;
            this.containerProvider = containerProvider;
        }

        private static IList<Aspect> GetAspects(
           IInvocation invocation)
        {
            return invocation.TargetType.GetAspects()
                .Union(invocation.MethodInvocationTarget.GetAspects())
                .ToList();
        }

        public void Intercept(IInvocation invocation)
        {
            var aspects = GetAspects(invocation);

            var args = new AspectArgs() { ContainerProvider = containerProvider };


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

                var delegateType = GetDelegateType(invocation);
                if (delegateType == MethodType.Synchronous)
                {
                    //_handler.HandleExceptions(() => invocation.Proceed());
                    invocation.Proceed();
                }
                if (delegateType == MethodType.AsyncAction)
                {
                    invocation.Proceed();
                    invocation.ReturnValue = HandleAsync((Task)invocation.ReturnValue);
                }
                if (delegateType == MethodType.AsyncFunction)
                {
                    invocation.Proceed();
                    ExecuteHandleAsyncWithResultUsingReflection(invocation);
                }
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

        private void ExecuteHandleAsyncWithResultUsingReflection(IInvocation invocation)
        {
            var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
            var mi = handleAsyncMethodInfo.MakeGenericMethod(resultType);
            invocation.ReturnValue = mi.Invoke(this, new[] { invocation.ReturnValue });
        }

        private async Task HandleAsync(Task task)
        {
            //await _handler.HandleExceptions(async () => await task);
            await task;
        }

        private async Task<T> HandleAsyncWithResult<T>(Task<T> task)
        {
            //return await _handler.HandleExceptions(async () => await task);
            return await task;
        }

        private MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Synchronous,
            AsyncAction,
            AsyncFunction
        }
    }
}
