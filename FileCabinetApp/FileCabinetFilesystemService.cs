using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private readonly BaseValidationRules validationRules;
        private readonly FileStream streamDB;
        private int lastRecordNumber;
        private string recordsFormat = "{0,-6} | {1,-10} | {2,-20} | {3,-30} | {4, -30}\n";

        public FileCabinetFilesystemService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
            this.streamDB = File.Create("cabinet-records.db");
            WriteText(this.streamDB, string.Format(CultureInfo.InvariantCulture, this.recordsFormat, "Offset", "DataType", "Field Size (bytes)", "Name", "Discription"));
            this.RecordSeparate(84, "-");
            this.streamDB.Flush();
        }

        public int CreateRecord(DataStorage storage)
        {
            BaseValidationRules.ValidationNull(storage);
            var record = new FileCabinetRecord(storage, this.validationRules, this.lastRecordNumber);
            this.WriteRecordToStream(record);
            this.streamDB.Flush();
            this.lastRecordNumber = record.Id;
            this.RecordSeparate(84, "-");
            return this.lastRecordNumber;
        }

        public void EditRecord(int id, DataStorage storeage)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Filesistem";
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.streamDB.Close();
            }
        }

        private static void WriteText(FileStream stream, string value)
        {
            byte[] data = new UTF8Encoding(true).GetBytes(value);
            stream.Write(data, 0, data.Length);
        }

        private void RecordSeparate(int count, string recordSeparatSymbol)
        {
            for (int i = 0; i < count; i++)
            {
                WriteText(this.streamDB, recordSeparatSymbol);
            }

            WriteText(this.streamDB, "\n");
        }

        private void WriteRecordToStream(FileCabinetRecord record)
        {
            int offset = 0;
            string formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "short", "2", "Status", "Reserved");
            WriteText(this.streamDB, formatRecord);
            offset += 2;

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "ID", record.Id);
            WriteText(this.streamDB, formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "Char[]", 120, "FirstName", record.FirstName);
            WriteText(this.streamDB, formatRecord);
            offset += 120;

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "Char[]", 120, "LastName", record.LastName);
            WriteText(this.streamDB, formatRecord);
            offset += 120;

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "Year", record.DateOfBirth.Year);
            WriteText(this.streamDB, formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "Month", record.DateOfBirth.Month);
            WriteText(this.streamDB, formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "Day", record.DateOfBirth.Day);
            WriteText(this.streamDB, formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "Char", sizeof(char), "Type", record.Type);
            WriteText(this.streamDB, formatRecord);
            offset += sizeof(char);

            formatRecord = string.Format(CultureInfo.InvariantCulture, "{0,-6} | {1,-10} | {2,-20} | {3,-30} | {4:0000}\n", offset, "short", sizeof(short), "Number", record.Number);
            WriteText(this.streamDB, formatRecord);
            offset += sizeof(short);

            formatRecord = string.Format(CultureInfo.InvariantCulture, "{0,-6} | {1,-10} | {2,-20} | {3,-30} | {4:N2}\n", offset, "decimal", sizeof(decimal), "Balance", record.Balance);
            WriteText(this.streamDB, formatRecord);
        }
    }
}
