using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FileCabinetApp.Decorations
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

        public string EditRecord(int id, InputDataPack storage)
        {
            this.timer.Restart();
            string message = this.service.EditRecord(id, storage);
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return message;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            this.timer.Restart();
            IEnumerable<FileCabinetRecord> result = Defines.ReturnCollection(this.service.FindByDateOfBirth(dateOfBirth));
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.timer.Restart();
            IEnumerable<FileCabinetRecord> result = Defines.ReturnCollection(this.service.FindByFirstName(firstName));
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.timer.Restart();
            IEnumerable<FileCabinetRecord> result = Defines.ReturnCollection(this.service.FindByLastName(lastName));
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return result;
        }

        public int GetDeletedRecords()
        {
            return this.service.GetDeletedRecords();
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            this.timer.Restart();
            IEnumerable<FileCabinetRecord> collection = Defines.ReturnCollection(this.service.GetRecords());
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
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

        public string Purge()
        {
            this.timer.Restart();
            string message = this.service.Purge();
            this.timer.Stop();
            Console.WriteLine($"Method execution duration is {this.timer.ElapsedTicks}");
            return message;
        }

        public override string ToString()
        {
            return $"{this.service}<Show execution duration>";
        }
    }
}
