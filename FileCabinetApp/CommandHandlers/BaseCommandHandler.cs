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

        public void HandleCommand(BaseValidationRules validationRules, AppCommandRequest request)
        {
            if (this.MyCommand.Equals(request?.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Realize(validationRules, request.Parametres);
            }
            else
            {
                if (this.nextHandler is null)
                {
                    PrintMissedCommandInfo(request.Command);
                }
                else
                {
                    this.nextHandler.HandleCommand(validationRules, request);
                }
            }
        }

        public void SetNext(ICommandHandler nexthandler)
        {
            this.nextHandler = nexthandler;
        }

        protected abstract void Realize(BaseValidationRules validationRules, string parameters);

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}
