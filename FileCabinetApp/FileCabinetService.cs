using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        public int CreateRecord(string firstname, string lastname, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstname,
                LastName = lastname,
                DateOfBirth = dateOfBirth,
                PersonalAccountType = type,
                PersonalAccountNumber = number,
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
