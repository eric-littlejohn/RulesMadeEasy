using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a condition used by the engine
    /// </summary>
    public interface ICondition
    {
        /// <summary>
        /// The operator of the condition
        /// </summary>
        ConditionOperator Operator { get; }

        /// <summary>
        /// The left side of the condition
        /// </summary>
        object LeftOperand { get; }

        /// <summary>
        /// The right side of the condition
        /// </summary>
        object RightOperand { get; }
    }
}
