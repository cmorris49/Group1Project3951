using MySqlConnector;

namespace TournamentManagerAPI.Infrastructure
{
    /// <summary>
    /// Provides helper methods for reading database values from a MySQL data reader.
    /// </summary>
    public static class DbReaderHelpers
    {
        /// <summary>
        /// Reads a required identifier column from a data reader and converts it to string.
        /// </summary>
        /// <param name="reader">The active data reader.</param>
        /// <param name="columnName">The identifier column name.</param>
        /// <returns>The identifier as a string.</returns>
        public static string ReadId(MySqlDataReader reader, string columnName)
        {
            return reader.GetValue(reader.GetOrdinal(columnName)).ToString()!;
        }

        /// <summary>
        /// Reads a nullable identifier column from a data reader and converts it to string when present.
        /// </summary>
        /// <param name="reader">The active data reader.</param>
        /// <param name="columnName">The identifier column name.</param>
        /// <returns>The identifier as a string, or null when the database value is null.</returns>
        public static string? ReadNullableId(MySqlDataReader reader, string columnName)
        {
            int ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? null : reader.GetValue(ordinal).ToString();
        }
    }
}