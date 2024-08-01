using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Services.Infrastructure;

namespace UtilityTools.Services.Extensions
{
    public static class ExtractResultExtensions
    {
        public static ExtractResult<T> Success<T>(this ExtractResult<T> result, IList<T> data) where T : new()
        {
            return result;
        }
    }
}
