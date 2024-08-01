using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor
{
    public interface IMediaPageDescriptor<T> : IDataDescriptor where T : new()
    {
        string GetNextAddress(string address,out int page);
        string GetProceedingAddress(string address, out int page);
        //IExtractResult<T> Run(IMediaGetContext taskContext);

        Task<IExtractResult<T>> RunAsync(IMediaGetContext taskContext);
    }
}
