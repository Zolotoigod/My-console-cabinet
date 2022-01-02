using FileCabinetApp.CommandHandlers;

namespace FileCabinetApp
{
    public interface ICommandHandler
    {
        public string MyCommand { get; }

        public bool HandleCommand(IFileCabinetService service, BaseValidationRules validationRules, string command, string data);

        public void SetNext(BaseCommandHandler nexthandler);
    }
}
