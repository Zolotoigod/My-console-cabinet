using System;
using System.Collections.Generic;
using System.IO;
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

        public List<FileCabinetRecord> XmlDeSerialize(XmlReader reader, BaseValidationRules validationRules)
        {
            if (this.serializer.Deserialize(reader) is List<FileCabinetRecord> records)
            {
                foreach (var record in records)
                {
                    this.ValidateRecord(record, validationRules);
                }

                return records;
            }

            return null;
        }

        private FileCabinetRecord ValidateRecord(FileCabinetRecord record, BaseValidationRules validationRules)
        {
            if (!validationRules.NameValidationRules(record.FirstName))
            {
                record.FirstName = "#DataError#";
            }

            if (!validationRules.NameValidationRules(record.LastName))
            {
                record.LastName = "#DataError#";
            }

            if (!validationRules.DateValidationRules(record.DateOfBirth))
            {
                record.DateOfBirth = default(DateTime);
            }

            if (!validationRules.TypeValidationRules(record.Type))
            {
                record.Type = default(char);
            }

            if (!validationRules.NumberValidationRules(record.Number))
            {
                record.Number = -1;
            }

            if (!validationRules.BalanceValidationRules(record.Balance))
            {
                record.Balance = default(decimal);
            }

            return record;
        }
    }
}
