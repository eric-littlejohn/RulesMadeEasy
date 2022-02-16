using RulesMadeEasy.Core;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class SByteValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, (sbyte)1, (sbyte)1));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, (sbyte)1, (sbyte)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, (sbyte)1, (sbyte)2);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, (sbyte)1, (sbyte)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, (sbyte)1, (sbyte)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, (sbyte)1, (sbyte)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, (sbyte)1, (sbyte)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, (sbyte)1, (sbyte)2);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, (sbyte)1, (sbyte)0);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, (sbyte)2, (sbyte)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, (sbyte)1, (sbyte)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, (sbyte)1, (sbyte)1);

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new SByteValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, (sbyte)0, (sbyte)1);

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
