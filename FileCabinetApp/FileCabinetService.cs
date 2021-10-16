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
        /// <param name="firstName">set firstname.</param>
        /// <param name="lastName">set lastname.</param>
        /// <param name="dateOfBirth">set dateOfBirth.</param>
        /// <param name="type">set type.</param>
        /// <param name="number">set number.</param>
        /// <param name="balance">set balance.</param>
        /// <returns>Return id of record.</returns>
        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "LastName can't be null");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName.Length >= 2 && firstName.Length <= 60 ? firstName : throw new ArgumentException("incorrect FirstName"),
                LastName = lastName.Length >= 2 && lastName.Length <= 60 ? lastName : throw new ArgumentException("incorrect FirstName"),
                DateOfBirth = dateOfBirth.Year >= 1950 && dateOfBirth.Year <= DateTime.Today.Year ? dateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date"),
                PersonalAccountType = char.ToUpper(type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('C') ? type : throw new ArgumentException("Type can be A, B, C only"),
                PersonalAccountNumber = number > 0 && number <= 9999 ? number : throw new ArgumentException("Number should be more than 0 end less than 9999"),
                PersonalAccountBalance = balance > 0 ? balance : throw new ArgumentException("Balance can't be less than zero"),
            };

            this.list.Add(record);
            this.UpdateDictionaries(firstName, lastName, dateOfBirth, record);
            return record.Id;
        }

        /// <summary>
        /// Edit record in service by id.
        /// </summary>
        /// <param name="id"> id of record.</param>
        /// <param name="firstName">set firstname.</param>
        /// <param name="lastName">set lastname.</param>
        /// <param name="dateOfBirth">set dateOfBirth.</param>
        /// <param name="type">set type.</param>
        /// <param name="number">set number.</param>
        /// <param name="balance">set balance.</param>
        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            var record = this.list[id - 1];

            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "LastName can't be null");
            }

            this.firstNameDictionary.Remove(record.FirstName.ToUpperInvariant());

            record.FirstName = firstName?.Length >= 2 && firstName.Length <= 60 ? firstName : throw new ArgumentException("incorrect FirstName");
            record.LastName = lastName?.Length >= 2 && lastName.Length <= 60 ? lastName : throw new ArgumentException("incorrect FirstName");
            record.DateOfBirth = dateOfBirth.Year >= 1950 && dateOfBirth.Year <= DateTime.Today.Year ? dateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date");
            record.PersonalAccountType = char.ToUpper(type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('C') ? type : throw new ArgumentException("Type can be A, B, C only");
            record.PersonalAccountNumber = number > 0 && number <= 9999 ? number : throw new ArgumentException("Number should be more than 0 end less than 9999");
            record.PersonalAccountBalance = balance > 0 ? balance : throw new ArgumentException("Balance can't be less than zero");

            this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
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
