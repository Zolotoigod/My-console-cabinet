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
            Console.WriteLine(this.Service.Purge());
        }
    }
}
