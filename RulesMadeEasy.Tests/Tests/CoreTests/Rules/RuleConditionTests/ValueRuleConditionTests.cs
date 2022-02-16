using System;
using System.Collections.Generic;
using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class ValueRuleConditionTests
    {
        [Fact]
        public void Ctor_InvalidOperator_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ValueRuleCondition(ConditionOperator.And, "Value1"));
        }

        [Fact]
        public void Ctor_InvalidKey_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ValueRuleCondition(ConditionOperator.Equal, ""));
        }

        [Fact]
        public void Ctor_Success()
        {
            var valueKey = "Value1";
            var value = 2;
            var subjectUnderTest = new ValueRuleCondition(ConditionOperator.Equal, valueKey, value);

            Assert.NotNull(subjectUnderTest);
            Assert.Equal(ConditionOperator.Equal, subjectUnderTest.Operator);
            Assert.Equal(valueKey, subjectUnderTest.ValueKey);
            Assert.Equal(value, subjectUnderTest.ExpectedValue);
        }

        [Fact]
        public void Compile_NoKeyFound_Throws()
        {
            var subjectUnderTest = new ValueRuleCondition(ConditionOperator.Equal, "Value1", 3);

            var raisedExc =
                Assert.Throws<ConditionEvaluationException>(() => subjectUnderTest
                    .Compile(new List<IDataValue>()));

            Assert.Equal(ConditionEvaluationException.ExceptionCause.NoDataValueFound, raisedExc.Cause);
        }

        [Fact]
        public void Compile_Success()
        {
            var dataValue = new DataValue("Value1", 3);

            var subjectUnderTest = new ValueRuleCondition(ConditionOperator.Equal, "Value1", 3);

            ICondition compiledCondition = subjectUnderTest.Compile(new[] {dataValue});

            var castedCondition = compiledCondition as IValueCondition;

            Assert.NotNull(castedCondition);
            Assert.Equal(subjectUnderTest.Operator, compiledCondition.Operator);
            Assert.Equal(subjectUnderTest.ValueKey, castedCondition.LeftOperand.Key);
            Assert.Equal(subjectUnderTest.ExpectedValue, castedCondition.LeftOperand.Value);
            Assert.Equal(dataValue.Key, castedCondition.RightOperand.Key);
            Assert.Equal(dataValue.Value, castedCondition.RightOperand.Value);
        }
    }
}