using DOCS.Process.DynamicRule.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Extensions
{
    internal static class DocRuleExtension
    {
        public static RuleGroup Convert2RuleGroup(this IList<DocsRule> docsRules)
        {

            //find root
            var rootRule = docsRules.First(p => p.IsGroup && string.IsNullOrEmpty(p.ParentID));

            var rootGroup = new RuleGroup(rootRule.Name, rootRule.LogicalOperator == " and " ? LogicalOperator.And : LogicalOperator.Or);

            foreach (var rule in docsRules.Where(p => !p.IsGroup && string.IsNullOrEmpty(p.ParentID)))
            {
                rootGroup.AddRule(new Rule
                {
                    Name = rule.Name,
                    Operator = (Operator)rule.Operator,
                    TargetValue = rule.TargetValue,
                    Value = rule.Value,
                    Type = rule.Type,
                });
            }

            foreach (var group in docsRules.Where(p => p.IsGroup && string.IsNullOrEmpty(p.ParentID)))
            {
                var ruleGroup = new RuleGroup(group.Name, group.LogicalOperator == " and " ? LogicalOperator.And : LogicalOperator.Or);
                rootGroup.AddRuleGroup(ruleGroup);

                FindRulesAndGroups(ruleGroup, group, docsRules);
            }


            return rootGroup;
        }

        private static void FindRulesAndGroups(RuleGroup ruleGroup, DocsRule group, IList<DocsRule> docsRules)
        {
            var childRules = docsRules.Where(p => !p.IsGroup && p.Parent == group);

            foreach (var item in childRules)
            {
                ruleGroup.AddRule(new Rule
                {
                    Name = item.Name,
                    Operator = (Operator)item.Operator,
                    TargetValue = item.TargetValue,
                    Value = item.Value,
                    Type= item.Type
                });
            }

            var groupChilds = docsRules.Where(p => p.IsGroup && p.Parent == group);

            foreach (var item in groupChilds)
            {
                var innerRuleGroup = new RuleGroup(item.Name, item.LogicalOperator == " and " ? LogicalOperator.And : LogicalOperator.Or);
                ruleGroup.AddRuleGroup(innerRuleGroup);

                FindRulesAndGroups(innerRuleGroup, item, docsRules);
            }
        }
    }
}
