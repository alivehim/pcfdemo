using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>Returns all public instance constructors for the type</summary> 
        /// <param name="type"></param> <returns></returns>
        public static IEnumerable<ConstructorInfo> GetPublicInstanceConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic);
        }

    }
}
