using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Create : ServiceCommandHandler
    {
        public Create(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input,  string parameters)
        {
            InputDataPack record = new (validator, input);
            int id = this.Service.CreateRecord(record);
            Console.WriteLine($"Record #{id} is created\n");
        }
    }
}
