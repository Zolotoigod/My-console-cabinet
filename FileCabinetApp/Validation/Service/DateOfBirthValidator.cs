using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation.Service
{
    public class DateOfBirthValidator : IRecordValidator
    {
        private DateTime from;
        private DateTime to;

        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        public void Validate(InputDataPack parametres)
        {
            if (parametres?.DateOfBirth is null)
            {
                throw new ArgumentNullException(nameof(parametres), "DateOfBirth is null!");
            }

            if (parametres.DateOfBirth > this.to || parametres.DateOfBirth < this.from)
            {
                throw new ArgumentException("incorrect DateOfBirth");
            }
        }
    }
}
