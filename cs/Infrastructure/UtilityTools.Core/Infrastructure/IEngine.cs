using Notifications.Wpf.Core;
using Prism.Events;
using Prism.Ioc;
using System.Collections.Generic;
using UtilityTools.Core.Models;

namespace UtilityTools.Core.Infrastructure
{
    public interface IEngine
    {
        IContainerProvider UnityContainer { get; }

        void ConfigureServices(IContainerProvider UnityContainer);

        void PublishEvent<TEventType>() where TEventType : PubSubEvent, new();

        void PublishEvent<TEventType, TPayload>(TPayload @event) where TEventType : PubSubEvent<TPayload>, new();

        IEventAggregator ResolveEventAggregator();

        T ResolveService<T>();

        void PostMessage(string message);
        void PostMessage(string message, MessageOwner messageOwner);

        void PostNotification(string message, NotificationType type = NotificationType.Information);
    }
}
