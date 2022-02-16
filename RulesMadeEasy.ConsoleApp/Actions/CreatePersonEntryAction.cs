using RulesMadeEasy.Core;
using RulesMadeEasy.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RulesMadeEasy.ConsoleApp
{
    public class CreatePersonEntryAction : AutoMappedAction
    {
        public static Guid ActionId = Guid.NewGuid(); //Normally this would be a hard coded consistant value for referencing by rules

        [ActionDataValueProperty(nameof(Person.Name), AllowNull = false)]
        public string PersonName { get; set; }

        public CreatePersonEntryAction(IServiceProvider serviceProvider, IRulesMadeEasyEngine engineInstance, IEnumerable<IDataValue> dataValues)
            : base(serviceProvider, engineInstance, dataValues)
        {
        }

 
        protected override async Task Execute_ProductionMode()
        {
            WriteOuputInColor($"Creating entry for {PersonName}.", ConsoleColor.Blue);

            await Task.Delay(250); //Simulate writing

            WriteOuputInColor($"Commit complete", ConsoleColor.Blue);
        }

        protected override async Task Execute_TestMode()
        {
            WriteOuputInColor($"Test Mode: Entry for {PersonName} would be created.", ConsoleColor.Yellow);
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
