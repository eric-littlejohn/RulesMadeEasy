using System;
using System.Collections.Generic;
using System.Linq;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The information and functionality for an <see cref="ILogicalCondition"/>
    /// </summary>
    public class LogicalCondition : Condition, ILogicalCondition
    {
        /// <summary>
        /// The left side of the condition
        /// </summary>
        public new ICondition LeftOperand => base.LeftOperand as ICondition;

        /// <summary>
        /// The right side of the condition
        /// </summary>
        public new ICondition RightOperand => base.RightOperand as ICondition;

        /// <summary>
        /// Creates a new instance of a <see cref="LogicalCondition"/>
        /// </summary>
        /// <param name="conditionOperator">The <see cref="ConditionOperator"/> of the condition</param>
        /// <param name="leftOperand">The left condition of the condition</param>
        /// <param name="rightOperand">The right condition of the condition</param>
        public LogicalCondition(ConditionOperator conditionOperator, ICondition leftOperand, ICondition rightOperand)
            : base(conditionOperator, leftOperand, rightOperand)
        {
            if (!IsLogicalOperator(conditionOperator))
            {
                throw new ArgumentException($"Condition operator of {conditionOperator} not supported by {nameof(LogicalCondition)}s");
            }
        }
    }
}
