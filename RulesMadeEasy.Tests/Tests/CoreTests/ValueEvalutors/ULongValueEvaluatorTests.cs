using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class ULongValueEvaluatorTests
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
            IValueEvaluator subjectUnderTest = new BooleanValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, true, false));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new BooleanValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, true, true);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new BooleanValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, true, false);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new BooleanValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, true, false);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new BooleanValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, true, true);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
