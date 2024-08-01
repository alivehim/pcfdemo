using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.MessageBus
{
    public class UXMessageRepository : IUXUpdateMessageRepository
    {
        private IObservable<IUXMessage> observable;
        private readonly IMessageStreamProvider<IUXMessage> messageStreamProvider;
        //public IObservable<IEnumerable<ModuleMessage>> GetTickerStream()
        //{
        //    return Observable.Defer(() => moduleMessageStream.GetModuleMessageStream())
        //        .Select(trades => trades.Select(tickerFactory.Create))
        //        .Catch(Observable.Return(new Ticker[0]))
        //        .Repeat()
        //        .Publish()
        //        .RefCount();
        //}

        public UXMessageRepository(IMessageStreamProvider<IUXMessage> messageStreamProvider)
        {
            this.messageStreamProvider = messageStreamProvider;
            observable = GetModuleMessageStream();
        }

        private IObservable<IUXMessage> GetModuleMessageStream()
        {
            return Observable.Create<IUXMessage>(observer =>
            {
                var disposable = messageStreamProvider.GetTaskStream().Subscribe(observer);
                return new CompositeDisposable { disposable };
            })
            .Publish()
            .RefCount();
        }

        public IDisposable Subscribe(Action<IUXMessage> func)
        {
            return observable.ObserveOn(DispatcherScheduler.Current)
                                             .SubscribeOn(ThreadPoolScheduler.Instance)
                                             .Subscribe(
                                                 func,
                                                 ex => { });
        }
    }
}
