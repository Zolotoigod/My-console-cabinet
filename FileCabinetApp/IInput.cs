using System;

namespace FileCabinetApp
{
    internal interface IInput
    {
        internal string Input_FirstName();

        internal string Input_LastName();

        internal DateTime Input_DateOfBirth();

        internal char Input_Type();

        internal short Input_Number();

        internal decimal Input_Balance();
    }
}
