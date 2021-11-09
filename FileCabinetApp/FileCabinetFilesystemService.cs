using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly Dictionary<string, List<long>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new ();
        private readonly string filename = "cabinet-records.db";
        private int lastRecordNumber;

        public FileCabinetFilesystemService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
            bool existNow = false;
            if (File.Exists(this.filename))
            {
                existNow = true;
            }

            this.streamDB = new FileStream(this.filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (existNow)
            {
                this.streamDB.Position = this.streamDB.Length - this.recordSize + 2;
                using (BinaryReader reader = new (this.streamDB, Encoding.UTF8, true))
                {
                    this.lastRecordNumber = reader.ReadInt32();
                }

                this.InitialDictionary();
            }
        }

        public int CreateRecord(DataStorage storage)
        {
            BaseValidationRules.ValidationNull(storage);
            var record = new FileCabinetRecord(storage, this.validationRules, this.lastRecordNumber);
            this.UpdateDictionary(storage, this.streamDB.Length);
            this.WriteRecordToFile(record, this.streamDB.Length);
            this.lastRecordNumber += 1;
            return record.Id;
        }

        public void EditRecord(int id, DataStorage storage)
        {
            long position = (this.recordSize * (id - 1)) + 6;
            var record = new FileCabinetRecord(storage, this.validationRules, this.lastRecordNumber);
            this.firstNameDictionary.Remove(storage.FirstName);
            this.lastNameDictionary.Remove(storage.LastName);
            this.dateOfBirthDictionary.Remove(storage.DateOfBirth);
            this.UpdateDictionary(storage, position - 6);
            this.WriteRecordToFile(record, position, false);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date = new ();
            if (DateTime.TryParse(dateOfBirth, out date) && this.dateOfBirthDictionary.ContainsKey(date))
            {
                List<FileCabinetRecord> result = new ();
                for (int i = 0; i < this.dateOfBirthDictionary[date].Count; i++)
                {
                    result.Add(this.ReadRecordFormFile(this.dateOfBirthDictionary[date][i]));
                }

                return result.AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                List<FileCabinetRecord> result = new ();
                for (int i = 0; i < this.firstNameDictionary[firstName.ToUpperInvariant()].Count; i++)
                {
                    result.Add(this.ReadRecordFormFile(this.firstNameDictionary[firstName.ToUpperInvariant()][i]));
                }

                return result.AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                List<FileCabinetRecord> result = new ();
                for (int i = 0; i < this.lastNameDictionary[lastName.ToUpperInvariant()].Count; i++)
                {
                    result.Add(this.ReadRecordFormFile(this.lastNameDictionary[lastName.ToUpperInvariant()][i]));
                }

                return result.AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
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

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            throw new NotImplementedException();
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

        private FileCabinetRecord ReadRecordFormFile(long position)
        {
            this.streamDB.Position = position;
            FileCabinetRecord readedRecord = new ();
            using (BinaryReader reader = new (this.streamDB, Encoding.UTF8, true))
            {
                reader.ReadInt16();
                readedRecord.Id = reader.ReadInt32();
                readedRecord.FirstName = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                readedRecord.LastName = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                readedRecord.DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
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

        private void InitialDictionary()
        {
            for (long pos = 0; pos < this.streamDB.Length; pos += this.recordSize)
            {
                this.streamDB.Position = pos + 6;
                using (BinaryReader reader = new (this.streamDB, Encoding.UTF8, true))
                {
                    string bufferS = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                    DictionaryManager.NameDictUpdate(this.firstNameDictionary, bufferS, pos);

                    bufferS = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                    DictionaryManager.NameDictUpdate(this.lastNameDictionary, bufferS, pos);

                    DateTime bufferD = new (reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, bufferD, pos);
                }
            }
        }

        private void UpdateDictionary(DataStorage starage, long position)
        {
            DictionaryManager.NameDictUpdate(this.firstNameDictionary, starage.FirstName, position);

            DictionaryManager.NameDictUpdate(this.lastNameDictionary, starage.LastName, position);

            DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, starage.DateOfBirth, position);
        }
    }
}
