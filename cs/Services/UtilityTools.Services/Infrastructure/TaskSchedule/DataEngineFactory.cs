using System;
using UtilityTools.Services.Interfaces.Infrastructure.ExtractDescriptor;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.TaskSchedule
{
    public class DataEngineFactory : IDataEngineFactory
    {
        public ITaskEngine Create(ITaskContext context)
        {

            //var dataDescriptor = context.DataDescriptor;

            //if (dataDescriptor != null)
            //{
            //    var tp = dataDescriptor.GetType();
            //    if (Array.Exists(tp.GetInterfaces(), t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ISimpleExtractDescriptor<>)))
            //    {
            //        return new TaskEngine(dataDescriptor);
            //    }
            //    else if (Array.Exists(tp.GetInterfaces(), t => t.GetGenericTypeDefinition() == typeof(IMultipleExtractDescriptor<>)))
            //    {
            //        //return new MultiPageTaskEngine(pageDescriptor);
            //        throw new NotImplementedException();
            //    }

            //}
            throw new Exception("Can't find specific engine");
        }
    }
}
