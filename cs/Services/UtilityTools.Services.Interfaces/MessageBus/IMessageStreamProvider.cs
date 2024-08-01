﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Services.Interfaces.MessageBus
{
    public interface IMessageStreamProvider<T>
    {
        void Publisher(T message);

        IObservable<T> GetTaskStream();
    }
}
