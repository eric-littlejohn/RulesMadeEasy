using Moq;
using RulesMadeEasy.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core.Tests
{
    internal class AccessModifierMappedAction : AutoMappedAction
    {
        public const string PUBLIC_VALUE_KEY = "PublicValueKey";
        public const string INTERNAL_VALUE_KEY = "InternalValueKey";
        public const string PROTECTED_INTERNAL_VALUE_KEY = "ProtectedInternalValueKey";
        public const string PROTECTED_VALUE_KEY = "ProtectedValueKey";
        public const string PRIVATE_VALUE_KEY = "PrivateValueKey";

        [ActionDataValueProperty(PUBLIC_VALUE_KEY, AllowNull = true)]
        public int PublicProperty { get; set; }

        [ActionDataValueProperty(INTERNAL_VALUE_KEY, AllowNull = true)]
        internal int InternalProperty { get; set; }

        [ActionDataValueProperty(PROTECTED_INTERNAL_VALUE_KEY, AllowNull = true)]
        protected internal int ProtectedInternalProperty { get; set; }

        [ActionDataValueProperty(PROTECTED_VALUE_KEY, AllowNull = true)]
        protected int ProtectedProperty { get; set; }

        [ActionDataValueProperty(PRIVATE_VALUE_KEY, AllowNull = true)]
        private int PrivateProperty { get; set; }

        /// <summary>
        /// Creates a new instance of a <see cref="AutoMappedAction"/>
        /// </summary>
        /// <remarks>Inheriting from this class will attempt to automatically map the data values to defined properties on the inherited class</remarks>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        public AccessModifierMappedAction(IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
            : base(Mock.Of<IServiceProvider>(), engineInstance, dataValues)
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

        public int GetProtectedValue() => ProtectedProperty;

        public int GetPrivateValue() => PrivateProperty;
    }
}
