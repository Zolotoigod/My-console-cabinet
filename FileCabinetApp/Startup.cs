using System;
using FileCabinetApp.Decorations;
using FileCabinetApp.Validation;
using FileCabinetApp.Validation.Input;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp
{
    public class Startup
    {
        public Startup(string[] args = null)
        {
            this.AppSettings = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();
        }

        public IConfigurationRoot AppSettings { get; set; }

        public void Configurate(out IFileCabinetService service, out ValidatorLibrary validator)
        {
            validator = SetValidator(this.AppSettings["v"] ?? string.Empty);
            service = SetStorage(this.AppSettings["v"], this.AppSettings["storage"] ?? string.Empty)
                .SetLogger(bool.Parse(this.AppSettings["logger"] ?? "false"))
                .SetTimer(bool.Parse(this.AppSettings["stopwatch"] ?? "false"));

            Console.WriteLine($"Using {service.ToString()} storage");
            Console.WriteLine($"Using {validator.ToString()} validation_rules");
        }

        private static ValidatorLibrary SetValidator(string validatorName) =>
            validatorName.ToLowerInvariant() switch
        {
            "default" => new ValidatorLibrary(validatorName, ParametersCreater.GetDefaultParams()),
            "custom" => new ValidatorLibrary(validatorName, ParametersCreater.GetCustomParams()),
            "config" => new ValidatorLibrary(validatorName, ParametersCreater.ReadConfigParams(Defines.ConfigPath)),
            _ => new ValidatorLibrary(validatorName, ParametersCreater.GetDefaultParams())
        };

        private static IFileCabinetService SetStorage(string validatorName, string storageName) =>
            storageName.ToLowerInvariant() switch
        {
            "file" => new FileCabinetFileService(validatorName),
            "memory" => new FileCabinetMemoryService(validatorName),
            _ => new FileCabinetMemoryService(validatorName)
        };
    }
}
