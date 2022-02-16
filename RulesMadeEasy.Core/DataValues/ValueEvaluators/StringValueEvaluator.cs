using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// An implementation of an <see cref="IValueEvaluator"/> for <see cref="string"/>s
    /// </summary>
    public class StringValueEvaluator : IValueEvaluator
    {
        /// <summary>
        /// The comparison mode used when comparing strings
        /// </summary>
        protected StringComparison ComparisonMode { get; }

        /// <summary>
        /// Creates a new instance of a <see cref="StringValueEvaluator"/>
        /// </summary>
        /// <param name="comparisonMode">Optional: The string comparison mode to use when comparing strings. Defaults to the default value for <see cref="StringComparison"/></param>
        public StringValueEvaluator(StringComparison comparisonMode = default(StringComparison))
        {
            ComparisonMode = comparisonMode;
        }

        /// <summary>
        /// Evaulates the two values provided using the condition specified
        /// </summary>
        /// <param name="operation">THe condition to apply to the two values</param>
        /// <param name="objA">The first object to evaulate</param>
        /// <param name="objB">The second object to evaulate against</param>
        /// <returns>Whether or not the evaulates to true or not</returns>
        public async Task<bool> Evaluate(ConditionOperator operation, object objA, object objB)
        {
            string valueA = objA as string;
            string valueB = objB as string;

            switch (operation)
            {
                case ConditionOperator.Equal:
                    return String.Equals(valueA, valueB, ComparisonMode);
                case ConditionOperator.NotEqual:
                    return !String.Equals(valueA, valueB, ComparisonMode);
                default:
                    throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, 
                        $"The {nameof(StringValueEvaluator)} does not support evaluation for the operator {operation}");
            }
        }
    }
}
