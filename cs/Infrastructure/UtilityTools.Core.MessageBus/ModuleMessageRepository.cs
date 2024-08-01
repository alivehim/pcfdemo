using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.MessageBus
{
    public class ModuleMessageRepository : IModuleMessageRepository
    {
        private IObservable<IExtractResult<BaseResourceMetadata>> observable;
        private readonly IMessageStreamProvider<IExtractResult<BaseResourceMetadata>> messageStreamProvider;
        //public IObservable<IEnumerable<ModuleMessage>> GetTickerStream()
        //{
        //    return Observable.Defer(() => moduleMessageStream.GetModuleMessageStream())
        //        .Select(trades => trades.Select(tickerFactory.Create))
        //        .Catch(Observable.Return(new Ticker[0]))
        //        .Repeat()
        //        .Publish()
        //        .RefCount();
        //}
    
        public ModuleMessageRepository(IMessageStreamProvider<IExtractResult<BaseResourceMetadata>> messageStreamProvider)
        {
            this.messageStreamProvider = messageStreamProvider;
            observable = GetModuleMessageStream();
        }

        private IObservable<IExtractResult<BaseResourceMetadata>> GetModuleMessageStream()
        {
            return Observable.Create<IExtractResult<BaseResourceMetadata>>(observer =>
            {
                var disposable = messageStreamProvider.GetTaskStream().Subscribe(observer);
                return new CompositeDisposable { disposable };
            })
            .Publish()
            .RefCount();
        }

        public IDisposable Subscribe(Action<IExtractResult<BaseResourceMetadata>> func)
        {
            return observable.ObserveOn(DispatcherScheduler.Current)
                                             .SubscribeOn(ThreadPoolScheduler.Instance)
                                             .Subscribe(
                                                 func,
                                                 ex => { });
        }
    }
}
