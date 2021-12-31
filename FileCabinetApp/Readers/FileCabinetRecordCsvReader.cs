using System;
using System.Collections.Generic;
using System.IO;

#pragma warning disable CA1062

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvReader
    {
        private readonly StreamReader reader;

        public FileCabinetRecordCsvReader(StreamReader streamReader)
        {
            this.reader = streamReader;
        }

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
                var validator = new DataValidator(validationRules);
                buffer = this.reader.ReadLine().Split(", ");

                newRecord.Id = int.TryParse(buffer[0], out int newId) ? newId : -1;

                newRecord.FirstName = validator.NameValidator(buffer[1]).Item1 ?
                    DataConverter.NameConvert(buffer[1]).Item3 : "#Incorrect data#";

                newRecord.LastName = validator.NameValidator(buffer[2]).Item1 ?
                    DataConverter.NameConvert(buffer[2]).Item3 : "#Incorrect data#";

                newRecord.DateOfBirth = validator.DateValidator(DataConverter.DateConvert(buffer[3]).Item3).Item1 ?
                    DataConverter.DateConvert(buffer[3]).Item3 : default(DateTime);

                newRecord.Type = validator.TypeValidator(DataConverter.TypeConvert(buffer[4]).Item3).Item1 ?
                    DataConverter.TypeConvert(buffer[4]).Item3 : char.MinValue;

                newRecord.Number = validator.NumberValidator(DataConverter.NumberConvert(buffer[5]).Item3).Item1 ?
                    DataConverter.NumberConvert(buffer[5]).Item3 : (short)0;

                newRecord.Balance = validator.BalanceValidator(DataConverter.BalanceConvert(buffer[6]).Item3).Item1 ?
                    DataConverter.BalanceConvert(buffer[6]).Item3 : decimal.Zero;

                result.Add(newRecord);
            }

            return result;
        }
    }
}
