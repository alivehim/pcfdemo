using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.CloudService;

namespace UtilityTools.Services.CloudService
{
    public class CloudResourceService: ICloudResourceService
    {
        public async Task<string> GetMeidaUrlAasync(string url)
        {
            var html = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);

            if (!string.IsNullOrEmpty(html))
            {
                var mc = Regex.Match(html, @"<source src=""(?<key>.*?)"" type=""video/mp4""");

                if (mc.Success)
                {
                    return mc.Groups["key"].Value.ToString();
                }
            }

            return "";
        }
    }
}
