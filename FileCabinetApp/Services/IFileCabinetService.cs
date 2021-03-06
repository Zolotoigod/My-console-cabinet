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
        public void EditRecord(int id, InputDataPack storage);

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

        public int GetDeletedRecords();

        /// <summary>
        /// Serch the record in service by firstname, use dictionary.
        /// </summary>
        /// <param name="firstName">Parametr for search.</param>
        /// <returns>list of FileCabinetRecord.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);

        public FileCabinetServiceSnapshot MakeSnapshot();

        public void Restore(FileCabinetServiceSnapshot snapshot);

        public string Remove(int id);

        public List<int> GetListId();
    }
}
