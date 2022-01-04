using System;

namespace FileCabinetApp.Validation.Service
{
    public class BalanceValidator : IRecordValidator
    {
        private decimal max;

        public BalanceValidator(decimal max)
        {
            this.max = max;
        }

        public void Validate(InputDataPack parametres)
        {
            if (parametres?.Balance < 0 || parametres.Balance > this.max)
            {
                throw new ArgumentException("incorrect Balance");
            }
        }
    }
}
