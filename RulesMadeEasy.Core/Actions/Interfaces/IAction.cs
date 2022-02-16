using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.Core
{
    public interface IAction
    {
        /// <summary>
        /// The logic to perform for this action
        /// </summary>
        /// <param name="evaluationMode">The eval</param>
        Task Execute(RuleEngineEvaluationMode evaluationMode);
    }
}