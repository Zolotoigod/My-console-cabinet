using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    public class ValidationParameters
    {
        public int MaxFirstNameLength { get; set; }

        public int MinFirstNameLength { get; set; }

        public int MaxLastNameLength { get; set; }

        public int MinLastNameLength { get; set; }

        public DateTime DateOfBirthFrom { get; set; }

        public DateTime DateOfBirthTo { get; set; }

        public ReadOnlyCollection<char> Types { get; set; }

        public short NumberMax { get; set; }
    }
}
