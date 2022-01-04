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

        public FileCabinetRecord(InputDataPack storage, int listCount)
        {
            this.Id = listCount + 1;
            this.FirstName = storage?.FirstName;
            this.LastName = storage.LastName;
            this.DateOfBirth = storage.DateOfBirth;
            this.Type = storage.Type;
            this.Number = storage.Number;
            this.Balance = storage.Balance;
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
