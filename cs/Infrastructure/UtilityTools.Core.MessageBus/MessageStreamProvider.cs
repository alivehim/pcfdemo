using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.MessageBus
{
    public class MessageStreamProvider<T> : IMessageStreamProvider<T>, IDisposable
    {
        private CompositeDisposable disposables = new CompositeDisposable();

        private Subject<T> subject;

        public MessageStreamProvider()
        {
            subject = new Subject<T>();
        }
      
        public void Publisher(T message)
        {
            subject.OnNext(message);
        }

        public IObservable<T> GetTaskStream()
        {
            return subject.AsObservable();
        }

        public void Dispose()
        {
            this.disposables.Dispose();
        }
    }


}
