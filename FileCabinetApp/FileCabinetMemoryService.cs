using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Service.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        /// <summary>
        /// The field sets the available dateformat.
        /// </summary>
        public static readonly string[] DateFormat = { "MM dd yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM,dd,yyyy", "dd MM yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "dd,MM,yyyy" };
        private const string AvailableFields = "ID, F.tName, L.Name, D.OfBirth, Type, Number, Balance";
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private readonly BaseValidationRules validationRules;
        private List<FileCabinetRecord> list = new ();

        public FileCabinetMemoryService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
        }

        public int CreateRecord(DataStorage storage)
        {
            BaseValidationRules.ValidationNull(storage);
            var record = new FileCabinetRecord(storage, this.validationRules, this.list.Count);
            this.list.Add(record);
            this.UpdateDictionaries(storage.FirstName, storage.LastName, storage.DateOfBirth, record);
            return record.Id;
        }

        public void EditRecord(int id, DataStorage storage)
        {
            var record = this.list[id - 1];
            BaseValidationRules.ValidationNull(storage);
            this.firstNameDictionary.Remove(record.FirstName.ToUpperInvariant());
            this.lastNameDictionary.Remove(record.LastName.ToUpperInvariant());
            this.dateOfBirthDictionary.Remove(record.DateOfBirth);

            record.FirstName = this.validationRules.NameValidationRules(storage.FirstName) ? storage?.FirstName : throw new ArgumentException("incorrect FirstName");
            record.LastName = this.validationRules.NameValidationRules(storage.LastName) ? storage.LastName : throw new ArgumentException("incorrect FirstName");
            record.DateOfBirth = this.validationRules.DateValidationRules(storage.DateOfBirth) ? storage.DateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date");
            record.Type = this.validationRules.TypeValidationRules(storage.Type) ? storage.Type : throw new ArgumentException("Type can be A, B, C only");
            record.Number = this.validationRules.NumberValidationRules(storage.Number) ? storage.Number : throw new ArgumentException("Number should be more than 0 end less than 9999");
            record.Balance = this.validationRules.BalanceValidationRules(storage.Balance) ? storage.Balance : throw new ArgumentException("Balance can't be less than zero");

            this.UpdateDictionaries(storage.FirstName, storage.LastName, storage.DateOfBirth, record);
        }

        /// <summary>
        /// Return all record in servise.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list.AsReadOnly();
        }

        /// <summary>
        /// Metod returned count of records.
        /// </summary>
        /// <returns>int count.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Serch the record in service by firstname, use dictionary.
        /// </summary>
        /// <param name="firstName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                return this.firstNameDictionary[firstName.ToUpperInvariant()].AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        /// <summary>
        /// Serch the record in service by Lastname, use dictionary.
        /// </summary>
        /// <param name="lastName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                return this.lastNameDictionary[lastName.ToUpperInvariant()].AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        /// <summary>
        /// Serch the record in service by DateOfBirth, use dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (!DateTime.TryParseExact(dateOfBirth, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate))
            {
                return null;
            }

            if (this.dateOfBirthDictionary.ContainsKey(resultDate))
            {
                return this.dateOfBirthDictionary[resultDate].AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return new FileCabinetServiceSnapshot(this.list, AvailableFields);
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            for (int i = 0; i < snapshot?.Records.Count; i++)
            {
                this.list.RemoveAll(match => match.Id == snapshot.Records[i].Id);
            }

            foreach (var record in snapshot.Records)
            {
                this.list.Add(record);
            }

            this.RestoreDictionary(this.list);
        }

        public override string ToString()
        {
            return "Memory";
        }

        /// <summary>
        /// Update dictionaries.
        /// </summary>
        /// <param name="firstName">set firstName.</param>
        /// <param name="lastName">set lastName.</param>
        /// <param name="dateOfBirth">set dateOfBirth.</param>
        /// <param name="record">record for udate.</param>
        private void UpdateDictionaries(string firstName, string lastName, DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                this.lastNameDictionary[lastName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(lastName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord> { record });
            }
        }

        private void RestoreDictionary(List<FileCabinetRecord> list)
        {
            this.firstNameDictionary.Clear();
            this.lastNameDictionary.Clear();
            this.dateOfBirthDictionary.Clear();

            foreach (var record in list)
            {
                this.UpdateDictionaries(record.FirstName, record.LastName, record.DateOfBirth, record);
            }
        }
    }
}
