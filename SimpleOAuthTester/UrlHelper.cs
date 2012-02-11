using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace SimpleOAuthTester
{
    /// <summary>
    /// URL encoding class.  Note: use at your own risk.
    /// Written by: Ian Hopkins (http://www.lucidhelix.com)
    /// Date: 2008-Dec-23
    /// (Ported to C# by t3rse (http://www.t3rse.com))
    /// Source: http://stackoverflow.com/questions/14731/urlencode-through-a-console-application
    /// </summary>
    public class UrlHelper
    {
        public static string Encode(string str)
        {
            var charClass = String.Format("0-9a-zA-Z{0}", Regex.Escape("-_.!~*'()"));
            return Regex.Replace(str,
                String.Format("[^{0}]", charClass),
                new MatchEvaluator(EncodeEvaluator));
        }

        public static string EncodeEvaluator(Match match)
        {
            return (match.Value == " ") ? "+" : String.Format("%{0:X2}", Convert.ToInt32(match.Value[0]));
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
