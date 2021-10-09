using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();

        public int CreateRecord(string firstname, string lastname, DateTime dateOfBirth)
        {
            // TODO: добавьте реализацию метода
            return 0;
        }

        public FileCabinetRecord[] GetRecord()
        {
            // TODO: добавьте реализацию метода
            return Array.Empty<FileCabinetRecord>();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
