using System;
using FileCabinetApp.DTO;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class BaseCommandHandler : ICommandHandler
    {
        private ICommandHandler nextHandler;

        protected BaseCommandHandler(string mycommand)
        {
            this.MyCommand = mycommand;
        }

        public string MyCommand { get; }

        public bool HandleCommand(BaseValidationRules validationRules, AppCommandRequest request)
        {
            if (this.MyCommand.Equals(request?.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Realize(validationRules, request.Parametres);
                return true;
            }
            else
            {
                if (this.nextHandler is null)
                {
                    return false;
                }
                else
                {
                    return this.nextHandler.HandleCommand(validationRules, request);
                }
            }
        }

        public void SetNext(ICommandHandler nexthandler)
        {
            this.nextHandler = nexthandler;
        }

        protected abstract void Realize(BaseValidationRules validationRules, string parameters);
    }
}
