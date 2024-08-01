using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.CloudService
{
    public interface ICloudResourceService
    {
        public  Task<string> GetMeidaUrlAasync(string url);
    }
}
