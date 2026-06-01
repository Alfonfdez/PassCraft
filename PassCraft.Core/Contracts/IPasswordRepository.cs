namespace PassCraft.Core.Contracts
{
    /// <summary>
    /// Defines the low-level data storage operations for reading and writing 
    /// raw generated password records.
    /// </summary>
    public interface IPasswordRepository
    {
        /// <summary>
        /// Persists a new raw password string into the underlying storage medium.
        /// </summary>
        /// <param name="password">The text password to store.</param>
        void Save(string password);

        /// <summary>
        /// Retrieves all raw password records currently stored in the system.
        /// </summary>
        /// <returns>A read-only collection of password strings in chronological order.</returns>
        IReadOnlyList<string> GetAll();

        /// <summary>
        /// Discards the chronologically oldest record sitting at the front of the collection.
        /// </summary>
        void RemoveOldest();
    }
}
