using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Create : BaseCommandHandler
    {
        public Create(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            DataStorage record = new (validationRules);
            int id = service.CreateRecord(record);
            Console.WriteLine($"Record #{id} is created\n");
        }
    }
}
