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
                if (this.Service.GetListId().Contains(id))
                {
                    this.Service.EditRecord(id, new InputDataPack(validator, input));
                    Console.WriteLine($"Record #{id} is updated\n");
                }
                else
                {
                    Console.WriteLine($"Record #{id} not found\n");
                }
            }
            else
            {
                Console.WriteLine("Incorrect id\n");
            }
        }
    }
}
