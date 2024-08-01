using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.Extensions
{
    public static class MessageBusExtensions
    {
        //public static void Info(this IMessageStreamProvider<IBaseLogMetaData> messageStreamProvider, string message)
        //{
        //    messageStreamProvider.Publisher(new ContentLogMetadata
        //    {
        //        ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Information,
        //        Message = message,
        //    });
        //}

        //public static void Info(this IMessageStreamProvider<IBaseLogMetaData> messageStreamProvider, IMessage data, string message)
        //{
        //    messageStreamProvider.Publisher(new ContentLogMetadata
        //    {
        //        ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Information,
        //        Message = message,
        //        MessageOwner = data.MessageOwner,
        //        ID = data.ID
        //    });
        //}

        //public static void Error(this IMessageStreamProvider<IBaseLogMetaData> messageStreamProvider, string message)
        //{
        //    messageStreamProvider.Publisher(new ContentLogMetadata
        //    {
        //        ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Error,
        //        Message = message,
        //    });
        //}

        //public static void Error(this IMessageStreamProvider<IBaseLogMetaData> messageStreamProvider, IMessage data, string message)
        //{
        //    messageStreamProvider.Publisher(new ContentLogMetadata
        //    {
        //        ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Error,
        //        Message = message,
        //        MessageOwner = data.MessageOwner,
        //        ID = data.ID
        //    });
        //}

        public static void Info(this IMessageStreamProvider<IUXMessage> messageStreamProvider, string message)
        {

            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Information,
                Message = message,
            });
        }

        public static void Info(this IMessageStreamProvider<IUXMessage> messageStreamProvider, string message,MessageOwner messageOwner)
        {

            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Information,
                Message = message,
                MessageOwner= messageOwner
            });
        }

        public static void Info(this IMessageStreamProvider<IUXMessage> messageStreamProvider, IMessage data, string message)
        {
            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Information,
                Message = message,
                MessageOwner = data.MessageOwner,
                ID = data.ID
            });
        }

        public static void Error(this IMessageStreamProvider<IUXMessage> messageStreamProvider, string message)
        {
            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Error,
                Message = message,
            });
        }

        public static void Error(this IMessageStreamProvider<IUXMessage> messageStreamProvider, IMessage data, string message)
        {
            messageStreamProvider.Publisher(new ContentLogMetadata
            {
                ErrorLevel = UtilityTools.Core.Models.ErrorListLevel.Error,
                Message = message,
                MessageOwner = data.MessageOwner,
                ID = data.ID
            });
        }

        public static void UpdateProgress(this IMessageStreamProvider<IUXMessage> messageStreamProvider, IMessage data, int totalCount, int currentCount)
        {
            messageStreamProvider.Publisher(new ProgressLogMetadata
            {
                MessageOwner = data.MessageOwner,
                ID = data.ID,
                CurrentCount = currentCount,
                TotalCount = totalCount

            });
        }

        public static void UpdateProgress(this IMessageStreamProvider<IUXMessage> messageStreamProvider, string Id, int totalCount, int currentCount)
        {
            messageStreamProvider.Publisher(new ProgressLogMetadata
            {
                ID = Id,
                CurrentCount = currentCount,
                TotalCount = totalCount

            });
        }
    }
}
