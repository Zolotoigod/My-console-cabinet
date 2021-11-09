using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;
        private readonly string fileTitle;

        public FileCabinetServiceSnapshot(StreamReader reader, BaseValidationRules validationRules)
        {
            var list = this.LoadFromCsv(reader, validationRules);
            this.records = list?.ToArray();
            this.Records = list.AsReadOnly();
        }

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list, string title)
        {
            this.records = list?.ToArray();
            this.Records = list.AsReadOnly();
            this.fileTitle = title;
        }

        public ReadOnlyCollection<FileCabinetRecord> Records { get; }

        public void SaveToCSV(TextWriter writer)
        {
            FileCabinetRecordCsvWriter newWriter = new (writer);
            newWriter.WriteRootStart(this.fileTitle);
            foreach (var record in this.records)
            {
                newWriter.Write(record);
            }
        }

        public void SaveToXML(TextWriter writer)
        {
            FileCabinetRecordXmlWriter newWriter = new (writer);
            newWriter.WriteRootStart("records");
            foreach (var record in this.records)
            {
                newWriter.Write(record);
            }

            newWriter.WriteRootEnd();
        }

        public List<FileCabinetRecord> LoadFromCsv(StreamReader reader, BaseValidationRules validationRules)
        {
            FileCabinetRecordCsvReader newReader = new (reader);
            return newReader.ReadAll(validationRules);
        }

        public void LoadFromXml(StreamReader reader, BaseValidationRules validationRules)
        {
            FileCabinetRecordCsvReader newReader = new (reader);
            newReader.ReadAll(validationRules);
        }
    }
}
