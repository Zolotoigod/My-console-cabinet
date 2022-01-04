using FileCabinetApp.DTO;

namespace FileCabinetApp
{
    public interface ICommandHandler
    {
        public string MyCommand { get; }

        public void HandleCommand(DataValidator validator, IInput input, AppCommandRequest request);

        public void SetNext(ICommandHandler nexthandler);
    }
}
