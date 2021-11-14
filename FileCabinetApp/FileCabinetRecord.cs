using System;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Service record.
    /// </summary>
    [Serializable]
    [XmlRoot]
    public class FileCabinetRecord
    {
        public FileCabinetRecord()
        {
        }

        public FileCabinetRecord(DataStorage storage, BaseValidationRules validationRules, int listCount)
        {
            this.Id = listCount + 1;
            BaseValidationRules.ValidationNull(storage);
            validationRules ??= new DefaultValidateRules();
            this.FirstName = validationRules.NameValidationRules(storage.FirstName) ? storage?.FirstName : throw new ArgumentException("incorrect FirstName");
            this.LastName = validationRules.NameValidationRules(storage.LastName) ? storage.LastName : throw new ArgumentException("incorrect FirstName");
            this.DateOfBirth = validationRules.DateValidationRules(storage.DateOfBirth) ? storage.DateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date");
            this.Type = validationRules.TypeValidationRules(storage.Type) ? storage.Type : throw new ArgumentException("Type can be A, B, C only");
            this.Number = validationRules.NumberValidationRules(storage.Number) ? storage.Number : throw new ArgumentException("Number should be more than 0 end less than 9999");
            this.Balance = validationRules.BalanceValidationRules(storage.Balance) ? storage.Balance : throw new ArgumentException("Balance can't be less than zero");
        }

        [XmlIgnore]
        public short IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets record ID.
        /// </summary>
        /// <value>
        /// Record ID.
        /// </value>
        [XmlElement]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets record FirstName.
        /// </summary>
        /// <value>
        /// Record FirstName.
        /// </value>
        [XmlElement]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets record LastName.
        /// </summary>
        /// <value>
        /// Record LastName.
        /// </value>
        [XmlElement]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets record DateOfBirth.
        /// </summary>
        /// <value>
        /// Record DateOfBirth.
        /// </value>
        [XmlElement(DataType = "date")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Type.
        /// </summary>
        /// <value>
        /// Record Personal Account Type.
        /// </value>
        [XmlElement]
        public char Type { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Number.
        /// </summary>
        /// <value>
        /// Record Personal Account Number.
        /// </value>
        [XmlElement]
        public short Number { get; set; }

        /// <summary>
        /// Gets or sets record Personal Account Balance.
        /// </summary>
        /// <value>
        /// Record Personal Account Balance.
        /// </value>
        [XmlElement]
        public decimal Balance { get; set; }

        public override string ToString()
        {
            return "Record";
        }
    }
}
