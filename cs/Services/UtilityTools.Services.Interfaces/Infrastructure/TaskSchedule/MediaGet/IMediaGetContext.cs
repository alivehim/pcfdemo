using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule.MediaGet
{
    public interface IMediaGetContext     {
        MediaSymbolType Symbol { get; set; }

        string Key { get; set; }

        string ExtendKey { get; set; }
        string ExtendKey2 { get; set; }
        PageBearing PageBearing { get; set; }

        IDataDescriptor DataDescriptor { get;  set; }

        CancellationTokenSource CancelationToken { get; }

        MessageOwner MessageOwner { get; }

        FileOrder Order { get; set; }

        int PageIndex { get; set; }

        string ModuleId { get;  set; }
    }
}
