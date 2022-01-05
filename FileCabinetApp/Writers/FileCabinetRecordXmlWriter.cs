using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlWriter : IFileCabinetRecordWriter
    {
        private const string XMLRoot = "records";
        private XmlWriter writer;

        public FileCabinetRecordXmlWriter(TextWriter newText)
        {
            this.writer = XmlWriter.Create(newText);
            this.writer.WriteStartDocument();
        }

        public void Write(ReadOnlyCollection<FileCabinetRecord> records)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<FileCabinetRecord>));
            serializer.Serialize(this.writer, new List<FileCabinetRecord>(records));
        }

        public void WriteRootStart(string root)
        {
            this.writer.WriteStartElement(root);
        }

        public void WriteRootEnd()
        {
            this.writer.WriteEndElement();
            this.writer.Close();
        }

        public string GetRoot()
        {
            return XMLRoot;
        }
    }
}
