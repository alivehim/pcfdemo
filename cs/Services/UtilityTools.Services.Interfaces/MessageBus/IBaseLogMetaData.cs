using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Services.Interfaces.MessageBus
{
    public interface IBaseLogMetaData: IMessage
    {
        ErrorListLevel ErrorLevel { get; set; }

        string Message { get; set; }
    }
}
