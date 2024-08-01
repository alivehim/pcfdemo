using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.Data
{
    public interface IGitService
    {
        IList<string> GetChangedFiles(string dir);
    }
}
