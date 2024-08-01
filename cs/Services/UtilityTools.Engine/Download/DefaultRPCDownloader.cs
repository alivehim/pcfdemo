using Burrow.RPC;
using Microsoft.Xaml.Behaviors.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Engine.Download
{
    public class DefaultRPCDownloader : IRPCDownloader
    {

        private string address;


        public string Address => address;

        private IRPCDownloader rpcClient;
        public DefaultRPCDownloader(string address)
        {
            this.address = address;
            rpcClient = RpcFactory.CreateClient<IRPCDownloader>(address);
        }

        public bool DownloadFile(string urlString, string fileName)
        {
            return rpcClient.DownloadFile(urlString, fileName);
        }
    }
}
