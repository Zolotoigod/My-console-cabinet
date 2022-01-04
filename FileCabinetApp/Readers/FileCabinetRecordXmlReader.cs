using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

#pragma warning disable CA1062

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlReader
    {
        private readonly XmlSerializer serializer;

        public FileCabinetRecordXmlReader()
        {
            this.serializer = new XmlSerializer(typeof(List<FileCabinetRecord>));
        }

        public List<FileCabinetRecord> XmlDeSerialize(XmlReader reader, DataValidator validator)
        {
            if (this.serializer.Deserialize(reader) is List<FileCabinetRecord> records)
            {
                foreach (var record in records)
                {
                    this.ValidateRecord(record, validator);
                }

                return records;
            }

            return null;
        }

        private FileCabinetRecord ValidateRecord(FileCabinetRecord record, DataValidator validator)
        {
            if (!validator.FirstNameValidator(record.FirstName).Item1)
            {
                record.FirstName = "#DataError#";
            }

            if (!validator.LastNameValidator(record.LastName).Item1)
            {
                record.LastName = "#DataError#";
            }

            if (!validator.DateValidator(record.DateOfBirth).Item1)
            {
                record.DateOfBirth = default(DateTime);
            }

            if (!validator.TypeValidator(record.Type).Item1)
            {
                record.Type = default(char);
            }

            if (!validator.NumberValidator(record.Number).Item1)
            {
                record.Number = -1;
            }

            if (!validator.BalanceValidator(record.Balance).Item1)
            {
                record.Balance = default(decimal);
            }

            return record;
        }
    }
}
