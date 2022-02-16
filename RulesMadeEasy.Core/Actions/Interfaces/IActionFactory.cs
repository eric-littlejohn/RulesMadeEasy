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
        /// <param name="actionId">The identifier of the action to retrieve</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> used to retrieve external services for use during action execution</param>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        /// <param name="dataValues">The data values available to the action instance</param>
        /// <returns>An instance of the <see cref="IAction"/> to be used when evaluating rules</returns>
        IAction GetActionInstance(Guid actionId, IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues);

        /// <summary>
        /// Registers the <see cref="IAction"/> under the identifier provided
        /// </summary>
        /// <param name="actionId">The identifier of the action. Used by the rules to indicate which actions to fire</param>
        /// <param name="actionCreationLogic">The logic that will create a new instance of an <see cref="IAction"/></param>
        /// <returns>The current <see cref="IActionFactory"/></returns>
        IActionFactory RegisterAction(Guid actionId, Func<IServiceProvider, IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction> actionCreationLogic);
    }
}
