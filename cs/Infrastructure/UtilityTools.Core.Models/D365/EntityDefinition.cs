using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class EntityDefinition
    {
        public IList<EntityAttribute> Attributes { get; set; }
    }

    public class EntityAttribute
    {
        public bool IsCustomAttribute { get; set; }

        public string AttributeType { get; set; }

        public string LogicalName { get; set; }

        public string SchemaName { get; set; }

        public DisplayName DisplayName { get; set; }
    }

    public class DisplayName
    {
        public IList<LocalizedLabel> LocalizedLabels { get; set; }
    }

   
}
