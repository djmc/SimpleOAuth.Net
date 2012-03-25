using System;
using System.Net;
using System.Linq;

namespace SimpleOAuthTester.WP.Mango.Classes
{
    public static class StringExtensions
    {
        /// <summary>
        /// Gets value for query string key
        /// </summary>
        /// <param name="queryString">Query string</param>
        /// <param name="key">Query string key</param>
        /// <returns>Returns value or null if not found</returns>
        public static string QueryStringValue(this string queryString, string key)
        {
            if (queryString == null)
                return null;

            if (key.IsEmpty())
                throw new ArgumentException("Query string key cannot be null.", "key");

            if (queryString.Contains('?'))
            {
                var newIndex = queryString.IndexOf('?') + 1;

                if (queryString.Length <= newIndex)
                    return null;

                queryString = queryString.Substring(newIndex);
            }

            string[] parts = queryString.Split('&');
            var match = parts
                .Where(x => x.Contains("=")
                    && x.StartsWith(key + "=", StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            if (match == null)
                return null;

            parts = match.Split('=');
            if (parts.Length < 2)
                return null;

            var finalValue = HttpUtility.UrlDecode(parts[1]);
            return finalValue;
        }

        public static bool IsEmpty(this string input)
        {
            return String.IsNullOrEmpty(input) || input.Trim().Length == 0;
        }

        public static bool HasValue(this string input)
        {
            return !IsEmpty(input);
        }
    }
}
