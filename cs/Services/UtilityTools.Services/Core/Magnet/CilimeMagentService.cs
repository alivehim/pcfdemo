using HtmlAgilityPack;
using Microsoft.ClearScript.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Core.Magnet
{
    public class CilimeMagentService : IMagnetService
    {
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        private string XPATH2 = "//*[@class='table table-hover file-list']/tbody/tr";
        public CilimeMagentService(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public string GetAddressByKey(string key)
        {
            var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Cilime.ToString());
            var val = str2hex(key);
            var address = $@"{symbol.Address}/cilime/{val}/1/";
            //var address = $@"{Settings.Current.CiliUrl}/search?q={key}";
            return address;
        }

        public async Task<IList<MagnetDescription>> GetMatchLinksByKeyAsync(string key)
        {
            var address = GetAddressByKey(key);

            return await GetMatchLinksByAddressAsync(address);
        }

        public async Task<IList<MagnetDescription>> GetMatchLinksByAddressAsync(string address)
        {

            var list = new List<MagnetDescription>();
            var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Cilime.ToString());
            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.UTF8);
            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                //title
                //sub images
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH2);
                int index = 0;

                foreach (HtmlNode categoryNode in categoryNodeList)
                {
                    var node = categoryNode.SelectSingleNode(".//td/a");
                    var url = node.GetAttributeValue("href", string.Empty);

                    var filename = node.InnerText;

                    filename = Regex.Replace(filename, @"-(?<key>[\d]*)-原版高清无水印", (mc) =>
                    {
                        return "";
                    });

                    var sizenode = categoryNode.SelectSingleNode(".//td[2]");

                    var rawSzie = sizenode.InnerText;

                    double size = 0;
                    if (rawSzie.Contains("MB"))
                    {
                        var mc = Regex.Match(rawSzie, @"(?<key>.*?)MB");

                        if (mc.Success)
                        {
                            size = double.Parse(mc.Groups["key"].Value);
                        }

                    }
                    else if (rawSzie.Contains("GB"))
                    {
                        var mc = Regex.Match(rawSzie, @"(?<key>.*?)GB");

                        if (mc.Success)
                        {
                            size = double.Parse(mc.Groups["key"].Value) * 1024;
                        }

                    }
                    else if (rawSzie.Contains("TB"))
                    {
                        var mc = Regex.Match(rawSzie, @"(?<key>.*?)TB");

                        if (mc.Success)
                        {
                            size = double.Parse(mc.Groups["key"].Value) * 1024 * 1024;
                        }
                    }

                    list.Add(new MagnetDescription
                    {
                        Address = $"{symbol.Address}{url}",
                        Count = 0,
                        FileName = HttpUtility.HtmlDecode(filename),
                        RawSize = (long)size,
                        Size = rawSzie
                    });
                    index++;
                }
            }


            var grp = (from n in list
                       group n by n.FileName into g
                       select new
                       {
                           Key = g.Key,
                           max = g.Max(x => x.RawSize)
                       }).ToList();

            var result = from m in list
                         from y in grp
                         where m.FileName == y.Key && m.RawSize == y.max
                         select m;

            return result.ToList();
        }

        public async Task<string> GetMagnetLinkAsync(string url)
        {

            string html = await HttpHelper.GetUrlContentAsync2(url);

            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                //title
                //sub images

                var node = document.GetElementbyId("input-magnet");

                var magnet = node.Attributes["value"].Value;

                var mc = Regex.Match(magnet, @"magnet(?<key>.*?)&amp;");

                if (mc.Success)
                {
                    return mc.Value;
                }
            }

            return string.Empty;
        }

        private string str2hex(string key)
        {
            using (var engine = new V8ScriptEngine())
            {
                engine.DocumentSettings.AccessFlags = Microsoft.ClearScript.DocumentAccessFlags.EnableFileLoading;
                engine.DefaultAccess = Microsoft.ClearScript.ScriptAccess.Full; // 这两行是为了允许加载js文件
                                                                                // do something

                engine.Execute("function utf16to8(str) {\r\n    var out, i, len, c;\r\n    out = \"\";\r\n    len = str.length;\r\n    for (i = 0; i < len; i++) {\r\n        c = str.charCodeAt(i);\r\n        if ((c >= 0x0001) && (c <= 0x007F)) {\r\n            out += str.charAt(i);\r\n        } else if (c > 0x07FF) {\r\n            out += String.fromCharCode(0xE0 | ((c >> 12) & 0x0F));\r\n            out += String.fromCharCode(0x80 | ((c >> 6) & 0x3F));\r\n            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));\r\n        } else {\r\n            out += String.fromCharCode(0xC0 | ((c >> 6) & 0x1F));\r\n            out += String.fromCharCode(0x80 | ((c >> 0) & 0x3F));\r\n        }\r\n    }\r\n    return out;\r\n}\r\nfunction str2hex(str) {\r\n    var val = \"\";\r\n    var i = 0;\r\n    for (; i < str.length; i++) {\r\n        val += str.charCodeAt(i).toString(16);\r\n    }\r\n    return val;\r\n}");
                var result = engine.Script.utf16to8(key);

                var nr = engine.Script.str2hex(result);

                return nr;

            }
        }

    }
}
