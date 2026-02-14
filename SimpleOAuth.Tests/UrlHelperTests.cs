using SimpleOAuth.Utilities;

namespace SimpleOAuth.Tests;

public class UrlHelperTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("hello", "hello")]
    [InlineData("hello world", "hello%20world")]
    [InlineData("test@example.com", "test%40example.com")]
    [InlineData("100%", "100%25")]
    [InlineData("a+b", "a%2Bb")]
    [InlineData("foo=bar&baz=qux", "foo%3Dbar%26baz%3Dqux")]
    public void Encode_EncodesCorrectly(string input, string expected)
    {
        var result = UrlHelper.Encode(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Encode_PreservesUnreservedCharacters()
    {
        // RFC 3986 unreserved characters: A-Z a-z 0-9 - . _ ~
        var unreserved = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-._~";
        var result = UrlHelper.Encode(unreserved);
        Assert.Equal(unreserved, result);
    }

    [Fact]
    public void Encode_NullReturnsEmpty()
    {
        var result = UrlHelper.Encode(null);
        Assert.Equal(string.Empty, result);
    }

    [Theory]
    [InlineData("hello%20world", "hello world")]
    [InlineData("hello+world", "hello world")]
    [InlineData("test%40example.com", "test@example.com")]
    public void Decode_DecodesCorrectly(string input, string expected)
    {
        var result = UrlHelper.Decode(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ParseQueryString_ParsesSimpleQuery()
    {
        var result = UrlHelper.ParseQueryString("foo=bar&baz=qux");
        Assert.Equal(2, result.Count);
        Assert.Equal("bar", result["foo"]);
        Assert.Equal("qux", result["baz"]);
    }

    [Fact]
    public void ParseQueryString_HandlesLeadingQuestionMark()
    {
        var result = UrlHelper.ParseQueryString("?foo=bar&baz=qux");
        Assert.Equal(2, result.Count);
        Assert.Equal("bar", result["foo"]);
        Assert.Equal("qux", result["baz"]);
    }

    [Fact]
    public void ParseQueryString_HandlesEncodedValues()
    {
        var result = UrlHelper.ParseQueryString("name=hello+world&key=test%40value");
        Assert.Equal("hello world", result["name"]);
        Assert.Equal("test@value", result["key"]);
    }

    [Fact]
    public void ParseQueryString_EmptyStringReturnsEmpty()
    {
        var result = UrlHelper.ParseQueryString("");
        Assert.Empty(result);
    }
}
