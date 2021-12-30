using System;
using System.Globalization;

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
                    Console.WriteLine(Defines.ConsoleFormat, record.Id, record.FirstName, record.LastName, record.DateOfBirth.ToString("yyyy MMM dd", CultureInfo.InvariantCulture), record.Type, record.Number, record.Balance);
                }
            }

            Console.WriteLine();
        }
    }
}
