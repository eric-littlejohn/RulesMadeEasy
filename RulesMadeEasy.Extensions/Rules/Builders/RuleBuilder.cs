using System;
using System.Collections.Generic;
using System.Linq;
using RulesMadeEasy.Core;

namespace RulesMadeEasy.Extensions
{
    public class RuleBuilder : IRuleBuilder
    {
        /// <summary>
        /// Contains the collection of <see cref="IRuleCondition"/>s the new <see cref="IRule"/> will be built with
        /// </summary>
        protected virtual List<IRuleCondition> RuleConditions { get; private set; } = new List<IRuleCondition>();

        /// <summary>
        /// Contains the collection of identifiers of the actions of the new <see cref="IRule"/> will be built with
        /// </summary>
        protected virtual List<Guid> RuleActionIdentifiers { get; private set; } = new List<Guid>();

        /// <inheritdoc />
        public IRuleBuilder AddRuleCondition(IRuleCondition condition)
        {
            if (condition != null)
            {
                RuleConditions.Add(condition);
            }

            return this;
        }

        /// <inheritdoc />
        public IRuleBuilder AddLogicalCondition(ConditionOperator op, params IRuleCondition[] nestedConditions)
        {
            AddRuleCondition(new LogicalRuleCondition(op, nestedConditions));
            return this;
        }

        /// <inheritdoc />
        public IRuleBuilder AddValueCondition(ConditionOperator op, string valueKey, object expectedValue = null)
        {
            AddRuleCondition(new ValueRuleCondition(op, valueKey, expectedValue));
            return this;
        }

        /// <inheritdoc />
        public IRuleBuilder CreateInCondition(string valueKey, params object[] allowedValues)
        {
            var dataValueConditons = allowedValues
                .Select(conditionValue => new ValueRuleCondition(ConditionOperator.Equal,
                        valueKey, conditionValue) as IRuleCondition)
                .ToArray();

            AddRuleCondition(new LogicalRuleCondition(ConditionOperator.Or, dataValueConditons));

            return this;
        }

        /// <inheritdoc />
        public IRuleBuilder CreateNotInCondition<T>(string valueKey, params T[] notAllowedValues)
        {
            var dataValueConditons = notAllowedValues
                .Select(conditionValue => new ValueRuleCondition(ConditionOperator.NotEqual,
                    valueKey, conditionValue) as IRuleCondition)
                .ToArray();

            AddRuleCondition(new LogicalRuleCondition(ConditionOperator.And, dataValueConditons));

            return this;
        }

        /// <inheritdoc />
        public IRuleBuilder AddRuleAction(Guid action)
        {
            RuleActionIdentifiers.Add(action);

            return this;
        }

        /// <inheritdoc />
        public IRule Build()
        {
            var newRule = new Rule(RuleConditions, RuleActionIdentifiers);

            Reset();

            return newRule;
        }

        private void Reset()
        {
            RuleConditions = new List<IRuleCondition>();
            RuleActionIdentifiers = new List<Guid>();
        }
    }
}