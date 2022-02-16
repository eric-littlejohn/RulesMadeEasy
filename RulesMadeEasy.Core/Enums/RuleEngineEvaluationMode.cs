using System;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The evaluation modes that are supported by the engine
    /// </summary>
    public enum RuleEngineEvaluationMode : int
    {
        /// <summary>
        /// Indicates the engine is running a "production" mode.
        /// </summary>
        Production = 0,

        /// <summary>
        /// Indicates the engine is running a "test" mode.
        /// </summary>
        Test
    }
}
