using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor
{

    /// <summary>
    /// https://github.com/dotnet/roslyn/issues/2981
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface ISimpleExtractDescriptor<out TResult> : IDataDescriptor where TResult : new()
    {
        //IExtractResult<T> Run(ITaskContext taskContext);
        IExtractResult<TResult> ExtractResult { get; }
        Task RunAsync(ITaskContext taskContext);
    }
}
