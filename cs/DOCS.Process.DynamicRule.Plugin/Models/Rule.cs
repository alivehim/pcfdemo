using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Models
{
    internal class Rule
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string TargetValue { get; set; }
        public Operator Operator { get; set; }
    }
}
