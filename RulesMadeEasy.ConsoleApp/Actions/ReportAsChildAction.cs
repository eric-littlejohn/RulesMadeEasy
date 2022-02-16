using RulesMadeEasy.Core;
using RulesMadeEasy.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.ConsoleApp
{
    public class MarkAsChildAction : BaseAction
    {
        public static Guid ActionId = Guid.NewGuid();  //Normally this would be a hard coded consistant value for referencing by rules

        public string ChildName { get; }

        public MarkAsChildAction(IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
            : base(serviceProvider, engineInstance, dataValues)
        {
            ChildName = DataValueLookup[nameof(Person.Name)].Value as string; //Explicit lookup of known key
        }

        protected override async Task Execute_ProductionMode()
        {
            WriteOuputInColor($"{ChildName} is marked as a child.", ConsoleColor.Blue);

            //Since its action is being run in production mode
            WriteOuputInColor($"Committing changes to store...", ConsoleColor.Blue);

            await Task.Delay(250); //Simulate writing

            WriteOuputInColor($"Commit complete", ConsoleColor.Blue);
        }

        protected override async Task Execute_TestMode()
        {
            WriteOuputInColor($"Test Mode: {ChildName} would be marked as a child.", ConsoleColor.Yellow);
        }

        private static void WriteOuputInColor(string text, ConsoleColor color)
        {
            var origConsoleColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = origConsoleColor;
        }
    }
}
