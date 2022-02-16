using System;
using System.Collections.Generic;
using System.Linq;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Base class for an <see cref="IRuleCondition"/>
    /// </summary>
    public class ValueRuleCondition : IValueRuleCondition
    {
        public ConditionOperator Operator { get; }

        public string ValueKey { get; }

        public object ExpectedValue { get; }

        public ValueRuleCondition(ConditionOperator op, string valueKey)
            :this(op, valueKey, null){ }

        public ValueRuleCondition(ConditionOperator op, string valueKey, object expectedValue)
        {
            if (!Condition.IsValueOperator(op))
            {
                throw new ArgumentException($"Condition operator of {op} not supported by type {nameof(ValueRuleCondition)}");
            }

            if (String.IsNullOrWhiteSpace(valueKey))
            {
                throw new ArgumentException("Value key cannot be null or empty", nameof(valueKey));
            }

            Operator = op;
            ValueKey = valueKey;
            ExpectedValue = expectedValue;
        }

        public virtual ICondition Compile(IEnumerable<IDataValue> dataValues)
        {
            IDataValue associatedDataValue = dataValues
                .FirstOrDefault(dataValue => dataValue.Key == ValueKey);

            if (associatedDataValue == null)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.NoDataValueFound,
                    $"No data value found with the key {ValueKey}.");
            }

            //Condition is built out in this order: {actual} {operator} {expected}
            //Order is important for comparison operators like less than, greater than, etc.
            return new ValueCondition(Operator, associatedDataValue, new DataValue(ValueKey, ExpectedValue));
        }
    }
}