using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp.Decorations
{
    public sealed class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly IFileCabinetService service;
        private readonly StreamWriter writer;

        public ServiceLogger(IFileCabinetService service)
        {
            this.writer = new StreamWriter(
                new FileStream(
                Defines.LoggerPath,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.None), System.Text.Encoding.UTF8);

            this.service = service;
        }

        public int CreateRecord(InputDataPack storage)
        {
            int id = this.service.CreateRecord(storage);
            LoggerPrinter.PrintLogInfo(this.writer, "Edit()", $"{id}", storage);
            return id;
        }

        public string EditRecord(int id, InputDataPack storage)
        {
            string message = this.service.EditRecord(id, storage);
            LoggerPrinter.PrintLogInfo(this.writer, "Edit()", message, storage, id);
            return message;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            IEnumerable<FileCabinetRecord> collection = Defines.ReturnCollection(this.service.FindByDateOfBirth(dateOfBirth));
            LoggerPrinter.PrintLogInfo(this.writer, "Find()", "collection of matching records", dateOfBirth);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            IEnumerable<FileCabinetRecord> collection = Defines.ReturnCollection(this.service.FindByFirstName(firstName));
            LoggerPrinter.PrintLogInfo(this.writer, "Find()", "collection of matching records", firstName);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            IEnumerable<FileCabinetRecord> collection = Defines.ReturnCollection(this.service.FindByLastName(lastName));
            LoggerPrinter.PrintLogInfo(this.writer, "Find()", "collection of matching records", lastName);
            return collection;
        }

        public int GetDeletedRecords()
        {
           return this.service.GetDeletedRecords();
        }

        public List<int> GetListId()
        {
            return new List<int>();
        }

        /// <summary>
        /// Fix log value.
        /// </summary>
        /// <returns>All service records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            IEnumerable<FileCabinetRecord> collection = Defines.ReturnCollection(this.service.GetRecords());
            LoggerPrinter.PrintLogInfo(this.writer, "List()", "collection of all records");
            return collection;
        }

        public int GetStat()
        {
            return this.service.GetStat();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            return this.service.MakeSnapshot();
        }

        public string Remove(int id)
        {
            string message = this.service.Remove(id);
            LoggerPrinter.PrintLogInfo(this.writer, "Remove()", message, id);
            return message;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.service.Restore(snapshot);
        }

        public string Purge()
        {
            string message = this.service.Purge();
            LoggerPrinter.PrintLogInfo(this.writer, "Purge()", message);
            return message;
        }

        public void Dispose()
        {
            if (this.service is FileCabinetFileService serv1)
            {
                serv1.Dispose();
            }
            else if (this.service is ServiceMeter serv2)
            {
                serv2.Dispose();
            }

            this.writer.Dispose();
        }

        public override string ToString()
        {
            return $"{this.service}<Write Log to file {Defines.LoggerPath}>";
        }
    }
}
