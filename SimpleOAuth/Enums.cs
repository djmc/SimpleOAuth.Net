// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleOAuth.Internal;
using System.ComponentModel;

namespace SimpleOAuth
{
    /// <summary>
    /// The types of Encryption that can be performed to generate an oauth_signature.
    /// </summary>
    public enum EncryptionMethod
    {
        /// <summary>
        /// PLAINTEXT
        /// </summary>
        [Description("PLAINTEXT")]
        Plain,

        /// <summary>
        /// HMAC-SHA1
        /// </summary>
        [SignatureType(typeof(Generators.HmacSha1Generator))]
        [Description("HMAC-SHA1")]
        HMACSHA1
    }
}
