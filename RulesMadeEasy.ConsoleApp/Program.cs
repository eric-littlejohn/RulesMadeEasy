using Microsoft.Extensions.DependencyInjection;
using RulesMadeEasy.Core;
using RulesMadeEasy.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RulesMadeEasy.ConsoleApp
{
    partial class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Demo app for the rules engine");
            Console.Write("Enter \"p\" to run rules in production mode: ");
            var modeInput = Console.ReadLine();

            RuleEngineEvaluationMode evalMode = modeInput == "p" ? 
                RuleEngineEvaluationMode.Production : RuleEngineEvaluationMode.Test;

            var services = new ServiceCollection();

            var provider = RegisterServices(services);

            var actionFactory = provider.GetRequiredService<IActionFactory>();

            actionFactory.RegisterAction<CreatePersonEntryAction>(nameof(CreatePersonEntryAction));
            actionFactory.RegisterAction<MarkAsChildAction>(nameof(MarkAsChildAction));

            var engine = provider.GetRequiredService<IRulesMadeEasyEngine>();

            //Get the rules
            //Normally these would be created in some external processes and would be pulled from some data source, not made in line.
            var ruleBuilder = new RuleBuilder();
            var rules = new List<IRule>();

            //This rule will fire for each evaluation
            rules.Add(ruleBuilder
                .AddRuleAction(nameof(CreatePersonEntryAction))
                .Build());

            rules.Add(ruleBuilder
                .AddValueCondition(ConditionOperator.LessThan, nameof(Person.Age), 18)
                .AddRuleAction(nameof(MarkAsChildAction))
                .Build());

            //Evaluate data object
            Console.WriteLine("Begining object evaluation");

            var evalObjects = new List<Person>
            {
                new Person { Name = "John", Age = 20 },
                new Person { Name = "Kimmy", Age = 15 }, //Should fire off the mark as child action
                new Person { Name = "Dave", Age = 30 }, 
            };

            for (int i = 0; i < evalObjects.Count; i++)
            {
                var dataValues = ExtractDataValues(evalObjects[i]);
                var evalResult = await engine.EvaluateRules(evalMode, dataValues, rules);

                Console.WriteLine($"Evaluation results for object {i +1}");
                OutputSessionResult(evalResult);
            }
        }

        private static void OutputSessionResult(IRulesMadeEasySessionResult sessionResult)
        {
            Console.WriteLine("------------------------------------");
            for (int j = 0; j < sessionResult.RuleEvaluationResults.Count; j++)
            {
                var ruleEvalResult = sessionResult.RuleEvaluationResults.ElementAt(j);
                Console.WriteLine($"\tRule {j + 1} Passed: {ruleEvalResult.RulePassed}");
                foreach (var actionExecutionResult in ruleEvalResult.ActionExecutionResults)
                {
                    Console.WriteLine($"\t\tAction fired: {actionExecutionResult.ActionKey}");
                }
            }
            Console.WriteLine("------------------------------------");
        }

        //TODO: Support loading these dependencies from a configuration file in addition to in code
        private static IServiceProvider RegisterServices(ServiceCollection services)
        {
            services.AddSingleton<IRulesMadeEasyEngine, RulesMadeEasyEngine>()
                    .AddSingleton<IActionFactory, ActionFactory>()
                    .AddSingleton<IValueEvaluatorFactory, ValueEvaluatorFactory>();

            return services.BuildServiceProvider();
        }

        private static IEnumerable<IDataValue> ExtractDataValues(Person obj)
        {
            return new List<IDataValue>
            {
                new DataValue<string>(nameof(Person.Name), obj?.Name),
                new DataValue<int>(nameof(Person.Age), obj?.Age ?? -1),
            };
        }
    }
}
