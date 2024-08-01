using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DataDescriptor;
using UtilityTools.Core.Models.Magnet;
using UtilityTools.Services.Interfaces.Core;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.DataExtractDescriptors.MediaGet
{
    public class CilimePageDescriptor : BaseMediaPageDescription, IMediaPageDescriptor<MediaDataDescription>
    {
        public string ShortIcon => "website.png";

        private readonly IMagnetServieFactory magnetServieFactory;

        public CilimePageDescriptor(IMagnetServieFactory magnetServieFactory)
        {
            this.magnetServieFactory = magnetServieFactory;
        }

        public override string GetNextAddress(string address, out int page)
        {
            throw new NotImplementedException();
        }

        public override string GetProceedingAddress(string address, out int page)
        {
            throw new NotImplementedException();
        }

        protected override async Task<IExtractResult<MediaDataDescription>> ProcessAsync()
        {

            var handler = magnetServieFactory.GetHandler(MagnetSearchSource.Cilime);

            var result = await handler.GetMatchLinksByAddressAsync(MediaGetContext.Key);
            var data = new List<MediaDataDescription>();

            var index = 1;
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
                        MediaType = MediaSymbolType.Cilime,
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
                        MediaType = MediaSymbolType.Cilime,
                        RawSize = item.RawSize,
                        Size = item.Size,
                    });
                    index++;
                }
            }


            return Result(data);

        }
    }
}
