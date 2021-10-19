using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Inteface for define the basiс input rules.
    /// </summary>
    public interface IInput
    {
        public T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator);
    }
}
