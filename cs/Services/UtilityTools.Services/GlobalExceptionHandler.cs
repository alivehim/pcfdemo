using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces;

namespace UtilityTools.Services
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public Task HandleExceptions(Action t)
        {
            t.Invoke();

            return Task.CompletedTask;
        }

        //public Task<T> HandleExceptions<T>(Action t)
        //{
        //    t.Invoke();

        //    return Task.CompletedTask;
        //}
    }
}
