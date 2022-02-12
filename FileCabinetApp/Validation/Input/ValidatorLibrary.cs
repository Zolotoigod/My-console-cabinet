#pragma warning disable CA1062

namespace FileCabinetApp.Validation.Input
{
    public class ValidatorLibrary
    {
        public ValidatorLibrary(string validatorName, ValidationParameters validationData)
        {
            this.ValidatorName = validatorName;
            this.Parameters = validationData;
            this.FirstName = new FirstNameValidator(validationData.MinFirstNameLength, validationData.MaxFirstNameLength);
            this.LastName = new LastNameValidator(validationData.MinLastNameLength, validationData.MaxLastNameLength);
            this.DateOfBirth = new DateOfBirthValidator(validationData.DateOfBirthFrom, validationData.DateOfBirthTo);
            this.Type = new TypeValidator(validationData.Types);
            this.Number = new NumberValidator(validationData.NumberMax);
            this.Balance = new BalanceValidator(validationData.BalanceMax);
        }

        public string ValidatorName { get; }

        public ValidationParameters Parameters { get; }

        public FirstNameValidator FirstName { get; }

        public LastNameValidator LastName { get; }

        public DateOfBirthValidator DateOfBirth { get; }

        public TypeValidator Type { get; }

        public NumberValidator Number { get; }

        public BalanceValidator Balance { get; }

        public override string ToString()
        {
            return $"{this.ValidatorName}";
        }
    }
}
