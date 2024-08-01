using Burrow.RPC;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using UtilityTools.ObjectPool;

namespace zmqRPC
{
    public class RpcConnect : PooledObject
    {
        RequestSocket client;
        public RpcConnect(string connectionStringCommands)
        {
            client = new RequestSocket(connectionStringCommands);
            OnReleaseResources = () =>
            {
                client.Close();
                client.Dispose();
                // Called if the resource needs to be manually cleaned before the memory is reclaimed.
            };

            OnResetState = () =>
            {
                // Called if the resource needs resetting before it is getting back into the pool.
            };
        }

        public RpcResponse DownloadFile(RpcRequest request)
        {
            string jsonRequest = JsonConvert.SerializeObject(request);

            client.SendFrame(jsonRequest);

            string jsonResponse = client.ReceiveFrameString();

            return JSON.DeserializeResponse(request, jsonResponse);
        }
    }
}
