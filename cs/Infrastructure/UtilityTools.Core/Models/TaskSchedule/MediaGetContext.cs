using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule.MediaGet;

namespace UtilityTools.Core.Models.TaskSchedule
{
    public class MediaGetContext: IMediaGetContext
    {
        public MediaGetContext(ITaskContext taskContext, MediaSymbolType symbolType)
        {
            Symbol = symbolType;
            Key = taskContext.Key;
            ExtendKey = taskContext.ExtendKey;
            Key = taskContext.Key;
            CancelationToken = taskContext.CancelationToken;
            MessageOwner = taskContext.MessageOwner;
            Key = taskContext.Key;
            PageBearing = taskContext.PageBearing;
            PageIndex = taskContext.PageIndex;
            Order = taskContext.Order;
            ModuleId = taskContext.ModuleId;
            ExtendKey2 = taskContext.ExtendFilter;
        }

        public MediaSymbolType Symbol { get;  set; }
        public string Key { get;  set; }
        public string ExtendKey { get;  set; }
        public string ExtendKey2 { get;  set; }
        public IDataDescriptor DataDescriptor { get;  set; }
        public string DataDescriptorTag { get;  set; }
        public CancellationTokenSource CancelationToken { get;  set; }
        public MessageOwner MessageOwner { get;  set; }
        public PageBearing PageBearing { get; set; }
        public FileOrder Order { get; set; }
        public int PageIndex { get; set; }

        public string ModuleId { get;  set; }
    }
}
