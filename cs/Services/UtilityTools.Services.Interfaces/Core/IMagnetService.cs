using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Services.Interfaces.Core
{
    public interface IMagnetService
    {

        /// <summary>
        /// return the searching url with key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetAddressByKey(string key);

        Task<string> GetMagnetLinkAsync(string url);

        //IList<MagnetDescription> GetLinksByFullLink(string address);

        Task<IList<MagnetDescription>> GetMatchLinksByKeyAsync(string key);
        Task<IList<MagnetDescription>> GetMatchLinksByAddressAsync(string address);

        //string GetPPVMagnetLink(string url);
    }
}
