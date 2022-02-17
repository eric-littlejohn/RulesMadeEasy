using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace RulesMadeEasy.Core.Tests
{
    public class RulesMadeEasyFixture
    {
        public IValueEvaluatorFactory CreateValueFactory(IDictionary<Type, IValueEvaluator> backingDictionary = null,
            Action<Mock<IValueEvaluatorFactory>> additionalMockingLogic = null)
        {
            backingDictionary = backingDictionary ?? new Dictionary<Type, IValueEvaluator>();

            Mock<IValueEvaluatorFactory> mockedFactory = new Mock<IValueEvaluatorFactory>();

            mockedFactory.Setup(m => m.GetValueEvaluator(It.IsAny<Type>()))
                .Returns<Type>(type =>
                {
                    backingDictionary.TryGetValue(type, out var matchingEvaluator);
                    return matchingEvaluator;
                });

            additionalMockingLogic?.Invoke(mockedFactory);

            return mockedFactory.Object;
        }

        public IActionFactory CreateActionFactory(ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>> backingDictionary = null,
            Action<Mock<IActionFactory>> additionalMockingLogic = null)
        {
            backingDictionary = backingDictionary ?? new ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>();

            var mockedFactory = new Mock<IActionFactory>();

            mockedFactory.Setup(m => m.GetActionInstance(It.IsAny<Guid>(), It.IsAny<IRulesMadeEasyEngine>(), It.IsAny<IEnumerable<IDataValue>>()))
                .Returns<object, IRulesMadeEasyEngine, IEnumerable<IDataValue>>((id, instance, values) =>
                {
                    backingDictionary.TryGetValue(id, out var actionCreationLogic);
                    return actionCreationLogic?.Invoke(instance, values);
                });

            additionalMockingLogic?.Invoke(mockedFactory);

            return mockedFactory.Object;
        }

        public IValueEvaluator CreateValueEvaluator(bool result)
        {
            var mockedEvaluator = new Mock<IValueEvaluator>();

            mockedEvaluator
                .Setup(m => m.Evaluate(It.IsAny<ConditionOperator>(), It.IsAny<object>(), It.IsAny<object>()))
                .ReturnsAsync(result);

            return mockedEvaluator.Object;
        }

        public IValueEvaluator CreateValueEvaluator(Func<ConditionOperator, object, object, bool> evalLogic)
        {
            var mockedEvaluator = new Mock<IValueEvaluator>();

            mockedEvaluator
                .Setup(m => m.Evaluate(It.IsAny<ConditionOperator>(), It.IsAny<object>(), It.IsAny<object>()))
                .Returns(evalLogic);

            return mockedEvaluator.Object;
        }

        public IValueEvaluator CreateValueEvaluator(Dictionary<ConditionOperator, bool> operatorResults)
        {
            var mockedEvaluator = new Mock<IValueEvaluator>();

            mockedEvaluator
                .Setup(m => m.Evaluate(It.IsAny<ConditionOperator>(), It.IsAny<object>(), It.IsAny<object>()))
                .Returns<ConditionOperator, object, object>((op, objA, objB) =>
                {
                    if (operatorResults?.ContainsKey(op) == true)
                    {
                        return Task.FromResult<bool>(operatorResults[op]);
                    }

                    return Task.FromResult(false);
                });

            return mockedEvaluator.Object;
        }

        public IServiceProvider CreateEngineServiceProvider(IValueEvaluatorFactory valueFactory, IActionFactory actionFactory)
        {
            var serviceCollection = new ServiceCollection();

            if (valueFactory != null)
            {
                serviceCollection.AddScoped(provider => valueFactory);
            }

            if (actionFactory != null)
            {
                serviceCollection.AddScoped(provider => actionFactory);
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
