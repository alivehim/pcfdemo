using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Models
{
    internal class RuleGroup : Rule
    {
        public LogicalOperator LogicalOperator  { get; set; }

        public IList<Rule> Rules = new List<Rule> { };
        public IList<RuleGroup> RuleGroups = new List<RuleGroup> { };

        public IList<Rule> AllRules => Rules.Union(RuleGroups).ToList();
        public RuleGroup(string name, LogicalOperator logicalType)
        {
            Name = name;
            LogicalOperator = logicalType;
        }


        public void AddRule(Rule rule)
        {
            Rules.Add(rule);
        }

        public void AddRuleGroup(RuleGroup rule)
        {
            RuleGroups.Add(rule);
        }
    }
}
