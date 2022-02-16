using System;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Contains the value provided to the engine in a form to be used during evaulation
    /// </summary>
    public class DataValue : IDataValue
    {
        public object Value { get; }

        public string Key  { get; }

        public Type ValueType => Value?.GetType();

        /// <summary>
        /// Creates a new instance of a <see cref="DataValue"/>
        /// </summary>
        /// <param name="key">The key used for value lookup</param>
        /// <param name="value">The underlying value of the <see cref="DataValue"/></param>
        public DataValue(string key, object value)
        {
            if (String.IsNullOrWhiteSpace(key)) 
            {
                throw new ArgumentException("Invalid key provided to the DataValue constructor", nameof(key));
            }

            Key = key;
            Value = value;
        }

        /// <summary>
        /// Determines whether or not two data values are equal to each other
        /// </summary>
        /// <param name="other">The <see cref="IDataValue"/> to compare against the current instance</param>
        /// <returns>Whether or not the value provided equals the current instance</returns>
        public bool Equals(IDataValue other) => Equals(this?.Value, other?.Value);
        
        /// <summary>
        /// Converts the underlying value as the provided type
        /// </summary>
        /// <returns>The underlying value as type <typeparamref name="T"/></returns>
        public virtual T As<T>() => (T)Value;
    }

    /// <summary>
    /// Contains the value, of type <typeparamref name="T"/>, provided to the engine in a form to be used during evaulation
    /// </summary>
    /// <typeparam name="T">The type of the underlying data value</typeparam>
    public class DataValue<T> : DataValue
    {
        /// <summary>
        /// The underlying value of the data value
        /// </summary>
        public new T Value => (T)base.Value;

        /// <summary>
        /// Creates a new instance of a <see cref="DataValue"/>
        /// </summary>
        /// <param name="key">The key used for value lookup</param>
        /// <param name="value">The underlying value of the <see cref="DataValue"/></param>
        public DataValue(string key, T value)
            : base(key, value) { }
    }
}
