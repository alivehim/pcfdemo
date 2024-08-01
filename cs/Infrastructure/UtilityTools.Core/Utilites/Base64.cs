using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Infrastructure
{
    public class Base64
    {
        public static string Encode(Encoding encode, string source)
        {
            byte[] bytes = encode.GetBytes(source);
            string result = string.Empty;
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }

        public static string Decode(Encoding encode, string source)
        {
            var bytes = Convert.FromBase64String(source);
            return encode.GetString(bytes);
        }
    }
}
