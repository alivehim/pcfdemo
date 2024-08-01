using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Core.Could
{
    public interface IStreamFileDownloadEngine
    {
        Task Run(IStreamUXItemDescription item);
    }

    public interface ILivingStreamMediaDownloadEngine : IStreamFileDownloadEngine
    {

    }
}
