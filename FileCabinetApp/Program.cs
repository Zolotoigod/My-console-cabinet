using System;
using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    /// <summary>
    /// Client for File-Cabinet-Service.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Vladislav Shalkevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static readonly BaseCommandHandler RootHandler = Creator.CrateCommandChain();
        private static IFileCabinetService service;
        private static BaseValidationRules validationRules;
        private static bool isRunning = true;

        /// <summary>
        /// Main Client metod.
        /// </summary>
        /// <param name="args">input data.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            if (args != null)
            {
                AdditionalComandsMain(args);
            }

            Console.WriteLine(HintMessage);
            Console.WriteLine();

            // Add eny test objekt
            /*service.CreateRecord(new DataStorage("vlad", "shalkevich", new DateTime(1995, 09, 30), 'C', 7970, 1000m));
            service.CreateRecord(new DataStorage("Vladimir", "Putin", new DateTime(1986, 10, 08), 'B', 1111, 42m));
            service.CreateRecord(new DataStorage("Isaaak", "Newton", new DateTime(1996, 05, 26), 'B', 3434, 3.14m));
            service.CreateRecord(new DataStorage("Isaaak", "Newton", new DateTime(1996, 05, 26), 'B', 3434, 3.14m));
            service.CreateRecord(new DataStorage("Anna", "Rose", new DateTime(1994, 07, 18), 'A', 4242, 10000000m));
            service.CreateRecord(new DataStorage("Lucifer", "Morningstar", new DateTime(1951, 01, 22), 'A', 6666, 666666.666m));*/

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];
                var parameters = inputs.Length > 1 ? inputs[1] : string.Empty;

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                if (command.Equals("exit", StringComparison.InvariantCultureIgnoreCase))
                {
                    Exit();
                }

                if (RootHandler.HandleCommand(service, validationRules, command, parameters))
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

        private static void Exit()
        {
            if (service is FileCabinetFileService end)
            {
                end.Dispose();
            }

            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void AdditionalComandsMain(string[] args)
        {
            int validationIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--validation_rules", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--v", StringComparison.InvariantCultureIgnoreCase));
            int storageIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--storage", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--s", StringComparison.InvariantCultureIgnoreCase));
            if (validationIndex >= 0)
            {
                SetValidator(args, validationIndex, out validationRules);
            }
            else
            {
                validationRules = new DefaultValidateRules();
            }

            if (storageIndex >= 0)
            {
                SetStorage(args, storageIndex, validationRules, out service);
            }
            else
            {
                service = new FileCabinetMemoryService(validationRules);
            }

            Console.WriteLine($"Using {validationRules} validation rules");
            Console.WriteLine($"Using {service} storage");
        }

        private static void SetValidator(string[] args, int validationIndex, out BaseValidationRules validationRules)
        {
            if (!string.IsNullOrWhiteSpace(args[validationIndex + 1]))
            {
                switch (args[validationIndex + 1].ToLowerInvariant())
                {
                    case "default":
                        {
                            validationRules = new DefaultValidateRules();
                            break;
                        }

                    case "custom":
                        {
                            validationRules = new CustomValdationRules();
                            break;
                        }

                    default:
                        {
                            validationRules = new DefaultValidateRules();
                            Console.WriteLine($"Validator {args[validationIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                validationRules = new DefaultValidateRules();
            }
        }

        private static void SetStorage(string[] args, int storageIndex, BaseValidationRules validationRules, out IFileCabinetService service)
        {
            if (!string.IsNullOrWhiteSpace(args[storageIndex + 1]))
            {
                switch (args[storageIndex + 1].ToLowerInvariant())
                {
                    case "memory":
                        {
                            service = new FileCabinetMemoryService(validationRules);
                            break;
                        }

                    case "file":
                        {
                            service = new FileCabinetFileService(validationRules);
                            break;
                        }

                    default:
                        {
                            service = new FileCabinetMemoryService(validationRules);
                            Console.WriteLine($"Service storage {args[storageIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                service = new FileCabinetMemoryService(validationRules);
            }
        }
    }
}