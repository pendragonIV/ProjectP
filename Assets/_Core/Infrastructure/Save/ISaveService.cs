namespace Things.Core.Infrastructure
{
    /// <summary>
    /// Service interface for persistent data storage.
    /// Implementations can use local files, PlayerPrefs, or cloud storage.
    /// </summary>
    public interface ISaveService : IService
    {
        /// <summary>
        /// Save data with the given key. Overwrites existing data.
        /// </summary>
        void Save<T>(string key, T data);

        /// <summary>
        /// Load data for the given key. Returns default(T) if key doesn't exist.
        /// </summary>
        T Load<T>(string key);

        /// <summary>
        /// Check if data exists for the given key.
        /// </summary>
        bool HasKey(string key);

        /// <summary>
        /// Delete data for the given key.
        /// </summary>
        void Delete(string key);

        /// <summary>
        /// Delete all saved data. Use with caution!
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Flush any pending writes to disk.
        /// </summary>
        void Flush();
    }
}
