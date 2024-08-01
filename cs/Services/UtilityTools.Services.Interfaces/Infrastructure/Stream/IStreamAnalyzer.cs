using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure.Stream
{
    public interface IStreamAnalyzer
    {
        Task<string> GetStreamFile(IStreamUXItemDescription item);
    }
}
