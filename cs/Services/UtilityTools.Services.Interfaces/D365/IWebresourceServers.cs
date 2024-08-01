using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.D365;

namespace UtilityTools.Services.Interfaces.D365
{
    public interface IWebresourceServers
    {
        Task<WebResourceMetaDefinition> GetWebresourcesAsync(string authorization);
        Task<string> UploadFileAsync(IMessage message, string id, string authorization, string filePath);
        Task<string> GetTokenAsync();

        Task<string> GetFlowTokenAsync();

        Task PublishAsync(IMessage message, string id, string authorization);

        Task<SolutionCollectoin> GetSolutionAsync(string authorization);
    }
}
