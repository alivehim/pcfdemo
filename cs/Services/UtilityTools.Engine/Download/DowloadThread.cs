using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Engine.Download
{
    public class DowloadThread
    {
        public static TResult RunTry<TResult, T>(Func<TResult> action, T item, Action<T, Exception, int> failureAction)
        {
            var retryTwoTimesPolicy =
                   Policy
                 .Handle<Exception>()
                 .Retry(3, (ex, count) =>
                 {
                     failureAction(item, ex, count);
                 });
            return retryTwoTimesPolicy.Execute<TResult>(() =>
            {
                return action();
            });

        }
    }
}
