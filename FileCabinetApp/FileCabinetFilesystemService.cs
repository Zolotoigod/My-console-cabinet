using System;
using System.Collections.Generic;
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
        private readonly long beginOF;
        private int lastRecordNumber;
        private string recordsFormat = "{0,-6} | {1,-10} | {2,-20} | {3,-30} | {4, -30}\n";
        private StreamWriter writer;
        private StreamReader reader;

        public FileCabinetFilesystemService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
            this.streamDB = new FileStream("cabinet-records.db", FileMode.Create);
            this.writer = new StreamWriter(this.streamDB);
            this.reader = new StreamReader(this.streamDB);
            this.writer.Write(string.Format(CultureInfo.InvariantCulture, this.recordsFormat, "Offset", "DataType", "Field Size (bytes)", "Name", "Discription"));
            this.writer.WriteLine(RecordSeparate("-"));
            this.beginOF = this.streamDB.Position;
            this.writer.Flush();
        }

        public int CreateRecord(DataStorage storage)
        {
            BaseValidationRules.ValidationNull(storage);
            var record = new FileCabinetRecord(storage, this.validationRules, this.lastRecordNumber);
            this.WriteRecordToStream(record);
            this.lastRecordNumber = record.Id;
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
            this.streamDB.Position = this.beginOF + 219;
            List<FileCabinetRecord> list = new ();
            for (int i = 0; i < this.lastRecordNumber; i++)
            {
                list.Add(this.ReadRecordFromStream());
            }

            return list.AsReadOnly();
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
                this.streamDB.Dispose();
            }
        }

        private static string RecordSeparate(string recordSeparatSymbol, int count = 108)
        {
            StringBuilder resultString = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
               resultString.Append(recordSeparatSymbol);
            }

            return resultString.ToString();
        }

        private void WriteRecordToStream(FileCabinetRecord record)
        {
            int offset = 0;
            string formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "short", "2", "Status", "Reserved");
            this.writer.Write(formatRecord);
            offset += 2;

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "ID", record.Id);
            this.writer.Write(formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "Char[]", 120, "FirstName", record.FirstName);
            this.writer.Write(formatRecord);
            offset += 120;

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "Char[]", 120, "LastName", record.LastName);
            this.writer.Write(formatRecord);
            offset += 120;

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "Year", record.DateOfBirth.Year);
            this.writer.Write(formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "Month", record.DateOfBirth.Month);
            this.writer.Write(formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "int32", sizeof(int), "Day", record.DateOfBirth.Day);
            this.writer.Write(formatRecord);
            offset += sizeof(int);

            formatRecord = string.Format(CultureInfo.InvariantCulture, this.recordsFormat, offset, "Char", sizeof(char), "Type", record.Type);
            this.writer.Write(formatRecord);
            offset += sizeof(char);

            formatRecord = string.Format(CultureInfo.InvariantCulture, "{0,-6} | {1,-10} | {2,-20} | {3,-30} | {4:0000}\n", offset, "short", sizeof(short), "Number", record.Number);
            this.writer.Write(formatRecord);
            offset += sizeof(short);

            formatRecord = string.Format(CultureInfo.InvariantCulture, "{0,-6} | {1,-10} | {2,-20} | {3,-30} | {4:N2}\n", offset, "decimal", sizeof(decimal), "Balance", record.Balance);
            this.writer.Write(formatRecord);
            this.writer.WriteLine(RecordSeparate("-"));
            this.writer.Flush();
        }

        private FileCabinetRecord ReadRecordFromStream()
        {
            FileCabinetRecord record = new ();
            this.reader.ReadLine();
            string[] valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            record.Id = int.Parse(valueArray[4], CultureInfo.InvariantCulture);

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            record.FirstName = valueArray[4];

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            record.LastName = valueArray[4];

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            int year = int.Parse(valueArray[4], CultureInfo.InvariantCulture);

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            int month = int.Parse(valueArray[4], CultureInfo.InvariantCulture);

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            int day = int.Parse(valueArray[4], CultureInfo.InvariantCulture);

            record.DateOfBirth = new DateTime(year, month, day);

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            record.Type = valueArray[4][0];

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            record.Number = short.Parse(valueArray[4], CultureInfo.InvariantCulture);

            valueArray = this.reader.ReadLine().Split('|', StringSplitOptions.TrimEntries);
            record.Balance = decimal.Parse(valueArray[4], CultureInfo.InvariantCulture);

            this.reader.ReadLine();
            return record;
        }
    }
}
