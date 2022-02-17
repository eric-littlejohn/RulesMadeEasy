using System;

namespace RulesMadeEasy.Core
{
    public class ActionExecutionResult : IActionExecutionResult
    {
        /// <inheritdoc />
        public ActionExecutionException Exception { get; internal set; }

        /// <inheritdoc />
        public object ActionKey { get; internal set; }

        /// <inheritdoc />
        public bool ActionExecutedSuccessfully { get; internal set; }

        /// <inheritdoc />
        public bool ActionRan { get; internal set; }
    }
}