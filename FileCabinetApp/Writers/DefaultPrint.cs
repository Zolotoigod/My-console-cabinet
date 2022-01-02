using System;
using System.Globalization;

namespace FileCabinetApp.Writers
{
    public static class DefaultPrint
    {
        public static void PrintRecocrd(FileCabinetRecord record)
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
