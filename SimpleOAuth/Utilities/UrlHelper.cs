// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace SimpleOAuth.Utilities
{
    /// <summary>
    /// Utility function to correctly percent encode URLs, and parse query strings, without use of System.Web.
    /// </summary>
    public class UrlHelper
    {
        public static string Encode(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return String.Empty;
            }
            var charClass = String.Format("0-9a-zA-Z{0}", Regex.Escape("-._~"));
            return Regex.Replace(str,
                String.Format("[^{0}]", charClass),
                new MatchEvaluator(EncodeEvaluator));
        }

        public static string EncodeEvaluator(Match match)
        {
            return String.Format("%{0:X2}", Convert.ToInt32(match.Value[0]));
        }

        public static string DecodeEvaluator(Match match)
        {
            return Convert.ToChar(int.Parse(match.Value.Substring(1), System.Globalization.NumberStyles.HexNumber)).ToString();
        }

        public static string Decode(string str)
        {
            return Regex.Replace(str.Replace('+', ' '), "%[0-9a-zA-Z][0-9a-zA-Z]", new MatchEvaluator(DecodeEvaluator));
        }

        public static Dictionary<string, string> ParseQueryString(string query)
        {
            var collection = new Dictionary<string, string>();
            var queryParts = query.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var segment in queryParts)
            {
                var segmentParts = segment.Split('=');
                collection.Add(segmentParts[0].Trim(new char[] { '?', ' ' }), UrlHelper.Decode(segmentParts[1].Trim()));
            }

            return collection;
        }
    }
}
