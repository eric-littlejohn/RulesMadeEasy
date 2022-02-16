using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class ObjectValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        [InlineData(ConditionOperator.LessThan)]
        [InlineData(ConditionOperator.LessEqualTo)]
        [InlineData(ConditionOperator.GreaterThan)]
        [InlineData(ConditionOperator.GreaterThanEqualTo)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new ObjectValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, new { a = "Test" }, new { a = "Test" }));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new ObjectValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, 
                new { a = "Test" }, new { a = "Test" });

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new ObjectValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual,
                new { a = "Test" }, new { b = "Test" });

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new ObjectValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal,
                new { a = "Test" }, new { b = "Test" });

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new ObjectValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual,
                new { a = "Test" }, new { a = "Test" });

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
