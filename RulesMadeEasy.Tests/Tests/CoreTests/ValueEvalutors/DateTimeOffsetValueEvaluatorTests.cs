using RulesMadeEasy.Core;
using System;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class DateTimeOffsetValueEvaluatorTests
    {
        [Theory]
        [InlineData(ConditionOperator.Unspecified)]
        [InlineData(ConditionOperator.And)]
        [InlineData(ConditionOperator.Or)]
        public async void Evaluate_InvalidConditionOperator_Throws(ConditionOperator conditionOperator)
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero)));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 27, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 27, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, 
                new DateTimeOffset(2018, 10, 27, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessThan, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_LessThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.LessEqualTo, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 27, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, 
                new DateTimeOffset(2018, 10, 29, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThan, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, 
                new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_GreaterThanEqualToOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new DateTimeOffsetValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.GreaterThanEqualTo, 
                new DateTimeOffset(2018, 10, 27, 0, 0, 0, 0, TimeSpan.Zero), new DateTimeOffset(2018, 10, 28, 0, 0, 0, 0, TimeSpan.Zero));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
