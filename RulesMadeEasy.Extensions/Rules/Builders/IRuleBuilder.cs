using RulesMadeEasy.Core;
using System;

namespace RulesMadeEasy.Extensions
{
    /// <summary>
    /// Interface for a builder that builds <see cref="IRule"/>s.
    /// </summary>
    public interface IRuleBuilder
    {
        /// <summary>
        /// Adds a condition
        /// </summary>
        /// <param name="condition">The <see cref="IRuleCondition"/> to add</param>
        /// <returns>The current <see cref="IRuleBuilder"/>instance </returns>
        IRuleBuilder AddRuleCondition(IRuleCondition condition);

        /// <summary>
        /// Adds a logical condition
        /// </summary>
        /// <param name="op">The operator of the logical condition</param>
        /// <param name="nestedConditions">The nested conditions of the <see cref="ILogicalCondition"/></param>
        /// <returns>The current <see cref="IRuleBuilder"/>instance </returns>
        IRuleBuilder AddLogicalCondition(ConditionOperator op, params IRuleCondition[] nestedConditions);

        /// <summary>
        /// Adds a value condition
        /// </summary>
        /// <param name="op">The operator of the logical condition</param>
        /// <param name="valueKey">The key used to search within the data value collection during rule evaluation</param>
        /// <param name="expectedValue">Optional: The expected value</param>
        /// <returns>The current <see cref="IRuleBuilder"/>instance </returns>
        IRuleBuilder AddValueCondition(ConditionOperator op, string valueKey, object expectedValue = null);

        /// <summary>
        /// Creates an <see cref="ILogicalCondition"/> that will check to see if the actual value matches any of the values stored in <see cref="allowedValues"/>
        /// </summary>
        /// <param name="valueKey">The value key used when compiling the <see cref="IValueRuleCondition"/></param>
        /// <param name="allowedValues">The collection of allowed values</param>
        /// <returns>The current <see cref="IRuleBuilder"/>instance </returns>
        IRuleBuilder CreateInCondition(string valueKey, params object[] allowedValues);

        /// <summary>
        /// Creates an <see cref="ILogicalCondition"/> that will check to see if the actual value doe not match any of the values stored in <see cref="notAllowedValues"/>
        /// </summary>
        /// <param name="valueKey">The value key used when compiling the <see cref="IValueRuleCondition"/></param>
        /// <param name="notAllowedValues">The collection of not allowed values</param>
        /// <returns>The current <see cref="IRuleBuilder"/>instance </returns>
        IRuleBuilder CreateNotInCondition<T>(string valueKey, params T[] notAllowedValues);

        /// <summary>
        /// Sets the provided <see cref="IAction"/> to be built when the <see cref="IRule"/> is created
        /// </summary>
        /// <param name="actionKey">The identifier of the action to add</param>
        /// <returns>The current <see cref="IRuleBuilder"/>instance </returns>
        IRuleBuilder AddRuleAction(object actionKey);

        /// <summary>
        /// Builds out an instance of an <see cref="IRule"/>
        /// </summary>
        /// <returns>The compiled <see cref="IRule"/></returns>
        IRule Build();
    }
}