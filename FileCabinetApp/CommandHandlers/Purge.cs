using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Purge : ServiceCommandHandler
    {
        public Purge(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            if (this.Service is FileCabinetFileService fileService)
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
