using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    using ActionRegistrationLogic = Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction>;

    //TODO: Rework this to work similar to how ServiceProvider works in .net core with descriptors.
    /// <summary>
    /// A concrete implementation of an <see cref="IActionFactory"/>
    /// </summary>
    public class ActionFactory : IActionFactory
    {
        protected virtual ConcurrentDictionary<Guid, ActionRegistrationLogic> RegisteredActions { get; set; } 
            = new ConcurrentDictionary<Guid, ActionRegistrationLogic>();

        /// <summary>
        /// Gets the <see cref="IAction"/> registered for the provided identifier
        /// </summary>
        /// <param name="actionId">The identifier of the action to retrieve</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> used to retrieve external services for use during action execution</param>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        /// <param name="dataValues">The data values available to the action instance</param>
        /// <returns>An instance of the <see cref="IAction"/> to be used when evaluating rules</returns>
        public IAction GetActionInstance(Guid actionId, IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
        {
            RegisteredActions.TryGetValue(actionId, out var actionCreationLogic);

            return actionCreationLogic?.Invoke(serviceProvider, engineInstance, dataValues);
        }

        /// <summary>
        /// Registers the <see cref="IAction"/> under the identifier provided
        /// </summary>
        /// <param name="actionId">The identifier of the action. Used by the rules to indicate which actions to fire</param>
        /// <param name="actionCreationLogic">The logic that will create a new instance of an <see cref="IAction"/></param>
        /// <returns>The current <see cref="IActionFactory"/></returns>
        public IActionFactory RegisterAction(Guid actionId, ActionRegistrationLogic actionCreationLogic)
        {
            if (!RegisteredActions.TryAdd(actionId, actionCreationLogic))
            {
                throw new ActionExecutionException(ActionExecutionException.ExceptionCause.ActionAlreadyRegistered,
                    $"There is already an entry for the id {actionId} registered");
            }

            return this;
        }
    }
}
