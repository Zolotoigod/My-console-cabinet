using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.DTO;
using Newtonsoft.Json;

namespace FileCabinetApp.Validation
{
    public static class ParametersCreater
    {
        public static ValidationParameters GetDefaultParams()
        {
            return new ValidationParameters()
            {
                MaxFirstNameLength = 120,
                MinFirstNameLength = 2,
                MaxLastNameLength = 120,
                MinLastNameLength = 2,
                DateOfBirthFrom = new DateTime(1950, 1, 1),
                Types = new ReadOnlyCollection<char>(new List<char>() { 'A', 'B', 'C' }),
                DateOfBirthTo = DateTime.Now,
                NumberMax = 10000,
                BalanceMax = decimal.MaxValue,
            };
        }

        public static ValidationParameters GetCustomParams()
        {
            return new ValidationParameters()
            {
                MaxFirstNameLength = 60,
                MinFirstNameLength = 2,
                MaxLastNameLength = 60,
                MinLastNameLength = 2,
                DateOfBirthFrom = new DateTime(1930, 1, 1),
                Types = new ReadOnlyCollection<char>(new List<char>() { 'A', 'B', 'C', 'D' }),
                DateOfBirthTo = DateTime.Now,
                NumberMax = 10000,
                BalanceMax = decimal.MaxValue,
            };
        }

        public static ValidationParameters ReadConfigParams()
        {
            ConfigParamsAdapter its;
            using (FileStream stream = new FileStream("../../../validation-rules.json", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    its = JsonConvert.DeserializeObject<ConfigParamsAdapter>(reader.ReadToEnd());
                }
            }

            return new ValidationParameters();
        }
    }
}
