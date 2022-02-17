using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    //TODO: Rework this to work similar to how ServiceProvider works in .net core with descriptors.
    /// <summary>
    /// A concrete implementation of an <see cref="IActionFactory"/>
    /// </summary>
    public class ActionFactory : IActionFactory
    {
        internal virtual ConcurrentDictionary<object, ActionDescriptor> RegisteredActions { get; set; }
            = new ConcurrentDictionary<object, ActionDescriptor>();

        private IServiceProvider ServiceProvider { get; }

        public ActionFactory(IServiceProvider provider)
        {
            ServiceProvider = provider;
        }

        /// <inheritdoc />
        public IAction GetActionInstance(object actionKey, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
        {
            RegisteredActions.TryGetValue(actionKey, out var actionCreationLogic);

            return actionCreationLogic?.InstanceFactory?.Invoke(engineInstance, dataValues);
        }

        /// <inheritdoc />
        public IActionFactory RegisterAction<TAction>(object actionKey)
            where TAction : class, IAction
        {
            var actionDescriptor = new ActionDescriptor(ServiceProvider, actionKey, typeof(TAction));

            return RegisterAction(actionKey, actionDescriptor);
        }

        /// <inheritdoc />
        public IActionFactory RegisterAction(object actionKey, Type actionConcreteType, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction> actionInstantiationLogic)
        {
            var actionDescriptor = new ActionDescriptor(actionKey, actionConcreteType, actionInstantiationLogic);

            return RegisterAction(actionKey, actionDescriptor);
        }

        private IActionFactory RegisterAction(object actionKey, ActionDescriptor descriptor)
        {
            if (!RegisteredActions.TryAdd(actionKey, descriptor))
            {
                throw new ActionExecutionException(ActionExecutionException.ExceptionCause.ActionAlreadyRegistered,
                    $"There is already an action entry registered with the key {actionKey}.");
            }
            return this;
        }
    }
}
