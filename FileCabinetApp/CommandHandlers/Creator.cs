namespace FileCabinetApp.CommandHandlers
{
    public static class Creator
    {
        public static BaseCommandHandler CrateCommandChain()
        {
            HandlersFabric creator = new HandlersFabric();
            creator.SetRoot(new Help("help"));
            creator.AddNextHandler(new Create("create"))
                   .AddNextHandler(new Find("find"))
                   .AddNextHandler(new Edit("edit"))
                   .AddNextHandler(new Export("export"))
                   .AddNextHandler(new Import("import"))
                   .AddNextHandler(new List("list"))
                   .AddNextHandler(new Stat("stat"))
                   .AddNextHandler(new Remove("remove"))
                   .AddNextHandler(new Purge("purge"));

            return creator.GetRoot();
        }
    }
}
