using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Service.
    /// </summary>
    public class FileCabinetService
    {
        /// <summary>
        /// Field set the dateformat.
        /// </summary>
        internal static readonly string[] DateFormat = { "MM dd yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM,dd,yyyy", "dd MM yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "dd,MM,yyyy" };
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        /// <summary>
        /// Create record in service.
        /// </summary>
        /// <param name="store">Record push in service.</param>
        /// <returns>Record ID.</returns>
        public int CreateRecord(FileCabinetRecord store)
        {
            if (store is null)
            {
                throw new ArgumentNullException(nameof(store.FirstName), "Record is null");
            }

            if (string.IsNullOrWhiteSpace(store.FirstName))
            {
                throw new ArgumentNullException(nameof(store.FirstName), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(store.LastName))
            {
                throw new ArgumentNullException(nameof(store.LastName), "LastName can't be null");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = store.FirstName.Length >= 2 && store.FirstName.Length <= 60 ? store.FirstName : throw new ArgumentException("incorrect FirstName"),
                LastName = store.LastName.Length >= 2 && store.LastName.Length <= 60 ? store.LastName : throw new ArgumentException("incorrect FirstName"),
                DateOfBirth = store.DateOfBirth.Year >= 1950 && store.DateOfBirth.Year <= DateTime.Today.Year ? store.DateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date"),
                Type = char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('C') ? store.Type : throw new ArgumentException("Type can be A, B, C only"),
                Number = store.Number > 0 && store.Number <= 9999 ? store.Number : throw new ArgumentException("Number should be more than 0 end less than 9999"),
                Balance = store.Balance > 0 ? store.Balance : throw new ArgumentException("Balance can't be less than zero"),
            };

            this.list.Add(record);
            this.UpdateDictionaries(store.FirstName, store.LastName, store.DateOfBirth, record);
            return record.Id;
        }

        /// <summary>
        /// Edit record in service by id.
        /// </summary>
        /// <param name="id"> id of record.</param>
        /// <param name="store">New record data.</param>
        public void EditRecord(int id, FileCabinetRecord store)
        {
            var record = this.list[id - 1];

            if (store is null)
            {
                throw new ArgumentNullException(nameof(store.FirstName), "New record is null");
            }

            if (string.IsNullOrWhiteSpace(store.FirstName))
            {
                throw new ArgumentNullException(nameof(store.FirstName), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(store.LastName))
            {
                throw new ArgumentNullException(nameof(store.LastName), "LastName can't be null");
            }

            this.firstNameDictionary.Remove(record.FirstName.ToUpperInvariant());
            this.lastNameDictionary.Remove(record.LastName.ToUpperInvariant());
            this.dateOfBirthDictionary.Remove(record.DateOfBirth);

            record.FirstName = store.FirstName.Length >= 2 && store.FirstName.Length <= 60 ? store.FirstName : throw new ArgumentException("incorrect FirstName");
            record.LastName = store.LastName.Length >= 2 && store.LastName.Length <= 60 ? store.LastName : throw new ArgumentException("incorrect FirstName");
            record.DateOfBirth = store.DateOfBirth.Year >= 1950 && store.DateOfBirth.Year <= DateTime.Today.Year ? store.DateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date");
            record.Type = char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('C') ? store.Type : throw new ArgumentException("Type can be A, B, C only");
            record.Number = store.Number > 0 && store.Number <= 9999 ? store.Number : throw new ArgumentException("Number should be more than 0 end less than 9999");
            record.Balance = store.Balance > 0 ? store.Balance : throw new ArgumentException("Balance can't be less than zero");

            this.UpdateDictionaries(store.FirstName, store.LastName, store.DateOfBirth, record);
        }

        /// <summary>
        /// Return all record in servise.
        /// </summary>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
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
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                return this.firstNameDictionary[firstName.ToUpperInvariant()].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Serch the record in service by Lastname, use dictionary.
        /// </summary>
        /// <param name="lastName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                return this.lastNameDictionary[lastName.ToUpperInvariant()].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
        }

        /// <summary>
        /// Serch the record in service by DateOfBirth, use dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(string dateOfBirth)
        {
            if (!DateTime.TryParseExact(dateOfBirth, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate))
            {
                return null;
            }

            if (this.dateOfBirthDictionary.ContainsKey(resultDate))
            {
                return this.dateOfBirthDictionary[resultDate].ToArray();
            }
            else
            {
                return Array.Empty<FileCabinetRecord>();
            }
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
    }
}
