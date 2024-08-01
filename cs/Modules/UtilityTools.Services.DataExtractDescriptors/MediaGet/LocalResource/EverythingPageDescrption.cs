using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Utilites;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.Stream;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class EverythingPageDescrption : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>, IStreamAnalyzer
    {
        public string ShortIcon   => "website.png";

        public override string GetNextAddress(string address, out int page)
        {
            throw new NotImplementedException();
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetStreamFile(IStreamUXItemDescription item)
        {
            throw new NotImplementedException();
        }

        protected  override Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {
            var data = new List<FileDataDescriptor>();
            var list = EverythingHelper.Search(MediaGetContext.Key);

            if (list.Any(p => p.FullName.Contains(".mp4")))
            {
                var index = 0;
                foreach (var item in list.Take(50))
                {
                    data.Add(new FileDataDescriptor
                    {
                        Name = item.FileName,
                        FileName= item.FileName,
                        StoragePath = item.FullName,
                        MediaType = MediaSymbolType.LOCAL,
                        Order = index,
                    });
                    index++;
                }
            }

            return Task.FromResult( Result(data));
        }
    }
}
