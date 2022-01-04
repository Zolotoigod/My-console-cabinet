using System.Collections.ObjectModel;

namespace FileCabinetApp.Validation.Input
{
    public class TypeValidator : IInputPackValidator<char>
    {
        private ReadOnlyCollection<char> types;

        public TypeValidator(ReadOnlyCollection<char> types)
        {
            this.types = types;
        }

        public bool IsValid(char data)
        {
            return this.types.Contains(data);
        }
    }
}
