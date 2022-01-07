using System;
using FileCabinetApp.Decorations;
using FileCabinetApp.Validation;
using FileCabinetApp.Validation.Input;

#pragma warning disable SA1117
#pragma warning disable CA2000

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

            int validationIndex = Array.FindIndex(args, 0, args.Length,
                match => match.Equals("--validation_rules", StringComparison.InvariantCultureIgnoreCase)
                || match.Equals("--v", StringComparison.InvariantCultureIgnoreCase));

            int storageIndex = Array.FindIndex(args, 0, args.Length,
                match => match.Equals("--storage", StringComparison.InvariantCultureIgnoreCase)
                || match.Equals("--s", StringComparison.InvariantCultureIgnoreCase));

            int metrIndex = Array.FindIndex(args, 0, args.Length,
                match => match.Equals("use-stopwatch", StringComparison.InvariantCultureIgnoreCase));

            int serviceIndex = 0;
            if (validationIndex >= 0)
            {
                SetValidator(args, validationIndex, out validator, ref serviceIndex);
            }
            else
            {
                validator = new ValidatorLibrary(ParametersCreater.ReadConfigParams(Defines.ConfigPath));
            }

            if (storageIndex >= 0)
            {
                SetStorage(args, storageIndex, serviceIndex, metrIndex, out service);
            }
            else
            {
                service = SetTimer(new FileCabinetMemoryService(serviceIndex), metrIndex);
            }

            string rules = SwitchText(serviceIndex);

            Console.WriteLine($"Using {rules} validation rules");
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

                    case "config":
                        {
                            validator = new ValidatorLibrary(ParametersCreater.ReadConfigParams(Defines.ConfigPath));
                            serviceIndex = 2;
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

        private static void SetStorage(string[] args, int storageIndex, int indexValidator, int timerIndex, out IFileCabinetService service)
        {
            if (!string.IsNullOrWhiteSpace(args[storageIndex + 1]))
            {
                switch (args[storageIndex + 1].ToLowerInvariant())
                {
                    case "memory":
                        {
                            service = SetTimer(new FileCabinetMemoryService(indexValidator), timerIndex);
                            break;
                        }

                    case "file":
                        {
                            service = SetTimer(new FileCabinetFileService(indexValidator), timerIndex);
                            break;
                        }

                    default:
                        {
                            service = SetTimer(new FileCabinetMemoryService(indexValidator), timerIndex);
                            Console.WriteLine($"Service storage {args[storageIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                service = SetTimer(new FileCabinetMemoryService(indexValidator), timerIndex);
            }
        }

        private static string SwitchText(int index) => index switch
        {
            1 => "Custom",
            2 => "Config",
            _ => "Default",
        };

        private static IFileCabinetService SetTimer(IFileCabinetService service, int index)
        {
            if (index > 0)
            {
                return new ServiceMeter(service);
            }
            else
            {
                return service;
            }
        }
    }
}
