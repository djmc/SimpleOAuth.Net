using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SimpleOAuth;
using System.Diagnostics;
using System.IO;

/// BEFORE RUNNING THIS, MAKE SURE YOU GO TO http://dev.twitter.com AND PUT IN SOME CONSUMER KEYS
/// BELOW, OTHERWISE NOTHING WILL RUN.
namespace SimpleOAuthTwitter
{
    class Program
    {
        static Tokens RequestTokens { get; set; }

        static void Main(string[] args)
        {

            Console.Out.WriteLine("TWITTER CONSOLE DEMO APP WITH SIMPLEOAUTH");

            RequestTokens = new Tokens() { ConsumerKey = "CONSUMER_KEY", ConsumerSecret = "CONSUMER_SECRET" };

            StartOAuthLogin();

            while (true)
            {
                MakeRequest();
            }
        }

        static void MakeRequest()
        {
            Console.Out.Write("API> ");
            var urlInput = Console.In.ReadLine();
            var request = WebRequest.Create(urlInput);

            Console.Out.Write("METHOD> ");
            var methodInput = Console.In.ReadLine();
            methodInput = methodInput.ToUpper();
            request.Method = methodInput;

            var signingRequest = request.SignRequest(RequestTokens);

            if (methodInput.Equals("POST"))
            {
                Console.Out.Write("POST> ");
                var requestString = Console.In.ReadLine();

                signingRequest.WithPostParameters(requestString).InHeader();

                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(requestString.Replace(' ', '+'));
                    stream.Close();
                }

                request.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                signingRequest.InHeader();
            }

            string twitterResponse = "<< NOTHING >>";
            using (var reader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                twitterResponse = reader.ReadToEnd();
            }

            Console.Out.WriteLine("TWITTER RESPONDED WITH");
            Console.Out.WriteLine(twitterResponse);
        }

        static void StartOAuthLogin()
        {
            var request = WebRequest.Create("https://api.twitter.com/oauth/request_token");

            request.Method = "POST";

            request.SignRequest(RequestTokens)
                .WithCallback("oob")
                .InHeader();

            var accessTokens = request.GetOAuthTokens();
            RequestTokens.MergeWith(accessTokens);

            Process.Start("https://api.twitter.com/oauth/authenticate?oauth_token=" + RequestTokens.AccessToken);

            Console.Out.WriteLine("Web browser is starting. When you have logged in, enter your PIN code...");
            Console.Out.Write("PIN> ");
            string pinCode = Console.In.ReadLine();

            var keySwapRequest = WebRequest.Create("https://api.twitter.com/oauth/access_token");

            keySwapRequest.Method = "POST";

            keySwapRequest.SignRequest(RequestTokens)
                .WithEncryption(EncryptionMethod.HMACSHA1)
                .WithVerifier(pinCode)
                .InHeader();

            var finalAccessTokens = keySwapRequest.GetOAuthTokens();
            RequestTokens.MergeWith(finalAccessTokens);

        }
    }
}
