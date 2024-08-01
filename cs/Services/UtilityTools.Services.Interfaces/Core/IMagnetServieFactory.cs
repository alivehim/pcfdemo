using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.Magnet;

namespace UtilityTools.Services.Interfaces.Core
{
    public interface IMagnetServieFactory
    {
        IMagnetService GetHandler(MagnetSearchSource symbol);
    }
}
