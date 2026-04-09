namespace TournamentManagerAPI.Infrastructure
{
    /// <summary>
    /// Provides helpers for reading authentication-related request headers.
    /// </summary>
    public static class AuthHeaderHelpers
    {
        /// <summary>
        /// Attempts to read the user identifier from the request header used for API authorization.
        /// </summary>
        public static bool TryReadUserId(HttpContext httpContext, out string userId)
        {
            userId = string.Empty;

            if (!httpContext.Request.Headers.TryGetValue("X-User-Id", out var values))
            {
                return false;
            }

            var headerValue = values.ToString().Trim();
            if (string.IsNullOrWhiteSpace(headerValue))
            {
                return false;
            }

            userId = headerValue;
            return true;
        }
    }
}