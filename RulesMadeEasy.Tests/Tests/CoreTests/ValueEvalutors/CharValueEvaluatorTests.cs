using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class CharValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, (char)1, (char)1));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, (char)1, (char)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, (char)1, (char)2);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, (char)1, (char)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, (char)1, (char)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, (char)1, (char)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, (char)1, (char)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, (char)1, (char)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, (char)1, (char)0);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, (char)2, (char)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, (char)1, (char)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, (char)1, (char)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new CharValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, (char)0, (char)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
