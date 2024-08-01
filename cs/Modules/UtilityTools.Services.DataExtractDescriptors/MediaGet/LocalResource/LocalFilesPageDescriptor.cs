using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core;
using UtilityTools.Core.Events;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class LocalFilesPageDescriptor : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        public int Order => 1000;

        const long Megabytes = 1048576;

        private readonly IEventAggregator eventAggregator;
        public LocalFilesPageDescriptor(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
        }

        public string ShortIcon => "website.png";

        public override string GetNextAddress(string address, out int page)
        {
            page = 0;
            return address;
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            page = 0;
            return address;
        }

        protected override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<FileDataDescriptor>();

            DirectoryInfo directoryInfo = new DirectoryInfo(MediaGetContext.Key);


            if (!Settings.Current.IsSearchFolder)
            {
                
                var allowedExtensions = new[] { ".avi", ".mp4", ".rar", ".zip", ".txt", ".mp3", ".wav",".pdf",".wmv",".TS",".ts" };
                var list = directoryInfo
                    .GetFiles(!string.IsNullOrEmpty(MediaGetContext.ExtendKey) ? $"*{MediaGetContext.ExtendKey}*" : "*", SearchOption.TopDirectoryOnly)
                    .Where(p => allowedExtensions.Contains(p.Extension)).Where(p=>p.Length > (!string.IsNullOrEmpty(MediaGetContext.ExtendKey2)? long.Parse(MediaGetContext.ExtendKey2)*Megabytes:0 ) )
                    .ToList();

                //filter by file name

                if (MediaGetContext.Order == FileOrder.Random)
                {
                    //随机10个

                    int[] randomArray = null;
                    if (list.Count > Settings.Current.RandomCount)
                    {
                        randomArray = Random(list.Count, Settings.Current.RandomCount);
                    }
                    else
                    {
                        randomArray = new int[list.Count];
                        for (int i = 0; i < list.Count; i++)
                        {
                            randomArray[i] = i;
                        }
                    }


                    foreach (var item in randomArray)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = list[item].FullName,
                            FileName = list[item].Name,
                            Order = item,
                            Size = FileHelper.CountSize(list[item].Length)
                        };

                        data.Add(vitem);
                    }
                }
                else if (MediaGetContext.Order == FileOrder.ByFileSizeDesc)
                {
                    //按大小排序 
                    var fileList = list.OrderByDescending(p => p.Length).Skip(MediaGetContext.PageIndex * Settings.Current.RandomCount).Take(Settings.Current.RandomCount);

                    int nindex = 1;
                    foreach (var item in fileList)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = item.FullName,
                            FileName = item.Name,
                            Order = nindex,
                            Size = FileHelper.CountSize(item.Length)
                        };

                        data.Add(vitem);
                        nindex++;
                    }
                }
                else if (MediaGetContext.Order == FileOrder.ByFileSizeAsc)
                {
                    //按大小排序 
                    var fileList = list.OrderBy(p => p.Length).Skip(MediaGetContext.PageIndex * Settings.Current.RandomCount).Take(Settings.Current.RandomCount);

                    int nindex = 1;
                    foreach (var item in fileList)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = item.FullName,
                            FileName = item.Name,
                            Order = nindex,
                            Size = FileHelper.CountSize(item.Length)
                        };

                        data.Add(vitem);
                        nindex++;
                    }
                }
                else if (MediaGetContext.Order == FileOrder.ByCreateOnDesc)
                {
                    //按时间排序 
                    var fileList = list.OrderByDescending(p => p.CreationTime).Skip(MediaGetContext.PageIndex * 10).Take(10);

                    int nindex = 1;
                    foreach (var item in fileList)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = item.FullName,
                            FileName = item.Name,
                            Order = nindex,
                            Size = FileHelper.CountSize(item.Length)
                        };

                        data.Add(vitem);
                        nindex++;
                    }
                }
                else if (MediaGetContext.Order == FileOrder.ByCreateOnAsc)
                {
                    //按时间排序 
                    var fileList = list.OrderBy(p => p.CreationTime).Skip(MediaGetContext.PageIndex * 10).Take(10);

                    int nindex = 1;
                    foreach (var item in fileList)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = item.FullName,
                            FileName = item.Name,
                            Order = nindex,
                            Size = FileHelper.CountSize(item.Length)
                        };

                        data.Add(vitem);
                        nindex++;
                    }
                }
                else if (MediaGetContext.Order == FileOrder.ByFileNameAsc)
                {
                    //按时间排序 
                    var fileList = list.OrderBy(p => p.Name).Skip(MediaGetContext.PageIndex * 10).Take(10);

                    int nindex = 1;
                    foreach (var item in fileList)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = item.FullName,
                            FileName = item.Name,
                            Order = nindex,
                            Size = FileHelper.CountSize(item.Length)
                        };

                        data.Add(vitem);
                        nindex++;
                    }
                }
                else if (MediaGetContext.Order == FileOrder.ByFileNameDesc)
                {
                    //按时间排序 
                    var fileList = list.OrderByDescending(p => p.Name).Skip(MediaGetContext.PageIndex * 10).Take(10);

                    int nindex = 1;
                    foreach (var item in fileList)
                    {
                        var vitem = new FileDataDescriptor
                        {
                            FullName = item.FullName,
                            FileName = item.Name,
                            Order = nindex,
                            Size = FileHelper.CountSize(item.Length)
                        };

                        data.Add(vitem);
                        nindex++;
                    }
                }

                if (MediaGetContext.PageIndex == 0)
                {
                    eventAggregator.GetEvent<FileSearchEvent>().Publish(list.Count);
                }

                return Task.FromResult(Result(data, MediaSymbolType.File, MediaGetContext.PageIndex));
            }
            else
            {
                var folderList = directoryInfo
                                   .GetDirectories(!string.IsNullOrEmpty(MediaGetContext.ExtendKey) ? $"*{MediaGetContext.ExtendKey}*" : "*", SearchOption.TopDirectoryOnly)
                                   .ToList();


                int nindex = 1;
                foreach (var item in folderList)
                {
                    var size = FileHelper.GetFolderSize(item);
                    var vitem = new FileDataDescriptor
                    {
                        FullName = item.FullName,
                        FileName = item.Name,
                        Order = nindex,
                        Size = FileHelper.CountSize(size),
                        RawSize = size
                    };

                    data.Add(vitem);
                    nindex++;
                }

                if (MediaGetContext.Order == FileOrder.ByFileSizeAsc)
                {
                    return Task.FromResult(Result(data.OrderBy(p => p.Size).Skip(MediaGetContext.PageIndex * Settings.Current.RandomCount).Take(Settings.Current.RandomCount), MediaSymbolType.Folder, MediaGetContext.PageIndex));
                }
                else
                {
                    return Task.FromResult(Result(data.OrderByDescending(p => p.Size).Skip(MediaGetContext.PageIndex * Settings.Current.RandomCount).Take(Settings.Current.RandomCount), MediaSymbolType.Folder, MediaGetContext.PageIndex));
                }
            }

        }

        private int[] Random(int maxValue, int length)
        {
            IList<int> result = new List<int>();

            while (result.Count < length)
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int iSeed = BitConverter.ToInt32(buffer, 0);

                Random t = new Random(iSeed);

                var val = t.Next(1, maxValue);
                if (!result.Contains(val))
                {
                    result.Add(val);
                }
            }

            return result.ToArray();


        }
    }
}
