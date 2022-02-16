using RulesMadeEasy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RulesMadeEasy.Extensions
{
    public abstract class AutoMappedAction : BaseAction
    {
        /// <summary>
        /// Creates a new instance of a <see cref="AutoMappedAction"/>
        /// </summary>
        /// <remarks>Inheriting from this class will attempt to automatically map the data values to defined properties on the inherited class</remarks>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> used to retrieve external services for use during action execution</param>
        /// <param name="engineInstance">The instance of the <see cref="IRulesMadeEasyEngine"/> that is executing the action</param>
        /// <param name="dataValues">The data values available to the action instance</param>
        protected AutoMappedAction(IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
            : base(serviceProvider, engineInstance, dataValues)
        {
            MapDecoratedProperties();
        }

        protected void MapDecoratedProperties()
        {
            var instanceType = this.GetType();

            IEnumerable<PropertyInfo> actionProperties = instanceType.GetProperties(BindingFlags.Public | 
                                                                                    BindingFlags.NonPublic |
                                                                                    BindingFlags.Instance);

            if (!actionProperties.Any())
            {
                //Exit
                return;
            }

            List<Exception> mappingExceptions = new List<Exception>();

            foreach (PropertyInfo actionProperty in actionProperties)
            {
                try
                {
                    ActionDataValuePropertyAttribute dataValuePropertyAttribute =
                        actionProperty.GetCustomAttribute<ActionDataValuePropertyAttribute>();

                    //If no attribute was found
                    if (dataValuePropertyAttribute == null)
                    {
                        //Go to the next property
                        continue;
                    }

                    object extractedPropValue = null;

                    if (DataValueLookup.ContainsKey(dataValuePropertyAttribute.Key))
                    {
                        extractedPropValue = DataValueLookup[dataValuePropertyAttribute.Key]?.Value;
                    }

                    if (extractedPropValue == null)
                    {
                        if (!dataValuePropertyAttribute.AllowNull)
                        {
                            throw new ActionExecutionException(ActionExecutionException.ExceptionCause.NoMatchingDataValueFound,
                                $"Unable to locate a data value with the key {dataValuePropertyAttribute.Key} to map to {actionProperty.Name} on {instanceType.Name}");
                        }
                    }
                    else
                    {
                        actionProperty.SetValue(this, extractedPropValue);
                    }
                }
                catch (Exception exc)
                {
                    mappingExceptions.Add(exc);
                }
            }

            if (mappingExceptions.Any())
            {
                throw new AggregateException(mappingExceptions);
            }
        }
    }
}