using Burrow.RPC;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Engine.Download
{
    public class RPCDownloader : IRPCDownloader
    {
        private IRPCDownloader rpcClient;
        public RPCDownloader()
        {
            rpcClient = RpcFactory.CreateClient<IRPCDownloader>(Settings.Current.RPCAddress);
        }

        public string Address => string.Empty;

        public bool DownloadFile(string urlString, string fileName)
        {
            return rpcClient.DownloadFile(urlString, fileName);
        }
    }
}
