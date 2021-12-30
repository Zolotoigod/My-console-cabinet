using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Edit : BaseCommandHandler
    {
        public Edit(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (int.TryParse(parameters, out int id) && id > 0)
            {
                if (service.GetListId().Contains(id))
                {
                    service.EditRecord(id, new DataStorage(validationRules));
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
