using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Contains the logic and functionality needed for executing rules
    /// </summary>
    public class RulesMadeEasyEngine : IRulesMadeEasyEngine
    {
        /// <summary>
        /// The <see cref="IValueEvaluatorFactory"/> used to get the <see cref="IValueEvaluator"/> instances when evaluating values
        /// </summary>
        protected IValueEvaluatorFactory ValueEvaluatorFactory { get; set; }

        /// <summary>
        /// The <see cref="IActionFactory"/> used to get the <see cref="IAction"/> instances for rules
        /// </summary>
        protected IActionFactory ActionFactory { get; }

        /// <summary>
        /// Creates a new instance of a <see cref="RulesMadeEasyEngine"/>
        /// </summary>
        /// <param name="valueFactory">The <see cref="IValueEvaluatorFactory"/> used to get the <see cref="IValueEvaluator"/>s that can evaluate the supported value types</param>
        /// <param name="actionFactory">The <see cref="IActionFactory"/> used to create action instances to invoke.</param>
        public RulesMadeEasyEngine(IValueEvaluatorFactory valueFactory, IActionFactory actionFactory)
        {
            ValueEvaluatorFactory = valueFactory ?? throw new ArgumentNullException(nameof(valueFactory),
                                  "A null value factory was passed to the rules engine instance"); ;
            ActionFactory = actionFactory ?? throw new ArgumentNullException(nameof(actionFactory),
                                  "A null action factory was passed to the rules engine instance"); ;
        }

        /// <inheritdoc />
        public async Task<IRulesMadeEasySessionResult> EvaluateRules(RuleEngineEvaluationMode evaluationMode, IEnumerable<IDataValue> dataValues, IEnumerable<IRule> rules,
        CancellationToken cancellationToken = default(CancellationToken))
        {
            var engineEvaluationResult = new RulesMadeEasySessionResult
            {
                EvaluationMode = evaluationMode
            };

            try
            {
                //Keep track of all exceptions raised during rule processing
                List<Exception> processingExceptions = new List<Exception>();

                //Track the start time of the processing
                engineEvaluationResult.StartTime = DateTimeOffset.Now;

                foreach (IRule rule in rules)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        engineEvaluationResult.WasCancelled = true;
                        break;
                    }

                    IRuleEvaluationResult ruleEvaluationResult;
                    List<IConditionEvaluationResult> conditionEvaluationResults =
                        new List<IConditionEvaluationResult>();
                    List<IActionExecutionResult> actionExecutionResults =
                        new List<IActionExecutionResult>();

                    try
                    {
                        //If there are conditions
                        if (rule.Conditions.Any())
                        {
                            //Run each condition in parallel for performance
                            var conditionEvaluations = rule.Conditions
                                .Select(async ruleCondition =>
                                {
                                    ICondition condition = null;

                                    try
                                    {
                                        //Build the condition
                                        condition = ruleCondition.Compile(dataValues);

                                        //Evaluate the condition
                                        return await EvaluateCondition(condition);
                                    }
                                    catch (ConditionEvaluationException conditionExc)
                                    {
                                        return new ConditionEvaluationResult
                                        {
                                            ConditionPassed = false,
                                            Exception = conditionExc
                                        };
                                    }
                                    catch (Exception exc)
                                    {
                                        return new ConditionEvaluationResult
                                        {
                                            Operator = ruleCondition.Operator,
                                            ConditionPassed = false,
                                            LeftOperand = condition?.LeftOperand,
                                            RightOperand = condition?.RightOperand,
                                            Exception = new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.Unspecified,
                                                exc.Message, exc)
                                        };
                                    }
                                })
                                .ToArray();

                            //Wait for all evaluations to complete
                            await Task.WhenAll(conditionEvaluations);

                            conditionEvaluationResults.AddRange(conditionEvaluations.Select(task => task.Result));
                        }

                        //If all conditions passed
                        if (conditionEvaluationResults.All(result => result.ConditionPassed))
                        {
                            bool stopActionExecution = false;
                            foreach (var actionId in rule.ActionIdentifiers)
                            {
                                //Execute the action
                                IActionExecutionResult actionExecutionResult =
                                    await ExecuteRuleAction(evaluationMode, dataValues,
                                        actionId, stopActionExecution);
                                
                                //If the action failed to execute
                                if (!actionExecutionResult.ActionExecutedSuccessfully)
                                {
                                    //Stop execution of remaining actions, but allow them to report that they didnt run
                                    stopActionExecution = true;
                                }

                                //Store the excecution result
                                actionExecutionResults.Add(actionExecutionResult);
                            }
                        }

                        //Store the evaluation results
                        ruleEvaluationResult = CreateRuleEvaluationResult(rule, conditionEvaluationResults, actionExecutionResults);
                    }
                    catch (Exception exc)
                    {
                        ruleEvaluationResult = CreateRuleEvaluationResult(rule, conditionEvaluationResults,
                            actionExecutionResults, exc);

                        processingExceptions.Add(exc);
                    }

                    engineEvaluationResult.RuleEvaluationResults.Add(ruleEvaluationResult);
                }

                if (processingExceptions.Any())
                {
                    engineEvaluationResult.RanToCompletion = false;
                    engineEvaluationResult.Exception = new AggregateException(processingExceptions);
                }
            }
            catch (Exception exc)
            {
                engineEvaluationResult.RanToCompletion = false;
                engineEvaluationResult.Exception = new AggregateException(exc);
            }

            //Log the end of session processing
            engineEvaluationResult.EndTime = DateTimeOffset.Now;

            return engineEvaluationResult;
        }

        /// <summary>
        /// Creates an <see cref="IRuleEvaluationResult"/> detailing the rule evaluation
        /// </summary>
        /// <param name="rule">The rule that was evaluated</param>
        /// <param name="conditionResults">The evaluation results of the conditions</param>
        /// <param name="actionExecutionResults">The results of executing the actions</param>
        /// <returns>An <see cref="IRuleEvaluationResult"/> detailing the evaluation results</returns>
        protected virtual IRuleEvaluationResult CreateRuleEvaluationResult(IRule rule,
            IEnumerable<IConditionEvaluationResult> conditionResults,
            IEnumerable<IActionExecutionResult> actionExecutionResults)
        {
            //Determine if all conditions passed
            bool conditionsPassed = conditionResults.All(conditionResult => conditionResult.ConditionPassed);

            bool actionsPassed = actionExecutionResults.All(actionResult => actionResult.ActionRan && actionResult.ActionExecutedSuccessfully);

            return new RuleEvaluationResult
            {
                RulePassed = conditionsPassed && actionsPassed,
                ConditionEvaluationResults = conditionResults,
                ActionExecutionResults = actionExecutionResults
            };
        }

        /// <summary>
        /// Creates an <see cref="IRuleEvaluationResult"/> detailing the rule evaluation
        /// </summary>
        /// <param name="rule">The rule that was evaluated</param>
        /// <param name="conditionResults">The evaluation results of the conditions</param>
        /// <param name="actionExecutionResults">The results of executing the actions</param>
        /// <param name="exc">The exception that was thrown, if any</param>
        /// <returns>An <see cref="IRuleEvaluationResult"/> detailing the evaluation results</returns>
        protected virtual IRuleEvaluationResult CreateRuleEvaluationResult(IRule rule, IEnumerable<IConditionEvaluationResult> conditionResults,
            IEnumerable<IActionExecutionResult> actionExecutionResults, Exception exc)
        {
            return new RuleEvaluationResult
            {
                RulePassed = false,
                Exception = exc,
                ConditionEvaluationResults = conditionResults,
                ActionExecutionResults = actionExecutionResults
            };
        }

        #region Conditions

        /// <summary>
        /// Evaluates the <see cref="ICondition"/> provided
        /// </summary>
        /// <param name="condition">The conditon to evaluate</param>
        /// <returns>An <see cref="IConditionEvaluationResult"/> detailing the results of the evaluation</returns>
        protected virtual async Task<IConditionEvaluationResult> EvaluateCondition(ICondition condition)
        {
            if (condition == null)
            {
                throw new ConditionEvaluationException(
                    ConditionEvaluationException.ExceptionCause.NullConditionProvided,
                    "A null condition cannot be evaluated.");
            }

            IConditionEvaluationResult result;

            try
            {
                switch (condition)
                {
                    case ILogicalCondition logicalCondition:
                        result = await ProcessLogicalCondition(logicalCondition);
                        break;
                    case IValueCondition valueCondition:
                        result = await ProcessValueCondition(valueCondition);
                        break;
                    default:
                        throw new ConditionEvaluationException(
                            ConditionEvaluationException.ExceptionCause.UnableToDetermineConditionType,
                            "Unable to determine how to process the condition.");
                }
            }
            catch (ConditionEvaluationException conditionExc)
            {
                result = new ConditionEvaluationResult
                {
                    ConditionPassed = false,
                    Exception = conditionExc
                };
            }
            catch (Exception exc)
            {
                result = new ConditionEvaluationResult
                {
                    ConditionPassed = false,
                    Exception = new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.Unspecified,
                        exc.Message, exc)
                };
            }

            return result;
        }

        /// <summary>
        /// Evaluates the <see cref="ILogicalCondition"/> provided
        /// </summary>
        /// <param name="logicalCondition">The logical condition to evaluate</param>
        /// <returns></returns>
        protected virtual async Task<IConditionEvaluationResult> ProcessLogicalCondition(ILogicalCondition logicalCondition)
        {
            var result = new ConditionEvaluationResult
            {
                Operator = logicalCondition.Operator
            };

            var leftOperandProcessing = EvaluateCondition(logicalCondition.LeftOperand);
            var rightOperandProcessing = EvaluateCondition(logicalCondition.RightOperand);

            await Task.WhenAll(leftOperandProcessing, rightOperandProcessing);

            var leftOperandResult = leftOperandProcessing.Result;
            var rightOperandResult = rightOperandProcessing.Result;

            result.LeftOperand = logicalCondition.LeftOperand;
            result.RightOperand = logicalCondition.RightOperand;
            result.NestedConditionResults.Add(leftOperandResult);
            result.NestedConditionResults.Add(rightOperandResult);

            switch (logicalCondition.Operator)
            {
                case ConditionOperator.And:
                    result.ConditionPassed = (leftOperandResult?.ConditionPassed ?? false) && (rightOperandResult?.ConditionPassed ?? false);
                    break;
                case ConditionOperator.Or:
                    result.ConditionPassed = (leftOperandResult?.ConditionPassed ?? false) || (rightOperandResult?.ConditionPassed ?? false);
                    break;
                default:
                    throw new ConditionEvaluationException(
                        ConditionEvaluationException.ExceptionCause.UnsupportedOperator,
                        $"Invalid Condition operator of {logicalCondition.Operator} provided to the logical condition");
            }

            return result;
        }

        /// <summary>
        /// Evaluates the <see cref="IValueCondition"/> provided
        /// </summary>
        /// <param name="valueCondition">The <see cref="IValueCondition"/> to evaluate</param>
        /// <returns></returns>
        protected virtual async Task<IConditionEvaluationResult> ProcessValueCondition(IValueCondition valueCondition)
        {
            var result = new ConditionEvaluationResult
            {
                Operator = valueCondition.Operator
            };

            if (valueCondition.LeftOperand == null)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.OperandMissing,
                    "Left operand is missing");
            }

            if (valueCondition.RightOperand == null)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.OperandMissing,
                    "Right operand is missing");
            }

            Type valueType = valueCondition.LeftOperand.ValueType ?? valueCondition.RightOperand.ValueType;

            if (valueType == null)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.UnableToDetermineValueType,
                    "Unable to determine the value type of the condition");
            }
            if (valueCondition.LeftOperand.ValueType != valueCondition.RightOperand.ValueType)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.ValueTypeMismatch,
                    "The left operand and right operand types mismtach. " +
                                    $"Left Operand Type '{valueCondition.LeftOperand.Value?.GetType()}'" +
                                    $"Right Operand Type '{valueCondition.RightOperand.Value?.GetType()}'");
            }

            result.LeftOperand = valueCondition.LeftOperand;
            result.RightOperand = valueCondition.RightOperand;

            var valueEvaluator = ValueEvaluatorFactory.GetValueEvaluator(valueType);

            if (valueEvaluator == null)
            {
                throw new ConditionEvaluationException(ConditionEvaluationException.ExceptionCause.EvaluatorNotFound,
                    $"Unable to locate a value evaluator for type {valueType.Name}");
            }

            result.ConditionPassed = await valueEvaluator.Evaluate(valueCondition.Operator,
                valueCondition.LeftOperand.Value,
                 valueCondition.RightOperand.Value);

            return result;
        }

        #endregion

        #region Actions

        protected virtual async Task<IActionExecutionResult> ExecuteRuleAction(RuleEngineEvaluationMode evaluationMode,
            IEnumerable<IDataValue> dataValues,
            object actionKey,
            bool stopActionExecution)
        {
            var actionExecutionResult = new ActionExecutionResult
            {
                ActionKey = actionKey
            };

            if (stopActionExecution)
            {
                actionExecutionResult.ActionRan = false;
                actionExecutionResult.ActionExecutedSuccessfully = false;
            }
            else
            {
                try
                {
                    var actionInstance = ActionFactory.GetActionInstance(actionKey, this, dataValues);

                    if (actionInstance == null)
                    {
                        throw new ActionExecutionException(ActionExecutionException.ExceptionCause.ActionNotFound,
                            $"No action found with key {actionKey}. Unable to execute action.");
                    }

                    actionExecutionResult.ActionRan = true;

                    await actionInstance.Execute(evaluationMode);

                    actionExecutionResult.ActionExecutedSuccessfully = true;
                }
                catch (ActionExecutionException actionExc)
                {
                    actionExecutionResult.ActionExecutedSuccessfully = false;
                    actionExecutionResult.Exception = actionExc;
                }
                catch (Exception exc)
                {
                    actionExecutionResult.ActionExecutedSuccessfully = false;
                    actionExecutionResult.Exception = new ActionExecutionException(ActionExecutionException.ExceptionCause.Unspecified,
                            exc.Message, exc);
                }
            }

            return actionExecutionResult;
        }

        #endregion
    }
}
