using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly string fileTitle;

        public FileCabinetServiceSnapshot(StreamReader reader, BaseValidationRules validationRules)
        {
            var list = this.LoadFromCsv(reader, validationRules);
            this.Records = list.AsReadOnly();
        }

        public FileCabinetServiceSnapshot(XmlReader reader, BaseValidationRules validationRules)
        {
            var list = this.LoadFromXml(reader, validationRules);
            this.Records = list.AsReadOnly();
        }

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list, string title)
        {
            this.Records = list?.AsReadOnly();
            this.fileTitle = title;
        }

        public ReadOnlyCollection<FileCabinetRecord> Records { get; }

        public void SaveToCSV(TextWriter writer)
        {
            FileCabinetRecordCsvWriter newWriter = new (writer);
            newWriter.WriteRootStart(this.fileTitle);
            foreach (var record in this.Records)
            {
                newWriter.Write(record);
            }
        }

        public void SaveToXML(TextWriter writer)
        {
            FileCabinetRecordXmlWriter newWriter = new (writer);
            newWriter.WriteRootStart("records");
            foreach (var record in this.Records)
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

        public List<FileCabinetRecord> LoadFromXml(XmlReader reader, BaseValidationRules validationRules)
        {
            FileCabinetRecordXmlReader newReader = new ();
            return newReader.XmlDeSerialize(reader, validationRules);
        }
    }
}
