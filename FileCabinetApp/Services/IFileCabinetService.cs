using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        /// <summary>
        /// Create record in service.
        /// </summary>
        /// <param name="storeage">Record push in service.</param>
        /// <returns>Record ID.</returns>
        public int CreateRecord(InputDataPack storeage);

        /// <summary>
        /// Edit record in service by id.
        /// </summary>
        /// <param name="id"> id of record.</param>
        /// <param name="storage">New record data.</param>
        /// <returns>Message about execute.</returns>
        public string EditRecord(int id, InputDataPack storage);

        /// <summary>
        /// Return all record in servise.
        /// </summary>
        /// <returns>Array of records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Metod returned count of records.
        /// </summary>
        /// <returns>int count.</returns>
        public int GetStat();

        public int GetDeletedRecords();

        /// <summary>
        /// Serch the record in service by firstname, use dictionary.
        /// </summary>
        /// <param name="firstName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName);

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName);

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        public FileCabinetServiceSnapshot MakeSnapshot();

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public string Remove(int id);

        public string Purge();
    }
}
