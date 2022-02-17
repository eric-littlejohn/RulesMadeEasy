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

            var backingDictionary = new ConcurrentDictionary<object, ActionDescriptor>
            {
                [actionId] = new ActionDescriptor(Mock.Of<IServiceProvider>(), actionId, typeof(TypeMappedAction))
            };

            var subjectUnderTest = GetActionFactory(backingDictionary);

            var raisedExc = Assert.Throws<ActionExecutionException>(() =>
                subjectUnderTest.RegisterAction(actionId, typeof(TypeMappedAction), (engine, dataValues) => actionToRegister));

            Assert.Equal(ActionExecutionException.ExceptionCause.ActionAlreadyRegistered, raisedExc.Cause);
        }

        [Fact]
        public void RegisterAction_Success()
        {
            var backingDictionary = new ConcurrentDictionary<object, ActionDescriptor>();

            var subjectUnderTest = GetActionFactory(backingDictionary);
            var actionConcreteType = typeof(TypeMappedAction);
            var actionId = Guid.NewGuid();

            subjectUnderTest.RegisterAction(actionId, actionConcreteType, (engine, dataValues) => Mock.Of<IAction>());

            Assert.Collection(backingDictionary,
                kvpAction => {
                    Assert.Equal(actionId, kvpAction.Key);
                    Assert.Equal(actionId, kvpAction.Value.ActionKey);
                    Assert.Equal(actionConcreteType, kvpAction.Value.ActionConcreteType);
                });
        }

        [Fact]
        public void GetActionInstance_NoMatchingId_ReturnsNull()
        {
            var backingDictionary = new ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>();

            var subjectUnderTest = _fixture.CreateActionFactory(backingDictionary);

            var retrievedAction = subjectUnderTest.GetActionInstance(Guid.NewGuid(), Mock.Of<IRulesMadeEasyEngine>(), new List<IDataValue>());

            Assert.Null(retrievedAction);
        }

        [Fact]
        public void GetActionInstance_Success()
        {
            var actionId = Guid.NewGuid();
            var actionToRegister = Mock.Of<IAction>();
            bool instanceLogicCalled = false;

            var backingDictionary = new ConcurrentDictionary<object, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>>
            {
                [actionId] = (instance, values) =>
                    {
                        instanceLogicCalled = true;
                        return actionToRegister;
                    }
            };

            var subjectUnderTest = _fixture.CreateActionFactory(backingDictionary);

            var retrievedAction = subjectUnderTest.GetActionInstance(actionId, Mock.Of<IRulesMadeEasyEngine>(), new List<IDataValue>());

            Assert.NotNull(retrievedAction);
            Assert.True(instanceLogicCalled, "Instance logic was not called");
            Assert.Same(actionToRegister, retrievedAction);
        }

        private IActionFactory GetActionFactory(ConcurrentDictionary<object, ActionDescriptor> backingDictionary = null)
        {
            backingDictionary = backingDictionary ?? new ConcurrentDictionary<object, ActionDescriptor>();

            var mockedFactory = new Mock<ActionFactory>(Mock.Of<IServiceProvider>());

            mockedFactory.Protected().SetupGet<ConcurrentDictionary<object, ActionDescriptor>>("RegisteredActions")
                .Returns(backingDictionary);

            return mockedFactory.Object;
        }
    }
}
