using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models
{
    public enum FileOrder
    {
        Random = 0,
        ByFileSizeDesc,
        ByFileSizeAsc,
        ByCreateOnDesc,
        ByCreateOnAsc,
        ByFileNameAsc,
        ByFileNameDesc,
    }
}
