using Notifications.Wpf.Core;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using UtilityTools.Core.Extensions;
using UtilityTools.Core.Models;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.Infrastructure
{
    public class AppEngine : IEngine
    {
        private IContainerProvider unityContainer;

        public IContainerProvider UnityContainer => unityContainer;
        public void ConfigureServices(IContainerProvider UnityContainer)
        {
            this.unityContainer = UnityContainer;
        }


        public void PublishEvent<TEventType>() where TEventType : PubSubEvent, new()
        {
            var eventAggregator = UnityContainer.ResolveService<IEventAggregator>();
            eventAggregator.GetEvent<TEventType>().Publish();

        }

        public void PublishEvent<TEventType, TPayload>(TPayload @event) where TEventType : PubSubEvent<TPayload>, new()
        {
            var eventAggregator = UnityContainer.ResolveService<IEventAggregator>();
            eventAggregator.GetEvent<TEventType>().Publish(@event);

        }

        public IEventAggregator ResolveEventAggregator()
        {
            return UnityContainer.ResolveService<IEventAggregator>();
        }

        public T ResolveService<T>()
        {
            return UnityContainer.ResolveService<T>();
        }

        public void PostMessage(string message)
        {
            var messageprovider = UnityContainer.ResolveService<IMessageStreamProvider<IUXMessage>>();
            messageprovider.Info(message);
        }

        public void PostMessage(string message,MessageOwner messageOwner)
        {
            var messageprovider = UnityContainer.ResolveService<IMessageStreamProvider<IUXMessage>>();
            messageprovider.Info(message, messageOwner);
        }

        public void PostNotification(string message, NotificationType type= NotificationType.Information)
        {
            var notificationManager = new NotificationManager();

            var notificationContent = new NotificationContent
            {
                Title = "Notification",
                Message = message,
                Type = NotificationType.Information
            };

            notificationManager.ShowAsync(notificationContent);
        }
    }
}
