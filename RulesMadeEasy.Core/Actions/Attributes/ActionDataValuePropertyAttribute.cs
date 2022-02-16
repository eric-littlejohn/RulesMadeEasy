using System;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// The attribute is used to decorate properties that are to be auto mapped using the data values recieved by the action instance
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ActionDataValuePropertyAttribute : Attribute
    {
        /// <summary>
        /// The key of the data value used to determine what value to map to the property
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Determines if the property allows for nulls
        /// </summary>
        public bool AllowNull { get; set; } = true;

        /// <summary>
        /// Creates an isntance of an <see cref="ActionDataValuePropertyAttribute"/>
        /// </summary>
        /// <param name="assoicatedDataValueKey">The data value key associated with the property this attribute is bound to</param>
        public ActionDataValuePropertyAttribute(string assoicatedDataValueKey)
        {
            if (string.IsNullOrWhiteSpace(assoicatedDataValueKey))
            {
                throw new ArgumentException(
                    $"Invalid data value key provided to the {nameof(ActionDataValuePropertyAttribute)} constructor. Key: {assoicatedDataValueKey ?? "Null"}",
                    nameof(assoicatedDataValueKey));
            }

            Key = assoicatedDataValueKey;
        }
    }
}