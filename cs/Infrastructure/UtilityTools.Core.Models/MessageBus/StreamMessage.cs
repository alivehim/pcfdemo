using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.MessageBus
{
    public class StreamMessage
    {
        public string Id { get; set; }

        public MesasgeType MesasgeType { get; set; }
    }
}
