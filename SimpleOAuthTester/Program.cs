using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using SimpleOAuth;
using System.Diagnostics;
using System.IO;

namespace SimpleOAuthTester
{
    class Program
    {
        static Tokens RequestTokens { get; set; }

        static void Main(string[] args)
        {
            Console.Out.WriteLine("SIMPLEOAUTH WITH http://term.ie/oauth/example/");

            RequestTokens = new Tokens() { ConsumerKey = "key", ConsumerSecret = "secret" };

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

            var signingRequest = request.SignRequest(RequestTokens).WithEncryption(EncryptionMethod.HMACSHA1);

            if (methodInput.Equals("POST"))
            {
                Console.Out.Write("POST> ");
                var requestString = Console.In.ReadLine();

                signingRequest.WithPostParameters(requestString).InHeader();

                using (var stream = new StreamWriter(request.GetRequestStream()))
                {
                    stream.Write(UrlHelper.Encode(requestString));
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

            Console.Out.WriteLine("SERVER RESPONDED WITH");
            Console.Out.WriteLine(twitterResponse);
        }

        static void StartOAuthLogin()
        {
            var request = WebRequest.Create("http://term.ie/oauth/example/request_token.php");

            request.Method = "POST";

            request.SignRequest()
                .WithTokens(RequestTokens)
                .InHeader();

            var accessTokens = request.GetOAuthTokens();
            RequestTokens.MergeWith(accessTokens);

            var keySwapRequest = WebRequest.Create("http://term.ie/oauth/example/access_token.php");

            keySwapRequest.Method = "POST";

            keySwapRequest.SignRequest()
                .WithTokens(RequestTokens)
                .InHeader();

            var finalAccessTokens = keySwapRequest.GetOAuthTokens();
            RequestTokens.MergeWith(finalAccessTokens);

        }
    }
}
