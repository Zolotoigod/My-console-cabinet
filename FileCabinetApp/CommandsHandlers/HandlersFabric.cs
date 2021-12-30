namespace FileCabinetApp.CommandHandlers
{
    public class HandlersFabric
    {
        private BaseCommandHandler root;
        private BaseCommandHandler current;

        public HandlersFabric AddNextHandler(BaseCommandHandler handler)
        {
            this.current.SetNext(handler);
            this.current = handler;
            return this;
        }

        public HandlersFabric SetRoot(BaseCommandHandler handler)
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
