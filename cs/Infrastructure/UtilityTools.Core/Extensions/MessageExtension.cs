using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Infrastructure;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.Extensions
{
    public static class MessageExtension
    {
        public static void Info(this IMessage data, string message)
        {
            var messageStreamProvider = ToolsContext.Current.UnityContainer.ResolveService<IMessageStreamProvider<IUXMessage>>();
            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Information,
                Message = message,
                MessageOwner = data.MessageOwner,
                ID = data.ID
            });
        }


        public static void Error(this IMessage data, string message)
        {
            var messageStreamProvider = ToolsContext.Current.UnityContainer.ResolveService<IMessageStreamProvider<IUXMessage>>();
            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Error,
                Message = message,
                MessageOwner = data.MessageOwner,
                ID = data.ID
            });
        }
    }
}
