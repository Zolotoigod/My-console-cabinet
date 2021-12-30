using System;

namespace FileCabinetApp
{
    public interface ICommandHandler
    {
        public string MyCommand { get; }

        public void HandleCommand(string command, string data);
    }
}
