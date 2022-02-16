using System;
using System.Collections.Generic;
using System.Text;
using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class DataValueTests
    {
        [Fact]
        public void Equals_Success()
        {
            var dataValueA = new DataValue("KeyA", "SomeString");
            var dataValueB = new DataValue("KeyB", "SomeString");

            Assert.True(dataValueA.Equals(dataValueB), 
                "Data value equality failed when it should have passed");
        }

        [Fact]
        public void Equals_Fails()
        {
            var dataValueA = new DataValue("KeyA", "SomeString");
            var dataValueB = new DataValue("KeyB", "SomeString2");

            Assert.False(dataValueA.Equals(dataValueB),
                "Data value equality passed when it should have failed");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_InvalidKey_Throws(string key)
        {
            Assert.Throws<ArgumentException>(() => new DataValue(key, "someValue"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Ctor_Generic_InvalidKey_Throws(string key)
        {
            Assert.Throws<ArgumentException>(() => new DataValue<string>(key, "someValue"));
        }

        [Fact]
        public void Ctor_Success()
        {
            var key = "SomeKey1";
            var value = 1;

            var subjectUnderTest = new DataValue(key, value);

            Assert.Equal(key, subjectUnderTest.Key);
            Assert.Equal(value, subjectUnderTest.Value);
            Assert.Equal(value.GetType(), subjectUnderTest.ValueType);
        }

        [Fact]
        public void Ctor_Generic_Success()
        {
            var key = "SomeKey1";
            var value = 1;

            var subjectUnderTest = new DataValue<int>(key, value);

            Assert.Equal(key, subjectUnderTest.Key);
            Assert.Equal(value, subjectUnderTest.Value);
            Assert.Equal(value.GetType(), subjectUnderTest.ValueType);
        }
    }
}
