namespace FileCabinetApp
{
    public interface IFileCabinetRecordWriter
    {
        public void Write(FileCabinetRecord record);

        public void WriteRootEnd();

        public void WriteRootStart(string title);

        public string GetRoot();
    }
}
