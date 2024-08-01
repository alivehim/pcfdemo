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
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class SokanPageDescriptor : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        public string ShortIcon => "website.png";

        private readonly IMagnetServieFactory magnetServieFactory;
        private readonly IMediaSymbolDBService mediaSymbolDBService;

        public SokanPageDescriptor(IMagnetServieFactory magnetServieFactory, IMediaSymbolDBService mediaSymbolDBService)
        {
            this.magnetServieFactory = magnetServieFactory;
            this.mediaSymbolDBService = mediaSymbolDBService;
        }

        public override string GetNextAddress(string address, out int page)
        {
            int localpage = 0;
            var nextAddress = Regex.Replace(address, @"page-(?<key>[\d]*).html", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"page-{localpage + 1}.html";
            });

            if (nextAddress == address)
            {
                //get search content
                //https://www.sokk14.one/search.html?name=口语

                var mc = Regex.Match(address, @"name=(?<key>.*?)$");
                if (mc.Success)
                {
                    var searchContent = mc.Groups["key"].Value.ToString();
                    var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Sokan.ToString());
                    nextAddress = symbol.Address + $"/search/{searchContent}/page-2.html";
                }
                page = 2;
                return nextAddress;
            }
            else
            {
                page = localpage + 1;
                return nextAddress;
            }

        }

        //private string escape(string s)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    byte[] ba = System.Text.Encoding.Unicode.GetBytes(s);
        //    for (int i = 0; i < ba.Length; i += 2)
        //    {    /**/
        //        sb.Append("%u");
        //        sb.Append(ba[i + 1].ToString("X2"));

        //        sb.Append(ba[i].ToString("X2"));
        //    }
        //    return sb.ToString();

        //}
        //public static string Escape(string str)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (char c in str)
        //    {
        //        sb.Append((Char.IsLetterOrDigit(c)
        //        || c == '-' || c == '_' || c == '\\'
        //        || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
        //    }
        //    return sb.ToString();
        //}

        public override string GetProceedingAddress(string address, out int page)
        {
            int localpage = 0;
            var nextAddress = Regex.Replace(address, @"page-(?<key>[\d]*).html", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"page-{localpage - 1}.html";
            });
            page = localpage - 1;
            return nextAddress;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var handler = magnetServieFactory.GetHandler(MagnetSearchSource.Sokankan);

            var result = await handler.GetMatchLinksByAddressAsync(MediaGetContext.Key);
            var data = new List<MediaDataDescription>();
            var index = 0;
            if (MediaGetContext.Order == FileOrder.ByFileSizeDesc)
            {
                foreach (var item in result.OrderByDescending(p => p.RawSize))
                {
                    var url = item.Address;
                    var name = item.FileName;

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        Url = url,
                        MediaType = MediaSymbolType.Sokan,
                        RawSize = item.RawSize,
                        Order = index,
                        Size = item.Size,
                    });
                    index++;
                }
            }
            else
            {
                foreach (var item in result.OrderBy(p => p.RawSize))
                {
                    var url = item.Address;
                    var name = item.FileName;

                    data.Add(new MediaDataDescription
                    {
                        Name = name,
                        Url = url,
                        MediaType = MediaSymbolType.Sokan,
                        RawSize = item.RawSize,
                        Order = index,
                        Size = item.Size,
                    });
                    index++;
                }
            }

            return Result(data);
        }
    }
}
