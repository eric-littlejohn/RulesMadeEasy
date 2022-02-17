using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Contains information about exceptions that occured during action execution
    /// </summary>
    public sealed class ActionExecutionException : Exception
    {
        /// <summary>
        /// The different causes of a condition evaluation exceptions
        /// </summary>
        public enum ExceptionCause
        {
            /// <summary>
            /// The reason for the exception is not specified
            /// </summary>
            Unspecified = 0,

            /// <summary>
            /// An action is already registered with the specified key
            /// </summary>
            ActionAlreadyRegistered = 1,

            /// <summary>
            /// The action requested was not found
            /// </summary>
            ActionNotFound = 2,

            /// <summary>
            /// No rule engine instance provided when creating a new <see cref="BaseAction"/>
            /// </summary>
            NoRuleEngineInstanceProvided = 3,

            /// <summary>
            /// No matching data value found when trying to lookup an <see cref="IDataValue"/> with a specified key
            /// </summary>
            NoMatchingDataValueFound = 4,

            /// <summary>
            /// No rule engine instance provided when creating a new <see cref="BaseAction"/>
            /// </summary>
            NoServiceProviderInstanceProvided = 5,

            /// <summary>
            /// Cannot instantiate an instance of the action.
            /// </summary>
            CannotInstantiateAction = 6,
        }

        /// <summary>
        /// What was the cause of the exception
        /// </summary>
        public ExceptionCause Cause { get; }

        public ActionExecutionException(ExceptionCause cause, string message, Exception innerException = null)
            :base(message, innerException)
        {
            Cause = cause;
        }
    }
}
