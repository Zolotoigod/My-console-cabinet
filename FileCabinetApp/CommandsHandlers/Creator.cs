namespace FileCabinetApp.CommandHandlers
{
    public static class Creator
    {
        public static BaseCommandHandler CrateCommandChain(IFileCabinetService service)
        {
            HandlersFabric creator = new HandlersFabric();
            creator.SetRoot(new Help("help"));
            creator.AddNextHandler(new Create(service, "create"))
                   .AddNextHandler(new Find(service, "find"))
                   .AddNextHandler(new Edit(service, "edit"))
                   .AddNextHandler(new Export(service, "export"))
                   .AddNextHandler(new Import(service, "import"))
                   .AddNextHandler(new List(service, "list"))
                   .AddNextHandler(new Stat(service, "stat"))
                   .AddNextHandler(new Remove(service, "remove"))
                   .AddNextHandler(new Purge(service, "purge"));

            return creator.GetRoot();
        }
    }
}
