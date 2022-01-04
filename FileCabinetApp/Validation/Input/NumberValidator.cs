namespace FileCabinetApp.Validation.Input
{
    public class NumberValidator : IInputPackValidator<short>
    {
        private short max;

        public NumberValidator(short max)
        {
            this.max = max;
        }

        public bool IsValid(short data)
        {
            return data < this.max && data > 0;
        }
    }
}
