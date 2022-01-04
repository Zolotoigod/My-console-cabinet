using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Inteface for difine basic validation rules.
    /// </summary>
    public abstract class BaseValidationRules
    {
        public static void ValidationNull(InputDataPack data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data.FirstName), "Record is null");
            }

            if (string.IsNullOrWhiteSpace(data.FirstName))
            {
                throw new ArgumentNullException(nameof(data.FirstName), "FirstName can't be null");
            }

            if (string.IsNullOrWhiteSpace(data.LastName))
            {
                throw new ArgumentNullException(nameof(data.LastName), "LastName can't be null");
            }
        }

        public abstract bool NameValidationRules(string name);

        public abstract bool DateValidationRules(DateTime dateOfBirth);

        public abstract bool TypeValidationRules(char type);

        public abstract bool NumberValidationRules(short number);

        public abstract bool BalanceValidationRules(decimal balance);
    }
}
