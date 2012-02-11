// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.Text;
using System.Security.Cryptography;

namespace SimpleOAuth.Generators
{
    /// <summary>
    /// This class generates HMAC-SHA1 signatures.
    /// </summary>
    public class HmacSha1Generator : ISignatureGenerator
    {

        private UTF8Encoding _encoder = new UTF8Encoding();

        public string Generate(string baseString, string key)
        {

            var hmac = new HMACSHA1(_encoder.GetBytes(key));
            var hash = hmac.ComputeHash(_encoder.GetBytes(baseString));
            return Convert.ToBase64String(hash);

        }
    }
}
