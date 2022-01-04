using System;
using FileCabinetApp.Validation;
using FileCabinetApp.Validation.Input;

#pragma warning disable SA1118

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
                $"FirstName should be great than {this.validators.Parameters.MinFirstNameLength} and" +
                $" less than {this.validators.Parameters.MaxFirstNameLength} letters, shouldn't include digits and punctuation");
            return result;
        }

        public Tuple<bool, string> LastNameValidator(string input)
        {
            Tuple<bool, string> result = new (
                this.validators.LastName.IsValid(input),
                $"LastName should be great than {this.validators.Parameters.MinLastNameLength} and" +
                $" less than {this.validators.Parameters.MaxLastNameLength} letters, shouldn't include digits and punctuation");
            return result;
        }

        public Tuple<bool, string> DateValidator(DateTime input)
        {
            Tuple<bool, string> result = new (
                this.validators.DateOfBirth.IsValid(input),
                $"Date should be great than {this.validators.Parameters.DateOfBirthFrom} and" +
                $" less than {this.validators.Parameters.DateOfBirthTo}");
            return result;
        }

        public Tuple<bool, string> TypeValidator(char input)
        {
            Tuple<bool, string> result = new (
                this.validators.Type.IsValid(input),
                $"Type may be {string.Join(", ", this.validators.Parameters.Types)} only");
            return result;
        }

        public Tuple<bool, string> NumberValidator(short input)
        {
            Tuple<bool, string> result = new (
                this.validators.Number.IsValid(input),
                $"Number should be great than 0 and less than {this.validators.Parameters.NumberMax}");
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
