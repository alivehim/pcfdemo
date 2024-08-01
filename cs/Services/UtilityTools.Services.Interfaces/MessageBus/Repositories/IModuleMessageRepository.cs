using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.Interfaces.MessageBus
{
    public interface IModuleMessageRepository
    {
        IDisposable Subscribe(Action<IExtractResult<BaseResourceMetadata>> func);
    }
}
