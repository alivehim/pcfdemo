using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor
{
    public interface IMultipleExtractDescriptor<out T> : IDataDescriptor where T : new()
    {
        
    }
}
