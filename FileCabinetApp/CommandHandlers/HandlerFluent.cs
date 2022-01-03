namespace FileCabinetApp.CommandHandlers
{
    public class HandlerFluent
    {
        private BaseCommandHandler root;
        private BaseCommandHandler current;

        public HandlerFluent AddNextHandler(BaseCommandHandler handler)
        {
            this.current.SetNext(handler);
            this.current = handler;
            return this;
        }

        public HandlerFluent SetRoot(BaseCommandHandler handler)
        {
            this.root = handler;
            this.current = handler;
            return this;
        }

        public BaseCommandHandler GetRoot()
        {
            return this.root;
        }
    }
}
