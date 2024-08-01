using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models
{
    public enum TaskStage
    {
        None = 0,
        Doing,
        Done,
        Error,

        Copy,
        Prepare,
        Prepared,

        Viewed


    }
}
