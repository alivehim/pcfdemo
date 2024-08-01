using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Utilites;
using UtilityTools.Engine.Extensions;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Core.Could.ChunkFile;
using UtilityTools.Services.Interfaces.Infrastructure;

namespace UtilityTools.Engine.ChunkFile
{
    public class ChunkFileProvider : IChunkFileProvider
    {
        private static readonly object lockobj = new object();

        private const char NewLineCharacter = '\n';
        private const char ForwardSlash = '/';
        private const string MetaDataLine = "#EXT";
        private IPathService pathService;
        private Uri _baseAddress;
        //private readonly MyWebClient _webClient;
        private IChunkFileNameProvider _chunkFileNameProvider;
        private Uri KeyUri
        {
            get
            {
                return new Uri(_baseAddress, "key.key");
            }
        }


        public ChunkFileProvider(IPathService pathService,
              IChunkFileNameProvider chunkFileNameProvider)
        {
            this.pathService = pathService;
            _chunkFileNameProvider = chunkFileNameProvider;
        }



        public async Task<Queue<string>> GetChunkFiles(IStreamUXItemDescription item, CancellationToken token)
        {

            var downloadTask = await GetHtmlSourceAsync(item.StreamUri, token);

            var htmlContent = downloadTask.Item1;
            var lastSlashLocation = downloadTask.Item2;
            var baseAddress = downloadTask.Item3;
            if (!string.IsNullOrEmpty(htmlContent))
            {
                _baseAddress = new Uri(baseAddress);

                string chunkFileList = htmlContent;
                string[] allFileLines = chunkFileList.Split(NewLineCharacter);

                if (chunkFileList.Contains("AES-128,URI"))
                {
                    item.IsNeedDecode = true;
                    //更改media文件名
                    //item.MediaFile = item.MediaFile.Replace(item.Name, item.ExtensionName);

                }

                //var lines = PreFilterAds(allFileLines.ToArray());
                SaveMediaListFile(item, allFileLines.ToArray());

                //progressData.PercentDone = 100;
                //progressIndicator.Report(progressData);
                return ProcessUnparsedFileIntoUrisX(item, allFileLines.Where(p => !p.Contains("v.youku22.com")));
            }

            return null;
        }

        private async Task<(string, int, string)> GetHtmlSourceAsync(string address, CancellationToken token)
        {
            //using (var _webClient = new MyWebClient())
            //{
            //    _webClient.Encoding = Encoding.UTF8;
            //    var downloadTask = _webClient.DownloadStringTaskAsync(address, token).GetAwaiter().GetResult();

            //    lastSlashLocation = _webClient.ResponseUri.ToString().LastIndexOf(ForwardSlash);
            //    baseAddress = _webClient.ResponseUri.ToString().Remove(lastSlashLocation + 1);

            //    return downloadTask;
            //}

            var content = await HttpHelper.GetUrlContentAsync(address, Encoding.UTF8);
            var lastSlashLocation = address.LastIndexOf(ForwardSlash);
            var baseAddress = address.Remove(lastSlashLocation + 1);
            return (content, lastSlashLocation, baseAddress);
        }


        private IList<string> PreFilterAds(string[] lines)
        {
            List<string> fileContent = new List<string>();

            bool skip = false;
            foreach (var line in lines)
            {
                if (line.Contains("#EXT-X-DISCONTINUITY"))
                {
                    skip = false;
                    continue;
                }

                if (line.Contains("Spc4SGeO") && !skip)
                {
                    skip = true;
                    continue;
                }

                if (line.Contains("#EXT-X-ENDLIST"))
                {
                    fileContent.Add(line);
                    continue;
                }

                if (skip)
                {
                    continue;
                }
                fileContent.Add(line);
            }

            return fileContent;
        }
        private void SaveMediaListFile(IStreamUXItemDescription item, string[] content)
        {

            //var parentdir = $"{pathService.TemporaryLocation}\\{item.ExtensionName}";
            //if (!Directory.Exists(parentdir))
            //{
            //    //Directory.CreateDirectory(parentdir);

            //    ////创建chunk文件夹
            //    //var info = Directory.CreateDirectory(parentdir);
            //    //var newtemp = info.CreateSubdirectory("chunk");
            //    //item.TempDirectory = newtemp.FullName;
            //}
            //else
            //{
            //    //删除原临时文件夹
            //    var oldDir = $"{pathService.TemporaryLocation}\\{item.Name}";
            //    if (parentdir != oldDir)
            //    {
            //        FileHelper.DelectDir(oldDir);
            //        Directory.Move(parentdir, oldDir);
            //    }

            //}
            string path = $"{pathService.TemporaryLocation}\\{item.Name}\\list.m3u8";
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            Uri originSrc = new Uri(item.StreamUri);
            int paddingLength = content.Count(i => !i.StartsWith(MetaDataLine)).ToString().Length;
            int index = 0;
            int fileIndex = 0;
            //int KeyFileIndex = 0;
            //int LastFileIndex = 0;
            StringBuilder nameBuilder = new StringBuilder();

            List<string> fileContent = new List<string>();

            foreach (var line in content)
            {
                if (line.Contains("URI"))
                {
                    //if (originSrc.Host.Contains("wart.war666.cn"))
                    //{
                    //    var newline = Regex.Replace(line, @"URI=""(?<key>.*?)""", (mc) => { return $@"URI=""http://wart.war666.cn{mc.Groups["key"].Value}"""; });
                    //    sw.WriteLine(newline);
                    //}
                    //else
                    {
                        Match mc = null;
                        if (line.Contains("URI ="))
                        {
                            mc = Regex.Match(line, @"URI =""(?<key>.*?)""");
                        }
                        else
                        {
                            mc = Regex.Match(line, @"URI=""(?<key>.*?)""");
                        }

                        var condition = mc.Groups["key"].Value;
                        if (condition.StartsWith("/"))
                        {
                            var keyArray = new string[] { "uJ27lprC", "Spc4SGeO", "hYdENn43", "PhBNVljs", "2H80RTQ9", "Acu66hAI", "mUiF9C9l" };
                            if (keyArray.Any(p=>condition.Contains(p)))
                            {
                                var newline = Regex.Replace(line, @"URI=""(?<key>.*?)""", $@"URI=""https://jkunbf.com{condition}""");
                                fileContent.Add(newline);
                            }
                            else
                            {
                                var newline = Regex.Replace(line, @"URI=""(?<key>.*?)""", $@"URI=""{KeyUri.ToString()}""");
                                fileContent.Add(newline);
                            }

                        }
                        else
                        {
                            if ((condition.Contains("http") || condition.Contains("https")) && condition.Contains("key.key"))
                            {
                                //var newline = line.Replace(@"key.key", KeyUri.ToString());
                                fileContent.Add(line);
                            }
                            else
                            {
                                var newline = line.Replace(@"key.key", KeyUri.ToString());
                                fileContent.Add(newline);
                            }

                        }


                    }

                    //if (KeyFileIndex != 0)
                    //{
                    //    // have duplicated file

                    //    LastFileIndex = index;
                    //}
                    //else
                    //{

                    //    KeyFileIndex = index;
                    //}
                    index++;

                }
                else if (line.Contains(".ts") || line.Contains(".js") || line.Contains(".undefined"))
                {
                    var name = _chunkFileNameProvider.GetIndividualChunkFileName(fileIndex, paddingLength, nameBuilder);
                    fileContent.Add($"{Settings.Current.M3u8LocationRoot}\\{item.Name}\\chunk\\{name}");
                    index++;
                    fileIndex++;
                }
                else
                {
                    fileContent.Add(line);
                    index++;
                }
            }

            //if (LastFileIndex != 0)
            //{
            //    fileContent.RemoveRange(KeyFileIndex, LastFileIndex - KeyFileIndex);
            //}

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
                {
                    foreach (var line in fileContent)
                    {
                        sw.WriteLine(line);
                    }
                    // Add some text to the file.
                    //foreach (var line in content)
                    //{
                    //    if (line.Contains("URI"))
                    //    {
                    //        //if (originSrc.Host.Contains("wart.war666.cn"))
                    //        //{
                    //        //    var newline = Regex.Replace(line, @"URI=""(?<key>.*?)""", (mc) => { return $@"URI=""http://wart.war666.cn{mc.Groups["key"].Value}"""; });
                    //        //    sw.WriteLine(newline);
                    //        //}
                    //        //else
                    //        {
                    //            Match mc = null;
                    //            if (line.Contains("URI ="))
                    //            {
                    //                mc = Regex.Match(line, @"URI =""(?<key>.*?)""");
                    //            }
                    //            else
                    //            {
                    //                mc = Regex.Match(line, @"URI=""(?<key>.*?)""");
                    //            }

                    //            var condition = mc.Groups["key"].Value;
                    //            if (condition.StartsWith("/"))
                    //            {
                    //                var newline = Regex.Replace(line, @"URI=""(?<key>.*?)""", $@"URI=""{KeyUri.ToString()}""");
                    //                sw.WriteLine(newline);
                    //            }
                    //            else
                    //            {
                    //                if ((condition.Contains("http") || condition.Contains("https")) && condition.Contains("key.key"))
                    //                {
                    //                    //var newline = line.Replace(@"key.key", KeyUri.ToString());
                    //                    sw.WriteLine(line);
                    //                }
                    //                else
                    //                {
                    //                    var newline = line.Replace(@"key.key", KeyUri.ToString());
                    //                    sw.WriteLine(newline);
                    //                }

                    //            }


                    //        }

                    //        if (KeyFileIndex != 0)
                    //        {
                    //            // have duplicated file

                    //            LastFileIndex = index-1;
                    //        }
                    //        else
                    //        {

                    //            KeyFileIndex = index;
                    //        }
                    //        index++;

                    //    }
                    //    else if (line.Contains(".ts") || line.Contains(".js") || line.Contains(".undefined"))
                    //    {
                    //        var name = _chunkFileNameProvider.GetIndividualChunkFileName(index, paddingLength, nameBuilder);
                    //        sw.WriteLine($"{Settings.Current.M3u8LocationRoot}\\{item.Name}\\chunk\\{name}");
                    //        index++;
                    //    }
                    //    else
                    //    {
                    //        sw.WriteLine(line);
                    //        index++;
                    //    }
                    //}
                }
            }
        }



        private Queue<Uri> ProcessUnparsedFileIntoUris(IStreamUXItemDescription item, IEnumerable<string> allFileLines)
        {
            var streamingFileLines = ParseStreamFiles(allFileLines);

            return GetStreamFileUris(streamingFileLines);
        }

        private Queue<string> ProcessUnparsedFileIntoUrisX(IStreamUXItemDescription item, IEnumerable<string> allFileLines)
        {
            var streamingFileLines = ParseStreamFiles(allFileLines);

            return GetStreamFileUrisX(streamingFileLines);
        }

        /// <summary>
        /// Gets the stream file uris.
        /// </summary>
        /// <param name="streamingFileLines">The streaming file lines.</param>
        /// <returns>The collection of Uri's parsed from the input lines.</returns>
        private Queue<Uri> GetStreamFileUris(IEnumerable<string> streamingFileLines)
        {
            Queue<Uri> orderedStreamFileUris = new Queue<Uri>();

            foreach (string streamingFileLine in streamingFileLines)
            {
                string trimmedLine = streamingFileLine.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }

                Uri combinedUri = new Uri(_baseAddress, trimmedLine);
                var url = $"{_baseAddress}{trimmedLine}";
                orderedStreamFileUris.Enqueue(combinedUri);
            }

            return orderedStreamFileUris;
        }

        private Queue<string> GetStreamFileUrisX(IEnumerable<string> streamingFileLines)
        {
            Queue<string> orderedStreamFileUris = new Queue<string>();

            foreach (string streamingFileLine in streamingFileLines)
            {
                string trimmedLine = streamingFileLine.Trim();

                if (string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }

                if (trimmedLine.StartsWith("http"))
                {
                    orderedStreamFileUris.Enqueue(trimmedLine);
                }
                else
                {
                    var url = string.Empty;
                    if (_baseAddress.ToString().StartsWith("https://cdn-shenma-iqiyi.com")
                        || _baseAddress.ToString().StartsWith("https://lbbf9.com"))
                    {
                        var prefix = $"{_baseAddress.Scheme}://{_baseAddress.Host}";
                        url = $"{prefix}{trimmedLine}";
                    }
                    else if (streamingFileLine.StartsWith("/"))
                    {
                        var prefix = $"{_baseAddress.Scheme}://{_baseAddress.Host}";
                        url = $"{prefix}{trimmedLine}";
                    }
                    else
                    {
                        if (_baseAddress.ToString().Contains("ppvod/"))
                        {
                            url = $"{_baseAddress.ToString().Replace("/ppvod/", "")}{trimmedLine}";
                        }
                        else if (_baseAddress.ToString().Contains("https://ev-h-ph.ypncdn.com/hls/"))
                        {
                            var source = _baseAddress.ToString();
                            int pos = source.IndexOf("urlset/");
                            if (pos != -1)
                            {

                                url = $"{source.Substring(0, pos + "urlset/".Length)}{trimmedLine}";
                            }
                            else
                            {

                                url = $"{_baseAddress}{trimmedLine}";
                            }
                        }
                        else
                        {
                            url = $"{_baseAddress}{trimmedLine}";
                        }
                    }
                    orderedStreamFileUris.Enqueue(url);
                }

            }

            return orderedStreamFileUris;
        }

        private IEnumerable<string> ParseStreamFiles(IEnumerable<string> unparsedLines)
        {
            return unparsedLines.Where(i => !i.StartsWith(MetaDataLine));
        }
    }
}
