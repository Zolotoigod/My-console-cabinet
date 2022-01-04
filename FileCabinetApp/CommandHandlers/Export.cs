using System;
using System.IO;

namespace FileCabinetApp.CommandHandlers
{
    public class Export : ServiceCommandHandler
    {
        public Export(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            string[] exportParams = parameters?.Split(' ', 2);
            var index = Array.FindIndex(Defines.AvailableExportFormats, 0, Defines.AvailableExportFormats.Length, match => match.Equals(exportParams[0], StringComparison.InvariantCultureIgnoreCase));

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
                    exportFormat(this.Service, exportParams[1]);
                }
            }
            else
            {
                exportFormat(this.Service, exportParams[1]);
            }
        }

        private static Action<IFileCabinetService, string> WriterFormatSwitch(int index) => index switch
        {
            0 => CallCSVWriter,
            1 => CallXMLWriter,
            _ => UnsupportedFormat,
        };

        private static void CallCSVWriter(IFileCabinetService service, string parameters)
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

        private static void CallXMLWriter(IFileCabinetService service, string parameters)
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

        private static void UnsupportedFormat(IFileCabinetService service, string parameters)
        {
            Console.WriteLine($"Export format {parameters} unsupported now!(");
        }

        private static bool KeySwitch(ConsoleKey key) => key switch
        {
            ConsoleKey.Y => true,
            ConsoleKey.N => false,
            _ => false
        };
    }
}
