using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Inteface for define the basiс input rules.
    /// </summary>
    internal interface IInput
    {
        /// <summary>
        /// rule for input FirstName.
        /// </summary>
        /// <returns>string.</returns>
        internal string Input_FirstName();

        /// <summary>
        /// rule for input LastName.
        /// </summary>
        /// <returns>string.</returns>
        internal string Input_LastName();

        /// <summary>
        /// rule for input DateOfBirth.
        /// </summary>
        /// <returns>DateTime.</returns>
        internal DateTime Input_DateOfBirth();

        /// <summary>
        /// rule for input Type.
        /// </summary>
        /// <returns>char.</returns>
        internal char Input_Type();

        /// <summary>
        /// rule for input Number.
        /// </summary>
        /// <returns>short.</returns>
        internal short Input_Number();

        /// <summary>
        /// rule for input Balance.
        /// </summary>
        /// <returns>decimal.</returns>
        internal decimal Input_Balance();
    }
}
