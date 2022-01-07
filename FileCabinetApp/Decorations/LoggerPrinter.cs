using System;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.Decorations
{
    public static class LoggerPrinter
    {
        public static void PrintLogCreate(StreamWriter writer, InputDataPack storage, int id)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling Create() with" + PrintRecordInfo(storage);
            string logReturn = $"Create() returned '{id}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            writer?.WriteLine(logReturn);
        }

        public static void PrintLogEdit(StreamWriter writer, InputDataPack storage, int id)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling Edit() record #{id} with" + PrintRecordInfo(storage);
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
        }

        public static void PrintLogFind(StreamWriter writer, string data, int count)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling Find() with '{data}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"Find() returned {count} matching records";
            writer?.WriteLine(logReturn);
        }

        public static void PrintLogRemove(StreamWriter writer, int id, string message)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling Remove() with '{id}'";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"Remove() returned '{message}'";
            writer?.WriteLine(logReturn);
        }

        public static void PrintLogGetRecord(StreamWriter writer, int count)
        {
            string logDate = DateTime.Now.ToString(Defines.LoggerDateFormat, CultureInfo.InvariantCulture);
            string logAction = $"Calling List()";
            writer?.WriteLine(string.Join(" - ", logDate, logAction));
            string logReturn = $"Find() returned {count} records";
            writer?.WriteLine(logReturn);
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
