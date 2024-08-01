using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Models
{
    [DataContract]
    internal class DocsRule : IEquatable<DocsRule>
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string ParentID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Operator { get; set; }
        [DataMember]
        public bool IsGroup { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string TargetValue { get; set; }
        [DataMember]
        public string LogicalOperator { get; set; }

        public DocsRule Parent => new DocsRule() { ID = ParentID };

        public static bool operator ==(DocsRule obj1, DocsRule obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;

            return obj1.ID == obj2.ID;

        }
        public static bool operator !=(DocsRule obj1, DocsRule obj2) => !(obj1 == obj2);
        public bool Equals(DocsRule other)
        {
            return ID == other.ID;
        }
        public override bool Equals(object obj) => Equals(obj as DocsRule);

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = ID.GetHashCode();
                return hashCode;
            }
        }
    }
}
