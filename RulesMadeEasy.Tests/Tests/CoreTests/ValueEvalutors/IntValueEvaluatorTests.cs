using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class IntValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, 1, 1));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, 1, 1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, 1, 2);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, 1, 2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, 1, 1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, 1, 2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, 1, 1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, 1, 2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, 1, 0);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, 2, 1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, 1, 1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, 1, 1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new IntValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, 0, 1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

    }
}
