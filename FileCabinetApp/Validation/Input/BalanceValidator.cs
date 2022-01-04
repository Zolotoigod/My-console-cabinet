namespace FileCabinetApp.Validation.Input
{
    public class BalanceValidator : IInputPackValidator<decimal>
    {
        public bool IsValid(decimal data)
        {
            return data > 0;
        }
    }
}
