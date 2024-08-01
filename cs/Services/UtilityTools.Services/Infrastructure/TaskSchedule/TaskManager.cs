using System;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Infrastructure.TaskSchedule
{
    public class TaskManager : ITaskManager
    {
        private readonly IDataDescriptorFactory dataDescriptorFactory;
        private readonly ITaskEngine taskEngine;

        public TaskManager(IDataDescriptorFactory dataDescriptorFactory, ITaskEngine taskEngine)
        {
            this.dataDescriptorFactory = dataDescriptorFactory;
            this.taskEngine = taskEngine;
        }

        public void StartNew(ITaskContext taskContext)
        {
            try
            {
                Task.Run(async () =>
                {
                    var dataDescriptor = dataDescriptorFactory.GetPageDescriptorByMessageOwner(taskContext.MessageOwner);
                    taskContext.DataDescriptor = dataDescriptor;
                    //var engine = dataEngineFactory.Create(taskContext);

                    await taskEngine.Work(taskContext);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
