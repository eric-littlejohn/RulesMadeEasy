using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for the rules engine
    /// </summary>
    public interface IRulesMadeEasyEngine
    {
        /// <summary>
        /// Evaluates the trigger instance against the rules provided
        /// </summary>
        /// <param name="evaluationMode">The evaluation mode in which to evaluate the rules in</param>
        /// <param name="dataValues">The data values that are to be evaluated</param>
        /// <param name="rules">The rules that the trigger will be evaluated against</param>
        /// <param name="cancellationToken">A cancellation token used for exiting evaulation early</param>
        /// <returns>An <see ref="IEvaluationResult" /> detailing the evaluation results</returns>
        Task<IRulesMadeEasySessionResult> EvaluateRules(RuleEngineEvaluationMode evaluationMode, IEnumerable<IDataValue> dataValues, IEnumerable<IRule> rules, 
            CancellationToken cancellationToken = default(CancellationToken));
    }
}