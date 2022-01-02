using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp.Writers
{
    public static class DefaultPrint
    {
        public static void PrintRecocrd(IEnumerable<FileCabinetRecord> collection, string valueFind = null, string fildName = null)
        {
            if (collection == null)
            {
                Console.WriteLine("Incorrect date\n");
                return;
            }

            foreach (var record in collection)
            {
                if (!(record == null))
                {
                    PrintRecord(record);
                }
                else
                {
                    Console.WriteLine($"{fildName} {valueFind} not found\n");
                    return;
                }
            }
        }

        private static void PrintRecord(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            Console.WriteLine(
                        Defines.DefaultConsoleFormat,
                        record.Id,
                        record.FirstName,
                        record.LastName,
                        record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture),
                        record.Type,
                        record.Number,
                        record.Balance);
            Console.WriteLine();
        }
    }
}
