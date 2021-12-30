using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class DataConverter
    {
        public static Tuple<bool, string, string> NameConvert(string input)
        {
            Tuple<bool, string, string> result = new (
                    input != null,
                    $"Name should be string",
                    input);
            return result;
        }

        public static Tuple<bool, string, DateTime> DateConvert(string input)
        {
            Tuple<bool, string, DateTime> result = new (
                    DateTime.TryParseExact(input, FileCabinetMemoryService.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateOfBirth),
                    $"Correct date format (month.day.year)",
                    dateOfBirth);
            return result;
        }

        public static Tuple<bool, string, char> TypeConvert(string input)
        {
            input ??= string.Empty;
            Tuple<bool, string, char> result = new (
                    input.Length == 1,
                    $"Accont type should be char",
                    input[0]);
            return result;
        }

        public static Tuple<bool, string, short> NumberConvert(string input)
        {
            Tuple<bool, string, short> result = new (
                    short.TryParse(input, out short number),
                    $"Parametr should be number",
                    number);
            return result;
        }

        public static Tuple<bool, string, decimal> BalanceConvert(string input)
        {
            Tuple<bool, string, decimal> result = new (
                    decimal.TryParse(input, out decimal balance),
                    $"Balance should by number",
                    balance);
            return result;
        }
    }
}
