﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Service.
    /// </summary>
    public class FileCabinetService : IFileCabinetService
    {
        /// <summary>
        /// Field set the dateformat.
        /// </summary>
        internal static readonly string[] DateFormat = { "MM dd yyyy", "MM/dd/yyyy", "MM.dd.yyyy", "MM,dd,yyyy", "dd MM yyyy", "dd/MM/yyyy", "dd.MM.yyyy", "dd,MM,yyyy" };
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private readonly string[] validationComands = { "--validation-rules", "--v" };
        private IRecordValidator validator;

        /// <summary>
        /// Create record in service.
        /// </summary>
        /// <param name="store">Record push in service.</param>
        /// <returns>Record ID.</returns>
        public int CreateRecord(ValidationData store)
        {
            this.validator.ValidationNull(store);
            bool[] rules = this.validator.ValidateParametres(store);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = rules[0] ? store?.FirstName : throw new ArgumentException("incorrect FirstName"),
                LastName = rules[1] ? store.LastName : throw new ArgumentException("incorrect FirstName"),
                DateOfBirth = rules[2] ? store.DateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date"),
                Type = rules[3] ? store.Type : throw new ArgumentException("Type can be A, B, C only"),
                Number = rules[4] ? store.Number : throw new ArgumentException("Number should be more than 0 end less than 9999"),
                Balance = rules[5] ? store.Balance : throw new ArgumentException("Balance can't be less than zero"),
            };

            this.list.Add(record);
            this.UpdateDictionaries(store.FirstName, store.LastName, store.DateOfBirth, record);
            return record.Id;
        }

        /// <summary>
        /// Edit record in service by id.
        /// </summary>
        /// <param name="id"> id of record.</param>
        /// <param name="store">New record data.</param>
        public void EditRecord(int id, ValidationData store)
        {
            var record = this.list[id - 1];

            this.validator.ValidationNull(store);
            bool[] rules = this.validator.ValidateParametres(store);
            this.firstNameDictionary.Remove(record.FirstName.ToUpperInvariant());
            this.lastNameDictionary.Remove(record.LastName.ToUpperInvariant());
            this.dateOfBirthDictionary.Remove(record.DateOfBirth);

            record.FirstName = rules[0] ? store?.FirstName : throw new ArgumentException("incorrect FirstName");
            record.LastName = rules[1] ? store.LastName : throw new ArgumentException("incorrect FirstName");
            record.DateOfBirth = rules[2] ? store.DateOfBirth : throw new ArgumentException("Year of birth should be more than 1950 end less than current date");
            record.Type = rules[3] ? store.Type : throw new ArgumentException("Type can be A, B, C only");
            record.Number = rules[4] ? store.Number : throw new ArgumentException("Number should be more than 0 end less than 9999");
            record.Balance = rules[5] ? store.Balance : throw new ArgumentException("Balance can't be less than zero");

            this.UpdateDictionaries(store.FirstName, store.LastName, store.DateOfBirth, record);
        }

        /// <summary>
        /// Return all record in servise.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return this.list.AsReadOnly();
        }

        /// <summary>
        /// Metod returned count of records.
        /// </summary>
        /// <returns>int count.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Serch the record in service by firstname, use dictionary.
        /// </summary>
        /// <param name="firstName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName?.ToUpperInvariant()))
            {
                return this.firstNameDictionary[firstName.ToUpperInvariant()].AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        /// <summary>
        /// Serch the record in service by Lastname, use dictionary.
        /// </summary>
        /// <param name="lastName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName?.ToUpperInvariant()))
            {
                return this.lastNameDictionary[lastName.ToUpperInvariant()].AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        /// <summary>
        /// Serch the record in service by DateOfBirth, use dictionary.
        /// </summary>
        /// <param name="dateOfBirth">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth)
        {
            if (!DateTime.TryParseExact(dateOfBirth, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate))
            {
                return null;
            }

            if (this.dateOfBirthDictionary.ContainsKey(resultDate))
            {
                return this.dateOfBirthDictionary[resultDate].AsReadOnly();
            }
            else
            {
                return new List<FileCabinetRecord>().AsReadOnly();
            }
        }

        /// <summary>
        /// Create validator with given validatin rules.
        /// </summary>
        /// <param name="args">input data.</param>
        /// /// <returns>validaor.</returns>
        internal IRecordValidator CreateValidator(string[] args)
        {
            if (args == null || args.Length == 0 || string.IsNullOrEmpty(args[0]))
            {
                this.validator = new DefaultValidator();
                return new DefaultValidator();
            }

            string[] validator = args[0].Split(new char[] { '=', ' ' }, 2);

            if (Array.IndexOf(this.validationComands, validator[0].ToLowerInvariant()) >= 0)
            {
                switch (validator[1].ToLowerInvariant())
                {
                    case "default":
                        {
                            this.validator = new DefaultValidator();
                            return new DefaultValidator();
                        }

                    case "custom":
                        {
                            this.validator = new CustomValidator();
                            return new CustomValidator();
                        }

                    default:
                        {
                            this.validator = new DefaultValidator();
                            Console.WriteLine($"Validator {validator[1]} unsupported");
                            return new DefaultValidator();
                        }
                }
            }
            else
            {
                this.validator = new DefaultValidator();
                Console.WriteLine($"command {validator[0]} unsupported");
                return new DefaultValidator();
            }
        }

        /// <summary>
        /// Update dictionaries.
        /// </summary>
        /// <param name="firstName">set firstName.</param>
        /// <param name="lastName">set lastName.</param>
        /// <param name="dateOfBirth">set dateOfBirth.</param>
        /// <param name="record">record for udate.</param>
        private void UpdateDictionaries(string firstName, string lastName, DateTime dateOfBirth, FileCabinetRecord record)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpperInvariant()))
            {
                this.firstNameDictionary[firstName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(firstName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(lastName.ToUpperInvariant()))
            {
                this.lastNameDictionary[lastName.ToUpperInvariant()].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(lastName.ToUpperInvariant(), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(dateOfBirth, new List<FileCabinetRecord> { record });
            }
        }
    }
}
