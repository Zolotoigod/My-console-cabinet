using System;
using System.Collections.Generic;
using System.Globalization;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        public int CreateRecord(string firstname, string lastname, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            if (string.IsNullOrWhiteSpace(firstname))
            {
                throw new ArgumentNullException(nameof(firstname), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(lastname))
            {
                throw new ArgumentNullException(nameof(lastname), "LastName can't be null");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = (firstname.Length >= 2 && firstname.Length <= 60) ? firstname : throw new ArgumentException("incorrect FirstName"),
                LastName = (lastname.Length >= 2 && lastname.Length <= 60) ? lastname : throw new ArgumentException("incorrect FirstName"),
                DateOfBirth = (dateOfBirth.Year >= 1950 && dateOfBirth.Year <= DateTime.Today.Year) ? dateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date"),
                PersonalAccountType = (char.ToUpper(type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(type, CultureInfo.InvariantCulture).Equals('C')) ? type : throw new ArgumentException("Type can be A, B, C only"),
                PersonalAccountNumber = (number > 0 && number <= 9999) ? number : throw new ArgumentException("Number should be more than 0 end less than 9999"),
                PersonalAccountBalance = balance,
            };

            this.list.Add(record);
            return record.Id;
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
