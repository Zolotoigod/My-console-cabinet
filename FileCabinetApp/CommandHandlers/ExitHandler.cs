using System;

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

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            this.isRuning.Invoke();
        }
    }
}
