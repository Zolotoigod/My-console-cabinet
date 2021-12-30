using System;
using FileCabinetApp.DTO;

namespace FileCabinetApp.CommandHandlers
{
    public class Help : BaseCommandHandler
    {
        public Help(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(HelpMessage.Messages, 0, HelpMessage.Messages.Length, i => string.Equals(i[HelpMessage.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessage.Messages[index][HelpMessage.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessage.Messages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[HelpMessage.CommandHelpIndex], helpMessage[HelpMessage.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }
    }
}
