using System;
using FileCabinetApp.DTO;

namespace FileCabinetApp.CommandHandlers
{
    public class Help : BaseCommandHandler
    {
        public Help(BaseCommandHandler nexthandler, string mycommand)
            : base(nexthandler, mycommand)
        {
        }

        public override void Realize(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                var index = Array.FindIndex(HelpMessage.Messages, 0, HelpMessage.Messages.Length, i => string.Equals(i[HelpMessage.CommandHelpIndex], data, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessage.Messages[index][HelpMessage.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{data}' command.");
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
