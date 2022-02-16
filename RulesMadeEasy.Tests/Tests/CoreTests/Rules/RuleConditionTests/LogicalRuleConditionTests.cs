using System;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class LogicalRuleConditionTests
    {
        [Fact]
        public void Ctor_InvalidOperator_Throws()
        {
            Assert.Throws<ArgumentException>(() => new LogicalRuleCondition(ConditionOperator.Equal));
        }

        [Fact]
        public void Ctor_Success()
        {
            void VerifyNestedCondition(IRuleCondition expectedRuleCondition, IRuleCondition actualRuleCondition)
            {
                var castedCondition = actualRuleCondition as IValueRuleCondition;

                Assert.NotNull(castedCondition);
                Assert.Same(expectedRuleCondition, actualRuleCondition);
            }

            var nestedCondition1 = new ValueRuleCondition(ConditionOperator.Equal, "Value1", 1);
            var nestedCondition2 = new ValueRuleCondition(ConditionOperator.Equal, "Value2", "Test");

            var subjectUnderTest = new LogicalRuleCondition(ConditionOperator.And, nestedCondition1, nestedCondition2);

            Assert.NotNull(subjectUnderTest);
            Assert.Collection(subjectUnderTest.NestedConditions, 
                condition =>VerifyNestedCondition(nestedCondition1, condition),
                condition =>VerifyNestedCondition(nestedCondition2, condition));
        }


        [Fact]
        public void Compile_LessThanTwoConditions_Throws()
        {
            var nestedCondition1 = new ValueRuleCondition(ConditionOperator.Equal, "Value1", 1);

            var subjectUnderTest = new LogicalRuleCondition(ConditionOperator.And, nestedCondition1);

            var raisedExc =
                Assert.Throws<ConditionEvaluationException>(() => subjectUnderTest
                    .Compile(new IDataValue[] { new DataValue("Value1", 1) }));

            Assert.Equal(ConditionEvaluationException.ExceptionCause.InvalidConditionValueCount, raisedExc.Cause);
        }

        [Fact]
        public void Compile_Success()
        {
            void VerifyNestedCondition(IValueRuleCondition originalRuleCondition, IDataValue associatedDataValue, ICondition actualCondition)
            {
                var castedNestedCondition = actualCondition as IValueCondition;

                Assert.NotNull(castedNestedCondition);
                Assert.Equal(originalRuleCondition.ValueKey, castedNestedCondition.LeftOperand.Key);
                Assert.Equal(originalRuleCondition.ExpectedValue, castedNestedCondition.LeftOperand.Value);
                Assert.Equal(associatedDataValue.Key, castedNestedCondition.RightOperand.Key);
                Assert.Equal(associatedDataValue.Value, castedNestedCondition.RightOperand.Value);
            }

            var nestedCondition1 = new ValueRuleCondition(ConditionOperator.Equal, "Value1", 3);
            var nestedCondition1DataValue = new DataValue("Value1", 3);
            var nestedCondition2 = new ValueRuleCondition(ConditionOperator.Equal, "Value2", "Test");
            var nestedCondition2DataValue = new DataValue("Value2", "Test");

            var subjectUnderTest = new LogicalRuleCondition(ConditionOperator.And, nestedCondition1, nestedCondition2);

            ICondition compiledCondition = subjectUnderTest.Compile(new [] { nestedCondition1DataValue, nestedCondition2DataValue});

            var castedCondition = compiledCondition as ILogicalCondition;

            Assert.NotNull(castedCondition);
            Assert.Equal(subjectUnderTest.Operator, compiledCondition.Operator);
            VerifyNestedCondition(nestedCondition1, nestedCondition1DataValue, castedCondition.LeftOperand);
            VerifyNestedCondition(nestedCondition2, nestedCondition2DataValue, castedCondition.RightOperand);
        }
    }
}
