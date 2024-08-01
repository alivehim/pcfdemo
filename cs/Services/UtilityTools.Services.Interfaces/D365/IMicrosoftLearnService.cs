using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.DataDescriptor;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IMicrosoftLearnService
    {
        Task<List<MediaDataDescription>> GetChatpersAsync(string address);

        Task<string> GetChatperContentAsync(string address);
    }
}
