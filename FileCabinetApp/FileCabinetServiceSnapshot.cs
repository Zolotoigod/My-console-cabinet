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
            newWriter.WriteTitle(this.fileTitle);
            foreach (var record in this.records)
            {
                newWriter.Write(record);
            }
        }

        public void SaveToXML(TextWriter writer)
        {
            FileCabinetRecordXmlWriter newWriter = new (writer);
            newWriter.RootStart("records");
            foreach (var record in this.records)
            {
                newWriter.Write(record);
            }

            newWriter.RootEnd();
        }
    }
}
