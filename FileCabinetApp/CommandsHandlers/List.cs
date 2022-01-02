using System;
using System.Globalization;
using FileCabinetApp.Writers;

namespace FileCabinetApp.CommandHandlers
{
    public class List : BaseCommandHandler
    {
        public List(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            foreach (var record in service.GetRecords())
            {
                if (!(record == null))
                {
                    DefaultPrint.PrintRecocrd(record);
                }
            }

            Console.WriteLine();
        }
    }
}
