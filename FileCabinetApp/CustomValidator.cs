using System;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator vith custom parameters.
    /// </summary>
    internal class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validate data.
        /// </summary>
        /// <param name="store">validate parameters.</param>
        /// <returns>Return array of validate results.</returns>
        bool[] IRecordValidator.ValidateParametres(ValidationData store)
        {
            bool[] validationResult = new bool[6];
            validationResult[0] = !string.IsNullOrWhiteSpace(store.FirstName) && store.FirstName.All(char.IsLetter) && store.FirstName.Length >= 2 && store.FirstName.Length <= 30;
            validationResult[1] = !string.IsNullOrWhiteSpace(store.LastName) && store.LastName.All(char.IsLetter) && store.LastName.Length >= 2 && store.LastName.Length <= 30;
            validationResult[2] = store.DateOfBirth.Year >= 1000 && store.DateOfBirth.Year <= DateTime.Today.Year;
            validationResult[3] = char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('A') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('B') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('C') || char.ToUpper(store.Type, CultureInfo.InvariantCulture).Equals('F');
            validationResult[4] = store.Number > 1000 && store.Number <= 9999;
            validationResult[5] = store.Balance > 0;
            return validationResult;
        }

        void IRecordValidator.ValidationNull(ValidationData store)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store.FirstName), "Record is null");
            }

            if (string.IsNullOrWhiteSpace(store.FirstName))
            {
                throw new ArgumentNullException(nameof(store.FirstName), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(store.LastName))
            {
                throw new ArgumentNullException(nameof(store.LastName), "LastName can't be null");
            }
        }

        public override string ToString()
        {
            return "custom";
        }
    }
}
