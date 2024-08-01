using HtmlAgilityPack;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Core.Utilites;
using UtilityTools.Data.Domain;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class XianPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IStreamAnalyzer
    {
        private string XPATH = @"//*[@class='primary-list min-video-list fn-clear']/li";
        private const char ForwardSlash = '/';
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public XianPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.eventAggregator = eventAggregator;
        }

        public override string GetNextAddress(string address, out int page)
        {
            if (address.Contains("page="))
            {

                return GetNextAddressWithEqualSign(address, out page);
            }

            int localpage = 0;
            var result = Regex.Replace(address, @"_(?<key>[\d]*).html", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"_{localpage + 1}.html";
            });
            page = localpage + 1;
            return result;
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            if (address.Contains("page="))
            {

                return GetProceedingAddressWithEqualSign(address, out page);
            }

            int localpage = 0;
            var result = Regex.Replace(address, @"_(?<key>[\d]*).html", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"_{localpage - 1}.html";
            });
            page = localpage + 1;
            return result;
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("gb2312"));
            if (!string.IsNullOrEmpty(html))
            {

                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Xian.ToString());
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                HtmlNode rootNode = document.DocumentNode;
                //title
                //sub images
                HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
                int index = 0;
                var path = GetStoragePath();

                foreach (HtmlNode categoryNode in categoryNodeList)
                {


                    var innerNode = categoryNode.SelectSingleNode(@".//a");
                    var url = $"{symbol.Address}{innerNode.GetAttributeValue("href", string.Empty)}";
                    //var name = innerNode.InnerText;
                    var imageNode = categoryNode.SelectSingleNode(@".//a/img");
                    var name = innerNode.GetAttributeValue("title", string.Empty);
                    var imageurl = imageNode.GetAttributeValue("src", string.Empty);

                    if (!imageurl.StartsWith("https"))
                    {
                        imageurl = "https" + imageurl;
                    }


                    var filename = FileHelper.GetValideName(name.Trim().Replace(" ", "-").Replace(".", "-"), false);

                    data.Add(new MediaDataDescription
                    {
                        Name = filename,
                        ImageUrl = imageurl,
                        Url = url,
                        Duration = string.Empty,
                        StoragePath = $"{path}\\{filename}.mp4",
                        MediaType = MediaSymbolType.Xian
                    });

                    index++;

                }
            }

            return Result(data);
        }

        public async Task<string> GetStreamFile(IStreamUXItemDescription item)
        {

            try
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Xian.ToString());
                var source = await HttpHelper.GetUrlContentAsync(item.Url, Encoding.GetEncoding("gb2312"));

                if (!string.IsNullOrEmpty(source))
                {

                    //var mc = Regex.Match(source, @"<a title='(中文字幕|第01集|日语)' href='(?<key>.*?)' target=""_blank"">(中文字幕|第01集|日语)");
                    var mc = Regex.Match(source, @"<a title='中文字幕' href='(?<key>.*?)' target=""_blank"">中文字幕");
                    if (mc.Success)
                    {
                        item.StreamUri = $"{symbol.Address}{mc.Groups["key"].Value}";

                        GetM3u8Address(item, symbol);
                    }
                    else
                    {
                        mc = Regex.Match(source, @"<a title='(第1集|第01集|日语|高清)' href='(?<key>.*?)' target=""_blank"">(第1集|第01集|日语|高清)");
                        if (mc.Success)
                        {
                            var nmc = mc.NextMatch();
                            if (nmc.Success)
                            {
                                item.StreamUri = $"{symbol.Address}{nmc.Groups["key"].Value}";

                                GetM3u8Address(item, symbol);
                            }
                            else
                            {
                                item.StreamUri = $"{symbol.Address}{mc.Groups["key"].Value}";

                                GetM3u8Address(item, symbol);
                            }

                        }
                        else
                        {
                            item.Info("get m3u8 file failed");
                        }
                    }
                }
                return item.StreamUri;
            }
            catch (Exception ex)
            {
                item.TaskStage = TaskStage.Error;
                item.Error(ex.ToString());
                return string.Empty;
            }
        }

        private void GetM3u8Address(IStreamUXItemDescription item, MediaSymbol mediaSymbol)
        {
            var source = HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.UTF8).GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(source))
            {
                var mc = Regex.Match(source, @"<div class=""playing""><script type=""text/javascript"" src=""(?<key>.*?)"">");
                if (mc.Success)
                {
                    item.StreamUri = $"{mediaSymbol.Address}{mc.Groups["key"].Value}";
                    source = HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.GetEncoding("gb2312")).GetAwaiter().GetResult();
                    if (!string.IsNullOrWhiteSpace(source))
                    {
                        source = HttpHelper.UnicodeToString(source);
                        if (source.Contains("中文字幕"))
                        {
                            mc = Regex.Match(source, @"中文字幕\$http(?<key>.*?)index.m3u8");
                        }
                        else
                        {
                            mc = Regex.Match(source, @"http(?<key>.*?)index.m3u8");
                        }


                        if (mc.Success)
                        {

                            if (mc.ToString().Contains("中文字幕"))
                            {
                                item.StreamUri = mc.ToString().Replace("中文字幕$", "");
                                Uri uri = new Uri(item.StreamUri);
                                var content = HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.GetEncoding("gb2312")).GetAwaiter().GetResult();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    if (!content.Contains("#EXT-X-VERSION"))
                                    {
                                        var allFileLines = content.Split('\n').Where(p => !string.IsNullOrEmpty(p)).ToArray();
                                        var m3u8 = allFileLines[allFileLines.Length - 1];

                                        if (m3u8.StartsWith("/"))
                                        {
                                            var host = $"{uri.Scheme}://{uri.Host}";
                                            item.StreamUri = host + m3u8;
                                            item.StreamUri = item.StreamUri.Replace("https", "http");
                                        }
                                        else
                                        {
                                            int lastSlashLocation = item.StreamUri.LastIndexOf(ForwardSlash);
                                            string baseAddress = item.StreamUri.Remove(lastSlashLocation + 1);
                                            item.StreamUri = baseAddress + m3u8;

                                        }
                                    }

                                    item.TaskStage = TaskStage.Prepared;

                                }
                            }
                            else
                            {


                                //if (mc.ToString().Contains("3sybf.com")
                                //    || mc.ToString().Contains("vod.hjbfq.com")
                                //    || mc.ToString().Contains("vip5.3sybf.com")
                                //    || mc.ToString().Contains("vip1.fhbf9.com")
                                //    || mc.ToString().Contains("cdn2.lajiao-bo.com")
                                //    || mc.ToString().Contains("lajiao-bo.com")
                                //    || mc.ToString().Contains("vip5.bobolj.com")
                                //    || mc.ToString().Contains("vod.hjbfq1.com")
                                //    || mc.ToString().Contains("lajiao-bo.com")
                                //    || mc.ToString().Contains("www.fhbf9.com")
                                //    || mc.ToString().Contains("bobolj.com")
                                //    || mc.ToString().Contains("play3.sewobofang.com")
                                //    || mc.ToString().Contains("sy4.3sybf.com"))
                                //{

                                var nmc = mc.NextMatch();

                                if (nmc.Success)
                                {
                                    item.StreamUri = nmc.ToString();
                                }
                                else
                                {

                                    item.StreamUri = mc.ToString();
                                }

                                Uri uri = new Uri(item.StreamUri);
                                var content = HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.GetEncoding("gb2312")).GetAwaiter().GetResult();
                                if (!string.IsNullOrEmpty(content))
                                {
                                    if (!content.Contains(".ts"))
                                    {
                                        var allFileLines = content.Split('\n').Where(p => !string.IsNullOrEmpty(p)).ToArray();

                                        var m3u8 = allFileLines[allFileLines.Length - 1];

                                        if (m3u8.StartsWith("/"))
                                        {
                                            var host = $"{uri.Scheme}://{uri.Host}";
                                            item.StreamUri = host + m3u8;
                                        }
                                        else
                                        {
                                            int lastSlashLocation = item.StreamUri.LastIndexOf(ForwardSlash);
                                            string baseAddress = item.StreamUri.Remove(lastSlashLocation + 1);
                                            item.StreamUri = baseAddress + m3u8;

                                        }
                                    }
                                    item.TaskStage = TaskStage.Prepared;

                                    item.Info("get m3u8 file successfully");
                                    eventAggregator.GetEvent<MediaStreamEvent>().Publish(new StreamMessage { MesasgeType = MesasgeType.StreamFileCompleted, Id = item.ID });

                                    //item.Prepared();
                                }
                                //}
                                //else if (mc.ToString().Contains("日语"))
                                //{
                                //    item.StreamUri = mc.ToString();

                                //    Uri uri = new Uri(item.StreamUri);
                                //    var content = HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.GetEncoding("gb2312")).GetAwaiter().GetResult();
                                //    if (!string.IsNullOrEmpty(content))
                                //    {
                                //        var allFileLines = content.Split('\n').Where(p => !string.IsNullOrEmpty(p)).ToArray();
                                //        var m3u8 = allFileLines[allFileLines.Length - 1];

                                //        if (m3u8.StartsWith("/"))
                                //        {
                                //            var host = $"{uri.Scheme}://{uri.Host}";
                                //            item.StreamUri = host + m3u8;
                                //        }
                                //        else
                                //        {
                                //            int lastSlashLocation = item.StreamUri.LastIndexOf(ForwardSlash);
                                //            string baseAddress = item.StreamUri.Remove(lastSlashLocation + 1);
                                //            item.StreamUri = baseAddress + m3u8;

                                //        }

                                //        item.TaskStage = TaskStage.Prepared;

                                //        //item.Prepared();
                                //    }

                                //}

                                //else
                                //{
                                //    item.StreamUri = mc.ToString();
                                //    //item.StreamUri = item.StreamUri.Replace("https", "http");

                                //    item.TaskStage = TaskStage.Prepared;

                                //    //item.Prepared();
                                //}



                            }
                        }

                    }
                    else
                    {
                        item.TaskStage = TaskStage.None;
                    }
                }
            }
        }
    }
}
