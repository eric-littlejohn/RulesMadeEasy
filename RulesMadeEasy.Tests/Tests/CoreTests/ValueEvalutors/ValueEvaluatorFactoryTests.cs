using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using Moq.Protected;
using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class ValueEvaluatorFactoryTests
    {
        [Theory]
        [InlineData(typeof(bool), typeof(BooleanValueEvaluator))]
        [InlineData(typeof(byte), typeof(ByteValueEvaluator))]
        [InlineData(typeof(sbyte), typeof(SByteValueEvaluator))]
        [InlineData(typeof(char), typeof(CharValueEvaluator))]
        [InlineData(typeof(string), typeof(StringValueEvaluator))]
        [InlineData(typeof(decimal), typeof(DecimalValueEvaluator))]
        [InlineData(typeof(double), typeof(DoubleValueEvaluator))]
        [InlineData(typeof(float), typeof(FloatValueEvaluator))]
        [InlineData(typeof(int), typeof(IntValueEvaluator))]
        [InlineData(typeof(long), typeof(LongValueEvaluator))]
        [InlineData(typeof(short), typeof(ShortValueEvaluator))]
        [InlineData(typeof(uint), typeof(UIntValueEvaluator))]
        [InlineData(typeof(ushort), typeof(UShortValueEvaluator))]
        [InlineData(typeof(ulong), typeof(ULongValueEvaluator))]
        [InlineData(typeof(Guid), typeof(GuidValueEvaluator))]
        [InlineData(typeof(DateTime), typeof(DateTimeValueEvaluator))]
        [InlineData(typeof(DateTimeOffset), typeof(DateTimeOffsetValueEvaluator))]
        [InlineData(typeof(object), typeof(ObjectValueEvaluator))]
        public void Ctor_RegistersDefaults(Type valueTypeToCheck, Type expectedValueEvaluatorType)
        {
            IValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory(registerDefaults: true);

            var result = subjectUnderTest.GetValueEvaluator(valueTypeToCheck);
            Assert.NotNull(result);
            Assert.IsType(expectedValueEvaluatorType, result);
        }

        [Theory]
        [InlineData(typeof(bool), typeof(BooleanValueEvaluator))]
        [InlineData(typeof(byte), typeof(ByteValueEvaluator))]
        [InlineData(typeof(sbyte), typeof(SByteValueEvaluator))]
        [InlineData(typeof(char), typeof(CharValueEvaluator))]
        [InlineData(typeof(string), typeof(StringValueEvaluator))]
        [InlineData(typeof(decimal), typeof(DecimalValueEvaluator))]
        [InlineData(typeof(double), typeof(DoubleValueEvaluator))]
        [InlineData(typeof(float), typeof(FloatValueEvaluator))]
        [InlineData(typeof(int), typeof(IntValueEvaluator))]
        [InlineData(typeof(long), typeof(LongValueEvaluator))]
        [InlineData(typeof(short), typeof(ShortValueEvaluator))]
        [InlineData(typeof(uint), typeof(UIntValueEvaluator))]
        [InlineData(typeof(ushort), typeof(UShortValueEvaluator))]
        [InlineData(typeof(ulong), typeof(ULongValueEvaluator))]
        [InlineData(typeof(Guid), typeof(GuidValueEvaluator))]
        [InlineData(typeof(DateTime), typeof(DateTimeValueEvaluator))]
        [InlineData(typeof(DateTimeOffset), typeof(DateTimeOffsetValueEvaluator))]
        [InlineData(typeof(object), typeof(ObjectValueEvaluator))]
        public void Ctor_RegisterDefaultsSetToFalse_ShouldNotRegisterDefaults(Type valueTypeToCheck, Type expectedValueEvaluatorType)
        {
            IValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory(registerDefaults: false);

            var result = subjectUnderTest.GetValueEvaluator(valueTypeToCheck);
            Assert.Null(result);
        }

        [Fact]
        public void RegisterDefaultValueEvaluator_NullType_Throws()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            Assert.Throws<ArgumentNullException>(() =>
                subjectUnderTest.RegisterValueEvaluator(null, null));
        }

        [Fact]
        public void RegisterValueEvaluator_NullEvaluator_Throws()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            Assert.Throws<ArgumentNullException>(() =>
                subjectUnderTest.RegisterValueEvaluator(typeof(object), null));
        }

        [Fact]
        public void RegisterValueEvaluator_Generic_NullEvaluator_Throws()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            Assert.Throws<ArgumentNullException>(() =>
                subjectUnderTest.RegisterValueEvaluator<object>(null));
        }

        [Fact]
        public void RegisterValueEvaluator_TypeRegisteredAlready_Throws()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            subjectUnderTest.RegisterValueEvaluator(typeof(string),
                Mock.Of<IValueEvaluator>());

            var newEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterValueEvaluator<string>(newEvaluator);

            var result = subjectUnderTest.GetValueEvaluator(typeof(string));

            Assert.NotNull(result);
            Assert.Same(newEvaluator, result);
        }

        [Fact]
        public void RegisterValueEvaluator_Generic_TypeRegisteredAlready_Throws()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            subjectUnderTest.RegisterValueEvaluator(typeof(object),
                Mock.Of<IValueEvaluator>());

            var newEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterValueEvaluator<object>(newEvaluator);

            var result = subjectUnderTest.GetValueEvaluator(typeof(object));

            Assert.NotNull(result);
            Assert.Same(newEvaluator, result);
        }

        [Fact]
        public void RegisterValueEvaluator_Success()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            var mockedValueEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterValueEvaluator(typeof(object), mockedValueEvaluator);

            var registeredValueEvaluator = subjectUnderTest.GetValueEvaluator(typeof(object));
            Assert.NotNull(registeredValueEvaluator);
            Assert.Same(mockedValueEvaluator, registeredValueEvaluator);
        }

        [Fact]
        public void RegisterValueEvaluator_Generic_Success()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            var mockedValueEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterValueEvaluator<object>(mockedValueEvaluator);

            var registeredValueEvaluator = subjectUnderTest.GetValueEvaluator(typeof(object));
            Assert.NotNull(registeredValueEvaluator);
            Assert.Same(mockedValueEvaluator, registeredValueEvaluator);
        }

        [Fact]
        public void RegisterDefaultValueEvaluator_Null_Throws()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            Assert.Throws<ArgumentNullException>(() =>
                subjectUnderTest.RegisterDefaultValueEvaluator(null));
        }

        [Fact]
        public void RegisterDefaultValueEvaluator_Success()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory(false);

            var mockedValueEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterDefaultValueEvaluator(mockedValueEvaluator);

            Assert.Same(mockedValueEvaluator, subjectUnderTest.DefaultValueEvaluator);
        }

        [Fact]
        public void GetValueEvaluator_Success()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory();

            var mockedValueEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterValueEvaluator(typeof(object), mockedValueEvaluator);

            var registeredValueEvaluator = subjectUnderTest.GetValueEvaluator(typeof(object));
            Assert.NotNull(registeredValueEvaluator);
            Assert.Same(mockedValueEvaluator, registeredValueEvaluator);
        }

        [Fact]
        public void GetValueEvaluator_NoDefaultEvaluatorSet_ReturnsNull()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory(false);

            var mockedValueEvaluator = Mock.Of<IValueEvaluator>();

            var registeredValueEvaluator = subjectUnderTest.GetValueEvaluator(typeof(object));
            Assert.Null(registeredValueEvaluator);
        }

        [Fact]
        public void GetValueEvaluator_DefaultEvaluatorSet_ReturnsDefault()
        {
            ValueEvaluatorFactory subjectUnderTest = new ValueEvaluatorFactory(false);

            var mockedValueEvaluator = Mock.Of<IValueEvaluator>();

            subjectUnderTest.RegisterDefaultValueEvaluator(mockedValueEvaluator);

            var registeredValueEvaluator = subjectUnderTest.GetValueEvaluator(typeof(object));
            Assert.NotNull(registeredValueEvaluator);
            Assert.Same(mockedValueEvaluator, registeredValueEvaluator);
        }
    }
}
