using System;
using FileCabinetApp.Validation.Service;

namespace FileCabinetApp.CommandHandlers
{
    public class ExitHandler : BaseCommandHandler
    {
        private Action isRuning;

        public ExitHandler(Action isRun, string mycommand)
            : base(mycommand)
        {
            this.isRuning = isRun;
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            this.isRuning.Invoke();
        }
    }
}
