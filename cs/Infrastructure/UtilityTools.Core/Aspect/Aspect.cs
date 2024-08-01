using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Aspect
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class Aspect : Attribute
    {
        public virtual void OnEntry(AspectArgs args)
        {

        }

        public virtual void OnExit(AspectArgs args)
        {
        }

        public virtual void OnSucess(AspectArgs args)
        {
        }

        public virtual void OnException(AspectArgs args, Exception exception)
        {
        }
    }

}
