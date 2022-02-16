using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The result of a condition that was evaluated during rule evaluation
    /// </summary>
    public class ConditionEvaluationResult : IConditionEvaluationResult
    {
        public bool ConditionPassed { get; internal set; }
        
        public ConditionOperator Operator { get; internal set; }

        public object LeftOperand { get; internal set; }

        public object RightOperand { get; internal set; }

        public List<IConditionEvaluationResult> NestedConditionResults { get; } = new List<IConditionEvaluationResult>();

        public ConditionEvaluationException Exception { get; internal set; }
    }
}