using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.Interfaces.MessageBus
{
    public interface IExtractResult<out T> : IBaseMessageDescription where T : new()
    {
        int Count { get; }
        IEnumerable<T>  Collection{ get; }
    }
}
