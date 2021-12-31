using System;
#pragma warning disable CA1062

namespace FileCabinetApp
{
    /// <summary>
    /// Class gets data from console.
    /// </summary>
    public class ConsoleInput : IInput
    {
        public T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}.\nPlease, repeat your input:\n> ");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}.\nPlease, repeat your input:\n> ");
                    continue;
                }

                Console.WriteLine();
                return value;
            }
            while (true);
        }
    }
}