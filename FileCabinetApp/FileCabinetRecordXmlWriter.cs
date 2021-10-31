using System.Globalization;
using System.IO;
using System.Xml;

namespace FileCabinetApp
{
    public class FileCabinetRecordXmlWriter : IFileCabinetRecordWriter
    {
        private XmlWriter writer;

        public FileCabinetRecordXmlWriter(TextWriter newText)
        {
            this.writer = XmlWriter.Create(newText);
            this.writer.WriteStartDocument();
        }

        public void Write(FileCabinetRecord record)
        {
            this.writer.WriteStartElement("record");

            this.writer.WriteStartAttribute("id");
            this.writer.WriteValue(record?.Id);
            this.writer.WriteEndAttribute();

            this.writer.WriteStartElement("name");
            this.writer.WriteAttributeString("last", record?.LastName);
            this.writer.WriteAttributeString("first", record?.FirstName);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("DateOfBirth");
            this.writer.WriteValue(record.DateOfBirth.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("AccountType");
            this.writer.WriteCharEntity(record.Type);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("AccountNumber");
            this.writer.WriteValue(record.Number);
            this.writer.WriteEndElement();

            this.writer.WriteStartElement("AccountBalance");
            this.writer.WriteValue(record.Balance);
            this.writer.WriteEndElement();

            this.writer.WriteEndElement();
            this.writer.Flush();
        }

        public void RootStart(string root)
        {
            this.writer.WriteStartElement(root);
        }

        public void RootEnd()
        {
            this.writer.WriteEndElement();
            this.writer.Close();
        }
    }
}
