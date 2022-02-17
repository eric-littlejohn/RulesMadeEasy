using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Implementation of the <see cref="IRule"/> interface
    /// </summary>
    public class Rule : IRule
    {
        /// <summary>
        /// The conditions of the rule to be tested
        /// </summary>
        public IEnumerable<IRuleCondition> Conditions { get; }

        /// <summary>
        /// The identifiers of the actions of the rule to be fired
        /// </summary>
        public IEnumerable<object> ActionIdentifiers { get; }

        public Rule(IEnumerable<IRuleCondition> conditions, IEnumerable<object> actionIdentifiers)
        {
            Conditions = conditions ?? new List<IRuleCondition>();
            ActionIdentifiers = actionIdentifiers ?? new List<object>();
        }
    }
}
