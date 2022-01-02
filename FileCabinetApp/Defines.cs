namespace FileCabinetApp
{
    public static class Defines
    {
        public const string AvailableFields = "ID, F.tName, L.Name, D.OfBirth, Type, Number, Balance";

        public const string DefaultConsoleFormat = "#{0}, {1}, {2}, {3}, {4}, {5}, {6:f2}";

        public static readonly string[] AvailableExportFormats = { "csv", "xml" };

        public static readonly string[] DateFormat = { "MM dd yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM,dd,yyyy", "dd MM yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "dd,MM,yyyy" };

        public static readonly int RecordSize = sizeof(short) + sizeof(int) + 120 + 120 + (3 * sizeof(int)) + 2 + sizeof(short) + sizeof(decimal);
    }
}
