// Simple OAuth .Net
// (c) 2012 Daniel McKenzie
// Simple OAuth .Net may be freely distributed under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using SimpleOAuth.Generators;
using SimpleOAuth.Utilities;
using SimpleOAuth.Internal;
using System.ComponentModel;
using System.IO;

namespace SimpleOAuth
{
    /// <summary>
    /// Contains a <see cref="WebRequest"/> and does all the necessary work in order to sign it
    /// as a valid OAuth request before it gets sent.
    /// </summary>
    public sealed class OAuthRequestWrapper
    {
        #region " Static Defaults "
        private static EncryptionMethod _defaultSigningMethod = EncryptionMethod.HMACSHA1;
        /// <summary>
        /// Change the default signing method to use on all future OAuth requests.
        /// </summary>
        public static EncryptionMethod DefaultSigningMethod
        {
            get
            {
                return _defaultSigningMethod;
            }
            set
            {
                _defaultSigningMethod = value;
            }
        }

        #endregion

        #region " Constants "

        private const string DefaultOAuthVersion = "1.0";
        private const string FormUrlEncodedMimeType = "application/x-www-form-urlencoded";

        #endregion

        #region " Properties "

        private WebRequest ContainedRequest { get; set; }

        /// <summary>
        /// The consumer and access keys (if required) to sign the OAuth request.
        /// </summary>
        /// <remarks>Typically, this would be provided with <see cref="OAuthRequestWrapper.WithTokens"/>.</remarks>
        public Tokens RequestTokens { get; set; }

        /// <summary>
        /// The signing method to use. By default it is <see cref="EncryptionMethod.HMACSHA1"/>.
        /// </summary>
        /// <remarks>Typically, this would be provided with <see cref="OAuthRequestWrapper.WithEncryption"/>.</remarks>
        public EncryptionMethod SigningMethod { get; set; }

        /// <summary>
        /// The parameters to include in the OAuth base signature string when doing a POST request. 
        /// </summary>
        /// <remarks>Typically, this would be provided with <see cref="OAuthRequestWrapper.WithPostParameters"/>.</remarks>
        public string PostParameters { get; set; }

        private string _oauthVersion = OAuthRequestWrapper.DefaultOAuthVersion;
        /// <summary>
        /// The OAuth version to use, by default it is 1.0.
        /// </summary>
        public string OAuthVersion { 
            get { 
                return _oauthVersion; 
            } 
            set { 
                _oauthVersion = value; 
            }
        }

        private Dictionary<string, string> _authorizationHeader;
        private Dictionary<string, string> AuthorizationHeader
        {
            get
            {
                if (_authorizationHeader == null)
                {
                    _authorizationHeader = new Dictionary<string, string>();
                }
                return _authorizationHeader;
            }
        }

        #endregion

        #region " Constructor "

        /// <summary>
        /// There is only one constructor, and the OAuthRequestWrapper can only be instantiated internally to the library.
        /// </summary>
        internal OAuthRequestWrapper(WebRequest toContain)
        {
            ContainedRequest = toContain;
            SigningMethod = OAuthRequestWrapper.DefaultSigningMethod;
        }

        #endregion

        #region " Chaining Methods "
        /// <summary>
        /// Set the tokens that should be used for this request.
        /// </summary>
        /// <param name="oauthTokens">A <see cref="Tokens"/> object containing the Consumer and/or Access tokens.</param>
        /// <returns>Itself to chain.</returns>
        /// <remarks>This is equivalent to setting <see cref="OAuthRequestWrapper.RequestTokens"/>.</remarks>
        public OAuthRequestWrapper WithTokens(Tokens oauthTokens)
        {
            RequestTokens = oauthTokens;
            return this;
        }

        /// <summary>
        /// Set the encryption method that should be used for this request.
        /// </summary>
        /// <param name="enc">A <see cref="EncryptionMethod"/></param>
        /// <returns>Itself to chain.</returns>
        /// <remarks>This is equivalent to setting <see cref="OAuthRequestWrapper.SigningMethod"/>. If it is not set,
        /// the default is to use <see cref="EncryptionMethod.HMACSHA1"/>.</remarks>
        public OAuthRequestWrapper WithEncryption(EncryptionMethod enc)
        {
            SigningMethod = enc;
            return this;
        }

        /// <summary>
        /// Set the callback address that the OAuth Request Token stage should use.
        /// </summary>
        /// <param name="callbackUrl">An address or some other string to pass as the OAuth callback.</param>
        /// <remarks>The oauth_callback is sometimes required at the Request Token stage in a three-legged OAuth
        /// login request. It is possible to set this for other request, but may be unexpected and cause issues.</remarks>
        /// <returns>Itself to chain.</returns>
        public OAuthRequestWrapper WithCallback(string callbackUrl)
        {
            AuthorizationHeader.Add("oauth_callback", callbackUrl);
            return this;
        }

        /// <summary>
        /// Set the oauth_verifier in the OAuth Access Token stage.
        /// </summary>
        /// <param name="verifier">The oauth_verifier string used to verify the request.</param>
        /// <returns>Itself to chain.</returns>
        public OAuthRequestWrapper WithVerifier(string verifier)
        {
            AuthorizationHeader.Add("oauth_verifier", verifier);
            return this;
        }

        /// <summary>
        /// Set the version of OAuth being used.
        /// </summary>
        /// <param name="version">The version number to use.</param>
        /// <returns>Itself to chain.</returns>
        /// <remarks>This is equivalent to setting <see cref="OAuthRequestWrapper.OAuthVersion"/>. If this is not set, then "1.0" is used.</remarks>
        public OAuthRequestWrapper WithVersion(string version)
        {
            OAuthVersion = version;
            return this;
        }

        /// <summary>
        /// Provide the POST request to generate a valid OAuth signature.
        /// </summary>
        /// <param name="parameters">The POST parameters that will be sent.</param>
        /// <returns>Itself to chain.</returns>
        /// <remarks>The OAuth standard requires POST parameters sent in application/x-www-form-urlencoded
        /// requests to be included in the OAuth base string to create a valid signature. If it is not provided,
        /// then the request will fail. If this is set, then the <see cref="WebRequest.ContentType"/> of the 
        /// <see cref="OAuthRequestWrapper.ContainedRequest"/> will  be set to 'application/x-www-form-urlencoded' 
        /// if it is not already set.</remarks>
        public OAuthRequestWrapper WithPostParameters(string parameters)
        {
            PostParameters = parameters;

            if (String.IsNullOrEmpty(ContainedRequest.ContentType))
            {
                ContainedRequest.ContentType = FormUrlEncodedMimeType;
            }

            return this;
        }
        #endregion

        #region " Finalisation Methods "
        /// <summary>
        /// Complete the request signing and add the OAuth information to the Authorization header.
        /// </summary>
        /// <remarks>This must be your last step in the function chain.</remarks>
        public void InHeader()
        {
            createAuthorization();
            createSignature();

            var builder = new StringBuilder();
            builder.Append("OAuth ");
            foreach (var pair in AuthorizationHeader)
            {
                if (builder.Length > 6)
                {
                    builder.Append(",");
                }
                builder.AppendFormat("{0}=\"{1}\"", UrlHelper.Encode(pair.Key), UrlHelper.Encode(pair.Value));
            }

            ContainedRequest.Headers["Authorization"] = builder.ToString();
        }
        #endregion

        #region " Private Methods "
        private void createAuthorization()
        {
            AuthorizationHeader.Add("oauth_consumer_key", RequestTokens.ConsumerKey);
            AuthorizationHeader.Add("oauth_nonce", new NonceGenerator().Generate());
            AuthorizationHeader.Add("oauth_signature_method", SigningMethod.GetDescription());
            AuthorizationHeader.Add("oauth_timestamp", new TimestampGenerator().Generate());
            if (!String.IsNullOrEmpty(RequestTokens.AccessToken)) {
                AuthorizationHeader.Add("oauth_token", RequestTokens.AccessToken);
            }
            AuthorizationHeader.Add("oauth_version", OAuthVersion);
        }

        private void createSignature()
        {

            var method = ContainedRequest.Method;
            var baseUrl = String.Format("{0}://{1}{2}", ContainedRequest.RequestUri.Scheme, ContainedRequest.RequestUri.Host, ContainedRequest.RequestUri.AbsolutePath);

            var parameters = new SortedDictionary<string, string>(AuthorizationHeader);
            var queryParams = UrlHelper.ParseQueryString(ContainedRequest.RequestUri.Query);
            foreach (var pair in queryParams)
            {
                parameters.Add(pair.Key, pair.Value);
            }

            if (method.Equals("POST") && !String.IsNullOrEmpty(ContainedRequest.ContentType) && ContainedRequest.ContentType.Equals(FormUrlEncodedMimeType))
            {
                if (!String.IsNullOrEmpty(PostParameters))
                {
                    var postParams = UrlHelper.ParseQueryString(PostParameters);

                    foreach (var pair in postParams)
                    {
                        parameters.Add(pair.Key, pair.Value);
                    }
                }
            }

            var paramString = new StringBuilder();
            foreach (var pair in parameters)
            {
                if (paramString.Length > 0)
                {
                    paramString.Append("&");
                }
                paramString.AppendFormat("{0}={1}", UrlHelper.Encode(pair.Key), UrlHelper.Encode(pair.Value));
            }

            // percent encode everything
            var encodedParams = String.Format("{0}&{1}&{2}", ContainedRequest.Method.ToUpper(), 
                UrlHelper.Encode(baseUrl), UrlHelper.Encode(paramString.ToString()));

            // key
            var key = String.Format("{0}&{1}", UrlHelper.Encode(RequestTokens.ConsumerSecret),
                UrlHelper.Encode(RequestTokens.AccessTokenSecret));

            // signature time!
            string signature = SignatureMethod.CreateSignature(this.SigningMethod, encodedParams.ToString(), key);

            AuthorizationHeader.Add("oauth_signature", signature);
        }
        #endregion
    }
}
