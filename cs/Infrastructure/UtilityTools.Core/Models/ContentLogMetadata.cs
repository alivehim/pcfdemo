using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.Models
{
    public class ContentLogMetadata : BaseLogMetaData, IBaseLogMetaData, IMessage,IUXMessage
    {
        public string Message { get; set; }
    }
}
