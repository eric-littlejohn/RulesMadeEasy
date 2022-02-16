using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a logical condition used by the engine
    /// </summary>
    public interface ILogicalCondition : ICondition
    {
        /// <summary>
        /// The left side of the condition
        /// </summary>
        new ICondition LeftOperand { get; }

        /// <summary>
        /// The right side of the condition
        /// </summary>
        new ICondition RightOperand { get; }
    }
}