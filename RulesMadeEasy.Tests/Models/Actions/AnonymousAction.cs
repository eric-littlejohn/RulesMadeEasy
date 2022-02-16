using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core.Tests
{
    using AnonymousActionLogic = Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, Task>;

    /// <summary>
    /// Contains the implementation of an <see cref="IAction"/> that allows for anonymous functions as the execution logic
    /// </summary>
    public class AnonymousAction : IAction
    {
        protected readonly AnonymousActionLogic _testModeLogic;
        protected readonly AnonymousActionLogic _productionModeLogic;

        private IServiceProvider ServiceProvider { get; }
        private IRulesMadeEasyEngine EngineInstance { get; }
        private IEnumerable<IDataValue> DataValues { get; }

        /// <summary>
        /// Creates a new instance of an <see cref="AnonymousAction"/>
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> used to retrieve external services for use during action execution</param>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        /// <param name="dataValues">The data values available to the action instance</param>
        /// <param name="productionModeLogic">The logic to be executed when the rules are evaluated in <see cref="RuleEngineEvaluationMode.Production"/></param>
        /// <param name="testModeLogic">Optional: The logic to be executed when the rules are evaluated in <see cref="RuleEngineEvaluationMode.Test"/></param>
        public AnonymousAction(IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues, AnonymousActionLogic productionModeLogic,
            AnonymousActionLogic testModeLogic = null)
        {
            ServiceProvider = serviceProvider;
            EngineInstance = engineInstance;
            DataValues = dataValues;
            _testModeLogic = testModeLogic;
            _productionModeLogic = productionModeLogic;
        }

        public async Task Execute(RuleEngineEvaluationMode evaluationMode)
        {
            switch (evaluationMode)
            {
                case RuleEngineEvaluationMode.Production:
                    await _productionModeLogic?.Invoke(ServiceProvider, EngineInstance, DataValues);
                    break;
                case RuleEngineEvaluationMode.Test:
                    await _testModeLogic?.Invoke(ServiceProvider, EngineInstance, DataValues);
                    break;
                default:
                    break;
            }
        }
    }
}