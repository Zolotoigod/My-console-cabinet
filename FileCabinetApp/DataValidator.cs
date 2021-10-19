using System;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    public class DataValidator
    {
        private BaseValidationRules validationRules;

        public DataValidator()
        {
            this.validationRules = new DefaultValidateRules();
        }

        public DataValidator(BaseValidationRules newRules)
        {
            this.validationRules = newRules;
        }

        public Tuple<bool, string> NameValidator(string input)
        {
            Tuple<bool, string> result = new (
                this.validationRules.NameValidationRules(input),
                "Name should be geat then 2 less than 60 letters, shouldn't include digits and punctuation");
            return result;
        }

        public Tuple<bool, string> DateValidator(DateTime input)
        {
            Tuple<bool, string> result = new (
                this.validationRules.DateValidationRules(input),
                $"Date should be grate then 01.01.1950 an less then {DateTime.Today}");
            return result;
        }

        public Tuple<bool, string> TypeValidator(char input)
        {
            Tuple<bool, string> result = new (
                this.validationRules.TypeValidationRules(input),
                "Type may be 'A', 'B' or 'C' only");
            return result;
        }

        public Tuple<bool, string> NumberValidator(short input)
        {
            Tuple<bool, string> result = new (
                this.validationRules.NumberValidationRules(input),
                "Number should be great than 0 and less than 9999");
            return result;
        }

        public Tuple<bool, string> BalanceValidator(decimal input)
        {
            Tuple<bool, string> result = new (
                this.validationRules.BalanceValidationRules(input),
                "Balanse should be great than zero");
            return result;
        }
    }
}
