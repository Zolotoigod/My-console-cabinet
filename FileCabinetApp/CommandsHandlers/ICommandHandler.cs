using FileCabinetApp.CommandHandlers;
using FileCabinetApp.DTO;

namespace FileCabinetApp
{
    public interface ICommandHandler
    {
        public string MyCommand { get; }

        public bool HandleCommand(BaseValidationRules validationRules, AppCommandRequest request);

        public void SetNext(ICommandHandler nexthandler);
    }
}
