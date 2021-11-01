using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    class FileCabinetGenerator
    {
        static void Main(string[] args)
        {
            int[] commandsIndex = AdditionalComandsMain(args);
            int amount = int.Parse(args[commandsIndex[2] + 1]);
            int firstID = int.Parse(args[commandsIndex[3] + 1]);
            using (StreamWriter stream = new(args[commandsIndex[1] + 1], false, System.Text.Encoding.UTF8))
            {
                IFileCabinetRecordWriter formatWriter = SetWriter(stream, args[commandsIndex[0] + 1].ToUpperInvariant());
                if (formatWriter != null)
                {
                    if (formatWriter is FileCabinetRecordXmlWriter && true)
                    {
                        XMLSerializ(stream, amount, firstID);
                    }
                    else
                    {
                        formatWriter.WriteRootStart(formatWriter.GetRoot());

                        for (int i = 0; i < amount; i++)
                        {
                            formatWriter.Write(GetRandomRecordDefaultVal(firstID));
                            firstID++;
                        }

                        formatWriter.WriteRootEnd();
                    }

                    Console.WriteLine($"{args[commandsIndex[2] + 1]} records were written to {args[commandsIndex[1] + 1]}");
                }
                else
                {
                    Console.WriteLine("Export format unsupported");
                }
            }
        }

        private static int[] AdditionalComandsMain(string[] args)
        {
            int[] comands = new int[]
            {
                Array.FindIndex(args, 0, args.Length, match => match.Equals("--output-type", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--t", StringComparison.InvariantCultureIgnoreCase)),
                Array.FindIndex(args, 0, args.Length, match => match.Equals("--output", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--o", StringComparison.InvariantCultureIgnoreCase)),
                Array.FindIndex(args, 0, args.Length, match => match.Equals("--records-amount", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--a", StringComparison.InvariantCultureIgnoreCase)),
                Array.FindIndex(args, 0, args.Length, match => match.Equals("--start-id", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--i", StringComparison.InvariantCultureIgnoreCase))
            };

            if (CommandsValidator(comands))
            {
                return comands;
            }

            Console.WriteLine("Unsuported commands");
            return Array.Empty<int>();
        }

        public static bool CommandsValidator(int[] comands)
        {
            foreach (var index in comands)
            {
                if (index < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static DateTime RandomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            Random days = new Random();
            return start.AddDays(days.Next(range));
        }

        private static FileCabinetRecord GetRandomRecordDefaultVal(int id)
        {
            FileCabinetRecord result = new ();
            Random joker = new Random();
            result.Id = id;
            result.FirstName = ((FirstName)joker.Next(0, 9)).ToString();
            result.LastName = ((LastName)joker.Next(0, 9)).ToString();
            result.DateOfBirth = RandomDate(new DateTime(1950, 1, 1), DateTime.Today);
            result.Type = ((Types)joker.Next(0, 2)).ToString()[0];
            result.Number = (short)joker.Next(1, 9999);
            result.Balance = (decimal)(joker.NextDouble() * joker.Next(1, 1000000000));
            return result;
        }

        private static IFileCabinetRecordWriter SetWriter(TextWriter writer, string format) => format switch
        {
            "CSV" => new FileCabinetRecordCsvWriter(writer),
            "XML" => new FileCabinetRecordXmlWriter(writer),
            _ => null
        };

        private static void XMLSerializ(StreamWriter stream, int amount, int id)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<FileCabinetRecord>));
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            for (int i = 0; i < amount; i++)
            {
                records.Add(GetRandomRecordDefaultVal(id));
                id++;
            }

            serializer.Serialize(stream, records);
        }
    }
}