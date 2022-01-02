using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Create : ServiceCommandHandler
    {
        public Create(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            DataStorage record = new (validationRules);
            int id = this.Service.CreateRecord(record);
            Console.WriteLine($"Record #{id} is created\n");
        }
    }
}
