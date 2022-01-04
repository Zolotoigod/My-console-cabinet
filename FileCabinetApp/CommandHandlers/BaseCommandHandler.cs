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

        public void HandleCommand(DataValidator validator, IInput input, AppCommandRequest request)
        {
            if (this.MyCommand.Equals(request?.Command, StringComparison.InvariantCultureIgnoreCase))
            {
                this.Realize(validator, input, request.Parametres);
            }
            else
            {
                if (this.nextHandler is null)
                {
                    PrintMissedCommandInfo(request.Command);
                }
                else
                {
                    this.nextHandler.HandleCommand(validator, input, request);
                }
            }
        }

        public void SetNext(ICommandHandler nexthandler)
        {
            this.nextHandler = nexthandler;
        }

        protected abstract void Realize(DataValidator validator, IInput input, string parameters);

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }
    }
}
