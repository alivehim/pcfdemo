using System;
using System.Reflection;
using Castle.DynamicProxy;
using UtilityTools.Extensions;

namespace UtilityTools.DryIoc
{
    public class ProxyGenerationHook
        : IProxyGenerationHook
    {
        public void MethodsInspected()
        {
        }

        public void NonProxyableMemberNotification(
            Type type,
            MemberInfo memberInfo)
        {
        }

        public bool ShouldInterceptMethod(
            Type type,
            MethodInfo methodInfo)
        {
            return type.HasAspects() || methodInfo.HasAspects();
        }
    }
}
