// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleOAuth.Utilities;

namespace SimpleOAuth
{
    /// <summary>
    /// Contains all extensions required to kick this magic off.
    /// </summary>
    public static class Extensions
    {
        #region WebRequest Extensions

        /// <summary>
        /// Begin signing this <see cref="WebRequest"/> object with OAuth.
        /// </summary>
        /// <param name="request">The request that needs to be signed with OAuth.</param>
        /// <returns>An <see cref="OAuthRequestWrapper"/> used to provide the required parameters for OAuth signing.</returns>
        public static OAuthRequestWrapper SignRequest(this WebRequest request)
        {
            return new OAuthRequestWrapper(request);
        }

        /// <summary>
        /// Begin signing this <see cref="WebRequest"/> object with OAuth using the tokens provided.
        /// </summary>
        /// <param name="request">The request that needs to be signed with OAuth.</param>
        /// <param name="withTokens">The <see cref="Tokens"/> to use to sign the request with.</param>
        /// <returns>An <see cref="OAuthRequestWrapper"/> used to provide the required parameters for OAuth signing.</returns>
        public static OAuthRequestWrapper SignRequest(this WebRequest request, Tokens withTokens)
        {
            return new OAuthRequestWrapper(request) { RequestTokens = withTokens };
        }

        /// <summary>
        /// For the Request and Access Token stages, makes the request and parses out the OAuth tokens from
        /// the server.
        /// </summary>
        /// <param name="request">The request that needs to be signed with OAuth.</param>
        /// <returns>A <see cref="Tokens"/> object containing the Access Token and Access Secret provided by the OAuth server.</returns>
        /// <remarks>You typically call this when making a request to get the users access tokens and combine this with the <see cref="Tokens.MergeWith"/> function.</remarks>
        /// <exception cref="WebException">Thrown when the <paramref name="request"/> encounters an error.</exception>
        public static Tokens GetOAuthTokens(this WebRequest request)
        {
            var newTokens = new Tokens();

            var output = string.Empty;
            using (var responseStream = request.GetResponse())
            {
                using (var response = new StreamReader(responseStream.GetResponseStream()))
                {
                    output = response.ReadToEnd();
                }
            }

            if (!String.IsNullOrEmpty(output))
            {
                var dataValues = UrlHelper.ParseQueryString(output);

                newTokens.AccessToken = dataValues["oauth_token"];
                newTokens.AccessTokenSecret = dataValues["oauth_token_secret"];
            }

            return newTokens;
        }

        /// <summary>
        /// For the Request and Access Token stages, makes the request asynchronously and parses out the OAuth tokens from
        /// the server.
        /// </summary>
        /// <param name="request">The request that needs to be signed with OAuth.</param>
        /// <returns>A <see cref="Tokens"/> object containing the Access Token and Access Secret provided by the OAuth server.</returns>
        /// <remarks>You typically call this when making a request to get the users access tokens and combine this with the <see cref="Tokens.MergeWith"/> function.</remarks>
        /// <exception cref="WebException">Thrown when the <paramref name="request"/> encounters an error.</exception>
        public static async Task<Tokens> GetOAuthTokensAsync(this WebRequest request)
        {
            var newTokens = new Tokens();

            using (var response = await request.GetResponseAsync())
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var output = await reader.ReadToEndAsync();

                if (!String.IsNullOrEmpty(output))
                {
                    var dataValues = UrlHelper.ParseQueryString(output);

                    if (!dataValues.ContainsKey("oauth_token")
                        || !dataValues.ContainsKey("oauth_token_secret"))
                    {
                        var ex = new InvalidOperationException(
                            "Response did not contain oauth_token and oauth_token_secret.");
                        ex.Data.Add("ResponseText", output);
                        ex.Data.Add("RequestUri", request.RequestUri);
                        ex.Data.Add("RequestMethod", request.Method);
                        throw ex;
                    }

                    newTokens.AccessToken = dataValues["oauth_token"];
                    newTokens.AccessTokenSecret = dataValues["oauth_token_secret"];
                }
            }

            return newTokens;
        }

        #endregion

        #region HttpRequestMessage Extensions

        /// <summary>
        /// Begin signing this <see cref="HttpRequestMessage"/> with OAuth.
        /// </summary>
        /// <param name="request">The HTTP request message that needs to be signed with OAuth.</param>
        /// <returns>An <see cref="OAuthRequestWrapper"/> used to provide the required parameters for OAuth signing.</returns>
        public static OAuthRequestWrapper SignRequest(this HttpRequestMessage request)
        {
            return new OAuthRequestWrapper(request);
        }

        /// <summary>
        /// Begin signing this <see cref="HttpRequestMessage"/> with OAuth using the tokens provided.
        /// </summary>
        /// <param name="request">The HTTP request message that needs to be signed with OAuth.</param>
        /// <param name="withTokens">The <see cref="Tokens"/> to use to sign the request with.</param>
        /// <returns>An <see cref="OAuthRequestWrapper"/> used to provide the required parameters for OAuth signing.</returns>
        public static OAuthRequestWrapper SignRequest(this HttpRequestMessage request, Tokens withTokens)
        {
            return new OAuthRequestWrapper(request) { RequestTokens = withTokens };
        }

        /// <summary>
        /// Sends the signed request using the provided <see cref="HttpClient"/> and parses out the OAuth tokens
        /// from the server response.
        /// </summary>
        /// <param name="request">The signed HTTP request message.</param>
        /// <param name="client">The <see cref="HttpClient"/> to send the request with.</param>
        /// <returns>A <see cref="Tokens"/> object containing the Access Token and Access Secret provided by the OAuth server.</returns>
        /// <exception cref="HttpRequestException">Thrown when the request encounters an error.</exception>
        public static async Task<Tokens> GetOAuthTokensAsync(this HttpRequestMessage request, HttpClient client)
        {
            var newTokens = new Tokens();

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var output = await response.Content.ReadAsStringAsync();

                if (!String.IsNullOrEmpty(output))
                {
                    var dataValues = UrlHelper.ParseQueryString(output);

                    if (!dataValues.ContainsKey("oauth_token")
                        || !dataValues.ContainsKey("oauth_token_secret"))
                    {
                        var ex = new InvalidOperationException(
                            "Response did not contain oauth_token and oauth_token_secret.");
                        ex.Data.Add("ResponseText", output);
                        ex.Data.Add("RequestUri", request.RequestUri);
                        ex.Data.Add("RequestMethod", request.Method);
                        throw ex;
                    }

                    newTokens.AccessToken = dataValues["oauth_token"];
                    newTokens.AccessTokenSecret = dataValues["oauth_token_secret"];
                }
            }

            return newTokens;
        }

        #endregion
    }
}
