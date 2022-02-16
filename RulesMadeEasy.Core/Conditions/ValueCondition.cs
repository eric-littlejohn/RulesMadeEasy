using System;
using System.Collections.Generic;
using System.Linq;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The information and functionality for a <see cref="ValueCondition"/>
    /// </summary>
    public class ValueCondition : Condition, IValueCondition
    {
        /// <summary>
        /// The left side of the condition
        /// </summary>
        public new IDataValue LeftOperand => base.LeftOperand as IDataValue;

        /// <summary>
        /// The right side of the condition
        /// </summary>
        public new IDataValue RightOperand => base.RightOperand as IDataValue;

        /// <summary>
        /// Creates a new instance of a <see cref="ValueCondition"/>
        /// </summary>
        /// <param name="conditionOperator">The <see cref="ConditionOperator"/> of the condition</param>
        /// <param name="leftOperand">The left side of the condition</param>
        /// <param name="rightOperand">The right side of the condition</param>
        public ValueCondition(ConditionOperator conditionOperator,
            IDataValue leftOperand, IDataValue rightOperand)
            : base(conditionOperator, leftOperand, rightOperand)
        {
            if (!IsValueOperator(conditionOperator))
            {
                throw new ArgumentException($"Condition operator of {conditionOperator} not supported by {nameof(ValueCondition)}s");
            }
        }
    }
}
