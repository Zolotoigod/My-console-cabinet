using System;

#pragma warning disable CA1062

namespace FileCabinetApp
{
    public class InputDataPack
    {
        public InputDataPack(DataValidator validator, IInput input)
        {
            Console.Write("FirstName: ");
            this.FirstName = input.ReadInput(DataConverter.NameConvert, validator.FirstNameValidator);
            Console.Write("LastName: ");
            this.LastName = input.ReadInput(DataConverter.NameConvert, validator.FirstNameValidator);
            Console.Write("Date of birth (month.day.year): ");
            this.DateOfBirth = input.ReadInput(DataConverter.DateConvert, validator.DateValidator);
            Console.Write("Personal account type (A, B, C): ");
            this.Type = input.ReadInput(DataConverter.TypeConvert, validator.TypeValidator);
            Console.Write("Personal account number: ");
            this.Number = input.ReadInput(DataConverter.NumberConvert, validator.NumberValidator);
            Console.Write("Account balance: ");
            this.Balance = input.ReadInput(DataConverter.BalanceConvert, validator.BalanceValidator);
        }

        public InputDataPack(string firstname, string lastname, DateTime date, char type, short number, decimal balance)
        {
            this.FirstName = firstname;
            this.LastName = lastname;
            this.DateOfBirth = date;
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
