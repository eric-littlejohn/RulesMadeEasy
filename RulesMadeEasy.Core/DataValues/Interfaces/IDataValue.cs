using System;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The interface declaration for a data value used by the engine
    /// </summary>
    public interface IDataValue : IEquatable<IDataValue>
    {
        /// <summary>
        /// The value stored inside the data value
        /// </summary>
        object Value { get; }

        /// <summary>
        /// The unique key used for looking up values
        /// </summary>
        string Key {get; }

        /// <summary>
        /// Gets the type for the data stored in <see cref="Value"/>
        /// </summary>
        Type ValueType { get; }
    }
}
