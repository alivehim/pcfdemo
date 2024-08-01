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
    public class SokankanMagnetService : IMagnetService
    {
        private string XPATH = "//*[@class='list-view']/article";

        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public SokankanMagnetService(IMediaSymbolDBService mediaSymbolDBService)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public async Task<string> GetMagnetLinkAsync(string url)
        {
            string html = await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);

            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                //title
                //sub images
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(@"//*[@class='media-body']");

                foreach (HtmlNode categoryNode in categoryNodeList)
                {
                    var node = categoryNode.SelectSingleNode(".//a");

                    var link = node.GetAttributeValue("href", string.Empty);
                    if (link.Contains("magnet"))
                    {
                        return link;
                    }

                }
            }

            return string.Empty;
        }


        public string GetAddressByKey(string key)
        {
            var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Sokan.ToString());
            var address = $@"{symbol.Address}/search.html?name={key}";
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
            var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Sokan.ToString());
            string html = await HttpHelper.GetUrlContentAsync(address, Encoding.UTF8);
            if (!string.IsNullOrEmpty(html))
            {
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                //title
                //sub images
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;

                /*
                 * 
                 * <article class="item" data-key="7829307"><div style="overflow: hidden;"><a href="/hash/b26368322f9c3cc7a9cd07c33fc915ff691f3f75.html" target="_blank"><h4><span class="label label-success">mp4</span>&nbsp;[Tiny4k] Miley Cole - Duel In The Pool (19.10.2017) rq.mp4</h4></a><p>Hot：233&nbsp;&nbsp;Size：594.58 MB&nbsp;&nbsp;Created：2017-10-20 20:34:26&nbsp;&nbsp;File Count：未知</p><p></p></div></article>
                 * 
                 */

                if (categoryNodeList != null)
                {
                    foreach (HtmlNode categoryNode in categoryNodeList)
                    {
                        var node = categoryNode.SelectSingleNode(".//div/a");
                        var url = node.GetAttributeValue("href", string.Empty);

                        var textnode = node.SelectSingleNode(".//h4");
                        var filename = textnode.InnerText;

                        if (filename.Contains("在线播放"))
                            continue;



                        var extent = HttpUtility.HtmlDecode(categoryNode.SelectSingleNode(".//div/p")?.InnerText);
                        double size = 0;
                        string rawSize = "";
                        if (extent.Contains("MB"))
                        {
                            var mc = Regex.Match(extent, @"(文件大小|Size)：(?<key>.*?) MB");

                            if (mc.Success)
                            {
                                size = double.Parse(mc.Groups["key"].Value);
                                rawSize = $"{size}MB";

                            }
                        }

                        else if (extent.Contains("GB"))
                        {
                            var mc = Regex.Match(extent, @"(文件大小|Size)：(?<key>.*?) GB");

                            if (mc.Success)
                            {
                                size = double.Parse(mc.Groups["key"].Value) * 1024;
                                rawSize = $"{mc.Groups["key"].Value}GB";
                            }

                        }

                        var name = HttpUtility.HtmlDecode(filename);
                        if (!list.Any(p => p.FileName == name && p.Size == rawSize))
                        {
                            list.Add(new MagnetDescription
                            {
                                Address = $"{symbol.Address}{url}",
                                Count = 0,
                                Size = rawSize,
                                FileName = name,
                                RawSize = (long)size
                            });
                            index++;
                        }
                    }
                }



            }

            return list;

        }

        //public IList<MagnetDescription> GetPPVLinks(string address)
        //{

        //    var list = new List<MagnetDescription>();

        //    string html = HttpHelper.GetUrlContentAsync(address, Encoding.UTF8).GetAwaiter().GetResult();
        //    if (!string.IsNullOrEmpty(html))
        //    {
        //        HtmlDocument document = new HtmlDocument();
        //        document.LoadHtml(html);
        //        HtmlNode rootNode = document.DocumentNode;
        //        //title
        //        //sub images
        //        HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH2);
        //        int index = 0;

        //        /*
        //         * 
        //         * <article class="item" data-key="7829307"><div style="overflow: hidden;"><a href="/hash/b26368322f9c3cc7a9cd07c33fc915ff691f3f75.html" target="_blank"><h4><span class="label label-success">mp4</span>&nbsp;[Tiny4k] Miley Cole - Duel In The Pool (19.10.2017) rq.mp4</h4></a><p>Hot：233&nbsp;&nbsp;Size：594.58 MB&nbsp;&nbsp;Created：2017-10-20 20:34:26&nbsp;&nbsp;File Count：未知</p><p></p></div></article>
        //         * 
        //         */
        //        foreach (HtmlNode categoryNode in categoryNodeList)
        //        {
        //            var node = categoryNode.SelectSingleNode(".//td/a");
        //            var url = node.GetAttributeValue("href", string.Empty);

        //            var textnode = node.SelectSingleNode(".//p");
        //            var filename = textnode.InnerText;

        //            var sizenode = categoryNode.SelectSingleNode(".//td[2]");

        //            var count = sizenode.InnerText;

        //            list.Add(new MagnetDescription
        //            {
        //                Address = $"{Settings.Current.CiliUrl}{url}",
        //                Count = 0,
        //                FileName = HttpUtility.HtmlDecode(filename),
        //                RawSize = count
        //            });
        //            index++;
        //        }


        //    }

        //    return list;

        //}
    }
}
