using System;
using System.Collections.ObjectModel;
using FileCabinetApp.Writers;

namespace FileCabinetApp.CommandHandlers
{
    public class Find : BaseCommandHandler
    {
        public Find(string mycommand)
            : base(mycommand)
        {
        }

        protected override void Realize(IFileCabinetService service, BaseValidationRules validationRules, string parameters)
        {
            if (service is null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            string[] serchedField = parameters?.Split(' ', 2);
            if (serchedField.Length == 2)
            {
                serchedField[0] = serchedField[0].ToLowerInvariant();
                switch (serchedField[0])
                {
                    case "firstname":
                        {
                            FindRecord(service.FindByFirstName(serchedField[1]), serchedField[1], "FirstName");
                            break;
                        }

                    case "lastname":
                        {
                            FindRecord(service.FindByLastName(serchedField[1]), serchedField[1], "LastName");
                            break;
                        }

                    case "dateofbirth":
                        {
                            FindRecord(service.FindByDateOfBirth(serchedField[1]), serchedField[1], "DateOfBirth");
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

        private static void FindRecord(ReadOnlyCollection<FileCabinetRecord> collection, string valueFind, string fildName)
        {
            if (collection == null)
            {
                Console.WriteLine("Incorrect date\n");
                return;
            }

            if (collection.Count == 0)
            {
                Console.WriteLine($"{fildName} {valueFind} not found\n");
                return;
            }

            foreach (var record in collection)
            {
                if (!(record == null))
                {
                    DefaultPrint.PrintRecocrd(record);
                }
                else
                {
                    Console.WriteLine($"{fildName} {valueFind} not found\n");
                    return;
                }
            }
        }
    }
}
