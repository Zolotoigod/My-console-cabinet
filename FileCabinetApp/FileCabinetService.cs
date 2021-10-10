using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

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
            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            var record = this.list[id - 1];
            record.FirstName = firstName?.Length >= 2 && firstName.Length <= 60 ? firstName : throw new ArgumentException("incorrect FirstName");
            record.LastName = lastName?.Length >= 2 && lastName.Length <= 60 ? lastName : throw new ArgumentException("incorrect FirstName");
            record.DateOfBirth = dateOfBirth.Year >= 1950 && dateOfBirth.Year <= DateTime.Today.Year ? dateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date");
            record.PersonalAccountType = char.ToUpper(type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('C') ? type : throw new ArgumentException("Type can be A, B, C only");
            record.PersonalAccountNumber = number > 0 && number <= 9999 ? number : throw new ArgumentException("Number should be more than 0 end less than 9999");
            record.PersonalAccountBalance = balance > 0 ? balance : throw new ArgumentException("Balance can't be less than zero");
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
