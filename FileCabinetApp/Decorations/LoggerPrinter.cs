using System;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.Decorations
{
    public static class LoggerPrinter
    {
        public static void PrintLogInfo(StreamWriter writer, string metodName, string metodMessage)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName}";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"{metodName} returned {metodMessage}";
            writer?.WriteLine(logReturn);
            writer?.Flush();
        }

        public static void PrintLogInfo<T>(StreamWriter writer, string metodName, string metodMessage, T parameters)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName} whit '{parameters}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"{metodName} returned {metodMessage}";
            writer?.WriteLine(logReturn);
            writer?.Flush();
        }

        public static void PrintLogInfo<T>(StreamWriter writer, string metodName, string metodMessage, InputDataPack storage, T parameters)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling {metodName} record #{parameters} with" + PrintRecordInfo(storage);
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"{metodName} returned {metodMessage}";
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
