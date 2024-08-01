using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityTools.Core.Models.D365
{
    public class ChoiceDefinition
    {
        public IList<ChoiceOption> Options { get; set; }
    }

    public class ChoiceOption
    {
        public long Value { get; set; }
        public ChoiceOptionLabel Label { get; set; }
    }

    public class ChoiceOptionLabel
    {
        public IList<LocalizedLabel> LocalizedLabels { get; set; }
    }

    public class LocalizedLabel
    {
        public string Label { get; set; }
    }

    public class EntityChioceDefinition
    {
        public string LogicalName { get; set; }
        public EntityOptionSet OptionSet { get; set; }
    }

    public class EntityOptionSet
    {
        public IList<ChoiceOption> Options { get; set; }
    }

    
}
