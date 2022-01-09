using System;
using FileCabinetApp.Validation.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class Edit : ServiceCommandHandler
    {
        public Edit(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            if (int.TryParse(parameters, out int id) && id > 0)
            {
                Console.WriteLine(this.Service.EditRecord(id, new InputDataPack(validator, input)));
            }
            else
            {
                Console.WriteLine("Incorrect id\n");
            }
        }
    }
}
