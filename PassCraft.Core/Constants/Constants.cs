namespace PassCraft.Core.Constants
{
    /// <summary>
    /// The central configuration vault for all immutable application constants.
    /// </summary>
    public static class Constants
    {
        public static string DefaultSymbols => CharacterPools.DefaultSymbols;

        /// <summary>
        /// Contains character sets utilized by the cryptographic generation engine.
        /// </summary>
        public static class CharacterPools
        {
            public const string DefaultSymbols = "!@#$%^&*()-_=+[]{};:,.<>/?";
            public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
            public const string Numbers = "0123456789";
        }

        /// <summary>
        /// Contains core application configuration constraints.
        /// </summary>
        public static class SecuritySettings
        {
            public const int MaxHistoryRecords = 99;
            public const int DefaultPasswordLength = 15;
        }

        /// <summary>
        /// Contains reusable layout configuration metrics for UI rendering.
        /// </summary>
        public static class UIConfig
        {
            public const string CopiedToastMessage = "Password copied to clipboard!";
            public const string HistoryCopiedToastTemplate = "Copied password #{0}!";
            public const int ToastFontSize = 14;
        }

        /// <summary>
        /// Contains centralized string routing keys used by the <see cref="Shell"/> 
        /// navigation engine to transition between application pages.
        /// </summary>
        public static class NavigationRoutes
        {
            /// <summary>
            /// The absolute navigation route path linking directly to the password history tracking view.
            /// </summary>
            public const string HistoryPage = "history";
        }
    }
}