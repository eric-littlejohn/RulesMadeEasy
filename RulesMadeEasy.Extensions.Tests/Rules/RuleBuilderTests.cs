using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Moq.Protected;
using RulesMadeEasy.Core;
using RulesMadeEasy.Extensions;
using Xunit;

namespace RulesMadeEasy.Extensions.Tests
{
    public class RuleBuilderTests
    {
        [Fact]
        public void AddRuleCondition_Success()
        {
            List<IRuleCondition> builderConditions = new List<IRuleCondition>();

            var mockedRuleBuilder = new Mock<RuleBuilder>() { CallBase = true };

            mockedRuleBuilder.Protected().SetupGet<List<IRuleCondition>>("RuleConditions")
                .Returns(builderConditions);

            var subjectUnderTest = mockedRuleBuilder.Object;

            var conditionOperator = ConditionOperator.Equal;
            var valueKey = "TestKey1";
            var value = "abc";

            var ruleValCondition = new ValueRuleCondition(conditionOperator, valueKey, value);

            subjectUnderTest.AddRuleCondition(ruleValCondition);

            Assert.Single(builderConditions);
            Assert.NotNull(builderConditions[0] as IValueRuleCondition);
            Assert.Equal(conditionOperator, ((IValueRuleCondition)builderConditions[0]).Operator);
            Assert.Equal(valueKey, ((IValueRuleCondition)builderConditions[0]).ValueKey);
            Assert.Equal(value, ((IValueRuleCondition)builderConditions[0]).ExpectedValue);
        }

        [Fact]
        public void AddLogicalCondition_Success()
        {
            List<IRuleCondition> builderConditions = new List<IRuleCondition>();

            var mockedRuleBuilder = new Mock<RuleBuilder>() {CallBase = true};

            mockedRuleBuilder.Protected().SetupGet<List<IRuleCondition>>("RuleConditions")
                .Returns(builderConditions);

            var subjectUnderTest = mockedRuleBuilder.Object;

            var ruleValCondition1 = new ValueRuleCondition(ConditionOperator.Equal, "TestKey1", 1);
            var ruleValCondition2 = new ValueRuleCondition(ConditionOperator.Equal, "TestKey2", "abc");

            subjectUnderTest.AddLogicalCondition(ConditionOperator.And, ruleValCondition1, ruleValCondition2);

            Assert.Single(builderConditions);
            Assert.NotNull(builderConditions[0] as ILogicalRuleCondition);
            Assert.Collection(((ILogicalRuleCondition) builderConditions[0]).NestedConditions, 
                condition1 => Assert.Same(ruleValCondition1, condition1),
                condition2 => Assert.Same(ruleValCondition2, condition2));
        }

        [Fact]
        public void AddValueCondition_Success()
        {
            List<IRuleCondition> builderConditions = new List<IRuleCondition>();

            var mockedRuleBuilder = new Mock<RuleBuilder>() { CallBase = true };

            mockedRuleBuilder.Protected().SetupGet<List<IRuleCondition>>("RuleConditions")
                .Returns(builderConditions);

            var subjectUnderTest = mockedRuleBuilder.Object;

            var conditionOperator = ConditionOperator.Equal;
            var valueKey = "TestKey1";
            var value = "abc";

            subjectUnderTest.AddValueCondition(conditionOperator, valueKey, value);

            Assert.Single(builderConditions);
            Assert.NotNull(builderConditions[0] as IValueRuleCondition);
            Assert.Equal(conditionOperator, ((IValueRuleCondition) builderConditions[0]).Operator);
            Assert.Equal(valueKey, ((IValueRuleCondition)builderConditions[0]).ValueKey);
            Assert.Equal(value, ((IValueRuleCondition)builderConditions[0]).ExpectedValue);
        }

        [Fact]
        public void Build_Success()
        { 
            var subjectUnderTest = new RuleBuilder();

            var conditionOperator = ConditionOperator.Equal;
            var valueKey = "TestKey3";
            var value = new object();

            var ruleValCondition1 = new ValueRuleCondition(ConditionOperator.Equal, "TestKey1", 1);
            var ruleValCondition2 = new ValueRuleCondition(ConditionOperator.Equal, "TestKey2", "abc");

            var builtRule = subjectUnderTest
                .AddLogicalCondition(ConditionOperator.And, ruleValCondition1, ruleValCondition2)
                .AddValueCondition(conditionOperator, valueKey, value)
                .Build();

            Assert.NotNull(builtRule);
            Assert.Equal(2, builtRule.Conditions.Count());
            Assert.Collection(builtRule.Conditions,
                logicalCondition =>
                    {
                        var actualLogicalCondition = logicalCondition as ILogicalRuleCondition;
                        Assert.NotNull(actualLogicalCondition);
                        Assert.Equal(2, actualLogicalCondition.NestedConditions.Count());
                        Assert.Collection(actualLogicalCondition.NestedConditions,
                            condition1 => Assert.Same(ruleValCondition1, condition1),
                            condition2 => Assert.Same(ruleValCondition2, condition2));
                    },
                valueCondition =>
                {
                    var actualValueCondition = valueCondition as IValueRuleCondition;
                    Assert.NotNull(actualValueCondition);
                    Assert.Equal(conditionOperator, actualValueCondition.Operator);
                    Assert.Equal(valueKey, actualValueCondition.ValueKey);
                    Assert.Equal(value, actualValueCondition.ExpectedValue);
                });
        }

        [Fact]
        public void CreateInCondition_Success()
        {
            void VerifyNestedCondition(IRuleCondition actualRuleCondition, ConditionOperator expectedConditionOperator,
                object expectedConditionValue)
            {
                var castedCondition = actualRuleCondition as IValueRuleCondition;

                Assert.NotNull(castedCondition);
                Assert.Equal(expectedConditionOperator, castedCondition.Operator);
                Assert.Equal(expectedConditionValue, castedCondition.ExpectedValue);
            }

            var subjectUnderTest = new RuleBuilder();

            var builtRule = subjectUnderTest
                .CreateInCondition("Value1", 1, 2, 3)
                .Build();

            Assert.IsType<Rule>(builtRule);
            var castedRule = builtRule as Rule;
            Assert.Single(castedRule.Conditions);
            var condition = castedRule.Conditions.First();
            Assert.IsType<LogicalRuleCondition>(condition);
            var logicalCondition = condition as LogicalRuleCondition;

            Assert.Equal(ConditionOperator.Or, logicalCondition.Operator);
            Assert.Collection(logicalCondition.NestedConditions,
                condition => VerifyNestedCondition(condition, ConditionOperator.Equal, 1),
                condition => VerifyNestedCondition(condition, ConditionOperator.Equal, 2),
                condition => VerifyNestedCondition(condition, ConditionOperator.Equal, 3));
        }

        [Fact]
        public void CreateNotInCondition_Success()
        {
            void VerifyNestedCondition(IRuleCondition actualRuleCondition, ConditionOperator expectedConditionOperator,
                object expectedConditionValue)
            {
                var castedCondition = actualRuleCondition as IValueRuleCondition;

                Assert.NotNull(castedCondition);
                Assert.Equal(expectedConditionOperator, castedCondition.Operator);
                Assert.Equal(expectedConditionValue, castedCondition.ExpectedValue);
            }

            var subjectUnderTest = new RuleBuilder();

            var builtRule = subjectUnderTest
                .CreateNotInCondition("Value1", 1, 2, 3)
                .Build();

            Assert.IsType<Rule>(builtRule);
            var castedRule = builtRule as Rule;
            Assert.Single(castedRule.Conditions);
            var condition = castedRule.Conditions.First();
            Assert.IsType<LogicalRuleCondition>(condition);
            var logicalCondition = condition as LogicalRuleCondition;

            Assert.Equal(ConditionOperator.And, logicalCondition.Operator);
            Assert.Collection(logicalCondition.NestedConditions,
                condition => VerifyNestedCondition(condition, ConditionOperator.NotEqual, 1),
                condition => VerifyNestedCondition(condition, ConditionOperator.NotEqual, 2),
                condition => VerifyNestedCondition(condition, ConditionOperator.NotEqual, 3));
        }

        [Fact]
        public void AddRuleAction_Success()
        {
            List<object> builderActions = new List<object>();

            var mockedRuleBuilder = new Mock<RuleBuilder>() { CallBase = true };

            mockedRuleBuilder.Protected().SetupGet<List<object>>("RuleActionIdentifiers")
                .Returns(builderActions);

            var subjectUnderTest = mockedRuleBuilder.Object;

            var actionId = Guid.NewGuid();

            subjectUnderTest.AddRuleAction(actionId);

            Assert.Single(builderActions);
            Assert.Equal(actionId, builderActions[0]);
        }
    }
}
