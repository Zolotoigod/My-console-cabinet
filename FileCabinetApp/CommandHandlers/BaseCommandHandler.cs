using System;

namespace FileCabinetApp.CommandHandlers
{
    public abstract class BaseCommandHandler : ICommandHandler
    {
        private BaseCommandHandler nextHandler;

        protected BaseCommandHandler(BaseCommandHandler nexthandler, string mycommand)
        {
            this.nextHandler = nexthandler;
            this.MyCommand = mycommand;
        }

        public string MyCommand { get; }

        public abstract void Realize(string data);

        public void HandleCommand(string command, string data)
        {
            if (command == this.MyCommand)
            {
                this.Realize(data);
            }
            else
            {
                if (!(this.nextHandler is null))
                {
                    this.nextHandler.HandleCommand(command, data);
                }
                else
                {
                    throw new InvalidOperationException("Next handler is not found");
                }
            }
        }
    }
}
