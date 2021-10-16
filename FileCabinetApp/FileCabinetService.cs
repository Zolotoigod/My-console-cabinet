using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        internal static readonly string[] DateFormat = { "MM dd yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM,dd,yyyy", "dd MM yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "dd,MM,yyyy" };
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

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

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }

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
