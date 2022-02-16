using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a value condition contained by an <see cref="IRule"/>
    /// </summary>
    public interface IValueRuleCondition : IRuleCondition
    {
        /// <summary>
        /// The value key that the condition is associated with
        /// </summary>
        /// <returns></returns>
        string ValueKey { get; }

        /// <summary>
        /// Contains the expected value of the <see cref="IValueRuleCondition"/>
        /// </summary>
        object ExpectedValue { get; }
    }
}