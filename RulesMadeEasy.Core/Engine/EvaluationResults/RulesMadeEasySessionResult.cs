using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Contains the evaulation result information for an evaluation task performed by an <see cref="IRulesMadeEasyEngine"/>
    /// </summary>
    public class RulesMadeEasySessionResult : IRulesMadeEasySessionResult
    {
        /// <summary>
        /// The identifier of the session
        /// </summary>
        public Guid SessionId { get; internal set; } = Guid.NewGuid();

        /// <summary>
        /// The start time of the session
        /// </summary>
        public DateTimeOffset? StartTime { get; internal set; }

        /// <summary>
        /// The end time of the session
        /// </summary>
        public DateTimeOffset? EndTime { get; internal set; }

        /// <summary>
        /// The runtime of the session
        /// </summary>
        public TimeSpan RunTime
        {
            get
            {
                if (StartTime == null)
                {
                    return TimeSpan.Zero;
                }

                return EndTime?.Subtract(StartTime.Value) ??
                       DateTimeOffset.Now.Subtract(StartTime.Value);
            }
        }

        /// <summary>
        /// The <see cref="IRulesMadeEasySession.EvaluationMode"/> the session was set to run in
        /// </summary>
        public RuleEngineEvaluationMode EvaluationMode { get; internal set; }

        public bool RanToCompletion { get; set; } = true;

        public bool WasCancelled { get; set; } = false;

        public AggregateException Exception { get; set; }

        public ICollection<IRuleEvaluationResult> RuleEvaluationResults { get; internal set; } = new List<IRuleEvaluationResult>();
    }
}
