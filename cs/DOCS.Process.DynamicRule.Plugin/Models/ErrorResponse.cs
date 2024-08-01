using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Models
{
    [DataContract]
    internal class ErrorResponse
    {
        [DataMember]
        public int code { get; set; }
        [DataMember]
        public string message { get; set; }
        //public bool result { get; set; }
    }
}
