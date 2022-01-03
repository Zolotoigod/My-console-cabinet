using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Edit : ServiceCommandHandler
    {
        public Edit(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            if (int.TryParse(parameters, out int id) && id > 0)
            {
                if (this.Service.GetListId().Contains(id))
                {
                    this.Service.EditRecord(id, new DataStorage(validationRules));
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
