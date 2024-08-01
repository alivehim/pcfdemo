using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Data;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Data;

namespace UtilityTools.Services.Core
{
    public class CiliMagnetService : IMagnetService
    {
        private string XPATH2 = "/html/body/div/table/tbody/tr";
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public CiliMagnetService(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
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

        public string GetAddressByKey(string key)
        {
            var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Cili.ToString());
            var address = $@"{symbol.Address}/search?q={key}";
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
            var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Cili.ToString());
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

                /*
                 * 
                 * <article class="item" data-key="7829307"><div style="overflow: hidden;"><a href="/hash/b26368322f9c3cc7a9cd07c33fc915ff691f3f75.html" target="_blank"><h4><span class="label label-success">mp4</span>&nbsp;[Tiny4k] Miley Cole - Duel In The Pool (19.10.2017) rq.mp4</h4></a><p>Hot：233&nbsp;&nbsp;Size：594.58 MB&nbsp;&nbsp;Created：2017-10-20 20:34:26&nbsp;&nbsp;File Count：未知</p><p></p></div></article>
                 * 
                 */
                foreach (HtmlNode categoryNode in categoryNodeList)
                {
                    var node = categoryNode.SelectSingleNode(".//td/a");
                    var url = node.GetAttributeValue("href", string.Empty);

                    var textnode = node.SelectSingleNode(".//p");
                    var filename = textnode.InnerText;

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
    }
}
