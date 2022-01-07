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
            LoggerPrinter.PrintLogCreate(this.writer, storage, id);
            return id;
        }

        public void EditRecord(int id, InputDataPack storage)
        {
            this.service.EditRecord(id, storage);
            LoggerPrinter.PrintLogEdit(this.writer, storage, id);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            ReadOnlyCollection<FileCabinetRecord> collection = this.service.FindByDateOfBirth(dateOfBirth);
            LoggerPrinter.PrintLogFind(this.writer, dateOfBirth, collection.Count);
            return collection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            ReadOnlyCollection<FileCabinetRecord> collection = this.service.FindByFirstName(firstName);
            LoggerPrinter.PrintLogFind(this.writer, firstName, collection.Count);
            return collection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            ReadOnlyCollection<FileCabinetRecord> collection = this.service.FindByLastName(lastName);
            LoggerPrinter.PrintLogFind(this.writer, lastName, collection.Count);
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

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            ReadOnlyCollection<FileCabinetRecord> collection = this.service.GetRecords();
            LoggerPrinter.PrintLogGetRecord(this.writer, collection.Count);
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
            LoggerPrinter.PrintLogRemove(this.writer, id, message);
            return message;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.service.Restore(snapshot);
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
