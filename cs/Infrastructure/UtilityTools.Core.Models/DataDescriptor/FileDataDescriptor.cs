using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Core.Models.MessageBus;

namespace UtilityTools.Core.Models.DataDescriptor
{
    public class FileDataDescriptor : MediaDataDescription
    {
        public string FileName { get; set; }
        public string FullName { get; set; }

        //public int Order { get; set; }

        
    }
}
