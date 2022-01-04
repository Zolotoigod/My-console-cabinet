using System;
using System.Collections.Generic;

namespace FileCabinetApp.CommandHandlers
{
    public class Find : ServiceCommandHandler
    {
        private Action<IEnumerable<FileCabinetRecord>> printer;

        public Find(IFileCabinetService service, Action<IEnumerable<FileCabinetRecord>> printer, string mycommand)
            : base(service, mycommand)
        {
            this.printer = printer;
        }

        protected override void Realize(DataValidator validator, IInput input, string parameters)
        {
            string[] serchedField = parameters?.Split(' ', 2);
            if (serchedField.Length == 2)
            {
                serchedField[0] = serchedField[0].ToLowerInvariant();
                switch (serchedField[0])
                {
                    case "firstname":
                        {
                            this.printer(this.Service.FindByFirstName(serchedField[1]));
                            break;
                        }

                    case "lastname":
                        {
                            this.printer(this.Service.FindByLastName(serchedField[1]));
                            break;
                        }

                    case "dateofbirth":
                        {
                            this.printer(this.Service.FindByDateOfBirth(serchedField[1]));
                            break;
                        }

                    default:
                        {
                            Console.WriteLine($"I can`t search by field '{serchedField[0]}'\n");
                            break;
                        }
                }
            }
            else
            {
                Console.WriteLine("Incorrect find parameters\n");
            }
        }
    }
}
