using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UtilityTools.Core.Events;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Services.Data;
using UtilityTools.Services.Infrastructure;
using UtilityTools.Services.Interfaces.Data;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.MediaGet;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule.MediaGet;
using System.Collections;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public abstract class BaseMediaPageDescription
    {
        private IMediaGetContext mediaGetContext;

        public IMediaGetContext MediaGetContext
        {
            protected get
            {
                return mediaGetContext;
            }
            set
            {
                mediaGetContext = value;
            }
        }

        protected abstract Task<IExtractResult<MediaDataDescription>> ProcessAsync();

        public abstract string GetNextAddress(string address, out int page);
        public abstract string GetProceedingAddress(string address, out int page);

        public int CurrentPage { get; set; }
        public string GetStoragePath()
        {
            if (mediaGetContext == null)
                throw new Exception("mediaGetContext is null");

            var mediaSymbolService = ToolsContext.Current.UnityContainer.ResolveService<IMediaSymbolService>();
            return mediaSymbolService.GetStoragePath(mediaGetContext.Symbol);
        }

        public IExtractResult<MediaDataDescription> Result(IEnumerable<MediaDataDescription> data)
        {
            return new ExtractResult<MediaDataDescription>(data, mediaGetContext.Symbol)
            {
                MessageOwner = UtilityTools.Core.Models.MessageOwner.MediaGet,
                ModuleId = mediaGetContext.ModuleId,
                Page = CurrentPage
            };
        }

        public IExtractResult<MediaDataDescription> Result(IEnumerable<MediaDataDescription> data, MediaSymbolType mediaSymbolType)
        {
            return new ExtractResult<MediaDataDescription>(data, mediaSymbolType)
            {
                MessageOwner = UtilityTools.Core.Models.MessageOwner.MediaGet,
                ModuleId = mediaGetContext.ModuleId
            };
        }
        public IExtractResult<MediaDataDescription> Result(IEnumerable<MediaDataDescription> data, MediaSymbolType mediaSymbolType, int page)
        {
            return new ExtractResult<MediaDataDescription>(data, mediaSymbolType)
            {
                MessageOwner = UtilityTools.Core.Models.MessageOwner.MediaGet,
                ModuleId = mediaGetContext.ModuleId,
                Page = page
            };
        }

        public async Task<IExtractResult<MediaDataDescription>> RunAsync(IMediaGetContext taskContext)
        {
            MediaGetContext = taskContext;

            if (taskContext.PageBearing == PageBearing.Next)
            {
                var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();
                taskContext.Key = GetNextAddress(taskContext.Key, out int page);
                CurrentPage = page;
                eventAggregator.GetEvent<KeyChangeEvent>().Publish(new KeyChange
                {
                    Url = taskContext.Key,
                    ModuleId = taskContext.ModuleId
                });
            }
            else if (taskContext.PageBearing == PageBearing.Back)
            {
                var eventAggregator = ToolsContext.Current.UnityContainer.ResolveService<IEventAggregator>();
                taskContext.Key = GetProceedingAddress(taskContext.Key, out int page);
                CurrentPage = page;
                eventAggregator.GetEvent<KeyChangeEvent>().Publish(new KeyChange
                {
                    Url = taskContext.Key,
                    ModuleId = taskContext.ModuleId
                });
            }
            return await ProcessAsync();
        }

        public static string ContentReplace(string input)
        {
            input = Regex.Replace(input, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"-->", "", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"<!--.*", "", RegexOptions.IgnoreCase);

            input = Regex.Replace(input, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            input = Regex.Replace(input, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            input.Replace("<", "");
            input.Replace(">", "");
            input.Replace("\r\n", "");
            input.Replace("\\", "");
            //去两端空格，中间多余空格
            //input = Regex.Replace(input.Trim(), "\\s+", " ");
            return input;
        }

        protected async Task<string> GetHtmlSource(string url)
        {
            return await HttpHelper.GetUrlContentAsync(url, Encoding.UTF8);
        }

        /// <summary>
        /// start with /page/
        /// </summary>
        /// <param name="address"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        protected string GetNextAddressWithPage(string address, out int page)
        {

            int localpage = 0;
            var nextaddress = Regex.Replace(address, @"/page/(?<key>[\d]*)/", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"/page/{localpage + 1}/";
            });

            if (address == nextaddress)
            {
                nextaddress = $"{address}/page/2/";
            }

            page = localpage + 1;
            return nextaddress;
        }

        /// <summary>
        /// start with /page/
        /// </summary>
        /// <param name="address"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        protected string GetProceedingAddressWithPage(string address, out int page)
        {
            int localpage = 0;
            var result = Regex.Replace(address, @"/page/(?<key>[\d]*)/", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"/page/{localpage - 1}/";
            });

            page = localpage - 1;
            return result;
        }

        protected string GetNextAddressWithEqualSign(string address, out int page)
        {
            int localpage = 0;
            var result = Regex.Replace(address, @"page=(?<key>[\d]*)", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"page={localpage + 1}";
            });
            page = localpage + 1;
            return result;
        }

   
        /// <summary>
        /// start with /page/
        /// </summary>
        /// <param name="address"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        protected string GetProceedingAddressWithEqualSign(string address, out int page)
        {
            int localpage = 0;
            var result = Regex.Replace(address, @"/page/(?<key>[\d]*)/", (mc) =>
            {
                localpage = int.Parse(mc.Groups["key"].Value);
                return $"/page/{localpage - 1}/";
            });

            page = localpage - 1;
            return result;
        }
    }
}
