using System;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.Decorations
{
    public static class LoggerPrinter
    {
        public static void PrintLogCreate(StreamWriter writer, InputDataPack storage, int id)
        {
            string metodName = "Create()";
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName} with" + PrintRecordInfo(storage);
            string logReturn = $"{metodName} returned '{id}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            writer?.WriteLine(logReturn);
            writer?.Flush();
        }

        public static void PrintLogEdit(StreamWriter writer, InputDataPack storage, int id)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling Edit() record #{id} with" + PrintRecordInfo(storage);
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            writer?.WriteLine($"Record #{id} is updated");
            writer?.Flush();
        }

        public static void PrintLogFind(StreamWriter writer, string data, int count)
        {
            string metodName = "Find()";
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName} with '{data}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"{metodName} returned {count} matching records";
            writer?.WriteLine(logReturn);
            writer?.Flush();
        }

        public static void PrintLogRemove(StreamWriter writer, int id, string message)
        {
            string metodName = "Remove()";
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName} with '{id}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"{metodName} returned '{message}'";
            writer?.WriteLine(logReturn);
            writer?.Flush();
        }

        public static void PrintLogGetRecord(StreamWriter writer, int count)
        {
            string metodName = "List()";
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName}";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"{metodName} returned {count} records";
            writer?.WriteLine(logReturn);
            writer?.Flush();
        }

        private static string PrintRecordInfo(InputDataPack storage)
        {
            return $" FirstName = '{storage?.FirstName}'," +
                $" LastName = '{storage.LastName}'," +
                $" DateOfBirth = '{storage.DateOfBirth.ToString(Defines.DateFormat[1], CultureInfo.InvariantCulture)}'," +
                $" Type '{string.Concat(storage.Type)}'," +
                $" Number '{string.Format(CultureInfo.InvariantCulture, "{0:0000}", storage.Number)}'," +
                $" Balance '{string.Format(CultureInfo.InvariantCulture, "{0:f2}", storage.Balance)}'";
        }
    }
}
