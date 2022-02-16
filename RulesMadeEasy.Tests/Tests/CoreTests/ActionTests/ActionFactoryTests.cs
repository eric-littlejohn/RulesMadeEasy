using Moq;
using Moq.Protected;
using RulesMadeEasy.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RulesMadeEasy.Core.Tests
{
    public class ActionFactoryTests : IClassFixture<RulesMadeEasyFixture>
    {
        private readonly RulesMadeEasyFixture _fixture;

        public ActionFactoryTests(RulesMadeEasyFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void RegisterAction_DuplicateKey_Success()
        {
            var actionId = Guid.NewGuid();
            var actionToRegister = Mock.Of<IAction>();

            var backingDictionary = new ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>
            {
                [actionId] = (services, instance, dataValues) => actionToRegister
            };

            var subjectUnderTest = GetActionFactory(backingDictionary);

            var raisedExc = Assert.Throws<ActionExecutionException>(() =>
                subjectUnderTest.RegisterAction(actionId, (services, engine, dataValues) => actionToRegister));

            Assert.Equal(ActionExecutionException.ExceptionCause.ActionAlreadyRegistered, raisedExc.Cause);
        }

        [Fact]
        public void RegisterAction_Success()
        {
            var backingDictionary = new ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>();

            var subjectUnderTest = GetActionFactory(backingDictionary);

            var actionId = Guid.NewGuid();
            var actionToRegister = Mock.Of<IAction>();

            subjectUnderTest.RegisterAction(actionId, (services, engine, dataValues) => actionToRegister);

            Assert.Collection(backingDictionary,
                kvpAction => {
                    Assert.Equal(actionId, kvpAction.Key);
                    Assert.Same(actionToRegister, kvpAction.Value?.Invoke(Mock.Of<IServiceProvider>(), Mock.Of<IRulesMadeEasyEngine>(), new List<IDataValue>()));
                });
        }

        [Fact]
        public void GetActionInstance_NoMatchingId_ReturnsNull()
        {
            var backingDictionary = new ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>();

            var subjectUnderTest = _fixture.CreateActionFactory(backingDictionary);

            var retrievedAction = subjectUnderTest.GetActionInstance(Guid.NewGuid(), Mock.Of<IServiceProvider>(), Mock.Of<IRulesMadeEasyEngine>(), new List<IDataValue>());

            Assert.Null(retrievedAction);
        }

        [Fact]
        public void GetActionInstance_Success()
        {
            var actionId = Guid.NewGuid();
            var actionToRegister = Mock.Of<IAction>();
            bool instanceLogicCalled = false;

            var backingDictionary = new ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>
            {
                [actionId] = (services, instance, values) =>
                    {
                        instanceLogicCalled = true;
                        return actionToRegister;
                    }
            };

            var subjectUnderTest = _fixture.CreateActionFactory(backingDictionary);

            var retrievedAction = subjectUnderTest.GetActionInstance(actionId, Mock.Of<IServiceProvider>(), Mock.Of<IRulesMadeEasyEngine>(), new List<IDataValue>());

            Assert.NotNull(retrievedAction);
            Assert.True(instanceLogicCalled, "Instance logic was not called");
            Assert.Same(actionToRegister, retrievedAction);
        }

        private IActionFactory GetActionFactory(ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>> backingDictionary = null)
        {
            backingDictionary = backingDictionary ?? new ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>();

            var mockedFactory = new Mock<ActionFactory>();

            mockedFactory.Protected().SetupGet<ConcurrentDictionary<Guid, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>>("RegisteredActions")
                .Returns(backingDictionary);

            return mockedFactory.Object;
        }
    }
}
