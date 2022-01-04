using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Stat : ServiceCommandHandler
    {
        public Stat(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            Console.WriteLine($"Total records - {this.Service.GetStat()}");
            Console.WriteLine($"Removed records - {this.Service.GetDeletedRecords()}\n");
        }
    }
}
