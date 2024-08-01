using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityTools.Core.Models.MessageBus;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.MessageBus
{
    public class LogMessageRepository : ILogMessageRepository
    {
        private IObservable<IBaseLogMetaData> observable;
        private readonly IMessageStreamProvider<IBaseLogMetaData> messageStreamProvider;
        //public IObservable<IEnumerable<ModuleMessage>> GetTickerStream()
        //{
        //    return Observable.Defer(() => moduleMessageStream.GetModuleMessageStream())
        //        .Select(trades => trades.Select(tickerFactory.Create))
        //        .Catch(Observable.Return(new Ticker[0]))
        //        .Repeat()
        //        .Publish()
        //        .RefCount();
        //}

        public LogMessageRepository(IMessageStreamProvider<IBaseLogMetaData> messageStreamProvider)
        {
            this.messageStreamProvider = messageStreamProvider;
            observable = GetModuleMessageStream();
        }

        private IObservable<IBaseLogMetaData> GetModuleMessageStream()
        {
            return Observable.Create<IBaseLogMetaData>(observer =>
            {
                var disposable = messageStreamProvider.GetTaskStream().Subscribe(observer);
                return new CompositeDisposable { disposable };
            })
            .Publish()
            .RefCount();
        }

        /// <summary>
        /// ObserveOn 指定Observable发送数据时使用的调度器（Scheduler），即数据接收的线程。
        /// SubscribeOn 指定Observable订阅时所在的调度器，即数据发送的线程。
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public IDisposable Subscribe(Action<IBaseLogMetaData> func)
        {

            return observable.ObserveOn(DispatcherScheduler.Current)
                                             .SubscribeOn(ThreadPoolScheduler.Instance)
                                             .Subscribe(
                                                 func,
                                                 ex => { });

        }
    }
}
