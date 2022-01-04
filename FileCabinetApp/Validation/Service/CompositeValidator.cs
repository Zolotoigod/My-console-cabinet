using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp.Validation.Service
{
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        public void Validate(InputDataPack parameters)
        {
            foreach (var validator in this.validators)
            {
                validator.Validate(parameters);
            }
        }
    }
}
