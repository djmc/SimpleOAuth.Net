namespace SimpleOAuth.Tests;

public class TokensTests
{
    [Fact]
    public void MergeWith_UpdatesNonEmptyTokens()
    {
        var original = new Tokens
        {
            ConsumerKey = "key1",
            ConsumerSecret = "secret1",
            AccessToken = "token1",
            AccessTokenSecret = "tokenSecret1"
        };

        var newTokens = new Tokens
        {
            AccessToken = "newToken",
            AccessTokenSecret = "newTokenSecret"
        };

        original.MergeWith(newTokens);

        Assert.Equal("key1", original.ConsumerKey);
        Assert.Equal("secret1", original.ConsumerSecret);
        Assert.Equal("newToken", original.AccessToken);
        Assert.Equal("newTokenSecret", original.AccessTokenSecret);
    }

    [Fact]
    public void MergeWith_DoesNotOverwriteWithEmpty()
    {
        var original = new Tokens
        {
            ConsumerKey = "key1",
            ConsumerSecret = "secret1",
            AccessToken = "token1",
            AccessTokenSecret = "tokenSecret1"
        };

        var newTokens = new Tokens
        {
            ConsumerKey = "",
            ConsumerSecret = null,
            AccessToken = "newToken",
            AccessTokenSecret = ""
        };

        original.MergeWith(newTokens);

        Assert.Equal("key1", original.ConsumerKey);
        Assert.Equal("secret1", original.ConsumerSecret);
        Assert.Equal("newToken", original.AccessToken);
        Assert.Equal("tokenSecret1", original.AccessTokenSecret);
    }

    [Fact]
    public void MergeWith_AllNewTokens()
    {
        var original = new Tokens();
        var newTokens = new Tokens
        {
            ConsumerKey = "key",
            ConsumerSecret = "secret",
            AccessToken = "token",
            AccessTokenSecret = "tokenSecret"
        };

        original.MergeWith(newTokens);

        Assert.Equal("key", original.ConsumerKey);
        Assert.Equal("secret", original.ConsumerSecret);
        Assert.Equal("token", original.AccessToken);
        Assert.Equal("tokenSecret", original.AccessTokenSecret);
    }
}
