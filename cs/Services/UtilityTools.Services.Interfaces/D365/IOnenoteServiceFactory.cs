using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IOnenoteServiceFactory
    {
        IGraphOnenoteService GetHandler(OnenoteSource symbol);
    }
}
