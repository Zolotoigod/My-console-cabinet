using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Setup the program options base of command line`s arguments.
    /// </summary>
    public static class ProgramSetup
    {
        public static void SetOptions(out IFileCabinetService service, out BaseValidationRules validationRules, string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            int validationIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--validation_rules", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--v", StringComparison.InvariantCultureIgnoreCase));
            int storageIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--storage", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--s", StringComparison.InvariantCultureIgnoreCase));
            if (validationIndex >= 0)
            {
                SetValidator(args, validationIndex, out validationRules);
            }
            else
            {
                validationRules = new DefaultValidateRules();
            }

            if (storageIndex >= 0)
            {
                SetStorage(args, storageIndex, validationRules, out service);
            }
            else
            {
                service = new FileCabinetMemoryService(validationRules);
            }

            Console.WriteLine($"Using {validationRules} validation rules");
            Console.WriteLine($"Using {service} storage");
        }

        private static void SetValidator(string[] args, int validationIndex, out BaseValidationRules validationRules)
        {
            if (!string.IsNullOrWhiteSpace(args[validationIndex + 1]))
            {
                switch (args[validationIndex + 1].ToLowerInvariant())
                {
                    case "default":
                        {
                            validationRules = new DefaultValidateRules();
                            break;
                        }

                    case "custom":
                        {
                            validationRules = new CustomValdationRules();
                            break;
                        }

                    default:
                        {
                            validationRules = new DefaultValidateRules();
                            Console.WriteLine($"Validator {args[validationIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                validationRules = new DefaultValidateRules();
            }
        }

        private static void SetStorage(string[] args, int storageIndex, BaseValidationRules validationRules, out IFileCabinetService service)
        {
            if (!string.IsNullOrWhiteSpace(args[storageIndex + 1]))
            {
                switch (args[storageIndex + 1].ToLowerInvariant())
                {
                    case "memory":
                        {
                            service = new FileCabinetMemoryService(validationRules);
                            break;
                        }

                    case "file":
                        {
                            service = new FileCabinetFileService(validationRules);
                            break;
                        }

                    default:
                        {
                            service = new FileCabinetMemoryService(validationRules);
                            Console.WriteLine($"Service storage {args[storageIndex + 1]} unsupported");
                            break;
                        }
                }
            }
            else
            {
                service = new FileCabinetMemoryService(validationRules);
            }
        }
    }
}
