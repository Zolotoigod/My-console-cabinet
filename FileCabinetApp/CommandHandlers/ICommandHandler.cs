using FileCabinetApp.CommandHandlers;
using FileCabinetApp.DTO;

namespace FileCabinetApp
{
    public interface ICommandHandler
    {
        public string MyCommand { get; }

        public void HandleCommand(BaseValidationRules validationRules, AppCommandRequest request);

        public void SetNext(ICommandHandler nexthandler);
    }
}
