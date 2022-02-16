using System;
using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public partial class ConditionTests
    {
        [Fact]
        public void ValueCondition_Ctor_ValidOperator_Pass()
        {
            IDataValue leftOp = new DataValue("Val1", "1");
            IDataValue rightOp = new DataValue("Val1", "1");
            ConditionOperator conditionOperator = ConditionOperator.Equal;

            var conditionUnderTest = new ValueCondition(conditionOperator, leftOp, rightOp);

            Assert.NotNull(conditionUnderTest);
            Assert.Equal(conditionOperator, conditionUnderTest.Operator);
            Assert.Same(leftOp, conditionUnderTest.LeftOperand);
            Assert.Same(rightOp, conditionUnderTest.RightOperand);
        }

        [Fact]
        public void ValueCondition_Ctor_InvalidOperator_Throws()
        {
            IDataValue leftOp =  new DataValue("Val1", "1");
            IDataValue rightOp = new DataValue("Val1", "1");
            ConditionOperator conditionOperator = ConditionOperator.And;

            Assert.Throws<ArgumentException>(() => new ValueCondition(conditionOperator, leftOp, rightOp));
        }
    }
}
