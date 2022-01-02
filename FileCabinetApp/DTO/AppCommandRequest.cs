namespace FileCabinetApp.DTO
{
    public class AppCommandRequest
    {
        public AppCommandRequest(string command, string parametres)
        {
            this.Command = command;
            this.Parametres = parametres;
        }

        public string Command { get; }

        public string Parametres { get; }
    }
}
