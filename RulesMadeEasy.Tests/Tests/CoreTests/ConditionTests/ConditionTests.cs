using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public partial class ConditionTests
    {
        [Theory]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public void IsLogicalOperator_Passes(ConditionOperator conditionOperator)
        {
            Assert.True(Condition.IsLogicalOperator(conditionOperator), 
                $"IsLogicalOperator test for {conditionOperator} failed when it should have passed");
        }

        [Theory]
        [InlineData(ConditionOperator.Equal)]
        [InlineData(ConditionOperator.NotEqual)]
        [InlineData(ConditionOperator.LessThan)]
        [InlineData(ConditionOperator.LessEqualTo)]
        [InlineData(ConditionOperator.GreaterThan)]
        [InlineData(ConditionOperator.GreaterThanEqualTo)]
        public void IsLogicalOperator_Fails(ConditionOperator conditionOperator)
        {
            Assert.False(Condition.IsLogicalOperator(conditionOperator),
                $"IsLogicalOperator test for {conditionOperator} passed when it should have failed");
        }

        [Theory]
        [InlineData(ConditionOperator.Equal)]
        [InlineData(ConditionOperator.NotEqual)]
        [InlineData(ConditionOperator.LessThan)]
        [InlineData(ConditionOperator.LessEqualTo)]
        [InlineData(ConditionOperator.GreaterThan)]
        [InlineData(ConditionOperator.GreaterThanEqualTo)]
        public void IsValueOperator_Passes(ConditionOperator conditionOperator)
        {
            Assert.True(Condition.IsValueOperator(conditionOperator),
                $"IsValueOperator test for {conditionOperator} failed when it should have passed");
        }

        [Theory]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public void IsValueOperator_Fails(ConditionOperator conditionOperator)
        {
            Assert.False(Condition.IsValueOperator(conditionOperator),
                $"IsValueOperator test for {conditionOperator} passed when it should have failed");
        }
    }
}
