using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class LongValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, (long)1, (long)1));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, (long)1, (long)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, (long)1, (long)2);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, (long)1, (long)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, (long)1, (long)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, (long)1, (long)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, (long)1, (long)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, (long)1, (long)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, (long)1, (long)0);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, (long)2, (long)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, (long)1, (long)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, (long)1, (long)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new LongValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, (long)0, (long)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
