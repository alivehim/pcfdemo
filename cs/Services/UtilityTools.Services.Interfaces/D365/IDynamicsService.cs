using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IDynamicsService
    {
        Task<string> FetchXml( string xmlContent);

        Task<string> FetchOdata(string subUrl);
    }
}
