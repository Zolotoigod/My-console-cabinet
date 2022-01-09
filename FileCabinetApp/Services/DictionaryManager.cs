using System;
using System.Collections.Generic;

#pragma warning disable CA1062
namespace FileCabinetApp
{
    public static class DictionaryManager
    {
        public static void NameDictUpdate<T>(Dictionary<string, List<T>> dictionary, string key, T value)
        {
            if (dictionary.ContainsKey(key.ToUpperInvariant()))
            {
                dictionary[key.ToUpperInvariant()].Add(value);
            }
            else
            {
                dictionary.Add(key.ToUpperInvariant(), new List<T> { value });
            }
        }

        public static void DateDictUpdate<T>(Dictionary<DateTime, List<T>> dictionary, DateTime key, T value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(value);
            }
            else
            {
                dictionary.Add(key, new List<T> { value });
            }
        }

        public static void IdDictUpdate(Dictionary<int, long> dictionary, int key, long value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}
