// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;

namespace SimpleOAuth.Generators
{
    /// <summary>
    /// Generates an NOnce token, or a token that should only be used once... ever.
    /// </summary>
    public class NonceGenerator : IGenerator
    {
        public string Generate()
        {
            // easiest type of nonce is a guid, right?
            var myGuid = Guid.NewGuid();
            var unseparatedGuid = myGuid.ToString().Replace("-", "");
            return unseparatedGuid;
        }
    }
}
