using System;
using SimpleOAuth;

namespace SimpleOAuthTester.WP.Mango.Classes
{
    public static class TwitterTokensRepository
    {
        public static Tokens Tokens { get; set; }
        public static string OAuthRoot { get { return "https://api.twitter.com/oauth/"; } }
        public static string AuthenticatePath { get { return "authenticate"; } }
        public static string RequestTokenPath { get { return "request_token"; } }
        public static string AccessTokenPath { get { return "access_token"; } }
        public static string ApiRoot { get { return "https://api.twitter.com/1/"; } }

        static TwitterTokensRepository()
        {
            Tokens = new Tokens { ConsumerKey = SecretRepository.ConsumerKey, ConsumerSecret = SecretRepository.ConsumerSecret };
        }
    }
}
