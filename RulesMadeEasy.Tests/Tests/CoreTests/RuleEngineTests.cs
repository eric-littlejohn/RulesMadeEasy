using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using RulesMadeEasy.Core;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace RulesMadeEasy.Core.Tests
{
    public class RuleEngineTests : IClassFixture<RulesMadeEasyFixture>
    {
        private readonly RulesMadeEasyFixture _fixture;

        /// <summary>
        /// Wraps protected methods of the <see cref="RulesMadeEasyEngine"/> implementation
        /// </summary>
        /// <remarks>Used for Unit testing</remarks>
        private class RulesMadeEasyEngine_PublicWrapper : RulesMadeEasyEngine
        {
            public RulesMadeEasyEngine_PublicWrapper(IValueEvaluatorFactory valueFactory, IActionFactory actionFactory)
                : base(valueFactory, actionFactory)
            {
                
            }

            public new IRuleEvaluationResult CreateRuleEvaluationResult(IRule rule,
                IEnumerable<IConditionEvaluationResult> conditionResults,
                IEnumerable<IActionExecutionResult> actionExecutionResults)
            {
                return base.CreateRuleEvaluationResult(rule, conditionResults, actionExecutionResults);
            }

            public new IRuleEvaluationResult CreateRuleEvaluationResult(IRule rule,
                IEnumerable<IConditionEvaluationResult> conditionResults,
                IEnumerable<IActionExecutionResult> actionExecutionResults,
                Exception exc)
            {
                return base.CreateRuleEvaluationResult(rule, conditionResults, actionExecutionResults, exc);
            }

            public new async Task<IConditionEvaluationResult> EvaluateCondition(ICondition condition)
            {
                return await base.EvaluateCondition(condition);
            }

            public new async Task<IActionExecutionResult> ExecuteRuleAction(RuleEngineEvaluationMode evaluationMode,
            IEnumerable<IDataValue> dataValues,
            Guid actionId,
            bool stopActionExecution)
            {
                return await base.ExecuteRuleAction(evaluationMode, dataValues, actionId, stopActionExecution);
            }
        }

        public RuleEngineTests(RulesMadeEasyFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void Ctor_NoValueEvaluatorFactory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RulesMadeEasyEngine(null, Mock.Of<IActionFactory>()));
        }

        [Fact]
        public void Ctor_NoActionFactory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new RulesMadeEasyEngine(Mock.Of<IValueEvaluatorFactory>(), null));
        }

        #region Rule Evaluation Tests

        #endregion

        #region Condition Evaluation Tests

        [Fact]
        public async void EvaluateCondition_NullCondition_Throws()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var raisedExc = await Assert.ThrowsAsync<ConditionEvaluationException>(() =>
                subjectUnderTest.EvaluateCondition(null));

            Assert.Equal(ConditionEvaluationException.ExceptionCause.NullConditionProvided,
                raisedExc.Cause);
        }

        [Fact]
        public async void EvaluateCondition_UnknownConditionType_Fails()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var result = await subjectUnderTest.EvaluateCondition(Mock.Of<ICondition>());

            Assert.False(result.ConditionPassed);
            Assert.NotNull(result.Exception);
            Assert.IsType<ConditionEvaluationException>(result.Exception);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnableToDetermineConditionType, result.Exception.Cause);
        }

        [Fact]
        public async void EvaluateCondition_LogicalCondition_AndOperator_Passes()
        {
            var valueEvaluators = new Dictionary<Type, IValueEvaluator>
            {
                [typeof(int)] = _fixture.CreateValueEvaluator(true)             
            };

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(valueEvaluators), _fixture.CreateActionFactory());

            var leftOperand = GenerateValueCondition(ConditionOperator.Equal, 2, 2);

            var rightOperand = GenerateValueCondition(ConditionOperator.NotEqual, 2, 2);

            var logicalConditon = new LogicalCondition(ConditionOperator.And, 
                leftOperand, rightOperand);

            var conditionResult = await subjectUnderTest.EvaluateCondition(logicalConditon);

            Assert.True(conditionResult.ConditionPassed);
            Assert.Null(conditionResult.Exception);
            Assert.Same(leftOperand, conditionResult.LeftOperand);
            Assert.Same(rightOperand, conditionResult.RightOperand);
            Assert.All(conditionResult.NestedConditionResults, 
                result => Assert.True(result.ConditionPassed));
        }

        [Fact]
        public async void EvaluateCondition_LogicalCondition_AndOperator_Fails()
        {
            var valueEvaluators = new Dictionary<Type, IValueEvaluator>
            {
                [typeof(int)] = _fixture.CreateValueEvaluator(new Dictionary<ConditionOperator, bool>
                {
                    [ConditionOperator.Equal] = true,
                    [ConditionOperator.NotEqual] = false
                })
            };

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(valueEvaluators), _fixture.CreateActionFactory());

            //Expected true condition. 
            var leftOperand = GenerateValueCondition(ConditionOperator.Equal, 2, 2);

            //Expected false condition
            var rightOperand = GenerateValueCondition(ConditionOperator.NotEqual, 2, 2);

            var logicalConditon = new LogicalCondition(ConditionOperator.And,
                leftOperand, rightOperand);

            var conditionResult = await subjectUnderTest.EvaluateCondition(logicalConditon);

            Assert.False(conditionResult.ConditionPassed);
            Assert.Null(conditionResult.Exception);
            Assert.Same(leftOperand, conditionResult.LeftOperand);
            Assert.Same(rightOperand, conditionResult.RightOperand);
            Assert.Collection(conditionResult.NestedConditionResults,
                //First condition should fail
                result => Assert.True(result.ConditionPassed,
                    "Condition failed when it shouldve passed"), 
                //Second condition should fail
                result => Assert.False(result.ConditionPassed,
                    "Condition passed when it shouldve failed"));
        }

        [Fact]
        public async void EvaluateCondition_LogicalCondition_OrOperator_Passes()
        {
            var valueEvaluators = new Dictionary<Type, IValueEvaluator>
            {
                [typeof(int)] = _fixture.CreateValueEvaluator(new Dictionary<ConditionOperator, bool>
                {
                    [ConditionOperator.Equal] = false,
                    [ConditionOperator.NotEqual] = true
                })
            };

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(valueEvaluators), _fixture.CreateActionFactory());

            //False Condition
            var leftOperand = GenerateValueCondition(ConditionOperator.Equal, 1, 2);

            //True condition
            var rightOperand = GenerateValueCondition(ConditionOperator.NotEqual, 1, 2);

            var logicalConditon = new LogicalCondition(ConditionOperator.Or,
                leftOperand, rightOperand);

            var conditionResult = await subjectUnderTest.EvaluateCondition(logicalConditon);

            Assert.True(conditionResult.ConditionPassed);
            Assert.Null(conditionResult.Exception);
            Assert.Same(leftOperand, conditionResult.LeftOperand);
            Assert.Same(rightOperand, conditionResult.RightOperand);
            Assert.Collection(conditionResult.NestedConditionResults,
                //First condition should fail
                result => Assert.False(result.ConditionPassed,
                    "Condition passed when it shouldve failed"),
                //Second condition should pass
                result => Assert.True(result.ConditionPassed,
                    "Condition failed when it shouldve passed")          
                );
        }

        [Fact]
        public async void EvaluateCondition_LogicalCondition_OrOperator_Fails()
        {
            var valueEvaluators = new Dictionary<Type, IValueEvaluator>
            {
                [typeof(int)] = _fixture.CreateValueEvaluator(false)
            };

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(valueEvaluators), _fixture.CreateActionFactory());

            //Expected false condition. 
            var leftOperand = GenerateValueCondition(ConditionOperator.Equal, 2, 1);

            //Expected false condition
            var rightOperand = GenerateValueCondition(ConditionOperator.NotEqual, 2, 2);

            var logicalConditon = new LogicalCondition(ConditionOperator.Or,
                leftOperand, rightOperand);

            var conditionResult = await subjectUnderTest.EvaluateCondition(logicalConditon);

            Assert.False(conditionResult.ConditionPassed);
            Assert.Null(conditionResult.Exception);
            Assert.Same(leftOperand, conditionResult.LeftOperand);
            Assert.Same(rightOperand, conditionResult.RightOperand);
            Assert.All(conditionResult.NestedConditionResults,
                result => Assert.False(result.ConditionPassed));
        }

        [Fact]
        public async void EvaluateCondition_ValueCondition_MissingLeftOperand_Fails()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var valueConditon = new ValueCondition(ConditionOperator.Equal, null, new DataValue("SomeKey", 1));

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.False(result.ConditionPassed);
            Assert.NotNull(result.Exception);
            Assert.IsType<ConditionEvaluationException>(result.Exception);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.OperandMissing, result.Exception.Cause);
        }

        [Fact]
        public async void EvaluateCondition_ValueCondition_MissingRightOperand_Fails()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var valueConditon = new ValueCondition(ConditionOperator.Equal, new DataValue("SomeKey", 1), null);

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.False(result.ConditionPassed);
            Assert.NotNull(result.Exception);
            Assert.IsType<ConditionEvaluationException>(result.Exception);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.OperandMissing, result.Exception.Cause);
        }

        [Fact]
        public async void EvaluateCondition_ValueCondition_MissingValueType_Fails()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var valueConditon = new ValueCondition(ConditionOperator.Equal, new DataValue("SomeKey", null), 
                new DataValue("SomeKey", null));

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.False(result.ConditionPassed);
            Assert.NotNull(result.Exception);
            Assert.IsType<ConditionEvaluationException>(result.Exception);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.UnableToDetermineValueType, result.Exception.Cause);
        }

        [Fact]
        public async void EvaluateCondition_ValueCondition_ValueTypeMismatch_Fails()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var valueConditon = new ValueCondition(ConditionOperator.Equal, new DataValue("SomeKey", 1),
                new DataValue("SomeKey", "1"));

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.False(result.ConditionPassed);
            Assert.NotNull(result.Exception);
            Assert.IsType<ConditionEvaluationException>(result.Exception);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.ValueTypeMismatch, result.Exception.Cause);
        }

        [Fact]
        public async void EvaluateCondition_ValueCondition_MissingValueEvaluator_Fails()
        {
            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(), _fixture.CreateActionFactory());

            var valueConditon = new ValueCondition(ConditionOperator.Equal, new DataValue("SomeKey", 1),
                new DataValue("SomeKey", 1));

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.False(result.ConditionPassed);
            Assert.NotNull(result.Exception);
            Assert.IsType<ConditionEvaluationException>(result.Exception);
            Assert.Equal(ConditionEvaluationException.ExceptionCause.EvaluatorNotFound, result.Exception.Cause);
        }

        [Theory]
        [InlineData(ConditionOperator.Equal, 1, 1)]
        [InlineData(ConditionOperator.NotEqual, 1, 2)]
        [InlineData(ConditionOperator.LessThan, 2, 1)]
        [InlineData(ConditionOperator.LessEqualTo, 1, 2)]
        [InlineData(ConditionOperator.LessEqualTo, 2, 2)]
        [InlineData(ConditionOperator.GreaterThan, 1, 2)]
        [InlineData(ConditionOperator.GreaterThanEqualTo, 2, 2)]
        [InlineData(ConditionOperator.GreaterThanEqualTo, 3, 2)]

        public async void EvaluateCondition_ValueCondition_Operator_Passes(ConditionOperator op, int val1, int val2)
        {
            var valueEvaluators = new Dictionary<Type, IValueEvaluator>
            {
                [typeof(int)] = _fixture.CreateValueEvaluator(new Dictionary<ConditionOperator, bool>
                {
                    [op] = true,
                    //Everything else will return false
                })
            };

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(valueEvaluators), _fixture.CreateActionFactory());

            var leftDataValue = new DataValue("SomeKey", val1);
            var rightDataValue = new DataValue("SomeKey", val2);
            var valueConditon = new ValueCondition(op, leftDataValue, rightDataValue);

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.True(result.ConditionPassed);
            Assert.Null(result.Exception);
            Assert.Same(leftDataValue, result.LeftOperand);
            Assert.Same(rightDataValue, result.RightOperand);
        }

        [Theory]
        [InlineData(ConditionOperator.Equal, 2, 1)]
        [InlineData(ConditionOperator.NotEqual, 2, 2)]
        [InlineData(ConditionOperator.LessThan, 2, 2)]
        [InlineData(ConditionOperator.LessEqualTo, 3, 2)]
        [InlineData(ConditionOperator.GreaterThan, 2, 2)]
        [InlineData(ConditionOperator.GreaterThanEqualTo, 1, 2)]
        public async void EvaluateCondition_ValueCondition_Operator_Fails(ConditionOperator op, int val1, int val2)
        {
            //Build out the results so that all other conditions are set to pass
            var evalResults = Condition.VALUE_OPERATORS.ToDictionary(conditionOp => conditionOp, conditionOp => true);

            //If the operator under test is present
            if (evalResults.ContainsKey(op))
            {
                //Set it to fail
                evalResults[op] = false;
            }

            var valueEvaluators = new Dictionary<Type, IValueEvaluator>
            {
                [typeof(int)] = _fixture.CreateValueEvaluator(evalResults)
            };

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(valueEvaluators), _fixture.CreateActionFactory());

            var leftDataValue = new DataValue("SomeKey", val1);
            var rightDataValue = new DataValue("SomeKey", val2);
            var valueConditon = new ValueCondition(op, leftDataValue, rightDataValue);

            var result = await subjectUnderTest.EvaluateCondition(valueConditon);

            Assert.False(result.ConditionPassed);
            Assert.Null(result.Exception);
            Assert.Same(leftDataValue, result.LeftOperand);
            Assert.Same(rightDataValue, result.RightOperand);
        }

        #endregion

        #region Action Execution Tests

        [Fact]
        public async void ExecuteRuleAction_StopActionExecutionFlagTrue_Success()
        {
            var actionId = Guid.NewGuid();
            bool actionFired = false;
            var actionFactory = _fixture.CreateActionFactory(
                new System.Collections.Concurrent.ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>
                {
                    [actionId] = (engineInstance, dataValues) => new AnonymousAction(engineInstance, dataValues, 
                        async (instance, values) => { actionFired = true; })
                });

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(),
                    actionFactory);

            var result = await subjectUnderTest.ExecuteRuleAction(RuleEngineEvaluationMode.Production, new List<IDataValue>(),
                actionId, true);

            Assert.False(actionFired, "Action fired when it shouldnt have");
            Assert.NotNull(result);
            Assert.Equal(actionId, result.ActionKey);
            Assert.False(result.ActionExecutedSuccessfully);
            Assert.False(result.ActionRan);
        }

        [Fact]
        public async void ExecuteRuleAction_NoActionFound_Fails()
        {
            var actionId = Guid.NewGuid();
            var actionFactory = _fixture.CreateActionFactory();

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(),
                    actionFactory);

            var result = await subjectUnderTest.ExecuteRuleAction(RuleEngineEvaluationMode.Production, new List<IDataValue>(),
                actionId, false);
            
            Assert.NotNull(result);
            Assert.Equal(actionId, result.ActionKey);
            Assert.False(result.ActionExecutedSuccessfully);
            Assert.False(result.ActionRan);
            Assert.NotNull(result.Exception);
            Assert.Equal(ActionExecutionException.ExceptionCause.ActionNotFound, result.Exception.Cause);
        }

        [Fact]
        public async void ExecuteRuleAction_ExceptionThrownDuringAction_Fails()
        {
            var actionId = Guid.NewGuid();
            bool actionFired = false;
            var actionFactory = _fixture.CreateActionFactory(
                new System.Collections.Concurrent.ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>
                {
                    [actionId] = (engineInstance, dataValues) => new AnonymousAction(engineInstance, dataValues,
                        async (instance, values) => {
                            actionFired = true;
                            throw new Exception("ExpectedException");
                        })
                });

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(),
                    actionFactory);

            var result = await subjectUnderTest.ExecuteRuleAction(RuleEngineEvaluationMode.Production, new List<IDataValue>(),
                actionId, false);

            Assert.NotNull(result);
            Assert.Equal(actionId, result.ActionKey);
            Assert.True(actionFired, "Action did not fire when it should have");
            Assert.False(result.ActionExecutedSuccessfully);
            Assert.True(result.ActionRan);
            Assert.NotNull(result.Exception);
            Assert.Equal(ActionExecutionException.ExceptionCause.Unspecified, result.Exception.Cause);
        }

        [Fact]
        public async void ExecuteRuleAction_Success()
        {
            var actionId = Guid.NewGuid();
            bool actionFired = false;
            var actionFactory = _fixture.CreateActionFactory(
                new System.Collections.Concurrent.ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>
                {
                    [actionId] = (engineInstance, dataValues) => new AnonymousAction(engineInstance, dataValues, 
                        async (instance, values) => {
                            actionFired = true;
                        })
                });

            IServiceProvider serviceProvider =
                _fixture.CreateEngineServiceProvider(_fixture.CreateValueFactory(),
                    actionFactory);

            var subjectUnderTest = new RulesMadeEasyEngine_PublicWrapper(_fixture.CreateValueFactory(),
                    actionFactory);

            var result = await subjectUnderTest.ExecuteRuleAction(RuleEngineEvaluationMode.Production, new List<IDataValue>(),
                actionId, false);

            Assert.NotNull(result);
            Assert.Null(result.Exception);
            Assert.Equal(actionId, result.ActionKey);
            Assert.True(actionFired, "Action did not fire when it should have");
            Assert.True(result.ActionExecutedSuccessfully);
            Assert.True(result.ActionRan);
        }

        #endregion

        private ICondition GenerateValueCondition<T>(ConditionOperator op, T val1, T val2)
        {
            return new ValueCondition(op, 
                new DataValue<T>("SomeKey", val1), 
                new DataValue<T>("SomeKey", val2));
        }
    }
}
