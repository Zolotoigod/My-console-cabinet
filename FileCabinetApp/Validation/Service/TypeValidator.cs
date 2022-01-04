using System;
using System.Collections.ObjectModel;

#pragma warning disable CA1062

namespace FileCabinetApp.Validation.Service
{
    public class TypeValidator : IRecordValidator
    {
        private ReadOnlyCollection<char> types;

        public TypeValidator(ReadOnlyCollection<char> types)
        {
            this.types = types;
        }

        public void Validate(InputDataPack parametres)
        {
            if (!this.types.Contains(parametres.Type))
            {
                throw new ArgumentException("incorrect Type");
            }
        }
    }
}
