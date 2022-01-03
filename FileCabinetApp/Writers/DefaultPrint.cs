using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp.Writers
{
    public static class DefaultPrint
    {
        public static void PrintRecocrd(IEnumerable<FileCabinetRecord> collection)
        {
            if (collection == null)
            {
                Console.WriteLine("There is no data for you(\n");
                return;
            }

            byte swither = 0;
            foreach (var record in collection)
            {
                PrintRecord(record);
                swither++;
            }

            if (swither == 0)
            {
                Console.WriteLine("There is no data for you(\n");
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
