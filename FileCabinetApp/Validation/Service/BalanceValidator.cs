using System;

namespace FileCabinetApp.Validation.Service
{
    public class BalanceValidator : IRecordValidator
    {
        public void Validate(InputDataPack parametres)
        {
            if (parametres?.Balance < 0)
            {
                throw new ArgumentException("incorrect Balance");
            }
        }
    }
}
