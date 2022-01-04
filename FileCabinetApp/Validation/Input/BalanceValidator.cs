namespace FileCabinetApp.Validation.Input
{
    public class BalanceValidator : IInputPackValidator<decimal>
    {
        private decimal max;

        public BalanceValidator(decimal max)
        {
            this.max = max;
        }

        public bool IsValid(decimal data)
        {
            return data > 0 && data < this.max;
        }
    }
}
