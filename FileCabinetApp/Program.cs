using System;
using FileCabinetApp.CommandHandlers;
using FileCabinetApp.DTO;

namespace FileCabinetApp
{
    /// <summary>
    /// Client for File-Cabinet-Service.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Vladislav Shalkevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";

        private static ICommandHandler rootHandler;
        private static IFileCabinetService service;
        private static BaseValidationRules validator;
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
                ProgramSetup.SetOptions(out service, out validator, args);
                rootHandler = HandlerFabric.CrateCommandChain(service, Exit);
            }

            Console.WriteLine(HintMessage);
            Console.WriteLine();

            // Add eny test objekt
            /*service.CreateRecord(new DataStorage("Vlad", "Shalkevich", new DateTime(1995, 09, 30), 'C', 7970, 1000m));
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

                rootHandler.HandleCommand(validator, new AppCommandRequest(command, parameters));
            }
            while (isRunning);
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
    }
}