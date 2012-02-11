// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

namespace SimpleOAuth.Generators
{
    /// <summary>
    /// The base interface for all generators for OAuth parameters, excluding the signature.
    /// </summary>
    public interface IGenerator
    {
        string Generate();
    }
}
