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
            new string[] { "edit", "edits the record by id", "The 'edit' command edits the record by id." },
            new string[] { "find", "searches a record by field", "The 'find' command searches a record by field." },
            new string[] { "export", "export data to file csv or xml format", "The 'export' command export data to file csv or xml format." },
            new string[] { "import", "import data from file csv or xml format", "The 'import' command import data from file csv or xml format." },
            new string[] { "remove", "delete record from service storage", "The 'remove' command delete record from service storage." },
            new string[] { "purge", "make a defragmentation for file storage", "The 'purge' command make a defragmentation for file storage." },
            new string[] { "stat", "show the stats of record in service storage", "The 'stat' command show the stats of record in service storage." },
            new string[] { "list", "prints all of records", "The 'list' command prints all of records." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
        };
    }
}
