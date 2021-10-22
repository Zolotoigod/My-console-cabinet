using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private readonly BaseValidationRules validationRules;
        private readonly FileStream streamDB;

        public FileCabinetFilesystemService(BaseValidationRules validationRules)
        {
            this.validationRules = validationRules;
            this.streamDB = new FileStream("cabinet-records.db", FileMode.OpenOrCreate);
        }

        public int CreateRecord(DataStorage store)
        {
            throw new NotImplementedException();
        }

        public void EditRecord(int id, DataStorage store)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "Filesistem";
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.streamDB.Close();
            }
        }
    }
}
