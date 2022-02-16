using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for an evaluation result for a triggering event
    /// </summary>
    public interface IRulesMadeEasySessionResult
    {
        /// <summary>
        /// The identifier of the session
        /// </summary>
        Guid SessionId { get; }

        /// <summary>
        /// The start time of the session
        /// </summary>
        DateTimeOffset? StartTime { get; }

        /// <summary>
        /// The end time of the session
        /// </summary>
        DateTimeOffset? EndTime { get; }

        /// <summary>
        /// The runtime of the session
        /// </summary>
        TimeSpan RunTime { get; }

        /// <summary>
        /// The <see cref="IRulesMadeEasySession.EvaluationMode"/> the session was set to run in
        /// </summary>
        RuleEngineEvaluationMode EvaluationMode { get; }

        /// <summary>
        /// Indicates whether or not the evaluation ran completely through
        /// </summary>
        bool RanToCompletion { get; set; }

        /// <summary>
        /// Indicates if the trigger evaluation was cancelled 
        /// </summary>
        bool WasCancelled { get; set; }

        /// <summary>
        /// The exceptions that occured, if any
        /// </summary>
        AggregateException Exception { get; set; }

        /// <summary>
        /// A collection of the evaulation results of the rules that were fired in relation to the trigger
        /// </summary>
        ICollection<IRuleEvaluationResult> RuleEvaluationResults { get; }
    }
}