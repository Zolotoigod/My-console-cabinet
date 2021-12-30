using System;
using System.Collections.Generic;

namespace FileCabinetApp.DTO
{
    public static class HelpMessage
    {
        public const int CommandHelpIndex = 0;
        public const int DescriptionHelpIndex = 1;
        public const int ExplanationHelpIndex = 2;

        public static readonly string[][] Messages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates a record", "The 'create' command creates a record." },
            new string[] { "edit", "edits the record by id", "The 'edit' edits the record by id." },
            new string[] { "find", "searches a record by field", "The 'find' searches a record by field." },
            new string[] { "list", "prints all of records", "The 'list' command prints all of records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };
    }
}
