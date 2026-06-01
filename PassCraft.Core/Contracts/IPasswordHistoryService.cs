using PassCraft.Core.Models;

namespace PassCraft.Core.Contracts
{
    /// <summary>
    /// Defines the operations for tracking and managing the lifetime history 
    /// of generated passwords within the application session.
    /// </summary>
    public interface IPasswordHistoryService
    {
        /// <summary>
        /// Registers a newly generated password string into the underlying historical track log.
        /// </summary>
        /// <param name="password">The cleartext password string to append to history.</param>
        void AddPassword(string password);

        /// <summary>
        /// Retrieves the complete list of generated passwords, indexed chronologically 
        /// from oldest (Index 1) to newest (Index n), and sorted with the newest entries first.
        /// </summary>
        /// <returns>A reversed collection of <see cref="PasswordItem"/> objects optimized for visual UI list streaming.</returns>
        List<PasswordItem> GetHistory();
    }
}
