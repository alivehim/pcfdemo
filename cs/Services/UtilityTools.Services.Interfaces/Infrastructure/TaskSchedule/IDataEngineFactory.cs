using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule
{
    public interface IDataEngineFactory
    {
        ITaskEngine Create(ITaskContext context);
    }
}
