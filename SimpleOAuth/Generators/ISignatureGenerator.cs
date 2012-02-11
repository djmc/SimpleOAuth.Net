// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

namespace SimpleOAuth.Generators
{
    /// <summary>
    /// The interface for all signatures.
    /// </summary>
    interface ISignatureGenerator
    {
        string Generate(string baseString, string key);
    }
}
