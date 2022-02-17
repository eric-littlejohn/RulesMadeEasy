using RulesMadeEasy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RulesMadeEasy.Core
{
    internal class ActionDescriptor
    {
        private Lazy<ConstructorInfo> _actionConstructor;

        public object ActionKey { get; }

        public Type ActionConcreteType { get; }

        public Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction> InstanceFactory { get; init; }
        
        private IServiceProvider ServiceProvider { get; }

        public ActionDescriptor(IServiceProvider serviceProvider, object key, Type concreteType)
        {
            ServiceProvider = serviceProvider;
            ActionKey = key;
            ActionConcreteType = concreteType;
            _actionConstructor = new Lazy<ConstructorInfo>(GetActionConstructor, true);
            InstanceFactory = GetInstance;
        }

        public ActionDescriptor(object key, Type concreteType, 
            Func<IRulesMadeEasyEngine, IEnumerable<IDataValue>, IAction> instanceFactory)
        {
            ActionKey = key;
            ActionConcreteType = concreteType;
            InstanceFactory = instanceFactory;
        }

        private IAction GetInstance(IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
        {
            object[] ctorParams = GetActionConstructorParams(_actionConstructor.Value, engineInstance, dataValues);

            try
            {
                return Activator.CreateInstance(ActionConcreteType, ctorParams) as IAction;
            }
            catch (Exception exc)
            {
                throw new ActionExecutionException(ActionExecutionException.ExceptionCause.CannotInstantiateAction,
                    $"Unable to create an instance for action of type {ActionConcreteType.FullName}.",
                    exc);

            }
        }

        private ConstructorInfo GetActionConstructor()
        {
            /*
             * Order of workflow is:
             * 1) Get Decorated Constructors, sorted by priorty
             * 2) If no decorated constructors, and there is only one undecorated constructor, attempt to use that
             * 3) Use the first decorated constructor in priority order that can have its arguments resolved.
             */

            var constructors = ActionConcreteType
                .GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var decoratedConstructors = constructors
                .Select(c => new { ConstructorInfo = c, Attribute = c.GetCustomAttribute<ActionConstructorAttribute>() })
                .Where(c => c.Attribute != null)
                .OrderBy(c=> c.Attribute.Priority)
                .Select(c => c.ConstructorInfo);
;

            //If there are no decorated constructors
            if (!decoratedConstructors.Any())
            {
                if (constructors.Length > 1)
                {
                    throw new ActionExecutionException(ActionExecutionException.ExceptionCause.CannotInstantiateAction,
                        $"Unable to determine the constructor to use to instantiate the action for action of type {ActionConcreteType.FullName}.");
                }

                return constructors.First();
            }

            //Get the first decorated constructor that is able to resolve its parameters
            foreach (var ctorInfo in decoratedConstructors)
            {
                var ctorParams = ctorInfo.GetParameters();
                object[] ctorArgs = new object[ctorParams.Length];
                bool canResolve = true;
                for (int i = 0; i < ctorParams.Length; i++)
                {
                    var paramType = ctorParams[i].ParameterType;

                    object paramInstance;

                    //See if there is a registered service that can be resolved to that param type.
                    paramInstance = ServiceProvider.GetService(paramType);

                    //If the parameter still cannot be resolved
                    //The constructor cannot be auto resolved.
                    if (paramInstance == null)
                    {
                        canResolve = false;
                        break;
                    }

                    //Set the param instance in the in the ctorArgs array
                    ctorArgs[i] = paramInstance;
                }

                if (canResolve)
                {
                    return ctorInfo;
                }
            }

            throw new ActionExecutionException(ActionExecutionException.ExceptionCause.CannotInstantiateAction,
                        $"No constructor on action of type {ActionConcreteType.FullName} can be used to instantiate the action.");
        }

        private object[] GetActionConstructorParams(ConstructorInfo ctorInfo, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
        {
            var ctorParams = ctorInfo.GetParameters();
            object[] ctorArgs = new object[ctorParams.Length];

            for (int i = 0; i < ctorParams.Length; i++)
            {
                var paramType = ctorParams[i].ParameterType;

                object paramInstance;

                //If requesting the engine...
                if (paramType == typeof(IRulesMadeEasyEngine))
                {
                    //Assign the current engine instance
                    ctorArgs[i] = engineInstance;
                    continue;
                }

                //If requesting the data values...
                if (paramType == typeof(IEnumerable<IDataValue>))
                {
                    //Assign the current data values
                    ctorArgs[i] = dataValues;
                    continue;
                }

                //External service
                //See if there is a registered service that can be resolved to that param type.
                paramInstance = ServiceProvider.GetService(paramType);

                //If the parameter still cannot be resolved
                //The constructor cannot be auto resolved.
                if (paramInstance == null)
                {
                    throw new ActionExecutionException(ActionExecutionException.ExceptionCause.CannotInstantiateAction,
                                $"Unable to resolve parameter of type {paramType.FullName} for action {ActionConcreteType.FullName}.");

                }

                //Set the param instance in the in the ctorArgs array
                ctorArgs[i] = paramInstance;
            }

            return ctorArgs;
        }
    }
}
