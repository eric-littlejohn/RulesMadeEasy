using System;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Information and functionality related for an execution session by an <see cref="IRulesMadeEasyEngine"/>
    /// </summary>
    public class RulesMadeEasySession : IRulesMadeEasySession
    {
        /// <summary>
        /// The identifier of the session
        /// </summary>
        public Guid SessionId { get; }

        /// <summary>
        /// The start time of the session
        /// </summary>
        public DateTimeOffset? StartTime { get; }

        /// <summary>
        /// The end time of the session
        /// </summary>
        public DateTimeOffset? EndTime { get; }

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
        public RuleEngineEvaluationMode EvaluationMode { get; }

        public RulesMadeEasySession(RuleEngineEvaluationMode evalMode)
        {
            EvaluationMode = evalMode;
        }


    }
}