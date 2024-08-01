using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Core.Models.TaskSchedule
{
    public class TaskContext : ITaskContext
    {

        public TaskContext(CancellationTokenSource cancelToken, MessageOwner messageOwner,string moduleId)
        {
            CancelationToken = cancelToken;
            MessageOwner = messageOwner;
            ModuleId = moduleId;
        }

        public string ModuleId { get; set; }
        public string Key { get; set; }
        public string ExtendKey { get; set; }
        public string ExtendFilter { get; set; }
        public IDataDescriptor DataDescriptor { get; set; }
        public CancellationTokenSource CancelationToken { get; }

        public PageBearing PageBearing { get; set; } = PageBearing.None; 

        public MessageOwner MessageOwner { get; }
        public int PageIndex { get; set; }

        public FileOrder Order { get; set; }
    }
}
