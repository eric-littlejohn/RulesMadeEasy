using System;

namespace RulesMadeEasy.Core
{
    public interface IActionExecutionResult
    {
        /// <summary>
        /// The exception, if any, that occured during execution of the action
        /// </summary>
        ActionExecutionException Exception { get; }

        /// <summary>
        /// The identifier of the action that was executed
        /// </summary>
        Guid ActionId { get; }

        /// <summary>
        /// Whether or not the action executed successfully
        /// </summary>
        bool ActionExecutedSuccessfully { get; }

        /// <summary>
        /// Indicates whether or not the action was executed
        /// </summary>
        bool ActionRan { get; }
    }
}