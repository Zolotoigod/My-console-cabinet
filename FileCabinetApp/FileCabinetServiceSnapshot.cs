using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;
        private readonly string fileTitle;

        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list, string title)
        {
            this.records = list?.ToArray();
            this.fileTitle = title;
        }

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
    }
}
