using System;
using System.IO;
using System.Xml;
using FileCabinetApp.Validation.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class Import : ServiceCommandHandler
    {
        public Import(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            string[] importParams = parameters?.Split(' ', 2);
            if (importParams.Length == 2)
            {
                var index = Array.FindIndex(Defines.AvailableExportFormats, 0, Defines.AvailableExportFormats.Length, match => match.Equals(importParams[0], StringComparison.InvariantCultureIgnoreCase));

                if (File.Exists(importParams[1]))
                {
                    var importFormat = ReaderFormatSwitch(index);
                    importFormat(this.Service, validator, importParams[1]);
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

        private static Action<IFileCabinetService, DataValidator, string> ReaderFormatSwitch(int index) => index switch
        {
            0 => CallCSVReader,
            1 => CallXMLReader,
            _ => UnsupportedFormat,
        };

        private static void CallCSVReader(IFileCabinetService service, DataValidator validator, string parameters)
        {
            using (FileStream streamReader = new FileStream(parameters, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var newStreamReader = new StreamReader(streamReader))
                {
                    var importSnapshot = new FileCabinetServiceSnapshot(newStreamReader, validator);
                    Console.WriteLine($"{importSnapshot.Records.Count} records were imported from {parameters}");
                    service.Restore(importSnapshot);
                }
            }
        }

        private static void CallXMLReader(IFileCabinetService service, DataValidator validator, string parameters)
        {
            using (FileStream streamReader = new FileStream(parameters, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var xmlReader = XmlReader.Create(streamReader))
                {
                    var importSnapshot = new FileCabinetServiceSnapshot(xmlReader, validator);
                    Console.WriteLine($"{importSnapshot.Records.Count} records were imported from {parameters}");
                    service.Restore(importSnapshot);
                }
            }
        }

        private static void UnsupportedFormat(IFileCabinetService service, DataValidator validator, string parameters)
        {
            Console.WriteLine($"Import format {parameters} unsupported now!(");
        }
    }
}
