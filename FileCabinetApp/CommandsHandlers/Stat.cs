using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Stat : ServiceCommandHandler
    {
        public Stat(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            Console.WriteLine($"Total records - {this.Service.GetStat()}");
            Console.WriteLine($"Removed records - {this.Service.GetDeletedRecords()}\n");
        }
    }
}
