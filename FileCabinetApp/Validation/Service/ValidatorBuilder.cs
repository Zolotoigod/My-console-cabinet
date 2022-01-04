using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileCabinetApp.Validation.Service
{
    public class ValidatorBuilder
    {
        private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        public ValidatorBuilder ValidateType(ReadOnlyCollection<char> types)
        {
            this.validators.Add(new TypeValidator(types));
            return this;
        }

        public ValidatorBuilder ValidateNumber(short max)
        {
            this.validators.Add(new NumberValidator(max));
            return this;
        }

        public ValidatorBuilder ValidateBalance(decimal max)
        {
            this.validators.Add(new BalanceValidator(max));
            return this;
        }

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }
    }
}
