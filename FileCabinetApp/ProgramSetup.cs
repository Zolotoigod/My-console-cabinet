using System;
using FileCabinetApp.Validation;
using FileCabinetApp.Validation.Input;

namespace FileCabinetApp
{
    /// <summary>
    /// Setup the program options base of command line`s arguments.
    /// </summary>
    public static class ProgramSetup
    {
        public static void SetOptions(out IFileCabinetService service, out ValidatorLibrary validator, string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            int validationIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--validation_rules", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--v", StringComparison.InvariantCultureIgnoreCase));
            int storageIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--storage", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--s", StringComparison.InvariantCultureIgnoreCase));
            int serviceIndex = 0;
            if (validationIndex >= 0)
            {
                SetValidator(args, validationIndex, out validator, ref serviceIndex);
            }
            else
            {
                validator = new ValidatorLibrary(ParametersCreater.GetDefaultParams());
            }

            if (storageIndex >= 0)
            {
                SetStorage(args, storageIndex, serviceIndex, out service);
            }
            else
            {
                service = new FileCabinetMemoryService(serviceIndex);
            }

            Console.WriteLine($"Using {validator} validation rules");
            Console.WriteLine($"Using {service} storage");
        }

        private static void SetValidator(string[] args, int validationIndex, out ValidatorLibrary validator, ref int serviceIndex)
        {
            if (!string.IsNullOrWhiteSpace(args[validationIndex + 1]))
            {
                switch (args[validationIndex + 1].ToLowerInvariant())
                {
                    case "default":
                        {
                            validator = new ValidatorLibrary(ParametersCreater.GetDefaultParams());
                            break;
                        }

                    case "custom":
                        {
                            validator = new ValidatorLibrary(ParametersCreater.GetCustomParams());
                            serviceIndex = 1;
                            break;
                        }

                    default:
                        {
                            validator = new ValidatorLibrary(ParametersCreater.GetDefaultParams());
                            Console.WriteLine($"Validator {args[validationIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                validator = new ValidatorLibrary(ParametersCreater.GetDefaultParams());
            }
        }

        private static void SetStorage(string[] args, int storageIndex, int indexValidator, out IFileCabinetService service)
        {
            if (!string.IsNullOrWhiteSpace(args[storageIndex + 1]))
            {
                switch (args[storageIndex + 1].ToLowerInvariant())
                {
                    case "memory":
                        {
                            service = new FileCabinetMemoryService(indexValidator);
                            break;
                        }

                    case "file":
                        {
                            service = new FileCabinetFileService(indexValidator);
                            break;
                        }

                    default:
                        {
                            service = new FileCabinetMemoryService(indexValidator);
                            Console.WriteLine($"Service storage {args[storageIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                service = new FileCabinetMemoryService(indexValidator);
            }
        }
    }
}
