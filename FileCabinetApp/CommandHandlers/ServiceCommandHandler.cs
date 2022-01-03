namespace FileCabinetApp.CommandHandlers
{
    public abstract class ServiceCommandHandler : BaseCommandHandler
    {
        private IFileCabinetService service;

        protected ServiceCommandHandler(IFileCabinetService service, string mycommand)
            : base(mycommand)
        {
            this.service = service;
        }

        protected IFileCabinetService Service { get => this.service; }
    }
}
