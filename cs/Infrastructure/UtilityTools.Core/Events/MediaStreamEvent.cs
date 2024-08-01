using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;

namespace UtilityTools.Core.Events
{
    public class MediaStreamEvent: PubSubEvent<StreamMessage>
    {
        
    }
}
