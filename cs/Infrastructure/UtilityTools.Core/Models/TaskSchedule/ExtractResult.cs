using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;
using UtilityTools.Core.Mvvm;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;

namespace UtilityTools.Core.Models.TaskSchedule
{
    public class ExtractResult<T> : BaseUXItemDescription, IExtractResult<T> where T : new()
    {
        public ExtractResult()
        {
        }

        public ExtractResult(IEnumerable<T> data)
        {
            list = data;
        }

        public ExtractResult(IEnumerable<T> data, MediaSymbolType mediaSymbolType)
        {
            list = data;
            Symbol = mediaSymbolType;
        }


        private IEnumerable<T> list = new List<T>() { };
        public int Count => Collection.Count();
        public IEnumerable<T> Collection => list.AsEnumerable();


        public MediaSymbolType Symbol { get; set; }

        public string ModuleId { get; set; }

        public int Page { get; set; }
    }
}
