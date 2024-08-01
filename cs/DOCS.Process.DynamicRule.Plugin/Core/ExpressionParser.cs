using DOCS.Process.DynamicRule.Plugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DOCS.Process.DynamicRule.Plugin.Core
{
    internal class ExpressionParser
    {
        private const string StringStr = "string";

        private readonly string BooleanStr = nameof(Boolean).ToLower();
        private readonly string Number = nameof(Number).ToLower();
        private readonly string In = nameof(In).ToLower();
        private readonly string Contain = nameof(Contain).ToLower();
        private readonly string And = nameof(And).ToLower();

        private delegate Expression Binder(Expression left, Expression right);

        private Expression ParseTree(
          RuleGroup condition)
        {
            Expression left = null;
            var gate = condition.LogicalOperator;

            var rules = condition.AllRules;

            Binder binder = gate == LogicalOperator.And ? (Binder)Expression.And : Expression.Or;

            Expression bind(Expression leftExpression, Expression rightExpression) =>
                leftExpression == null ? rightExpression : binder(leftExpression, rightExpression);

            foreach (var rule in rules)
            {
                if (rule is RuleGroup group)
                {
                    var right = ParseTree(group);
                    left = bind(left, right);
                    continue;
                }

                var @operator = rule.Operator;
                var type = rule.Type;
                var targetvalue = rule.TargetValue;
                var value = rule.Value;


                if (@operator == Operator.Contains)
                {
                    Expression<Func<bool>> right = () => value.ToString().Contains(targetvalue.ToString());
                    left = bind(left, right.Body);
                }
                else if(@operator == Operator.DoesNotContains)
                {
                    Expression<Func<bool>> right = () => !value.ToString().Contains(targetvalue.ToString());
                    left = bind(left, right.Body);
                }
                else if(@operator == Operator.BeginsWith)
                {
                    Expression<Func<bool>> right = () => value.ToString().StartsWith(targetvalue.ToString());
                    left = bind(left, right.Body);
                }
                else if(@operator == Operator.DoesNotBeginWith)
                {
                    Expression<Func<bool>> right = () => !value.ToString().StartsWith(targetvalue.ToString());
                    left = bind(left, right.Body);
                }
                else if(@operator == Operator.EndsWith)
                {
                    Expression<Func<bool>> right = () => value.ToString().EndsWith(targetvalue.ToString());
                    left = bind(left, right.Body);
                }
                else if (@operator == Operator.DoesNotEnedWith)
                {
                    Expression<Func<bool>> right = () => !value.ToString().EndsWith(targetvalue.ToString());
                    left = bind(left, right.Body);
                }
                else if (@operator == Operator.Euqual)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                        (object)value : Decimal.Parse (value);
                    var toCompare = Expression.Constant(val);


                    var orig = (type == StringStr || type == BooleanStr) ? Expression.Constant(targetvalue.ToString()) 
                        : Expression.Constant(Decimal.Parse(targetvalue));
                    var right = Expression.Equal(orig, toCompare);
                    left = bind(left, right);
                }
                else if (@operator == Operator.DoesNotEqual)
                {
                    object val = (type == StringStr || type == BooleanStr) ?
                        (object)value : Decimal.Parse(value);
                    var toCompare = Expression.Constant(val);


                    var orig = (type == StringStr || type == BooleanStr) ? Expression.Constant(targetvalue.ToString())
                        : Expression.Constant(Decimal.Parse(targetvalue));
                    var right = Expression.NotEqual(orig, toCompare);
                    left = bind(left, right);
                }

            }

            return left;
        }

        public Expression<Func<bool>> ParseExpressionOf(RuleGroup ruleGroup)
        {
            var conditions = ParseTree(ruleGroup);
            if (conditions.CanReduce)
            {
                conditions = conditions.ReduceAndCheck();
            }

            Console.WriteLine(conditions.ToString());

            var query = Expression.Lambda<Func<bool>>(conditions);
            return query;
        }


        public Func<bool> ParsePredicateOf(RuleGroup ruleGroup)
        {
            var query = ParseExpressionOf(ruleGroup);
            return query.Compile();
        }

    }
}
