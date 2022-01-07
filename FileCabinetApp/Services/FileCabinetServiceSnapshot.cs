using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly string fileTitle;

        public FileCabinetServiceSnapshot(StreamReader reader, DataValidator validator)
        {
            var list = this.LoadFromCsv(reader, validator);
            this.Records = list.AsReadOnly();
        }

        public FileCabinetServiceSnapshot(XmlReader reader, DataValidator validator)
        {
            var list = this.LoadFromXml(reader, validator);
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
            newWriter.Write(this.Records);
        }

        public void SaveToXML(TextWriter writer)
        {
            FileCabinetRecordXmlWriter newWriter = new (writer);
            newWriter.Write(this.Records);
        }

        public List<FileCabinetRecord> LoadFromCsv(StreamReader reader, DataValidator validator)
        {
            FileCabinetRecordCsvReader newReader = new (reader);
            return newReader.ReadAll(validator);
        }

        public List<FileCabinetRecord> LoadFromXml(XmlReader reader, DataValidator validator)
        {
            FileCabinetRecordXmlReader newReader = new ();
            return newReader.XmlDeSerialize(reader, validator);
        }
    }
}
