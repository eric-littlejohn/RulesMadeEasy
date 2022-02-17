using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a rule used by the engine
    /// </summary>
    public interface IRule
    {
        /// <summary>
        /// The conditions of the rule to be tested
        /// </summary>
        IEnumerable<IRuleCondition> Conditions { get; }

        /// <summary>
        /// The identifiers of the actions of the rule to be fired
        /// </summary>
        IEnumerable<object> ActionIdentifiers { get; }
    }
}