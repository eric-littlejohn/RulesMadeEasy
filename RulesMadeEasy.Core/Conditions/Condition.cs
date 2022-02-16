using System;
using System.Collections.Generic;
using System.Linq;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The information and functionality shared amongst all <see cref="ICondition"/>s
    /// </summary>
    public abstract class Condition : ICondition
    {
        /// <summary>
        /// Contains the allowed <see cref="ConditionOperator"/>s for an <see cref="ILogicalCondition"/>
        /// </summary>
        public static readonly IEnumerable<ConditionOperator> LOGICAL_OPERATORS = new[]
        {
            ConditionOperator.And,
            ConditionOperator.Or
        };

        /// <summary>
        /// Contains the allowed <see cref="ConditionOperator"/>s for an <see cref="IValueCondition"/>
        /// </summary>
        public static readonly IEnumerable<ConditionOperator> VALUE_OPERATORS = new[]
        {
            ConditionOperator.Equal,
            ConditionOperator.NotEqual,
            ConditionOperator.LessThan,
            ConditionOperator.LessEqualTo,
            ConditionOperator.GreaterThan,
            ConditionOperator.GreaterThanEqualTo
        };

        /// <summary>
        /// The operator of the condition
        /// </summary>
        public ConditionOperator Operator { get; }

        /// <summary>
        /// The left side of the condition
        /// </summary>
        public object LeftOperand { get; }

        /// <summary>
        /// The right side of the condition
        /// </summary>
        public object RightOperand { get; }

        /// <summary>
        /// Creates a new instance of a <see cref="Condition"/>
        /// </summary>
        /// <param name="conditionOperator">The <see cref="ConditionOperator"/> of the condition</param>
        /// <param name="leftOperand">The left side of the condition</param>
        /// <param name="rightOperand">The right side of the condition</param>
        protected Condition(ConditionOperator conditionOperator,
            object leftOperand, object rightOperand)
        {
            Operator = conditionOperator;
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
        }

        /// <summary>
        /// Whether or not the provided <see cref="ConditionOperator"/> is a logical operator
        /// </summary>
        /// <param name="conditionOperator">The operator to check</param>
        /// <returns>Whether or not the operator is a logical operator or not</returns>
        public static bool IsLogicalOperator(ConditionOperator conditionOperator)
        {
            return LOGICAL_OPERATORS.Contains(conditionOperator);
        }

        /// <summary>
        /// Whether or not the provided <see cref="ConditionOperator"/> is a value operator
        /// </summary>
        /// <param name="conditionOperator">The operator to check</param>
        /// <returns>Whether or not the operator is a value operator or not</returns>
        public static bool IsValueOperator(ConditionOperator conditionOperator)
        {
            return VALUE_OPERATORS.Contains(conditionOperator);
        }
    }
}
