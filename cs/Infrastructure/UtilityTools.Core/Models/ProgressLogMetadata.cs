using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.Models
{
    public class ProgressLogMetadata : BaseLogMetaData, IUXMessage
    {
        public int TotalCount { get; set; }

        public int CurrentCount { get; set; }
        public string Message { get; set; }
    }
}
