using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models;

namespace UtilityTools.Services.Interfaces
{
    public interface IMessage
    {
        string ID { get; set; }

        MessageOwner MessageOwner { get; set; }
    }
}
