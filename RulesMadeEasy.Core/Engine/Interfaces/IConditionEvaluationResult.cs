using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The result of a condition that was evaluated during rule evaluation
    /// </summary>
    public interface IConditionEvaluationResult
    {
        /// <summary>
        /// Whether or not the condition passed evaluation
        /// </summary>
        bool ConditionPassed { get; }

        /// <summary>
        /// The operator the condition used during evaluation
        /// </summary>
        ConditionOperator Operator { get; }

        /// <summary>
        /// The left side of the comparison
        /// </summary>
        object LeftOperand { get; }

        /// <summary>
        /// The right side of the comparison
        /// </summary>
        object RightOperand { get; }

        /// <summary>
        /// Contains the results of the conditions nested under the condition being evaluated
        /// </summary>
        List<IConditionEvaluationResult> NestedConditionResults { get; }

        /// <summary>
        /// The exception, if any, that occured during the evaluation of the condition
        /// </summary>
        ConditionEvaluationException Exception { get; }
    }
}