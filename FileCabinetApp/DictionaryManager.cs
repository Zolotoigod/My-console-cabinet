using System;
using System.Collections.Generic;

#pragma warning disable CA1062
namespace FileCabinetApp
{
    public static class DictionaryManager
    {
        public static void NameDictUpdate(Dictionary<string, List<long>> dictionary, string key, long position)
        {
            if (dictionary.ContainsKey(key.ToUpperInvariant()))
            {
                dictionary[key.ToUpperInvariant()].Add(position);
            }
            else
            {
                dictionary.Add(key.ToUpperInvariant(), new List<long> { position });
            }
        }

        public static void DateDictUpdate(Dictionary<DateTime, List<long>> dictionary, DateTime key, long position)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key].Add(position);
            }
            else
            {
                dictionary.Add(key, new List<long> { position });
            }
        }
    }
}
