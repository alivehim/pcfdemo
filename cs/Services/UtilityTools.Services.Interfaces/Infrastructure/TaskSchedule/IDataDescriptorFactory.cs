using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule
{
    public interface IDataDescriptorFactory
    {
        IDataDescriptor GetPageDescriptorByMessageOwner(MessageOwner messageOwner);
    }
}
