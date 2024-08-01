using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityTools.Services.Interfaces;

namespace UtilityTools.Core.Aspect
{
    public class AspectArgs
    {
        public IMessage messagebody { get; set; }

        public IContainerProvider ContainerProvider { get; set; }

        public object[] Arguments { get; set; }
    }
}
