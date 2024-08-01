using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Services.Interfaces.Core
{
    public interface IJAVDBMagnetService
    {

        Task<(IList<MagnetDescription>, IList<string>)> GetMagnetLinksAsync(string address);
    }
}
