using RulesMadeEasy.Core;
using System;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class DateTimeValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28)));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, new DateTime(2018, 10, 28), new DateTime(2018, 10, 27));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, new DateTime(2018, 10, 28), new DateTime(2018, 10, 27));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, new DateTime(2018, 10, 27), new DateTime(2018, 10, 28));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, new DateTime(2018, 10, 28), new DateTime(2018, 10, 27));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, new DateTime(2018, 10, 29), new DateTime(2018, 10, 28));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, new DateTime(2018, 10, 28), new DateTime(2018, 10, 28));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, new DateTime(2018, 10, 27), new DateTime(2018, 10, 28));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
