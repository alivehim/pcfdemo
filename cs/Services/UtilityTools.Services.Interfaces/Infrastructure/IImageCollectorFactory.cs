using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;

namespace UtilityTools.Services.Interfaces.Infrastructure
{
    public interface IImageCollectorFactory
    {
        IImageCollector GetImageCollector(string key);
    }
}
