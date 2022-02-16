using Moq;
using RulesMadeEasy.Core;
using System;
using System.Text;
using Xunit;

namespace RulesMadeEasy.Extensions.Tests
{
    public partial class AutoMappedActionTests
    {
        [Fact]
        public void Ctor_MapsDecoratedProperties_NoDataValueFoundForNonNullableProperty_Throws()
        {
            void ConditionEvalLogic(Exception e)
            {
                var castedCondition = e as ActionExecutionException;
                Assert.NotNull(castedCondition);
                Assert.Equal(ActionExecutionException.ExceptionCause.NoMatchingDataValueFound, castedCondition.Cause);
            }

            var exc = Assert.Throws<AggregateException>(() => new TypeMappedAction(Mock.Of<IRulesMadeEasyEngine>(), new IDataValue[] { }));
            Assert.NotNull(exc);
            Assert.Collection(exc.InnerExceptions,
                ConditionEvalLogic,
                ConditionEvalLogic);
        }

        [Fact]
        public void Ctor_MapsDecoratedProperties_PrimativeTypes_Success()
        {
            var nonNullPrimativeDataValue = new DataValue(TypeMappedAction.NONNULL_PRIMATIVE_VALUE_KEY, 1);
            var nullablePrimativeDataValue = new DataValue(TypeMappedAction.NULLABLE_PRIMATIVE_VALUE_KEY, null);
            var nonNullCustomDataValue = new DataValue(TypeMappedAction.NONNULL_CUSTOM_VALUE_KEY, new ComplexObject
            {
                DateTimeOffsetProperty = DateTimeOffset.Now,
                IntProperty = 28,
                StringProperty = "SomeString"
            });
            var nullableCustomDataValue = new DataValue(TypeMappedAction.NULLABLE_CUSTOM_VALUE_KEY, null);

            var dataValues = new IDataValue[]
            {
                nonNullPrimativeDataValue, nullablePrimativeDataValue, nonNullCustomDataValue, nullableCustomDataValue
            };

            var subjectUnderTest = new TypeMappedAction(Mock.Of<IRulesMadeEasyEngine>(), dataValues);

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(nonNullPrimativeDataValue.Value, subjectUnderTest.NonNullPrimativeValue);
            Assert.Equal(nullablePrimativeDataValue.Value, subjectUnderTest.NullablePrimativeValue);
            Assert.Equal(nonNullCustomDataValue.Value, subjectUnderTest.NonNullCustomValue);
            Assert.Equal(nullableCustomDataValue.Value, subjectUnderTest.NullableCustomValue);
        }

        [Fact]
        public void Ctor_MapsDecoratedProperties_PublicAccessModifier_Success()
        {
            var propertyValue = new DataValue(AccessModifierMappedAction.PUBLIC_VALUE_KEY, 1);

            var subjectUnderTest = new AccessModifierMappedAction(Mock.Of<IRulesMadeEasyEngine>(),
                new IDataValue[] { propertyValue });

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(propertyValue.Value, subjectUnderTest.PublicProperty);
        }

        [Fact]
        public void Ctor_MapsDecoratedProperties_InternalAccessModifier_Success()
        {
            var propertyValue = new DataValue(AccessModifierMappedAction.INTERNAL_VALUE_KEY, 1);

            var subjectUnderTest = new AccessModifierMappedAction(Mock.Of<IRulesMadeEasyEngine>(), 
                new IDataValue[] { propertyValue });

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(propertyValue.Value, subjectUnderTest.InternalProperty);
        }

        [Fact]
        public void Ctor_MapsDecoratedProperties_ProtectedInternalAccessModifier_Success()
        {
            var propertyValue = new DataValue(AccessModifierMappedAction.PROTECTED_INTERNAL_VALUE_KEY, 1);

            var subjectUnderTest = new AccessModifierMappedAction(Mock.Of<IRulesMadeEasyEngine>(),
                new IDataValue[] { propertyValue });

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(propertyValue.Value, subjectUnderTest.ProtectedInternalProperty);
        }

        [Fact]
        public void Ctor_MapsDecoratedProperties_ProtectedAccessModifier_Success()
        {
            var propertyValue = new DataValue(AccessModifierMappedAction.PROTECTED_VALUE_KEY, 1);

            var subjectUnderTest = new AccessModifierMappedAction(Mock.Of<IRulesMadeEasyEngine>(),
                new IDataValue[] { propertyValue });

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(propertyValue.Value, subjectUnderTest.GetProtectedValue());
        }

        [Fact]
        public void Ctor_MapsDecoratedProperties_PrivateAccessModifier_Success()
        {
            var propertyValue = new DataValue(AccessModifierMappedAction.PRIVATE_VALUE_KEY, 1);

            var subjectUnderTest = new AccessModifierMappedAction(Mock.Of<IRulesMadeEasyEngine>(),
                new IDataValue[] { propertyValue });

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(propertyValue.Value, subjectUnderTest.GetPrivateValue());
        }
    }
}
