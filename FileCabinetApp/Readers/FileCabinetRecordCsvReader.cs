using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        public FileCabinetRecordCsvReader(StreamReader streamReader)
        {
            this.reader = streamReader;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Проверить аргументы или открытые методы", Justification = "<Ожидание>")]
        public List<FileCabinetRecord> ReadAll(BaseValidationRules validationRules)
        {
            List<FileCabinetRecord> result = new ();
            string[] buffer;
            if (!this.reader.EndOfStream)
            {
                this.reader.ReadLine();
            }

            while (!this.reader.EndOfStream)
            {
                FileCabinetRecord newRecord = new FileCabinetRecord();
                buffer = this.reader.ReadLine().Split(", ");
                int newId;
                if (int.TryParse(buffer[0], out newId))
                {
                    newRecord.Id = newId;
                }
                else
                {
                    newRecord.Id = -1;
                }

                newRecord.FirstName = validationRules.NameValidationRules(buffer[1]) ? buffer[1] : "#Incorrect data#";
                newRecord.LastName = validationRules.NameValidationRules(buffer[2]) ? buffer[2] : "#Incorrect data#";

                DateTime date;
                newRecord.DateOfBirth = DateTime.TryParse(buffer[3], out date) && validationRules.DateValidationRules(date) ? date : default(DateTime);

                newRecord.Type = validationRules.TypeValidationRules(buffer[4][0]) ? buffer[4][0] : char.MinValue;

                short number;
                newRecord.Number = short.TryParse(buffer[5], out number) && validationRules.NumberValidationRules(number) ? number : (short)0;

                decimal balance;
                newRecord.Balance = decimal.TryParse(buffer[6], out balance) && validationRules.BalanceValidationRules(balance) ? balance : decimal.Zero;
                result.Add(newRecord);
            }

            return result;
        }
    }
}
