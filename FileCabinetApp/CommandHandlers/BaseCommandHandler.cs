using System;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class BaseCommandHandler : ICommandHandler
    {
        private BaseCommandHandler nextHandler;

        protected BaseCommandHandler(string mycommand)
        {
            this.MyCommand = mycommand;
        }

        public string MyCommand { get; }

        public bool HandleCommand(IFileCabinetService service, BaseValidationRules validationRules, string command, string data)
        {
            if (this.MyCommand.Equals(command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Realize(service, validationRules, data);
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
                    return this.nextHandler.HandleCommand(service, validationRules, command, data);
                }
            }
        }

        public void SetNext(BaseCommandHandler nexthandler)
        {
            this.nextHandler = nexthandler;
        }

        protected abstract void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters);
    }
}
