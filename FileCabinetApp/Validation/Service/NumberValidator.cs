using System;

namespace FileCabinetApp.Validation.Service
{
    public class NumberValidator : IRecordValidator
    {
        private short max;

        public NumberValidator(short max)
        {
            this.max = max;
        }

        public void Validate(InputDataPack parametres)
        {
            if (parametres?.Number > this.max || parametres.Number < 0)
            {
                throw new ArgumentException("incorrect Number");
            }
        }
    }
}
