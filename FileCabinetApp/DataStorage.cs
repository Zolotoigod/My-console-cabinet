using System;

namespace FileCabinetApp
{
    public class DataStorage
    {
        public DataStorage(BaseValidationRules validationRules)
        {
            IInput input = new ConsoleInput();
            DataValidator validator = new (validationRules);
            Console.Write("FirstName: ");
            this.FirstName = input.ReadInput(DataConverter.NameConvert, validator.NameValidator);
            Console.Write("LastName: ");
            this.LastName = input.ReadInput(DataConverter.NameConvert, validator.NameValidator);
            Console.Write("Date of birth (month.day.year): ");
            this.DateOfBirth = input.ReadInput(DataConverter.DateConvert, validator.DateValidator);
            Console.Write("Personal account type (A, B, C): ");
            this.Type = input.ReadInput(DataConverter.TypeConvert, validator.TypeValidator);
            Console.Write("Personal account number: ");
            this.Number = input.ReadInput(DataConverter.NumberConvert, validator.NumberValidator);
            Console.Write("Account balance: ");
            this.Balance = input.ReadInput(DataConverter.BalanceConvert, validator.BalanceValidator);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataStorage"/> class whith custom parametres.
        /// </summary>
        /// <param name="firstName">Get firstName.</param>
        /// <param name="lastName">Get lastName.</param>
        /// <param name="dateOfBirth">Get dateOfBirth.</param>
        /// <param name="type">Get type.</param>
        /// <param name="number">Get number.</param>
        /// <param name="balance">Get balance.</param>
        public DataStorage(string firstName, string lastName, DateTime dateOfBirth, char type, short number, decimal balance)
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
