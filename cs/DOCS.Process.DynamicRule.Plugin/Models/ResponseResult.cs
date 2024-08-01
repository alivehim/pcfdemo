using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Models
{
    [DataContract]
    internal class ResponseResult
    {
        [DataMember]
        public int code { get; set; }
        //public string message { get; set; }
        [DataMember]
        public bool result { get; set; }
    }
}
