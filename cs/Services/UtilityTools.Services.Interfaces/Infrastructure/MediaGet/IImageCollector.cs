using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.Infrastructure.MediaGet
{
    public interface IImageCollector
    {
         Task<IList<MediaDataDescription>> GetImagesAsync(string address);
    }
}
