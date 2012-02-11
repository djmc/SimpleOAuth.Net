// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;

namespace SimpleOAuth
{
    /// <summary>
    /// Contains the Consumer and Access tokens (if required) for signing valid OAuth requests.
    /// </summary>
    public class Tokens
    {

        /// <summary>
        /// The Consumer Key, usually this is provided on your end.
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// The Consumer Secret, usually this is provided on your end.
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// The Access Token, usually this is provided by the OAuth provider.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// The Access Token Secret, usually this is provided by the OAuth provider.
        /// </summary>
        public string AccessTokenSecret { get; set; }

        /// <summary>
        /// Merge this set of tokens with another set of tokens. 
        /// </summary>
        /// <param name="newTokens">The new tokens to merge with.</param>
        /// <remarks>Any token that isn't an empty string in <paramref name="newTokens"/> 
        /// will replace the token in this <see cref="Tokens"/> object. Useful for when you have
        /// a new set of access tokens.</remarks>
        public void MergeWith(Tokens newTokens)
        {
            if (!String.IsNullOrEmpty(newTokens.ConsumerKey))
            {
                this.ConsumerKey = newTokens.ConsumerKey;
            }

            if (!String.IsNullOrEmpty(newTokens.ConsumerSecret))
            {
                this.ConsumerSecret = newTokens.ConsumerSecret;
            }

            if (!String.IsNullOrEmpty(newTokens.AccessToken))
            {
                this.AccessToken = newTokens.AccessToken;
            }

            if (!String.IsNullOrEmpty(newTokens.AccessTokenSecret))
            {
                this.AccessTokenSecret = newTokens.AccessTokenSecret;
            }
        }
    }
}
