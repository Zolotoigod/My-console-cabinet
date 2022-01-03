using System;
using FileCabinetApp.Writers;

namespace FileCabinetApp.CommandHandlers
{
    public static class HandlerFabric
    {
        public static BaseCommandHandler CrateCommandChain(IFileCabinetService service, Action exit)
        {
            HandlerFluent creator = new HandlerFluent();
            creator.SetRoot(new Help("help"));
            creator.AddNextHandler(new Create(service, "create"))
                   .AddNextHandler(new Find(service, DefaultPrint.PrintRecocrd, "find"))
                   .AddNextHandler(new Edit(service, "edit"))
                   .AddNextHandler(new Export(service, "export"))
                   .AddNextHandler(new Import(service, "import"))
                   .AddNextHandler(new List(service, DefaultPrint.PrintRecocrd, "list"))
                   .AddNextHandler(new Stat(service, "stat"))
                   .AddNextHandler(new Remove(service, "remove"))
                   .AddNextHandler(new Purge(service, "purge"))
                   .AddNextHandler(new ExitHandler(exit, "exit"));

            return creator.GetRoot();
        }
    }
}
