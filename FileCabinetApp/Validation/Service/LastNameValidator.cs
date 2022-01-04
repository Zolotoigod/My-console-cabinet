using System;

namespace FileCabinetApp.Validation.Service
{
    public class LastNameValidator : IRecordValidator
    {
        private int minLength;
        private int maxLength;

        public LastNameValidator(int min, int max)
        {
            this.minLength = min;
            this.maxLength = max;
        }

        public void Validate(InputDataPack parametres)
        {
            if (parametres?.LastName is null)
            {
                throw new ArgumentNullException(nameof(parametres), "LastName is null!");
            }

            if (parametres.LastName.Length > this.maxLength || parametres.LastName.Length < this.minLength)
            {
                throw new ArgumentException("incorrect LastName");
            }
        }
    }
}
