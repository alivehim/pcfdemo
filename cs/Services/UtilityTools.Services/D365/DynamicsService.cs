using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Aspects;
using UtilityTools.Services.Interfaces.D365;

namespace UtilityTools.Services.D365
{
    public class DynamicsService : IDynamicsService
    {
        [LogAspect("FetchXml")]
        public async Task<string> FetchXml( string xmlContent)
        {
            var tablename = string.Empty;
            var mc = Regex.Match(xmlContent, @"entity name=""(?<key>.*?)""");
            if (mc.Success)
            {
                tablename = $"{mc.Groups["key"].Value.ToString()}s";
            }

            string url = $"{Settings.Current.D365ResourceUrl}/api/data/v9.0/{tablename}?fetchXml=" + xmlContent;

            var content = await HttpHelper.GetDynamic365ContentAsync(url, Encoding.UTF8, Settings.Current.D365AccessToken);

            return content;
        }

        public async Task<string> FetchOdata(string subUrl)
        {
            
            string url = $"{Settings.Current.D365ResourceUrl}{subUrl}";

            var content = await HttpHelper.GetDynamic365ContentAsync(url, Encoding.UTF8, Settings.Current.D365AccessToken);

            return content;
        }

    }
}
