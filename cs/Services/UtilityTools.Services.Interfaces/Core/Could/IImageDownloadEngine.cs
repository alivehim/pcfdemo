using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Core.Could
{
    public interface IImageDownloadEngine
    {
        Task Run(IImageUXItemDescription item, CancellationToken token);
    }
}
