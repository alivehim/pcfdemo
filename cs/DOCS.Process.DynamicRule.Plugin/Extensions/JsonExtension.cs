using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DOCS.Process.DynamicRule.Plugin.Extensions
{
    internal static class JsonExtension
    {
        public static T DeserializeObject<T>(this string json)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializerSettings settings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true
                };

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T), settings);
                var data = (T)ser.ReadObject(stream);

                return data;
            }
        }

        public static string SerializeObject<T>(this T data)
        {
            using (var ms = new MemoryStream())
            {
                // Serializer the User object to the stream.
                var ser = new DataContractJsonSerializer(typeof(T));
                ser.WriteObject(ms, data);
                byte[] json = ms.ToArray();
                ms.Close();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }


        }

        public static T XMLDeserializeObject<T>(this string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

    }
}
