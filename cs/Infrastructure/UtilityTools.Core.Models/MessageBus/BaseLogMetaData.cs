using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.MessageBus
{
    public class BaseLogMetaData
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        public ErrorListLevel ErrorLevel { get; set; }
        public MessageOwner MessageOwner { get; set; }
    }
}
