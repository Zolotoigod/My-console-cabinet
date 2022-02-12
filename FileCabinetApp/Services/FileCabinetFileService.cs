using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Validation.Service;

namespace FileCabinetApp
{
    public class FileCabinetFileService : IFileCabinetService, IDisposable, IEnumerable<FileCabinetRecord>
    {
        private readonly IRecordValidator validator;
        private readonly FileStream fileStreamDb;
        private readonly UTF8Encoding coder = new ();
        private readonly short reservedField = 7;
        private readonly Dictionary<string, List<long>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<long>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<long>> dateOfBirthDictionary = new ();
        private readonly Dictionary<int, long> recordIdDdictionary = new ();
        private int deletedRecords;

        public FileCabinetFileService(string validatorName)
        {
            this.validator = Defines.GetValidator(validatorName);
            bool existNow = false;
            if (File.Exists(Defines.DBPath))
            {
                existNow = true;
            }

            this.fileStreamDb = new FileStream(Defines.DBPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (existNow)
            {
                this.InitialDictionary();
                this.deletedRecords = this.GetDeletedCount();
            }
        }

        public int CreateRecord(InputDataPack dataPack)
        {
            this.validator.Validate(dataPack);
            var record = new FileCabinetRecord(dataPack, this.GetStat());
            this.UpdateDictionary(record, this.fileStreamDb.Length);
            DictionaryManager.IdDictUpdate(this.recordIdDdictionary, record.Id, this.fileStreamDb.Length);
            this.WriteRecordToFile(record, this.fileStreamDb.Length);
            return record.Id;
        }

        public string EditRecord(int id, InputDataPack dataPack)
        {
            if (!this.recordIdDdictionary.ContainsKey(id))
            {
                return $"Record #{id} not found\n";
            }

            long position = this.recordIdDdictionary[id];
            var oldRecord = this.ReadRecordFormFile(position);
            this.RemoveDictKey(oldRecord, position);
            this.recordIdDdictionary.Remove(id);

            this.validator.Validate(dataPack);
            var record = new FileCabinetRecord(dataPack, id - 1);
            this.UpdateDictionary(record, position);
            DictionaryManager.IdDictUpdate(this.recordIdDdictionary, record.Id, position);
            this.WriteRecordToFile(record, position + 6, false);

            return $"Record #{id} uppdated";
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            DateTime date = new ();
            if (DateTime.TryParse(dateOfBirth, out date) && this.dateOfBirthDictionary.ContainsKey(date))
            {
                foreach (int index in this.dateOfBirthDictionary[date])
                {
                    yield return this.ReadRecordFormFile(index);
                }
            }
            else
            {
                yield break;
            }
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                foreach (int index in this.firstNameDictionary[firstName.ToUpperInvariant()])
                {
                    yield return this.ReadRecordFormFile(index);
                }
            }
            else
            {
                yield break;
            }
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                foreach (int index in this.lastNameDictionary[lastName.ToUpperInvariant()])
                {
                    yield return this.ReadRecordFormFile(index);
                }
            }
            else
            {
                yield break;
            }
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            int startPosition = 0;
            for (int i = 1; i <= this.GetStat(); i++, startPosition += Defines.RecordSize)
            {
                yield return this.ReadRecordFormFile(startPosition);
            }
        }

        public int GetStat()
        {
            return (int)(this.fileStreamDb.Length / Defines.RecordSize);
        }

        public int GetDeletedRecords()
        {
            return this.deletedRecords;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(new List<FileCabinetRecord>(this.GetRecords()), Defines.AvailableFields);
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
            this.recordIdDdictionary.Clear();
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
                this.recordIdDdictionary.Add(record.Id, position);
                position += Defines.RecordSize;
            }

            this.RestoreDictionary(list);
        }

        public string Remove(int id)
        {
            if (!this.recordIdDdictionary.ContainsKey(id))
            {
                return $"Record #{id} doesn't exists\n";
            }

            long position = this.recordIdDdictionary[id];
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
                this.recordIdDdictionary.Remove(id);
                return $"Record #{id} is deleted\n";
            }
            else
            {
                return $"Record #{id} doesn't exists\n";
            }
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
                    posWrite += Defines.RecordSize;
                }
                else
                {
                    purgedRecords++;
                }

                posRead += Defines.RecordSize;
                recordCount++;
            }
            while (recordCount < nowRecordCount);

            this.fileStreamDb.SetLength(this.fileStreamDb.Length - (Defines.RecordSize * purgedRecords));
            this.deletedRecords -= purgedRecords;

            return $"Data file processing is completed: {purgedRecords} of {nowRecordCount} records were purged.\n";
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            int startPosition = 0;
            for (int i = 1; i <= this.GetStat(); i++, startPosition += Defines.RecordSize)
            {
                yield return this.ReadRecordFormFile(startPosition);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

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
            for (long pos = 0; pos < this.fileStreamDb.Length; pos += Defines.RecordSize)
            {
                this.fileStreamDb.Position = pos + 2;
                using (BinaryReader reader = new (this.fileStreamDb, Encoding.UTF8, true))
                {
                    int bufferId = reader.ReadInt32();
                    this.recordIdDdictionary.Add(bufferId, pos);

                    string bufferS = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                    DictionaryManager.NameDictUpdate(this.firstNameDictionary, bufferS, pos);

                    bufferS = string.Concat(this.coder.GetChars(reader.ReadBytes(120))).Trim();
                    DictionaryManager.NameDictUpdate(this.lastNameDictionary, bufferS, pos);

                    DateTime bufferD = new (reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
                    DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, bufferD, pos);
                }
            }
        }

        private void UpdateDictionary(FileCabinetRecord record, long position)
        {
            DictionaryManager.NameDictUpdate(this.firstNameDictionary, record.FirstName, position);
            DictionaryManager.NameDictUpdate(this.lastNameDictionary, record.LastName, position);
            DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, record.DateOfBirth, position);
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
                position += Defines.RecordSize;
            }
        }

        private void RemoveDictKey(FileCabinetRecord record, long posittion)
        {
            this.firstNameDictionary[record?.FirstName.ToUpperInvariant()].Remove(posittion);
            this.lastNameDictionary[record?.LastName.ToUpperInvariant()].Remove(posittion);
            this.dateOfBirthDictionary[record.DateOfBirth].Remove(posittion);
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

                pos += Defines.RecordSize;
                this.fileStreamDb.Position = pos;
                i++;
            }

            return deletedCount;
        }
    }
}
