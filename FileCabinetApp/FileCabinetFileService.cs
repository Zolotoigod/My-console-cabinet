using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetFileService : IFileCabinetService, IDisposable
    {
        private const string AvailableFields = "ID, F.tName, L.Name, D.OfBirth, Type, Number, Balance";
        private readonly BaseValidationRules validationRules;
        private readonly FileStream fileStreamDb;
        private readonly int recordSize = sizeof(short) + sizeof(int) + 120 + 120 + (3 * sizeof(int)) + 2 + sizeof(short) + sizeof(decimal);
        private readonly UTF8Encoding coder = new ();
        private readonly short reservedField = 7;
        private readonly Dictionary<string, List<long>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new ();
        private readonly string filename = "cabinet-records.db";
        private readonly List<int> listId = new ();
        private int deletedRecords;

        public FileCabinetFileService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
            bool existNow = false;
            if (File.Exists(this.filename))
            {
                existNow = true;
            }

            this.fileStreamDb = new FileStream(this.filename, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (existNow)
            {
                this.InitialDictionary();
                this.listId = this.InitialListID();
                this.deletedRecords = this.GetDeletedCount();
            }
        }

        public int CreateRecord(DataStorage storage)
        {
            BaseValidationRules.ValidationNull(storage);
            var record = new FileCabinetRecord(storage, this.validationRules, this.GetStat());
            this.UpdateDictionary(storage, this.fileStreamDb.Length);
            this.WriteRecordToFile(record, this.fileStreamDb.Length);
            this.listId.Add(record.Id);
            return record.Id;
        }

        public void EditRecord(int id, DataStorage storage)
        {
            long position = (this.recordSize * (id - 1)) + 6;
            var record = new FileCabinetRecord(storage, this.validationRules, this.GetStat());
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
            for (int i = 1; i <= this.GetStat(); i++)
            {
                records.Add(this.ReadRecordFormFile(startPosition));
                startPosition += this.recordSize;
            }

            return records.AsReadOnly();
        }

        public int GetStat()
        {
            return (int)(this.fileStreamDb.Length / this.recordSize);
        }

        public int GetDeletedRecords()
        {
            return this.deletedRecords;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToList(), AvailableFields);
        }

        public override string ToString()
        {
            return "Filesystem";
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            List<FileCabinetRecord> list = this.GetRecords().ToList();
            this.listId.Clear();
            for (int i = 0; i < snapshot?.Records.Count; i++)
            {
                list.RemoveAll(match => match.Id == snapshot.Records[i].Id);
            }

            foreach (var record in snapshot.Records)
            {
                list.Add(record);
            }

            this.fileStreamDb.SetLength(0);

            long position = 0;
            foreach (var record in list)
            {
                this.WriteRecordToFile(record, position);
                this.listId.Add(record.Id);
                position += this.recordSize;
            }

            this.RestoreDictionary(list);
        }

        public string Remove(int id)
        {
            if (!this.GetListId().Contains(id))
            {
                return $"Record #{id} doesn't exists\n";
            }

            long position = this.recordSize * (id - 1);
            this.fileStreamDb.Position = position;
            short isDeleted;
            using (BinaryReader reder = new (this.fileStreamDb, Encoding.UTF8, true))
            {
                isDeleted = reder.ReadInt16();
            }

            this.fileStreamDb.Position = position;
            if (isDeleted == 7)
            {
                isDeleted = 1;
                using (BinaryWriter writer = new (this.fileStreamDb, Encoding.UTF8, true))
                {
                    writer.Write(isDeleted);
                }

                this.deletedRecords++;
                this.listId.Remove(id);
                return $"Record #{id} is deleted\n";
            }
            else
            {
                return $"Record #{id} doesn't exists\n";
            }
        }

        public List<int> GetListId()
        {
            return this.listId;
        }

        public string Purge()
        {
            int purgedRecords = 0;
            long posRead = 0;
            long posWrite = posRead;
            int recordCount = 0;
            int nowRecordCount = this.GetStat();

            do
            {
                var record = this.ReadRecordFormFile(posRead, true);
                if (record.IsDeleted == 7)
                {
                    this.WriteRecordToFile(record, posWrite);
                    posWrite += this.recordSize;
                }
                else
                {
                    purgedRecords++;
                }

                posRead += this.recordSize;
                recordCount++;
            }
            while (recordCount < nowRecordCount);

            this.fileStreamDb.SetLength(this.fileStreamDb.Length - (this.recordSize * purgedRecords));
            this.deletedRecords -= purgedRecords;

            return $"Data file processing is completed: {purgedRecords} of {nowRecordCount} records were purged.\n";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.fileStreamDb.Dispose();
            }
        }

        private static char[] GetFixLenthString(string value, int count)
        {
            char[] result = Enumerable.Repeat(' ', count).ToArray();
            char[] name = value.ToCharArray();
            name.CopyTo(result, 0);
            return result;
        }

        private FileCabinetRecord ReadRecordFormFile(long position, bool readDeleted = false)
        {
            this.fileStreamDb.Position = position;
            FileCabinetRecord readedRecord = new ();
            using (BinaryReader reader = new (this.fileStreamDb, Encoding.UTF8, true))
            {
                short isDeleted = reader.ReadInt16();
                if ((isDeleted == 7) || readDeleted)
                {
                    readedRecord.IsDeleted = isDeleted;
                    readedRecord.Id = reader.ReadInt32();
                    readedRecord.FirstName = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                    readedRecord.LastName = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                    readedRecord.DateOfBirth = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    readedRecord.Type = this.coder.GetChars(reader.ReadBytes(2))[0];
                    readedRecord.Number = reader.ReadInt16();
                    readedRecord.Balance = reader.ReadDecimal();
                }
                else
                {
                    return null;
                }
            }

            return readedRecord;
        }

        private void WriteRecordToFile(FileCabinetRecord record, long position, bool createRecord = true)
        {
            this.fileStreamDb.Position = position;
            using (BinaryWriter writer = new (this.fileStreamDb, Encoding.UTF8, true))
            {
                if (createRecord)
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
            for (long pos = 0; pos < this.fileStreamDb.Length; pos += this.recordSize)
            {
                this.fileStreamDb.Position = pos + 6;
                using (BinaryReader reader = new (this.fileStreamDb, Encoding.UTF8, true))
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

        private void RestoreDictionary(List<FileCabinetRecord> list)
        {
            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
            this.dateOfBirthDictionary.Clear();

            long position = 0;
            foreach (var record in list)
            {
                DictionaryManager.NameDictUpdate(this.firstNameDictionary, record.FirstName, position);
                DictionaryManager.NameDictUpdate(this.lastNameDictionary, record.LastName, position);
                DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, record.DateOfBirth, position);
                position += this.recordSize;
            }
        }

        private List<int> InitialListID()
        {
            List<int> listId = new ();
            long currentIdPos = 0;
            for (int i = 0; i < this.GetStat(); i++)
            {
                this.fileStreamDb.Position = currentIdPos;
                using (BinaryReader reader = new (this.fileStreamDb, Encoding.UTF8, true))
                {
                    if (reader.ReadInt16() == 7)
                    {
                        listId.Add(reader.ReadInt32());
                    }
                }

                currentIdPos += this.recordSize;
            }

            return listId;
        }

        private int GetDeletedCount()
        {
            int deletedCount = 0;
            int i = 0;
            long pos = 0;
            this.fileStreamDb.Position = pos;
            while (i < this.GetStat())
            {
                using (BinaryReader reader = new (this.fileStreamDb, Encoding.UTF8, true))
                {
                    if (reader.ReadInt16() == 1)
                    {
                        deletedCount++;
                    }
                }

                pos += this.recordSize;
                this.fileStreamDb.Position = pos;
                i++;
            }

            return deletedCount;
        }
    }
}
