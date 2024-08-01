using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Core.Could.Download;

namespace UtilityTools.Engine.Download
{
    public class DefaultDownloadProvider: IDownloadProvider
    {
        public IRPCDownloader[] NormalDownloads => new DefaultRPCDownloader[] {
                new DefaultRPCDownloader("tcp://127.0.0.1:13770"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13771"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13772"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13773"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13774"),
            };

        public IRPCDownloader[] SpecialDownloads => new DefaultRPCDownloader[] {


                new DefaultRPCDownloader("tcp://127.0.0.1:13780"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13781"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13782"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13783"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13784"),

                new DefaultRPCDownloader("tcp://127.0.0.1:13785"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13786"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13787"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13788"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13789"),

                new DefaultRPCDownloader("tcp://127.0.0.1:13790"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13791"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13792"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13793"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13794"),

                new DefaultRPCDownloader("tcp://127.0.0.1:13795"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13796"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13797"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13798"),
                new DefaultRPCDownloader("tcp://127.0.0.1:13799"),

            };
    }
}
