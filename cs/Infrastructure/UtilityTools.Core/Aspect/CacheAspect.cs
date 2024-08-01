using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;

namespace UtilityTools.Core.Aspect
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CacheAspect : Aspect
    {
        public CacheAspect(string key)
        {
            Key = key;
        }

        public CacheAspect()
        {
        }

        public string Key { get; set; }

        private string GetKey(AspectArgs args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Key);

            if (args.Arguments.Length >= 1)
            {
                sb.Append($"_{args.Arguments[0]}");
            }

            if (args.Arguments.Length >= 2)
            {
                sb.Append($"_{args.Arguments[1]}");
            }

            return sb.ToString();
        }

        public virtual TResult GetCache<TResult>(AspectArgs args)
        {
            if (MemoryCacheManager.Exists(GetKey(args)))
            {
                return MemoryCacheManager.Get<TResult>(GetKey(args));
            }

            return default(TResult);
        }

        public virtual bool Exists()
        {
            return MemoryCacheManager.Exists(Key);
        }

        public virtual void SaveCache<TResult>(AspectArgs args,TResult data)
        {
            if (!MemoryCacheManager.Exists(GetKey(args)))
            {
                MemoryCacheManager.Add<TResult>(GetKey(args), data);
            }
        }
    }
}
