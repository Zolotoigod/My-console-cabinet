﻿using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    internal interface IFileCabinetService
    {
        /// <summary>
        /// Create record in service.
        /// </summary>
        /// <param name="store">Record push in service.</param>
        /// <returns>Record ID.</returns>
        public int CreateRecord(ValidationData store);

        /// <summary>
        /// Edit record in service by id.
        /// </summary>
        /// <param name="id"> id of record.</param>
        /// <param name="store">New record data.</param>
        public void EditRecord(int id, ValidationData store);

        /// <summary>
        /// Return all record in servise.
        /// </summary>
        /// <returns>Array of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Metod returned count of records.
        /// </summary>
        /// <returns>int count.</returns>
        public int GetStat();

        /// <summary>
        /// Serch the record in service by firstname, use dictionary.
        /// </summary>
        /// <param name="firstName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);
    }
}