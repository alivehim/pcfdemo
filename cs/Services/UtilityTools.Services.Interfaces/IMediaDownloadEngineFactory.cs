using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Engine;
using UtilityTools.Services.Interfaces.Core.Could;

namespace UtilityTools.Services.Interfaces
{
    public interface IMediaDownloadEngineFactory
    {
        IStreamFileDownloadEngine GetHandler(StreamDownloadProvider downloadProvider);
    }

    public interface ILivingMediaDownloadEngineFactory
    {
        ILivingStreamMediaDownloadEngine GetHandler(LivingStreamDownloadProvider downloadProvider);
    }
}
