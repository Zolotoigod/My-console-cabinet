using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    public interface IFileCabinetRecordWriter
    {
        public void Write(ReadOnlyCollection<FileCabinetRecord> records);

        public void WriteRootEnd();

        public void WriteRootStart(string title);

        public string GetRoot();
    }
}
