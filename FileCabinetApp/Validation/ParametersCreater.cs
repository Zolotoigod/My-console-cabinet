using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
                DateOfBirthTo = DateTime.Now,
                NumberMax = 10000,
                Types = new ReadOnlyCollection<char>(new List<char>() { 'A', 'B', 'C' }),
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
                DateOfBirthTo = DateTime.Now,
                NumberMax = 10000,
                Types = new ReadOnlyCollection<char>(new List<char>() { 'A', 'B', 'C', 'D' }),
            };
        }
    }
}
