using System.Collections.Generic;
using UtilityTools.Core.Models;
using UtilityTools.Core.Models.DependencyInjection;
using UtilityTools.Services.Interfaces.Infrastructure.TaskSchedule;

namespace UtilityTools.Services.Interfaces.Infrastructure.MediaGet
{
    public interface IMediaPageDescriptorFactory
    {
        IDataDescriptor GetPageDescriptor(MediaSymbolType symbol);

        IList<MediaGetDescriptionNode> GetAllDescriptor();
    }
}
