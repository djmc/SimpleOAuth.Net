using SimpleOAuth.Generators;

namespace SimpleOAuth.Tests;

public class NonceGeneratorTests
{
    [Fact]
    public void Generate_ReturnsNonEmptyString()
    {
        var generator = new NonceGenerator();
        var nonce = generator.Generate();
        Assert.False(string.IsNullOrEmpty(nonce));
    }

    [Fact]
    public void Generate_ReturnsUniqueValues()
    {
        var generator = new NonceGenerator();
        var nonces = new HashSet<string>();
        for (int i = 0; i < 100; i++)
        {
            nonces.Add(generator.Generate());
        }
        Assert.Equal(100, nonces.Count);
    }

    [Fact]
    public void Generate_ReturnsStringWithoutDashes()
    {
        var generator = new NonceGenerator();
        var nonce = generator.Generate();
        Assert.DoesNotContain("-", nonce);
    }

    [Fact]
    public void Generate_Returns32CharacterString()
    {
        var generator = new NonceGenerator();
        var nonce = generator.Generate();
        // GUID without dashes = 32 hex chars
        Assert.Equal(32, nonce.Length);
    }
}

public class TimestampGeneratorTests
{
    [Fact]
    public void Generate_ReturnsNonEmptyString()
    {
        var generator = new TimestampGenerator();
        var timestamp = generator.Generate();
        Assert.False(string.IsNullOrEmpty(timestamp));
    }

    [Fact]
    public void Generate_ReturnsNumericString()
    {
        var generator = new TimestampGenerator();
        var timestamp = generator.Generate();
        Assert.True(long.TryParse(timestamp, out _));
    }

    [Fact]
    public void Generate_ReturnsReasonableTimestamp()
    {
        var generator = new TimestampGenerator();
        var timestamp = long.Parse(generator.Generate());

        // Should be after 2020-01-01 (1577836800) and before 2100-01-01
        Assert.True(timestamp > 1577836800);
        Assert.True(timestamp < 4102444800);
    }
}

public class HmacSha1GeneratorTests
{
    [Fact]
    public void Generate_ProducesConsistentOutput()
    {
        var generator = new HmacSha1Generator();
        var result1 = generator.Generate("base string", "key");
        var result2 = generator.Generate("base string", "key");
        Assert.Equal(result1, result2);
    }

    [Fact]
    public void Generate_DifferentInputsProduceDifferentOutput()
    {
        var generator = new HmacSha1Generator();
        var result1 = generator.Generate("base string 1", "key");
        var result2 = generator.Generate("base string 2", "key");
        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void Generate_DifferentKeysProduceDifferentOutput()
    {
        var generator = new HmacSha1Generator();
        var result1 = generator.Generate("base string", "key1");
        var result2 = generator.Generate("base string", "key2");
        Assert.NotEqual(result1, result2);
    }

    [Fact]
    public void Generate_ReturnsBase64String()
    {
        var generator = new HmacSha1Generator();
        var result = generator.Generate("test", "key");
        // Should be valid base64
        var bytes = Convert.FromBase64String(result);
        // HMAC-SHA1 produces 20 bytes
        Assert.Equal(20, bytes.Length);
    }
}
