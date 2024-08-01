using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;

namespace UtilityTools.Engine
{
    public class FakeStreamFileDownloadEngine : IStreamFileDownloadEngine
    {
        public Task Run(IStreamUXItemDescription item)
        {
            throw new NotImplementedException();
        }
    }
}
