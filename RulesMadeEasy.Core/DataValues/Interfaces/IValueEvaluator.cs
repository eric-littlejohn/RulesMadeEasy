using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Interface declaration for evaulating two objects against each other
    /// </summary>
    public interface IValueEvaluator
    {
        /// <summary>
        /// Evaulates the two values provided using the condition specified
        /// </summary>
        /// <param name="operation">The condition to apply to the two values</param>
        /// <param name="objA">The first object to evaulate</param>
        /// <param name="objB">The second object to evaulate against</param>
        /// <returns>Whether or not the evaulates to true or not</returns>
        Task<bool> Evaluate(ConditionOperator operation, object objA, object objB);
    }
}
