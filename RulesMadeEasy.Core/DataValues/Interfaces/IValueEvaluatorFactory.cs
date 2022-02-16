using System;
using System.Collections.Generic;

namespace RulesMadeEasy.Core
{
    public interface IValueEvaluatorFactory
    {
        /// <summary>
        /// Gets all value evaluators that are currently registered
        /// </summary>
        /// <returns>The collection of all evaluators that are registered</returns>
        IReadOnlyDictionary<Type, IValueEvaluator> GetValueEvaluators();

        /// <summary>
        /// Gets the <see cref="IValueEvaluator"/> for the provided <see cref="Type"/>
        /// </summary>
        /// <param name="valueType">The <see cref="Type"/> used to lookup the associated <see cref="IValueEvaluator"/></param>
        /// <returns>The associated <see cref="IValueEvaluator"/></returns>
        IValueEvaluator GetValueEvaluator(Type valueType);

        /// <summary>
        /// Registers the provided <see cref="IValueEvaluator"/> for the provided type 
        /// </summary>
        /// <param name="type">The type to register to the evaluator</param>
        /// <param name="evaluator">The <see cref="IValueEvaluator"/> to use when evaluating the specified type</param>
        /// <returns>The current <see cref="IValueEvaluatorFactory"/> to allow for method chaining</returns>
        IValueEvaluatorFactory RegisterValueEvaluator(Type type, IValueEvaluator evaluator);

        /// <summary>
        /// Registers the provided <see cref="IValueEvaluator"/> for the provided type 
        /// </summary>
        /// <typeparam name="T">The type to register to the evaluator</typeparam>
        /// <param name="evaluator">The <see cref="IValueEvaluator"/> to use when evaluating the specified type</param>
        /// <returns>The current <see cref="IValueEvaluatorFactory"/> to allow for method chaining</returns>
        IValueEvaluatorFactory RegisterValueEvaluator<T>(IValueEvaluator evaluator);

        /// <summary>
        /// Registers the provided <see cref="IValueEvaluator"/> as the default evaluator
        /// </summary>
        /// <param name="valueEvaluator">The <see cref="IValueEvaluator"/> to use when a type isnt found</param>
        /// <returns>The current <see cref="IValueEvaluatorFactory"/> to allow for method chaining</returns>
        IValueEvaluatorFactory RegisterDefaultValueEvaluator(IValueEvaluator valueEvaluator);
    }
}