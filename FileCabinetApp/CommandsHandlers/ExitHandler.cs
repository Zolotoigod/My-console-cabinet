using System;
using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp.CommandsHandlers
{
    public class ExitHandler : BaseCommandHandler
    {
        public ExitHandler(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            throw new NotImplementedException();
        }
    }
}
