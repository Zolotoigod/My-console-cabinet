using System;
using FileCabinetApp.Writers;

namespace FileCabinetApp.CommandHandlers
{
    public class Find : ServiceCommandHandler
    {
        public Find(IFileCabinetService service, string mycommand)
            : base(service, mycommand)
        {
        }

        protected override void Realize(BaseValidationRules validationRules, string parameters)
        {
            string[] serchedField = parameters?.Split(' ', 2);
            if (serchedField.Length == 2)
            {
                serchedField[0] = serchedField[0].ToLowerInvariant();
                switch (serchedField[0])
                {
                    case "firstname":
                        {
                            DefaultPrint.PrintRecocrd(this.Service.FindByFirstName(serchedField[1]), serchedField[1], "FirstName");
                            break;
                        }

                    case "lastname":
                        {
                            DefaultPrint.PrintRecocrd(this.Service.FindByLastName(serchedField[1]), serchedField[1], "LastName");
                            break;
                        }

                    case "dateofbirth":
                        {
                            DefaultPrint.PrintRecocrd(this.Service.FindByDateOfBirth(serchedField[1]), serchedField[1], "DateOfBirth");
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("Unknown field\n");
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
