namespace FileCabinetApp.Validation.Input
{
    public class FirstNameValidator : IInputPackValidator<string>
    {
        private int minLength;
        private int maxLength;

        public FirstNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        public bool IsValid(string data)
        {
            return data?.Length < this.maxLength && data.Length > this.minLength;
        }
    }
}
