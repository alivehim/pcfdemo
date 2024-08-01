using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UtilityTools.Core.Attributes;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.TaskSchedule;
using UtilityTools.Services.Infrastructure;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.MessageBus;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.DataExtractDescriptors
{
    public abstract class BaseSimpleExtractDescriptor<T> : ISimpleExtractDescriptor<T> where T : new()
    {
        public string ShortIcon { get; }

        public IExtractResult<T> ExtractResult { get; set; }

        //public abstract IExtractResult<T> Run(ITaskContext taskContext);
        public abstract Task RunAsync(ITaskContext taskContext);

        public virtual IExtractResult<T> Result(IList<T> result)
        {
            var type = this.GetType();
            var ownerAttribute = type.GetCustomAttribute<MessageOwnerAttribute>();
            ExtractResult = new ExtractResult<T>(result)
            {
                MessageOwner = ownerAttribute != null ? ownerAttribute.MessageOwner : MessageOwner.None
            };

            return ExtractResult;
        }

        //Task ISimpleExtractDescriptor<T>.RunAsync(ITaskContext taskContext)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
