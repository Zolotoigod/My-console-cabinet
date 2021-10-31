using System;
using System.Globalization;
using System.Xml;
using System.IO;

namespace FileCabinetGenerator
{
    class FileCabinetGenerator
    {
        static void Main(string[] args)
        {
            AdditionalComandsMain(args);
        }

        private static void AdditionalComandsMain(string[] args)
        {
            int typeIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--output-type", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--t", StringComparison.InvariantCultureIgnoreCase));
            int filenameIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--output", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--o", StringComparison.InvariantCultureIgnoreCase));
            int ammountIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--records-amount", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--a", StringComparison.InvariantCultureIgnoreCase));
            int idIndex = Array.FindIndex(args, 0, args.Length, match => match.Equals("--start-id", StringComparison.InvariantCultureIgnoreCase) || match.Equals("--i", StringComparison.InvariantCultureIgnoreCase));
            if (CommandsValidator(new int[] { typeIndex, filenameIndex, ammountIndex, idIndex}))
            {
                Console.WriteLine($"{args[ammountIndex + 1]} records were written to {filenameIndex}");
            }

            Console.WriteLine("Unsuported commands");
        }

        public static bool CommandsValidator(int[] comands)
        {
            foreach (var index in comands)
            {
                if (index < 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static DateTime RandomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            Random days = new Random();
            return start.AddDays(days.Next(range));
        }

        
    }
}