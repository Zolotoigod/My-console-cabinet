#pragma warning disable CA1062

namespace FileCabinetApp.Validation.Input
{
    public class ValidatorLibrary
    {
        public ValidatorLibrary(ValidationParameters validationData)
        {
            this.FirstName = new FirstNameValidator(validationData.MinFirstNameLength, validationData.MaxFirstNameLength);
            this.LastName = new LastNameValidator(validationData.MinLastNameLength, validationData.MaxLastNameLength);
            this.DateOfBirth = new DateOfBirthValidator(validationData.DateOfBirthFrom, validationData.DateOfBirthTo);
            this.Type = new TypeValidator(validationData.Types);
            this.Number = new NumberValidator(validationData.NumberMax);
            this.Balance = new BalanceValidator();
        }

        public FirstNameValidator FirstName { get; }

        public LastNameValidator LastName { get; }

        public DateOfBirthValidator DateOfBirth { get; }

        public TypeValidator Type { get; }

        public NumberValidator Number { get; }

        public BalanceValidator Balance { get; }
    }
}
