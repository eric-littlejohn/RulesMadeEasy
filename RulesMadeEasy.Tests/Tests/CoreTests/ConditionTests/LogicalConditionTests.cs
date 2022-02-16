using System;
using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public partial class ConditionTests
    {
        [Fact]
        public void LogicalCondition_Ctor_ValidOperator_Pass()
        {
            ICondition leftOp = new ValueCondition(ConditionOperator.Equal, 
                new DataValue("Val1", "1"), new DataValue("Val2", "1"));
            ICondition rightOp = new ValueCondition(ConditionOperator.NotEqual,
                new DataValue("Val1", 1), new DataValue("Val2", "1"));
            ConditionOperator conditionOperator = ConditionOperator.And;

            var conditionUnderTest = new LogicalCondition(conditionOperator, leftOp, rightOp);

            Assert.NotNull(conditionUnderTest);
            Assert.Equal(conditionOperator, conditionUnderTest.Operator);
            Assert.Same(leftOp, conditionUnderTest.LeftOperand);
            Assert.Same(rightOp, conditionUnderTest.RightOperand);
        }

        [Fact]
        public void LogicalCondition_Ctor_InvalidOperator_Throws()
        {
            ICondition leftOp = new ValueCondition(ConditionOperator.Equal,
                new DataValue("Val1", "1"), new DataValue("Val2", "1"));
            ICondition rightOp = new ValueCondition(ConditionOperator.NotEqual,
                new DataValue("Val1", 1), new DataValue("Val2", "1"));
            ConditionOperator conditionOperator = ConditionOperator.Equal;

            Assert.Throws<ArgumentException>(() => new LogicalCondition(conditionOperator, leftOp, rightOp));
        }
    }
}
