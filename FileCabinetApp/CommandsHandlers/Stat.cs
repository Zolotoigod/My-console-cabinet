using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Stat : BaseCommandHandler
    {
        public Stat(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            Console.WriteLine($"Total records - {service.GetStat()}");
            Console.WriteLine($"Removed records - {service.GetDeletedRecords()}\n");
        }
    }
}
