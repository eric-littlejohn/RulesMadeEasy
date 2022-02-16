using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// An implementation of an <see cref="IValueEvaluator"/> for <see cref="short"/>s
    /// </summary>
    public class ShortValueEvaluator : IValueEvaluator
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
            var valueA = (short)objA;
            var valueB = (short)objB;

            switch (operation)
            {
                case ConditionOperator.Equal:
                    return Equals(valueA, valueB);
                case ConditionOperator.NotEqual:
                    return !Equals(valueA, valueB);
                case ConditionOperator.LessThan:
                    return valueA < valueB;
                case ConditionOperator.LessEqualTo:
                    return valueA <= valueB;
                case ConditionOperator.GreaterThan:
                    return valueA > valueB;
                case ConditionOperator.GreaterThanEqualTo:
                    return valueA >= valueB;
                default:
                    throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.UnsupportedOperator,
                        $"The {nameof(ShortValueEvaluator)} does not support evaluation for the operator {operation}");
            }
        }
    }
}