using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
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

        public static ValidationParameters ReadConfigParams(string path)
        {
            return JsonConvert.DeserializeObject<ValidationParameters>(
                File.ReadAllText(path));
        }
    }
}
