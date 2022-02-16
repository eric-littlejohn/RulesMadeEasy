using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a logical condition contained by an <see cref="IRule"/>
    /// </summary>
    public interface ILogicalRuleCondition : IRuleCondition
    {
        /// <summary>
        /// Contains the nested <see cref="IRuleCondition"/>s of the <see cref="ILogicalRuleCondition"/>
        /// </summary>
        IEnumerable<IRuleCondition> NestedConditions { get; }
    }
}