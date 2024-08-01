using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;

namespace UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule
{
    public interface IStreamFileAnalyzerFactory
    {
        IStreamAnalyzer GetStreamAnalyzer(MediaSymbolType mediaSymbolType);
    }
}
