using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Interface decalration for a session run by an <see cref="IRulesMadeEasyEngine"/>
    /// </summary>
    public interface IRulesMadeEasySession
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
        /// The <see cref="EvaluationMode"/> the session was set to run in
        /// </summary>
        RuleEngineEvaluationMode EvaluationMode { get; }
    }
}