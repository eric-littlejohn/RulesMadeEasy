using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Contains the evaluation result of a rule that was run by the engine
    /// </summary>
    public class RuleEvaluationResult : IRuleEvaluationResult
    {
        public Exception Exception { get; internal set; }

        public bool RulePassed { get; internal set; }

        public bool ConditionsPassed { get; internal set; }

        public IEnumerable<IConditionEvaluationResult> ConditionEvaluationResults { get; internal set; } 
            = new List<IConditionEvaluationResult>();

        public IEnumerable<IActionExecutionResult> ActionExecutionResults { get; internal set; }
            = new List<IActionExecutionResult>();
    }
}
