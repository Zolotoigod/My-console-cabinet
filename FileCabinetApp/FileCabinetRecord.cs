using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Service record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class whith default parametres.
        /// </summary>
        internal FileCabinetRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecord"/> class whith custom parametres.
        /// </summary>
        /// <param name="firstName">Get firstName.</param>
        /// <param name="lastName">Get lastName.</param>
        /// <param name="dateOfBirth">Get dateOfBirth.</param>
        /// <param name="type">Get type.</param>
        /// <param name="number">Get number.</param>
        /// <param name="balance">Get balance.</param>
        internal FileCabinetRecord(string firstName, string lastName, DateTime dateOfBirth, char type, short number, decimal balance)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.DateOfBirth = dateOfBirth;
            this.Type = type;
            this.Number = number;
            this.Balance = balance;
        }

        /// <summary>
        /// Gets or sets record ID.
        /// </summary>
        /// <value>
        /// Record ID.
        /// </value>
        public int Id { get; set; }

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
