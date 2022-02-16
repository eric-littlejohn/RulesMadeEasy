using RulesMadeEasy.Core;
using System;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class GuidValueEvaluatorTests
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
            IValueEvaluator subjectUnderTest = new GuidValueEvaluator();

            var exc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.Evaluate(conditionOperator, Guid.NewGuid(), Guid.NewGuid()));

            Assert.NotNull(exc);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnsupportedOperator, exc.Cause);
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new GuidValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal, 
                new Guid("205E6C7B-C6CC-4B75-B3DA-83EBA46628AA"), new Guid("205E6C7B-C6CC-4B75-B3DA-83EBA46628AA"));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Success()
        {
            IValueEvaluator subjectUnderTest = new GuidValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual,
                new Guid("205E6C7B-C6CC-4B75-B3DA-83EBA46628AA"), new Guid("505E6C7B-C6CC-4B75-B3DA-83EBA46628AB"));

            Assert.True(result, "Evaluation failed when it shouldve passed");
        }

        [Fact]
        public async void Evaluate_EqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new GuidValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.Equal,
                new Guid("205E6C7B-C6CC-4B75-B3DA-83EBA46628AA"), new Guid("505E6C7B-C6CC-4B75-B3DA-83EBA46628AB"));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }

        [Fact]
        public async void Evaluate_NotEqualsOperator_Fails()
        {
            IValueEvaluator subjectUnderTest = new GuidValueEvaluator();

            var result = await subjectUnderTest.Evaluate(ConditionOperator.NotEqual,
                new Guid("205E6C7B-C6CC-4B75-B3DA-83EBA46628AA"), new Guid("205E6C7B-C6CC-4B75-B3DA-83EBA46628AA"));

            Assert.False(result, "Evaluation passed when it shouldve failed");
        }
    }
}
