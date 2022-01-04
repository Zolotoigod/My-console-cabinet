namespace FileCabinetApp.Validation.Input
{
    public class LastNameValidator : IInputPackValidator<string>
    {
        private int minLength;
        private int maxLength;

        public LastNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        public bool IsValid(string data)
        {
            return data?.Length > this.maxLength && data.Length < this.minLength;
        }
    }
}
