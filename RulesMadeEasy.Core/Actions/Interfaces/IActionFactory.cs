using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    /// <summary>
    /// Interface declaration for registering and retrieving <see cref="IAction"/> instances
    /// </summary>
    public interface IActionFactory
    {
        /// <summary>
        /// Gets the <see cref="IAction"/> registered for the provided identifier
        /// </summary>
        /// <param name="actionKey">The key of the action to retrieve</param>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        /// <param name="dataValues">The data values available to the action instance</param>
        /// <returns>An instance of the <see cref="IAction"/> to be used when evaluating rules</returns>
        IAction GetActionInstance(object actionKey, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues);

        /// <summary>
        /// Registers the <see cref="IAction"/> under the identifier provided
        /// </summary>
        /// <typeparam name="TAction">The type of the action to register.</typeparam>
        /// <param name="actionKey">The key of the action. Used by the rules to indicate which actions to fire</param>
        /// <returns>The current <see cref="IActionFactory"/></returns>
        IActionFactory RegisterAction<TAction>(object actionKey)
            where TAction : class, IAction;

        /// <summary>
        /// Registers the <see cref="IAction"/> under the identifier provided
        /// </summary>
        /// <param name="actionKey">The key of the action. Used by the rules to indicate which actions to fire</param>
        /// <param name="actionConcreteType">The <see cref="Type"/> of the action.</param>
        /// <param name="actionInstantiationLogic">The logic that will create a new instance of an <see cref="IAction"/></param>
        /// <returns>The current <see cref="IActionFactory"/></returns>
        IActionFactory RegisterAction(object actionKey, Type actionConcreteType, Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction> actionInstantiationLogic);
    }
}
