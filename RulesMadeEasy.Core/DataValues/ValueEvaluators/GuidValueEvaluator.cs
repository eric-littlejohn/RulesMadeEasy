using System;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// An implementation of an <see cref="IValueEvaluator"/> for <see cref="Guid"/>s
    /// </summary>
    public class GuidValueEvaluator : IValueEvaluator
    {
        /// <summary>
        /// Evaulates the two values provided using the condition specified
        /// </summary>
        /// <param name="operation">THe condition to apply to the two values</param>
        /// <param name="objA">The first object to evaulate</param>
        /// <param name="objB">The second object to evaulate against</param>
        /// <returns>Whether or not the evaulates to true or not</returns>
        public async Task<bool> Evaluate(ConditionOperator operation, object objA, object objB)
        {
            var valueA = (Guid)objA;
            var valueB = (Guid)objB;

            switch (operation)
            {
                case ConditionOperator.Equal:
                    return Equals(valueA, valueB);
                case ConditionOperator.NotEqual:
                    return !Equals(valueA, valueB);
                default:
                    throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.UnsupportedOperator,
                        $"The {nameof(GuidValueEvaluator)} does not support evaluation for the operator {operation}");
            }
        }
    }
}