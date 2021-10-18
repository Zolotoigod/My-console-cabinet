using System;

namespace FileCabinetApp
{
    public class ValidationData
    {
        internal ValidationData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationData"/> class whith custom parametres.
        /// </summary>
        /// <param name="firstName">Get firstName.</param>
        /// <param name="lastName">Get lastName.</param>
        /// <param name="dateOfBirth">Get dateOfBirth.</param>
        /// <param name="type">Get type.</param>
        /// <param name="number">Get number.</param>
        /// <param name="balance">Get balance.</param>
        internal ValidationData(string firstName, string lastName, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Type = type;
            this.Number = number;
            this.Balance = balance;
        }

        /// <summary>
        /// Gets or sets record FirstName.
        /// </summary>
        /// <value>
        /// Record FirstName.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets record LastName.
        /// </summary>
        /// <value>
        /// Record LastName.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets record DateOfBirth.
        /// </summary>
        /// <value>
        /// Record DateOfBirth.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Type.
        /// </summary>
        /// <value>
        /// Record Personal Account Type.
        /// </value>
        public char Type { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Number.
        /// </summary>
        /// <value>
        /// Record Personal Account Number.
        /// </value>
        public short Number { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Balance.
        /// </summary>
        /// <value>
        /// Record Personal Account Balance.
        /// </value>
        public decimal Balance { get; set; }
    }
}
