using System.Collections.Generic;
using System;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a condition contained by an <see cref="IRule"/>
    /// </summary>
    public interface IRuleCondition
    {
        /// <summary>
        /// The operator of the condition
        /// </summary>
        ConditionOperator Operator { get; }

        /// <summary>
        /// Compiles the rule condition into its <see cref="ICondition"/> equivalent
        /// </summary>
        /// <param name="dataValues">The available data values to use during compilation</param>
        /// <returns>The compiled <see cref="ICondition"/></returns>
        ICondition Compile(IEnumerable<IDataValue> dataValues);
    }
}
