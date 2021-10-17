using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Inteface for difine basic validation rules.
    /// </summary>
    internal interface IRecordValidator
    {
        /// <summary>
        /// Basic validation rule for string.
        /// </summary>
        /// <param name="parameters">Validation value.</param>
        /// <returns>bool, validation result.</returns>
        internal bool[] ValidateParametres(FileCabinetRecord parameters);

        internal void ValidationNull(FileCabinetRecord store);
    }
}
