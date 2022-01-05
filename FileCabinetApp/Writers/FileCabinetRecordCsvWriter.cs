using System.Collections.ObjectModel;
using System.IO;

#pragma warning disable CA1062

namespace FileCabinetApp
{
    public class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
        private const string CSVTitle = "ID, F.tName, L.Name, D.OfBirth, Type, Number, Balance";
        private readonly TextWriter writer;

        public FileCabinetRecordCsvWriter(TextWriter newText)
        {
            this.writer = newText;
        }

        public void WriteRootStart(string title = CSVTitle)
        {
            this.writer.WriteLine(title);
        }

        public void WriteRootEnd()
        {
        }

        public void Write(ReadOnlyCollection<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                this.WriteRecord(record);
            }
        }

        public string GetRoot()
        {
            return CSVTitle;
        }

        private void WriteRecord(FileCabinetRecord record)
        {
            this.writer.WriteLine(
                "{0:000}, {1}, {2}, {3:dd.MM.yyyy}, {4}, {5:0000}, {6:N2}",
                record?.Id,
                record?.FirstName,
                record?.LastName,
                record.DateOfBirth,
                record?.Type,
                record.Number,
                record.Balance);

            this.writer.Flush();
        }
    }
}
