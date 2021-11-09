using System;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Client for File-Cabinet-Service.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Vladislav Shalkevich";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;
        private const string ConsleFormat = "#{0}, {1}, {2}, {3}, {4}, {5}, {6:f2}";
        private static readonly string[] AvailableExportFormats = { "csv", "xml" };

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates a record", "The 'create' command creates a record." },
            new string[] { "edit", "edits the record by id", "The 'edit' edits the record by id." },
            new string[] { "find", "searches a record by field", "The 'find' searches a record by field." },
            new string[] { "list", "prints all of records", "The 'list' command prints all of records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };

        private static readonly Tuple<string, Action<string>>[] Comands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
        };

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

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(Comands, 0, Comands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    Comands[index].Item2(parameters);
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
                var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            if (service is FileCabinetFilesystemService)
            {
                FileCabinetFilesystemService end = (FileCabinetFilesystemService)service;
                end.Dispose();
            }

            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Create(string parameters)
        {
            DataStorage record = new (validationRules);
            int id = service.CreateRecord(record);
            Console.WriteLine($"Record #{id} is created\n");
        }

        private static void Edit(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                if (id <= service.GetStat() && id > 0)
                {
                    DataStorage record = new (validationRules);
                    service.EditRecord(id, record);
                    Console.WriteLine($"record #{id} is updated\n");
                }
                else
                {
                    Console.WriteLine($"#{parameters} record is not found\n");
                }
            }
            else
            {
                Console.WriteLine("Incorrect id\n");
            }
        }

        private static void Find(string parameters)
        {
            string[] serchedField = parameters.Split(' ', 2);
            if (serchedField.Length == 2)
            {
                serchedField[0] = serchedField[0].ToUpperInvariant();
                switch (serchedField[0])
                {
                    case "FIRSTNAME":
                        {
                            if (service.FindByFirstName(serchedField[1]).Count == 0)
                            {
                                Console.WriteLine($"firstName {serchedField[1]} not found\n");
                                break;
                            }

                            foreach (var record in service.FindByFirstName(serchedField[1]))
                            {
                                Console.WriteLine(ConsleFormat, record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
                            }

                            Console.WriteLine();
                            break;
                        }

                    case "LASTNAME":
                        {
                            if (service.FindByLastName(serchedField[1]).Count == 0)
                            {
                                Console.WriteLine($"Lastname {serchedField[1]} not found\n");
                                break;
                            }

                            foreach (var record in service.FindByLastName(serchedField[1]))
                            {
                                Console.WriteLine(ConsleFormat, record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
                            }

                            Console.WriteLine();
                            break;
                        }

                    case "DATEOFBIRTH":
                        {
                            if (service.FindByDateOfBirth(serchedField[1]) == null)
                            {
                                Console.WriteLine("Incorrect date\n");
                                break;
                            }

                            if (service.FindByDateOfBirth(serchedField[1]).Count == 0)
                            {
                                Console.WriteLine($"DateOfBirth {serchedField[1]} not found\n");
                                break;
                            }

                            foreach (var record in service.FindByDateOfBirth(serchedField[1]))
                            {
                                Console.WriteLine(ConsleFormat, record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
                            }

                            Console.WriteLine();
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Unknown field\n");
                            break;
                        }
                }
            }
            else
            {
                Console.WriteLine("Incorrect find parameters\n");
            }
        }

        private static void List(string parameters)
        {
            foreach (var record in service.GetRecords())
            {
                Console.WriteLine(ConsleFormat, record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
            }

            Console.WriteLine();
        }

        private static void Export(string parameters)
        {
            string[] exportParams = parameters.Split(' ', 2);
            var index = Array.FindIndex(AvailableExportFormats, 0, AvailableExportFormats.Length, match => match.Equals(exportParams[0], StringComparison.InvariantCultureIgnoreCase));
            if (index < 0)
            {
                exportParams[1] = "Export format unsupported!";
            }

            var exportFormat = WriterFormatSwitch(index);

            if (File.Exists(exportParams[1]))
            {
                ConsoleKey key;
                do
                {
                    Console.WriteLine($"File is exist - rewrite {exportParams[1]} [Y / N]> ");
                    key = Console.ReadKey().Key;
                    Console.WriteLine();
                }
                while (!(((int)key) == 89 || (int)key == 78));

                if (KeySwitch(key))
                {
                    exportFormat(exportParams[1]);
                }
            }
            else
            {
                exportFormat(exportParams[1]);
            }
        }

        private static void Import(string parameters)
        {
            string[] importParams = parameters.Split(' ', 2);
            if (importParams.Length == 2)
            {
                var index = Array.FindIndex(AvailableExportFormats, 0, AvailableExportFormats.Length, match => match.Equals(importParams[0], StringComparison.InvariantCultureIgnoreCase));
                if (index < 0)
                {
                    importParams[1] = "Export format unsupported!";
                }
                else if (File.Exists(importParams[1]))
                {
                    var importFormat = ReaderFormatSwitch(index);
                    importFormat(importParams[1]);
                }
                else
                {
                    Console.WriteLine($"Import error: file {importParams[1]} is not exist.");
                }
            }
            else
            {
                Console.WriteLine($"Import error: wrong parameters");
            }
        }

        private static Action<string> ReaderFormatSwitch(int index) => index switch
        {
            0 => CallCSVReader,
            1 => CallXMLReader,
            _ => Console.WriteLine
        };

        private static Action<string> WriterFormatSwitch(int index) => index switch
        {
            0 => CallCSVWriter,
            1 => CallXMLWriter,
            _ => Console.WriteLine
        };

        private static bool KeySwitch(ConsoleKey key) => key switch
        {
            ConsoleKey.Y => true,
            ConsoleKey.N => false,
            _ => false
        };

        private static void CallCSVWriter(string parameters)
        {
            var newSnapshot = service.MakeSnapshot();
            try
            {
                StreamWriter writer = new (parameters, false, System.Text.Encoding.UTF8);
                newSnapshot.SaveToCSV(writer);
                writer.Close();
                Console.WriteLine($"All records are exported to file {parameters}.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {parameters}");
            }
        }

        private static void CallXMLWriter(string parameters)
        {
            var newSnapshot = service.MakeSnapshot();
            try
            {
                StreamWriter writer = new (parameters, false, System.Text.Encoding.UTF8);
                newSnapshot.SaveToXML(writer);
                writer.Close();
                Console.WriteLine($"All records are exported to file {parameters}.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {parameters}");
            }
        }

        private static void CallCSVReader(string parameters)
        {
            using (FileStream streamReader = new FileStream(parameters, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var newStreamReader = new StreamReader(streamReader))
                {
                    var importSnapshot = new FileCabinetServiceSnapshot(newStreamReader, validationRules);
                    Console.WriteLine($"{importSnapshot.Records.Count} records were imported from {parameters}");
                    service.Restore(importSnapshot);
                }
            }
        }

        private static void CallXMLReader(string parameters)
        {
            /*var newSnapshot = service.MakeSnapshot();
            try
            {
                StreamWriter writer = new (parameters, false, System.Text.Encoding.UTF8);
                newSnapshot.SaveToXML(writer);
                writer.Close();
                Console.WriteLine($"All records are exported to file {parameters}.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {parameters}");
            }*/
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
                            service = new FileCabinetFilesystemService(validationRules);
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