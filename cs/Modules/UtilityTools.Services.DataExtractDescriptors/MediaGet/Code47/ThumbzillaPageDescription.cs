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
using System.Net;
using System.Reflection.Metadata;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class ThumbzillaPageDescription : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IStreamAnalyzer
    {
        private string XPATH = @"//*[@class='phimage']";
        private const char ForwardSlash = '/';
        public string ShortIcon => "website.png";

        private readonly IMediaSymbolDBService mediaSymbolDBService;
        private readonly IEventAggregator eventAggregator;

        public ThumbzillaPageDescription(IMediaSymbolDBService mediaSymbolDBService, IEventAggregator eventAggregator)
        {
            this.mediaSymbolDBService = mediaSymbolDBService;
            this.eventAggregator = eventAggregator;
        }

        public override string GetNextAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
         }

        public override string GetProceedingAddress(string address, out int page)
        {
            return GetNextAddressWithEqualSign(address, out page);
        }

        protected async override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<MediaDataDescription>();
            string html = await HttpHelper.GetUrlContentAsync(MediaGetContext.Key, Encoding.GetEncoding("utf-8"));
            if (!string.IsNullOrEmpty(html))
            {

                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Thumbzilla.ToString());
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
                    var namenode = categoryNode.SelectSingleNode(@".//a/span[2]/span");
                    var name = HttpUtility.HtmlDecode(namenode.InnerText);
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
                        MediaType = MediaSymbolType.Thumbzilla
                    });

                    index++;

                }
            }

            return Result(data);
        }

        private class ThumbzilaVideoList
        {
            public IList<mediaDefinition> mediaDefinitions { get; set; }
        }

        [JsonConverter(typeof(QulityJsonDataConverter))]
        private class QulityData : List<int>
        {
            public QulityData() : base() { }
            public QulityData(IEnumerable<int> data) : base(data) { }
        }

        /// <summary>
        /// https://stackoverflow.com/questions/35978392/deserialize-json-as-object-or-array-with-json-net
        /// </summary>
        private class QulityJsonDataConverter : JsonConverter
        {
            public override bool CanWrite { get { return false; } }

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(QulityData);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var token = JToken.ReadFrom(reader);

                if (token is JArray)
                    return new QulityData(token.Select(t => t.Value<int>()));
                else
                {
                    return new QulityData(new int[] { token.Value<int>() });
                }
                throw new NotSupportedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
        private class mediaDefinition
        {
            public string videoUrl { get; set; }

            public QulityData quality { get; set; }
        }

        public async Task<string> GetStreamFile(IStreamUXItemDescription item)
        {

            try
            {
                var symbol = mediaSymbolDBService.GetMediaSymbol(MediaSymbolType.Thumbzilla.ToString());
                var source = await HttpHelper.GetUrlContentAsync(item.Url, Encoding.GetEncoding("utf-8"));

                if (!string.IsNullOrEmpty(source))
                {

                    //var mc = Regex.Match(source, @"<a title='(中文字幕|第01集|日语)' href='(?<key>.*?)' target=""_blank"">(中文字幕|第01集|日语)");
                    var mc = Regex.Match(source, @"'videoVars' :(?<key>[\w\W]*)'embedSWF'");
                    if (mc.Success)
                    {
                        var json = $"{mc.Groups["key"].Value}".Trim();
                        json = json.Remove(json.Length - 1);
                        var jsonobj = Newtonsoft.Json.JsonConvert.DeserializeObject<ThumbzilaVideoList>(json);
                        item.StreamUri = jsonobj.mediaDefinitions.FirstOrDefault()?.videoUrl;

                        GetM3u8Address(item, symbol);
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
            var content = HttpHelper.GetUrlContentAsync(item.StreamUri, Encoding.UTF8).GetAwaiter().GetResult();
            if (!string.IsNullOrEmpty(content))
            {
                Uri uri = new Uri(item.StreamUri);
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
        }
    }
}
