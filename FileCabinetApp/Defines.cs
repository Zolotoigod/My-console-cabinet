using System.Collections.Generic;
using FileCabinetApp.Validation.Service;

#pragma warning disable CA1062

namespace FileCabinetApp
{
    public static class Defines
    {
        public const string LoggerDateFormat = "MM/dd/yyyy HH:mm";

        public const string LoggerPath = "../../../../ServiceLog.txt";

        public const string DBPath = "cabinet-records.db";

        public const string ConfigPath = "../../../validation-rules.json";

        public const string AvailableFields = "ID, F.tName, L.Name, D.OfBirth, Type, Number, Balance";

        public const string DefaultConsoleFormat = "#{0}, {1}, {2}, {3}, {4}, {5:0000}, {6:f2}";

        public static readonly string[] AvailableExportFormats = { "csv", "xml" };

        public static readonly string[] DateFormat = { "MM dd yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM,dd,yyyy", "dd MM yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "dd,MM,yyyy" };

        public static readonly int RecordSize = sizeof(short) + sizeof(int) + 120 + 120 + (3 * sizeof(int)) + 2 + sizeof(short) + sizeof(decimal);

        public static IRecordValidator GetValidator(string validatorName) => validatorName.ToLowerInvariant() switch
        {
            "custom" => new ValidatorBuilder().CreateCustom(),
            "config" => new ValidatorBuilder().ReadConfig(),
            _ => new ValidatorBuilder().CreateDefault(),
        };

        public static IEnumerable<FileCabinetRecord> ReturnCollection(IEnumerable<FileCabinetRecord> collection)
        {
            foreach (var record in collection)
            {
                yield return record;
            }
        }
    }
}
