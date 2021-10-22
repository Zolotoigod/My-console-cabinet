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
        private static readonly string[] AvailableExportFormats = { "csv", "xml" };
        private static readonly FileCabinetService Service = new ();

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
        };

        private static BaseValidationRules validationRules;
        private static bool isRunning = true;

        /// <summary>
        /// Main Client metod.
        /// </summary>
        /// <param name="args">input data.</param>
        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {DeveloperName}");
            validationRules = Service.CreateValidator(args);
            Console.WriteLine($"Using {validationRules} validation rules");
            Console.WriteLine(HintMessage);
            Console.WriteLine();

            // Add eny test objekt
            Service.CreateRecord(new DataStorage("vlad", "shalkevich", new DateTime(1995, 09, 30), 'C', 7970, 1000m));
            Service.CreateRecord(new DataStorage("Vladimir", "Putin", new DateTime(1986, 10, 08), 'B', 1111, 42m));
            Service.CreateRecord(new DataStorage("Isaaak", "Newton", new DateTime(1996, 05, 26), 'A', 3434, 3.14m));
            Service.CreateRecord(new DataStorage("Isaaak", "Newton", new DateTime(1996, 05, 26), 'A', 3434, 3.14m));

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
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Create(string parameters)
        {
            DataStorage record = new (validationRules);
            int id = Service.CreateRecord(record);
            Console.WriteLine($"Record #{id} is created\n");
        }

        private static void Edit(string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                if (id <= Service.GetStat() && id > 0)
                {
                    DataStorage record = new (validationRules);
                    Service.EditRecord(id, record);
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
                            if (Service.FindByFirstName(serchedField[1]).Count == 0)
                            {
                                Console.WriteLine($"firstName {serchedField[1]} not found\n");
                                break;
                            }

                            foreach (var record in Service.FindByFirstName(serchedField[1]))
                            {
                                Console.WriteLine("#{0}, {1}, {2}, {3}, {4}, {5}, {6:f3}", record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
                            }

                            Console.WriteLine();
                            break;
                        }

                    case "LASTNAME":
                        {
                            if (Service.FindByLastName(serchedField[1]).Count == 0)
                            {
                                Console.WriteLine($"Lastname {serchedField[1]} not found\n");
                                break;
                            }

                            foreach (var record in Service.FindByLastName(serchedField[1]))
                            {
                                Console.WriteLine("#{0}, {1}, {2}, {3}, {4}, {5}, {6:f3}", record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
                            }

                            Console.WriteLine();
                            break;
                        }

                    case "DATEOFBIRTH":
                        {
                            if (Service.FindByDateOfBirth(serchedField[1]) == null)
                            {
                                Console.WriteLine("Incorrect date\n");
                                break;
                            }

                            if (Service.FindByDateOfBirth(serchedField[1]).Count == 0)
                            {
                                Console.WriteLine($"DateOfBirth {serchedField[1]} not found\n");
                                break;
                            }

                            foreach (var record in Service.FindByDateOfBirth(serchedField[1]))
                            {
                                Console.WriteLine("#{0}, {1}, {2}, {3}, {4}, {5}, {6:f3}", record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
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
            foreach (var record in Service.GetRecords())
            {
                Console.WriteLine("#{0}, {1}, {2}, {3}, {4}, {5}, {6:f3}", record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
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
            var newSnapshot = Service.MakeSnapshot();
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
            var newSnapshot = Service.MakeSnapshot();
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
    }
}