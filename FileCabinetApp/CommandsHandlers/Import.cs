using System;
using System.IO;
using System.Xml;

namespace FileCabinetApp.CommandHandlers
{
    public class Import : BaseCommandHandler
    {
        public Import(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            string[] importParams = parameters?.Split(' ', 2);
            if (importParams.Length == 2)
            {
                var index = Array.FindIndex(Defines.AvailableExportFormats, 0, Defines.AvailableExportFormats.Length, match => match.Equals(importParams[0], StringComparison.InvariantCultureIgnoreCase));

                if (File.Exists(importParams[1]))
                {
                    var importFormat = ReaderFormatSwitch(index);
                    importFormat(service, validationRules, importParams[1]);
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

        private static Action<IFileCabinetService, BaseValidationRules, string> ReaderFormatSwitch(int index) => index switch
        {
            0 => CallCSVReader,
            1 => CallXMLReader,
            _ => UnsupportedFormat,
        };

        private static void CallCSVReader(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
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

        private static void CallXMLReader(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            using (FileStream streamReader = new FileStream(parameters, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var xmlReader = XmlReader.Create(streamReader))
                {
                    var importSnapshot = new FileCabinetServiceSnapshot(xmlReader, validationRules);
                    Console.WriteLine($"{importSnapshot.Records.Count} records were imported from {parameters}");
                    service.Restore(importSnapshot);
                }
            }
        }

        private static void UnsupportedFormat(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            Console.WriteLine($"Import format {parameters} unsupported now!(");
        }
    }
}
