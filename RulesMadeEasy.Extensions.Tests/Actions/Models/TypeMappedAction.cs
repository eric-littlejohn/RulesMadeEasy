using Moq;
using RulesMadeEasy.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Extensions.Tests
{
    internal class TypeMappedAction : AutoMappedAction
    {
        public const string NONNULL_PRIMATIVE_VALUE_KEY = "NonNullPrimativeValueKey";
        public const string NULLABLE_PRIMATIVE_VALUE_KEY = "NullablePrimativeValueKey";
        public const string NONNULL_CUSTOM_VALUE_KEY = "NonNullCustomValueKey";
        public const string NULLABLE_CUSTOM_VALUE_KEY = "NullableCustomValueKey";

        [ActionDataValueProperty(NONNULL_PRIMATIVE_VALUE_KEY, AllowNull = false)]
        public int NonNullPrimativeValue { get; protected set; } = 0;

        [ActionDataValueProperty(NULLABLE_PRIMATIVE_VALUE_KEY, AllowNull = true)]
        public int? NullablePrimativeValue { get; protected set; }

        [ActionDataValueProperty(NONNULL_CUSTOM_VALUE_KEY, AllowNull = false)]
        public ComplexObject NonNullCustomValue { get; protected set; } = new ComplexObject();

        [ActionDataValueProperty(NULLABLE_CUSTOM_VALUE_KEY, AllowNull = true)]
        public ComplexObject NullableCustomValue { get; protected set; }

        /// <summary>
        /// Creates a new instance of a <see cref="AutoMappedAction"/>
        /// </summary>
        /// <remarks>Inheriting from this class will attempt to automatically map the data values to defined properties on the inherited class</remarks>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        public TypeMappedAction(IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues) :
            base(Mock.Of<IServiceProvider>(), engineInstance, dataValues)
        {
        }

        protected override async Task Execute_ProductionMode()
        {
            await Task.CompletedTask;
        }

        protected override async Task Execute_TestMode()
        {
            await Task.CompletedTask;
        }
    }
}
