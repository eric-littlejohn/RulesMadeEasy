using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Contains information about exceptions that occured during condition processing
    /// </summary>
    public sealed class ConditionEvaluationException : Exception
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
            /// No condition was provided during evaluation
            /// </summary>
            NullConditionProvided = 1,

            /// <summary>
            /// Unable to determine the value type of the condition in order to evaluate it
            /// </summary>
            UnableToDetermineValueType = 2,

            /// <summary>
            /// Unable to determine the type of the condition in order to evaluate it 
            /// </summary>
            UnableToDetermineConditionType = 3,

            /// <summary>
            /// The operator provided is not supported
            /// </summary>
            UnsupportedOperator = 4,

            /// <summary>
            /// The operand of the condition was missing
            /// </summary>
            OperandMissing = 5,

            /// <summary>
            /// There is a mismatch between the value types stored in the condition
            /// </summary>
            ValueTypeMismatch = 7,

            /// <summary>
            /// No evaluator was found that can evaulate the values
            /// </summary>
            EvaluatorNotFound = 8,

            /// <summary>
            /// No data value found to build the condition
            /// </summary>
            NoDataValueFound = 9,

            /// <summary>
            /// Indicates that the number of values provided arent correct
            /// </summary>
            InvalidConditionValueCount = 10,
        }

        /// <summary>
        /// What was the cause of the exception
        /// </summary>
        public ExceptionCause Cause { get; }

        public ConditionEvaluationException(ExceptionCause cause, string message, Exception innerException = null)
            :base(message, innerException)
        {
            Cause = cause;
        }
    }
}
