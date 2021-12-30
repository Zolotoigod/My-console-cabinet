using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Remove : BaseCommandHandler
    {
        public Remove(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            if (int.TryParse(parameters, out int id))
            {
                Console.WriteLine(service.Remove(id));
            }
            else
            {
                Console.WriteLine("Incorrect Parameters");
            }
        }
    }
}
