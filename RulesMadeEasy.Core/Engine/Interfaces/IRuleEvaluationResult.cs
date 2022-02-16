using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for evaluation result of a rule that was run by the engine
    /// </summary>
    public interface IRuleEvaluationResult
    {
        /// <summary>
        /// Indicates whether or not the rule passed
        /// </summary>
        bool RulePassed { get; }

        /// <summary>
        /// Indicates whether or not the conditions passed or not
        /// </summary>
        bool ConditionsPassed { get; }

        /// <summary>
        /// The results of the conditions that were evaluated
        /// </summary>
        IEnumerable<IConditionEvaluationResult> ConditionEvaluationResults { get; }

        /// <summary>
        /// The results of executions results of the action
        /// </summary>
        IEnumerable<IActionExecutionResult> ActionExecutionResults { get; }

        /// <summary>
        /// The exception that occured, if any
        /// </summary>
        Exception Exception { get; }
    }
}