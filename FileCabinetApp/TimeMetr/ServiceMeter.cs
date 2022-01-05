using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace FileCabinetApp.TimeMetr
{
    public sealed class ServiceMeter : IFileCabinetService, IDisposable
    {
        private readonly Stopwatch timer = new Stopwatch();
        private IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            this.service = service;
        }

        public int CreateRecord(InputDataPack storage)
        {
            this.timer.Restart();
            int id = this.service.CreateRecord(storage);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return id;
        }

        public void Dispose()
        {
            if (this.service is FileCabinetFileService serv)
            {
                serv.Dispose();
            }
        }

        public void EditRecord(int id, InputDataPack storage)
        {
            this.timer.Restart();
            this.service.EditRecord(id, storage);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.timer.Restart();
            ReadOnlyCollection<FileCabinetRecord> result = this.service.FindByDateOfBirth(dateOfBirth);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.timer.Restart();
            ReadOnlyCollection<FileCabinetRecord> result = this.service.FindByFirstName(firstName);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.timer.Restart();
            ReadOnlyCollection<FileCabinetRecord> result = this.service.FindByLastName(lastName);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public int GetDeletedRecords()
        {
            return this.service.GetDeletedRecords();
        }

        public List<int> GetListId()
        {
            return this.service.GetListId();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.service.GetRecords();
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
            this.timer.Restart();
            string result = this.service.Remove(id);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public void Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.service.Restore(snapshot);
        }

        public override string ToString()
        {
            return $"{this.service}<Show execution duration>";
        }
    }
}
