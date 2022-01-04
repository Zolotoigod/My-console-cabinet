using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHandlers
{
    public class List : ServiceCommandHandler
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        public List(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer, string mycommand)
            : base(service, mycommand)
        {
            this.printer = printer;
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            this.printer(this.Service.GetRecords());

            Console.WriteLine();
        }
    }
}
