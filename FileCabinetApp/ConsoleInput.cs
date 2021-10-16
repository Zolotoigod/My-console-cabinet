using System;
using System.Globalization;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Class get data from console.
    /// </summary>
    internal class ConsoleInput : IInput
    {
        /// <summary>
        /// Get FirstName from console.
        /// </summary>
        /// <returns>string.</returns>
        string IInput.Input_FirstName()
        {
            Console.Write("FirstName: ");
            string firstname;
            while (true)
            {
                firstname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(firstname) && firstname.All(char.IsLetter) && firstname.Length >= 2 && firstname.Length <= 60)
                {
                    return firstname;
                }
                else
                {
                    Console.WriteLine("Incorrect FirstName");
                    Console.WriteLine("FirstName: ");
                }
            }
        }

        /// <summary>
        /// Get LastName from console.
        /// </summary>
        /// <returns>string.</returns>
        string IInput.Input_LastName()
        {
            Console.Write("LastName: ");
            string lastname;
            while (true)
            {
                lastname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(lastname) && lastname.All(char.IsLetter) && lastname.Length >= 2 && lastname.Length <= 60)
                {
                    return lastname;
                }
                else
                {
                    Console.WriteLine("Incorrect LastName");
                    Console.WriteLine("LastName: ");
                }
            }
        }

        /// <summary>
        /// Get DateOfBirth from console.
        /// </summary>
        /// <returns>DateTime.</returns>
        DateTime IInput.Input_DateOfBirth()
        {
            Console.Write("DateOfBirth (month.day.year): ");
            while (true)
            {
                if (DateTime.TryParseExact(Console.ReadLine(), FileCabinetService.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth) && dateOfBirth.Year >= 1950 && dateOfBirth.Year <= DateTime.Today.Year)
                {
                    return dateOfBirth;
                }
                else
                {
                    Console.WriteLine("Incorrect dateOfBirth");
                    Console.Write("DateOfBirth (month.day.year): ");
                }
            }
        }

        /// <summary>
        /// Get Type from console.
        /// </summary>
        /// <returns>char.</returns>
        char IInput.Input_Type()
        {
            Console.Write("Personal account type (A, B, C): ");
            string store;
            while (true)
            {
                store = Console.ReadLine().ToUpper(CultureInfo.InvariantCulture);
                if ((store.StartsWith('A') || store.StartsWith('B') || store.StartsWith('C')) && store.Length == 1)
                {
                    return store[0];
                }
                else
                {
                    Console.WriteLine("Incorrect type of account");
                    Console.Write("Personal account type (A, B, C): ");
                }
            }
        }

        /// <summary>
        /// Get Number from console.
        /// </summary>
        /// <returns>short.</returns>
        short IInput.Input_Number()
        {
            Console.Write("Personal account number (four digits): ");
            while (true)
            {
                if (short.TryParse(Console.ReadLine(), out short number) && number > 0 && number <= 9999)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Incorrect number of account");
                    Console.Write("Personal account number (four digits): ");
                }
            }
        }

        /// <summary>
        /// Get Balance from console.
        /// </summary>
        /// <returns>decimal.</returns>
        decimal IInput.Input_Balance()
        {
            Console.Write("Personal account balance: ");
            while (true)
            {
                if (decimal.TryParse(Console.ReadLine(), out decimal balance) && balance > 0)
                {
                    return balance;
                }
                else
                {
                    Console.WriteLine("Incorrect balance of account");
                    Console.Write("Personal account balance: ");
                }
            }
        }
    }
}
