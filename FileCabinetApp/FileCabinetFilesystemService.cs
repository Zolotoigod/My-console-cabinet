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
        private readonly int recordSize = sizeof(short) + sizeof(int) + 120 + 120 + (3 * sizeof(int)) + 2 + sizeof(short) + sizeof(decimal);
        private readonly UTF8Encoding coder = new ();
        private readonly short reservedField = 7;
        private int lastRecordNumber;

        public FileCabinetFilesystemService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
            bool existNow = false;
            if (File.Exists("cabinet-records.db"))
            {
                existNow = true;
            }

            this.streamDB = new FileStream("cabinet-records.db", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (existNow)
            {
                this.streamDB.Position = this.streamDB.Length - this.recordSize + 2;
                using (BinaryReader reader = new (this.streamDB, Encoding.UTF8, true))
                {
                    this.lastRecordNumber = reader.ReadInt32();
                }
            }
        }

        public int CreateRecord(DataStorage storage)
        {
            BaseValidationRules.ValidationNull(storage);
            var record = new FileCabinetRecord(storage, this.validationRules, this.lastRecordNumber);
            this.WriteRecordToFile(record, this.streamDB.Length);
            this.lastRecordNumber += 1;
            return record.Id;
        }

        public void EditRecord(int id, DataStorage storage)
        {
            long position = (this.recordSize * (id - 1)) + 6;
            var record = new FileCabinetRecord(storage, this.validationRules, this.lastRecordNumber);
            this.WriteRecordToFile(record, position, false);
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
            int startPosition = 0;
            List<FileCabinetRecord> records = new ();
            for (int i = 1; i <= this.lastRecordNumber; i++)
            {
                records.Add(this.ReadRecordFormFile(startPosition));
                startPosition += this.recordSize;
            }

            return records.AsReadOnly();
        }

        public int GetStat()
        {
            return this.lastRecordNumber;
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

        private static char[] GetFixLenthString(string value, int count)
        {
            char[] result = Enumerable.Repeat(' ', count).ToArray();
            char[] name = value.ToCharArray();
            name.CopyTo(result, 0);
            return result;
        }

        private FileCabinetRecord ReadRecordFormFile(int position)
        {
            this.streamDB.Position = position;
            FileCabinetRecord readedRecord = new ();
            using (BinaryReader reader = new (this.streamDB, Encoding.UTF8, true))
            {
                reader.ReadInt16();
                readedRecord.Id = reader.ReadInt32();
                readedRecord.FirstName = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                readedRecord.LastName = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                int year = reader.ReadInt32();
                int month = reader.ReadInt32();
                int day = reader.ReadInt32();
                readedRecord.DateOfBirth = new DateTime(year, month, day);
                readedRecord.Type = this.coder.GetChars(reader.ReadBytes(2))[0];
                readedRecord.Number = reader.ReadInt16();
                readedRecord.Balance = reader.ReadDecimal();
            }

            return readedRecord;
        }

        private void WriteRecordToFile(FileCabinetRecord record, long position, bool isCreate = true)
        {
            this.streamDB.Position = position;
            using (BinaryWriter writer = new (this.streamDB, Encoding.UTF8, true))
            {
                if (isCreate)
                {
                    writer.Write(this.reservedField);
                    writer.Write(record.Id);
                }

                writer.Write(this.coder.GetBytes(GetFixLenthString(record.FirstName, 120)));
                writer.Write(this.coder.GetBytes(GetFixLenthString(record.LastName, 120)));
                writer.Write(record.DateOfBirth.Year);
                writer.Write(record.DateOfBirth.Month);
                writer.Write(record.DateOfBirth.Day);
                writer.Write(this.coder.GetBytes(GetFixLenthString(record.Type.ToString(), 2)));
                writer.Write(record.Number);
                writer.Write(record.Balance);
            }
        }
    }
}
