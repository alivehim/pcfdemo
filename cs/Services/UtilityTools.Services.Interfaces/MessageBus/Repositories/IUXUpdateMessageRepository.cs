using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;

namespace UtilityTools.Services.Interfaces.MessageBus
{
    public interface IUXUpdateMessageRepository
    {
        IDisposable Subscribe(Action<IUXMessage> func);
    }
}
