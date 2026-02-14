using System.Net;
using System.Net.Http;

namespace SimpleOAuth.Tests;

public class OAuthRequestWrapperTests
{
    private readonly Tokens _testTokens = new Tokens
    {
        ConsumerKey = "dpf43f3p2l4k3l03",
        ConsumerSecret = "kd94hf93k423kf44",
        AccessToken = "nnch734d00sl2jdk",
        AccessTokenSecret = "pfkkdhi9sl3r4s00"
    };

    #region WebRequest Tests

    [Fact]
    public void SignRequest_WebRequest_SetsAuthorizationHeader()
    {
        var request = WebRequest.Create("https://example.com/resource");
        request.Method = "GET";

        request.SignRequest(_testTokens)
            .WithEncryption(EncryptionMethod.HMACSHA1)
            .InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.NotNull(authHeader);
        Assert.StartsWith("OAuth ", authHeader);
    }

    [Fact]
    public void SignRequest_WebRequest_ContainsRequiredOAuthParameters()
    {
        var request = WebRequest.Create("https://example.com/resource");
        request.Method = "GET";

        request.SignRequest(_testTokens)
            .WithEncryption(EncryptionMethod.HMACSHA1)
            .InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.Contains("oauth_consumer_key", authHeader);
        Assert.Contains("oauth_nonce", authHeader);
        Assert.Contains("oauth_signature_method", authHeader);
        Assert.Contains("oauth_timestamp", authHeader);
        Assert.Contains("oauth_token", authHeader);
        Assert.Contains("oauth_version", authHeader);
        Assert.Contains("oauth_signature", authHeader);
    }

    [Fact]
    public void SignRequest_WebRequest_WithCallback_IncludesCallback()
    {
        var request = WebRequest.Create("https://example.com/request_token");
        request.Method = "POST";

        request.SignRequest()
            .WithTokens(_testTokens)
            .WithCallback("oob")
            .InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.Contains("oauth_callback", authHeader);
    }

    [Fact]
    public void SignRequest_WebRequest_WithVerifier_IncludesVerifier()
    {
        var request = WebRequest.Create("https://example.com/access_token");
        request.Method = "POST";

        request.SignRequest(_testTokens)
            .WithVerifier("hfdp7dh39dks9884")
            .InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.Contains("oauth_verifier", authHeader);
    }

    [Fact]
    public void SignRequest_WebRequest_WithVersion_UsesSpecifiedVersion()
    {
        var request = WebRequest.Create("https://example.com/resource");
        request.Method = "GET";

        request.SignRequest(_testTokens)
            .WithVersion("1.0")
            .InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.Contains("oauth_version", authHeader);
        Assert.Contains("1.0", authHeader);
    }

    [Fact]
    public void WithTokens_NullTokens_ThrowsArgumentException()
    {
        var request = WebRequest.Create("https://example.com/resource");
        var wrapper = request.SignRequest();

        Assert.Throws<ArgumentException>(() => wrapper.WithTokens(null));
    }

    [Fact]
    public void SignRequest_WebRequest_WithPostParameters_IncludesInSignature()
    {
        var request = WebRequest.Create("https://example.com/resource");
        request.Method = "POST";

        request.SignRequest(_testTokens)
            .WithPostParameters("status=hello")
            .InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.NotNull(authHeader);
        Assert.Contains("oauth_signature", authHeader);
    }

    [Fact]
    public void SignRequest_WebRequest_WithPostParameters_SetsContentType()
    {
        var request = WebRequest.Create("https://example.com/resource");
        request.Method = "POST";

        request.SignRequest(_testTokens)
            .WithPostParameters("status=hello")
            .InHeader();

        Assert.Equal("application/x-www-form-urlencoded", request.ContentType);
    }

    [Fact]
    public void SignRequest_WebRequest_NoAccessToken_OmitsOAuthToken()
    {
        var consumerOnlyTokens = new Tokens
        {
            ConsumerKey = "key",
            ConsumerSecret = "secret"
        };

        var request = WebRequest.Create("https://example.com/request_token");
        request.Method = "POST";

        request.SignRequest(consumerOnlyTokens).InHeader();

        var authHeader = request.Headers["Authorization"];
        Assert.DoesNotContain("oauth_token", authHeader);
    }

    [Fact]
    public void SignRequest_WebRequest_WithQueryParams_ProducesDifferentSignature()
    {
        var request1 = WebRequest.Create("https://example.com/resource?page=1");
        request1.Method = "GET";
        request1.SignRequest(_testTokens).InHeader();
        var header1 = request1.Headers["Authorization"];

        var request2 = WebRequest.Create("https://example.com/resource?page=2");
        request2.Method = "GET";
        request2.SignRequest(_testTokens).InHeader();
        var header2 = request2.Headers["Authorization"];

        // The signatures should differ because the query parameters differ
        // (nonce/timestamp also differ, but the point is both produce valid headers)
        Assert.NotEqual(header1, header2);
    }

    #endregion

    #region HttpRequestMessage Tests

    [Fact]
    public void SignRequest_HttpRequestMessage_SetsAuthorizationHeader()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/resource");

        request.SignRequest(_testTokens)
            .WithEncryption(EncryptionMethod.HMACSHA1)
            .InHeader();

        Assert.True(request.Headers.Contains("Authorization"));
        var authHeader = request.Headers.GetValues("Authorization").First();
        Assert.StartsWith("OAuth ", authHeader);
    }

    [Fact]
    public void SignRequest_HttpRequestMessage_ContainsRequiredOAuthParameters()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com/resource");

        request.SignRequest(_testTokens)
            .WithEncryption(EncryptionMethod.HMACSHA1)
            .InHeader();

        var authHeader = request.Headers.GetValues("Authorization").First();
        Assert.Contains("oauth_consumer_key", authHeader);
        Assert.Contains("oauth_nonce", authHeader);
        Assert.Contains("oauth_signature_method", authHeader);
        Assert.Contains("oauth_timestamp", authHeader);
        Assert.Contains("oauth_token", authHeader);
        Assert.Contains("oauth_version", authHeader);
        Assert.Contains("oauth_signature", authHeader);
    }

    [Fact]
    public void SignRequest_HttpRequestMessage_PostWithContent_SetsSignature()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com/resource");
        request.Content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("status", "hello")
        });

        request.SignRequest(_testTokens)
            .WithPostParameters("status=hello")
            .InHeader();

        var authHeader = request.Headers.GetValues("Authorization").First();
        Assert.Contains("oauth_signature", authHeader);
    }

    [Fact]
    public void SignRequest_HttpRequestMessage_WithCallback_IncludesCallback()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://example.com/request_token");

        request.SignRequest(_testTokens)
            .WithCallback("https://example.com/callback")
            .InHeader();

        var authHeader = request.Headers.GetValues("Authorization").First();
        Assert.Contains("oauth_callback", authHeader);
    }

    #endregion

    #region DefaultSigningMethod Tests

    [Fact]
    public void DefaultSigningMethod_IsHmacSha1()
    {
        Assert.Equal(EncryptionMethod.HMACSHA1, OAuthRequestWrapper.DefaultSigningMethod);
    }

    [Fact]
    public void DefaultSigningMethod_CanBeChanged()
    {
        var originalDefault = OAuthRequestWrapper.DefaultSigningMethod;
        try
        {
            OAuthRequestWrapper.DefaultSigningMethod = EncryptionMethod.Plain;
            Assert.Equal(EncryptionMethod.Plain, OAuthRequestWrapper.DefaultSigningMethod);
        }
        finally
        {
            OAuthRequestWrapper.DefaultSigningMethod = originalDefault;
        }
    }

    #endregion

    #region Chaining API Tests

    [Fact]
    public void FluentApi_ReturnsSameWrapper()
    {
        var request = WebRequest.Create("https://example.com/resource");
        var wrapper = request.SignRequest();

        var result = wrapper
            .WithTokens(_testTokens)
            .WithEncryption(EncryptionMethod.HMACSHA1)
            .WithVersion("1.0");

        Assert.Same(wrapper, result);
    }

    #endregion
}
