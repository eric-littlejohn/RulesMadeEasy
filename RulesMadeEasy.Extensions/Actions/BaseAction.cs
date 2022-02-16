using RulesMadeEasy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesMadeEasy.Extensions
{
    public abstract class BaseAction : IAction
    {
        protected IRulesMadeEasyEngine EngineInstance { get; }

        protected IDictionary<string, IDataValue> DataValueLookup { get; }

        protected IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// Creates a new instance of a <see cref="BaseAction"/>
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> used to retrieve external services for use during action execution</param>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        /// <param name="dataValues">The data values available to the action instance</param>
        protected BaseAction(IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
        {
            ServiceProvider = serviceProvider ?? throw new ActionExecutionException(ActionExecutionException.ExceptionCause.NoRuleEngineInstanceProvided,
                                  "A null rule engine instance was provided to the constructor");
            EngineInstance = engineInstance ?? throw new ActionExecutionException(ActionExecutionException.ExceptionCause.NoRuleEngineInstanceProvided,
                                 "A null rule engine instance was provided to the constructor");
            DataValueLookup = dataValues.ToDictionary(val => val.Key);
        }

        /// <summary>
        /// The logic to perform for this action
        /// </summary>
        /// <returns></returns>
        public virtual async Task Execute(RuleEngineEvaluationMode evaluationMode)
        {
            switch (evaluationMode)
            {
                case RuleEngineEvaluationMode.Production:
                    await Execute_ProductionMode();
                    break;
                case RuleEngineEvaluationMode.Test:
                    await Execute_TestMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(evaluationMode), evaluationMode, null);
            }
        }

        protected abstract Task Execute_ProductionMode();

        protected abstract Task Execute_TestMode();
    }
}