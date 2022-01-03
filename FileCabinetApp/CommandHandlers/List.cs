using System;
using FileCabinetApp.Writers;

namespace FileCabinetApp.CommandHandlers
{
    public class List : ServiceCommandHandler
    {
        public List(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            DefaultPrint.PrintRecocrd(this.Service.GetRecords());

            Console.WriteLine();
        }
    }
}
