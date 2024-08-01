using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Aspect;
using UtilityTools.Core.Extensions;
using UtilityTools.Services.Extensions;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Services.Aspects
{
    public class LogAspect : Aspect
    {
        public LogAspect(string logBody)
        {
            LogBody = logBody;
        }

        public string LogBody { get; set; }

        public override void OnEntry(AspectArgs args)
        {

            var messageprovider = args.ContainerProvider.ResolveService<IMessageStreamProvider<IUXMessage>>();
            Console.WriteLine("Log OnEntry");
            if (args.messagebody != null)
            {
                messageprovider.Info(args.messagebody, $"starting {LogBody}");
            }
            else
            {
                messageprovider.Info($"starting {LogBody}");
            }
        }

        public override void OnExit(AspectArgs args)
        {
            var messageprovider = args.ContainerProvider.ResolveService<IMessageStreamProvider<IUXMessage>>();
            if (args.messagebody != null)
            {
                messageprovider.Info(args.messagebody, $"ending {LogBody}");
            }
            else
            {
                messageprovider.Info($"ending {LogBody}");
            }
        }

        public override void OnException(AspectArgs args, Exception exception)
        {
            var messageprovider = args.ContainerProvider.ResolveService<IMessageStreamProvider<IUXMessage>>();
            if (args.messagebody != null)
            {
                messageprovider.Error(args.messagebody, exception.Message);
            }
            else
            {
                messageprovider.Info(exception.Message);
            }
        }

    }
}
