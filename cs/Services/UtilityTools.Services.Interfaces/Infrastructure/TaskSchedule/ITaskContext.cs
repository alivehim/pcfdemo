using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule
{
    public interface ITaskContext
    {

        string Key { get; set; }

        string ExtendKey { get; set; }
        string ExtendFilter { get; set; }
        IDataDescriptor DataDescriptor { get; set; }

        CancellationTokenSource CancelationToken { get; }

        MessageOwner MessageOwner { get; }

        PageBearing PageBearing { get; set; }

        int PageIndex { get; set; }

        FileOrder Order { get; set; }

        string ModuleId { get;  set; }
    }
}
