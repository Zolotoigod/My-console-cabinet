using System;
using FileCabinetApp.Validation;
using FileCabinetApp.Validation.Input;

namespace FileCabinetApp
{
    public class DataValidator
    {
        private ValidatorLibrary validators;

        public DataValidator()
        {
            this.validators = new ValidatorLibrary(ParametersCreater.GetDefaultParams());
        }

        public DataValidator(int validationIndex)
        {
            if (validationIndex == 1)
            {
                this.validators = new ValidatorLibrary(ParametersCreater.GetCustomParams());
            }
            else
            {
                this.validators = new ValidatorLibrary(ParametersCreater.GetDefaultParams());
            }
        }

        public DataValidator(ValidatorLibrary library)
        {
            this.validators = library;
        }

        public Tuple<bool, string> FirstNameValidator(string input)
        {
            Tuple<bool, string> result = new (
                this.validators.FirstName.IsValid(input),
                "FirstName should be geat then 2 less than 60 letters, shouldn't include digits and punctuation");
            return result;
        }

        public Tuple<bool, string> LastNameValidator(string input)
        {
            Tuple<bool, string> result = new (
                this.validators.LastName.IsValid(input),
                "LastName should be geat then 2 less than 60 letters, shouldn't include digits and punctuation");
            return result;
        }

        public Tuple<bool, string> DateValidator(DateTime input)
        {
            Tuple<bool, string> result = new (
                this.validators.DateOfBirth.IsValid(input),
                $"Date should be grate then 01.01.1950 an less then {DateTime.Today}");
            return result;
        }

        public Tuple<bool, string> TypeValidator(char input)
        {
            Tuple<bool, string> result = new (
                this.validators.Type.IsValid(input),
                "Type may be 'A', 'B' or 'C' only");
            return result;
        }

        public Tuple<bool, string> NumberValidator(short input)
        {
            Tuple<bool, string> result = new (
                this.validators.Number.IsValid(input),
                "Number should be great than 0 and less than 9999");
            return result;
        }

        public Tuple<bool, string> BalanceValidator(decimal input)
        {
            Tuple<bool, string> result = new (
                this.validators.Balance.IsValid(input),
                "Balanse should be great than zero");
            return result;
        }
    }
}
