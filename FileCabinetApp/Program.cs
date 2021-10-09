using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Vladislav Shalkevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private static readonly FileCabinetService Service = new ();
        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Create(string parameters)
        {
            Console.Write("FirstName: ");
            string firstname = Console.ReadLine() ?? string.Empty;
            Console.Write("LastName: ");
            string lastname = Console.ReadLine() ?? string.Empty;
            Console.Write("DateOfBirth (month.day.year): ");
            DateTime dateOfBirth = Convert.ToDateTime(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Write("Personal count type (A, B, C): ");
            char type = Convert.ToChar(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Write("Personal count number (four digits): ");
            short number = Convert.ToInt16(Console.ReadLine(), CultureInfo.InvariantCulture);
            Console.Write("Personal count balance: ");
            decimal balance = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);

            int num = Service.CreateRecord(firstname, lastname, dateOfBirth, type, number, balance);
            Console.WriteLine($"Record #{num} is created");
        }

        private static void List(string parameters)
        {
            foreach (var record in Service.GetRecords())
            {
                Console.WriteLine("#{0}, {1}, {2}, {3}, {4}, {5}, {6:f3}", record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.PersonalAccountType, record.PersonalAccountNumber, record.PersonalAccountBalance);
            }
        }
    }
}