using System;

namespace FileCabinetApp.Validation.Input
{
    public class DateOfBirthValidator : IInputPackValidator<DateTime>
    {
        private DateTime from;
        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public bool IsValid(DateTime data)
        {
            return data < this.to && data > this.from;
        }
    }
}
