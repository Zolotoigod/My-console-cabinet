using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Validation.Service;

#pragma warning disable CA1805

namespace FileCabinetApp
{
    /// <summary>
    /// Service.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService, IEnumerable<FileCabinetRecord>
    {
        /// <summary>
        /// The field sets the available dateformat.
        /// </summary>
        private readonly Dictionary<string, List<int>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<int>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<int>> dateOfBirthDictionary = new ();
        private readonly Dictionary<int, FileCabinetRecord> recordDictionary = new ();
        private readonly IRecordValidator validator;

        public FileCabinetMemoryService(int validatorIndex)
        {
            this.validator = Defines.GetValidator(validatorIndex);
        }

        public int CreateRecord(InputDataPack dataPack)
        {
            this.validator.Validate(dataPack);
            var record = new FileCabinetRecord(dataPack, this.recordDictionary.Count);
            this.recordDictionary.Add(record.Id, record);
            this.UpdateDictionarey(record);
            return record.Id;
        }

        public string EditRecord(int id, InputDataPack dataPack)
        {
            if (!this.recordDictionary.ContainsKey(id))
            {
                return $"Record #{id} not found\n";
            }

            var record = this.recordDictionary[id];
            this.validator.Validate(dataPack);
            this.RemoveDictKey(record);
            record.FirstName = dataPack?.FirstName;
            record.LastName = dataPack.LastName;
            record.DateOfBirth = dataPack.DateOfBirth;
            record.Type = dataPack.Type;
            record.Number = dataPack.Number;
            record.Balance = dataPack.Balance;

            this.UpdateDictionarey(record);

            return $"Record #{id} uppdated";
        }

        /// <summary>
        /// Return all record in servise.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var record in this.recordDictionary)
            {
                yield return record.Value;
            }
        }

        /// <summary>
        /// Metod returned count of records.
        /// </summary>
        /// <returns>int count.</returns>
        public int GetStat()
        {
            return this.recordDictionary.Count;
        }

        public int GetDeletedRecords()
        {
            return 0;
        }

        /// <summary>
        /// Serch the record in service by firstname, use dictionary.
        /// </summary>
        /// <param name="firstName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                var collection = this.firstNameDictionary[firstName.ToUpperInvariant()];
                foreach (int recordIndex in collection)
                {
                    yield return this.recordDictionary[recordIndex];
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Serch the record in service by Lastname, use dictionary.
        /// </summary>
        /// <param name="lastName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                var collection = this.lastNameDictionary[lastName.ToUpperInvariant()];
                foreach (int recordIndex in collection)
                {
                    yield return this.recordDictionary[recordIndex];
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Serch the record in service by DateOfBirth, use dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (!DateTime.TryParseExact(dateOfBirth, Defines.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate))
            {
                yield break;
            }

            if (this.dateOfBirthDictionary.ContainsKey(resultDate))
            {
                var collection = this.dateOfBirthDictionary[resultDate];
                foreach (int recordIndex in collection)
                {
                    yield return this.recordDictionary[recordIndex];
                }
            }
            else
            {
                yield break;
            }
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            List<FileCabinetRecord> buffer = new List<FileCabinetRecord>();
            foreach (var record in this.recordDictionary)
            {
                buffer.Add(record.Value);
            }

            return new FileCabinetServiceSnapshot(buffer, Defines.AvailableFields);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            for (int i = 0; i < snapshot?.Records.Count; i++)
            {
                this.recordDictionary.Remove(snapshot.Records[i].Id);
            }

            foreach (var record in snapshot.Records)
            {
                this.recordDictionary.Add(record.Id, record);
            }

            this.RestoreDictionary(this.recordDictionary);
        }

        public string Remove(int id)
        {
            if (this.recordDictionary.ContainsKey(id))
            {
                this.RemoveDictKey(this.recordDictionary[id]);
                this.recordDictionary.Remove(id);
                return $"Record #{id} is removed.";
            }

            return $"Record #{id} doesn't exists.";
        }

        public string Purge()
        {
            return "There is no FileService!";
        }

        public override string ToString()
        {
            return "Memory";
        }

        public IEnumerator<FileCabinetRecord> GetEnumerator()
        {
            foreach (var record in this.recordDictionary)
            {
                yield return record.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        private void RestoreDictionary(Dictionary<int, FileCabinetRecord> list)
        {
            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
            this.dateOfBirthDictionary.Clear();

            foreach (var record in list)
            {
                DictionaryManager.NameDictUpdate(this.firstNameDictionary, record.Value.FirstName, record.Value.Id);
                DictionaryManager.NameDictUpdate(this.lastNameDictionary, record.Value.LastName, record.Value.Id);
                DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, record.Value.DateOfBirth, record.Value.Id);
            }
        }

        private void UpdateDictionarey(FileCabinetRecord record)
        {
            DictionaryManager.NameDictUpdate(this.firstNameDictionary, record.FirstName, record.Id);
            DictionaryManager.NameDictUpdate(this.lastNameDictionary, record.LastName, record.Id);
            DictionaryManager.DateDictUpdate(this.dateOfBirthDictionary, record.DateOfBirth, record.Id);
        }

        private void RemoveDictKey(FileCabinetRecord record)
        {
            this.firstNameDictionary.Remove(record.FirstName.ToUpperInvariant());
            this.lastNameDictionary.Remove(record.LastName.ToUpperInvariant());
            this.dateOfBirthDictionary.Remove(record.DateOfBirth);
        }
    }
}
