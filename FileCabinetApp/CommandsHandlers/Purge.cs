using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Purge : BaseCommandHandler
    {
        public Purge(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (service is FileCabinetFileService fileService)
            {
                Console.WriteLine(fileService.Purge());
            }
            else
            {
                Console.WriteLine("There is no FileStorage.");
            }
        }
    }
}
