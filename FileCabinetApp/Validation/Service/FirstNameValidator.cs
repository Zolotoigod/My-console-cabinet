using System;

namespace FileCabinetApp.Validation.Service
{
    public class FirstNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public FirstNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        public void Validate(InputDataPack parametres)
        {
            if (parametres?.FirstName is null)
            {
                throw new ArgumentNullException(nameof(parametres), "FirstName is null!");
            }

            if (parametres.FirstName.Length > this.maxLength || parametres.FirstName.Length < this.minLength)
            {
                throw new ArgumentException("incorrect FirstName");
            }
        }
    }
}
