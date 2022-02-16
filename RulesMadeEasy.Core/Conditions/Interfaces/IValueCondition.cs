using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a value condition used by the engine
    /// </summary>
    public interface IValueCondition : ICondition
    {
        /// <summary>
        /// The expected value of the condition
        /// </summary>
        new IDataValue LeftOperand { get; }

        /// <summary>
        /// The right side of the condition
        /// </summary>
        new IDataValue RightOperand { get; }
    }
}