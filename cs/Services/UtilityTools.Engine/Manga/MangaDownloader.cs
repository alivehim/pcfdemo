using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Mvvm;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Engine.Manga
{
    public class MangaDownloader : IMangaDownloader
    {
        private readonly string XPATH = @"//*[@id='select2']/option";
        private readonly string JPGXPATH = @"//*[@id='p1']";

        //private readonly string ROOTDIR = @"D:/manga";

        private readonly IMessageStreamProvider<IUXMessage> messageStreamProvider;

        public MangaDownloader(IMessageStreamProvider<IUXMessage> messageStreamProvider)
        {
            this.messageStreamProvider = messageStreamProvider;
        }

        private int GetMaxiumnPage(HtmlNode rootNode)
        {
            HtmlNodeCollection categoryNodeList = rootNode.SelectNodes(XPATH);
            var lastpage = categoryNodeList.Reverse().First();

            var lastPageIndex = lastpage.GetAttributeValue("value", 0) + 1;

            return lastPageIndex;
        }

        private string GetResourceLink(HtmlNode rootNode)
        {
            var jpgNode = rootNode.SelectSingleNode(JPGXPATH);

            var jpgfirstLink = jpgNode.GetAttributeValue("_src", string.Empty);

            // get all link for downling the resource

            //get the header of link

            int pox = jpgfirstLink.LastIndexOf('/');

            var headerurl = jpgfirstLink.Substring(0, pox + 1);

            return headerurl;
        }

        public class DownloadItem
        {
            public string DownloadUrl { get; set; }
            public string FilePath { get; set; }
        }

        public async Task<string> RunAsync(IMessage message, string Name, string url)
        {
            //get url content

            var html = await HttpHelper.GetUrlContentAsync(url, Encoding.GetEncoding("utf-8"));


            //analyze the maxiumn page index

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNode rootNode = document.DocumentNode;

            var lastPageIndex = GetMaxiumnPage(rootNode);
            //get the jpg link


            var headerurl = GetResourceLink(rootNode);


            var filePath = Path.Combine(Settings.Current.MangaFolder, Name);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var resoureList = new List<DownloadItem>();

            for (int i = 1; i <= lastPageIndex; i++)
            {
                resoureList.Add(new DownloadItem
                {
                    DownloadUrl = $"{headerurl}{i}.jpg",
                    FilePath = Path.Combine(filePath, $"{i.ToString().PadLeft(3,'0')}.jpg")
                });
            }

            //download picure in bulk
            await DownloadAsync(message, resoureList);


            //compress files


            string zipPath = Path.Combine(Settings.Current.MangaFolder, @$".\{Name}.zip");

            ZipFile.CreateFromDirectory(filePath, zipPath);

            //delete folder

            FileHelper.DelectDir(filePath);


            return zipPath;
        }

        private Task DownloadAsync(IMessage message, IList<DownloadItem> list)
        {
            return Task.Run(async () =>
            {
                var index = 1;
                foreach (var item in list)
                {
                    if (!File.Exists(item.FilePath))
                    {
                        await HttpHelper.DownloadAsync(item.DownloadUrl, item.FilePath);

                        messageStreamProvider.UpdateProgress(message, list.Count, index);
                    }
                   
                    index++;
                }

                messageStreamProvider.UpdateProgress(message, list.Count, list.Count);



            }).ContinueWith((res) => { });
        }
    }
}
