using System;

namespace RulesMadeEasy.Core
{
    public class ActionExecutionResult : IActionExecutionResult
    {
        /// <summary>
        /// The exception, if any, that occured during execution of the action
        /// </summary>
        public ActionExecutionException Exception { get; internal set; }

        /// <summary>
        /// The identifier of the action that was executed
        /// </summary>
        public Guid ActionId { get; internal set; }

        /// <summary>
        /// Whether or not the action executed successfully
        /// </summary>
        public bool ActionExecutedSuccessfully { get; internal set; }

        /// <summary>
        /// Indicates whether or not the action was executed
        /// </summary>
        public bool ActionRan { get; internal set; }
    }
}