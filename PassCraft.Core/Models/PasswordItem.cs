namespace PassCraft.Core.Models
{
    /// <summary>
    /// Represents a single password entry within the tracking log, 
    /// containing its chronological index and the generated string value.
    /// </summary>
    public class PasswordItem
    {
        /// <summary>
        /// Gets or sets the chronological position of the generated password.
        /// <para>
        /// Lower values (starting at 1) represent older entries, while the maximum value 
        /// represents the most recently generated password in the session.
        /// </para>
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the plain text value of the cryptographically generated password.
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
