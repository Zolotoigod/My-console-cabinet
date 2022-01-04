using System;

namespace FileCabinetApp.CommandHandlers
{
    public class Remove : ServiceCommandHandler
    {
        public Remove(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            if (int.TryParse(parameters, out int id))
            {
                Console.WriteLine(this.Service.Remove(id));
            }
            else
            {
                Console.WriteLine("Incorrect Parameters");
            }
        }
    }
}
