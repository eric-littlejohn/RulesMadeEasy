using System;
using System.Collections.Generic;
using System.Linq;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Base class for an <see cref="IRuleCondition"/>
    /// </summary>
    public class LogicalRuleCondition : ILogicalRuleCondition
    {
        public ConditionOperator Operator { get; }

        public IEnumerable<IRuleCondition> NestedConditions { get; }

        public LogicalRuleCondition(ConditionOperator op, params IRuleCondition[] nestedConditions)
        {
            if (!Condition.IsLogicalOperator(op))
            {
                throw new ArgumentException($"Condition operator of {op} not supported by type {nameof(LogicalRuleCondition)}");
            }

            Operator = op;
            NestedConditions = nestedConditions;
        }

        public virtual ICondition Compile(IEnumerable<IDataValue> dataValues)
        {
            var compiledNestedConditions = NestedConditions
                .Select(condition => condition.Compile(dataValues))
                .ToArray();

            if (compiledNestedConditions.Length < 2)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.InvalidConditionValueCount,
                    "Invalid number of conditions to build a logical condition. At least 2 conditions are needed");
            }

            return compiledNestedConditions.Aggregate((c1, c2) => new LogicalCondition(Operator, c1, c2)) as ILogicalCondition;
        }
    }
}