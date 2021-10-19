using System;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator vith custom parameters.
    /// </summary>
    public class CustomValdationRules : BaseValidationRules
    {
        public override bool BalanceValidationRules(decimal balance)
        {
            return balance > 0;
        }

        public override bool DateValidationRules(DateTime dateOfBirth)
        {
            return dateOfBirth.Year >= 1000 && dateOfBirth.Year <= 2015;
        }

        public override bool NameValidationRules(string name)
        {
            return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter) && name.Length >= 2 && name.Length <= 30;
        }

        public override bool NumberValidationRules(short number)
        {
            return number > 1000 && number <= 9999;
        }

        public override string ToString()
        {
            return "Custom";
        }

        public override bool TypeValidationRules(char type)
        {
            char[] availableType = { 'A', 'B', 'C', 'D' };
            return availableType.Contains(char.ToUpper(type, CultureInfo.InvariantCulture));
        }
    }
}
