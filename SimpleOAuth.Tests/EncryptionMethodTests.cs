using SimpleOAuth.Internal;

namespace SimpleOAuth.Tests;

public class EncryptionMethodTests
{
    [Fact]
    public void Plain_HasCorrectDescription()
    {
        var description = EncryptionMethod.Plain.GetDescription();
        Assert.Equal("PLAINTEXT", description);
    }

    [Fact]
    public void HMACSHA1_HasCorrectDescription()
    {
        var description = EncryptionMethod.HMACSHA1.GetDescription();
        Assert.Equal("HMAC-SHA1", description);
    }

    [Fact]
    public void HMACSHA1_HasSignatureType()
    {
        var signatureType = EncryptionMethod.HMACSHA1.GetSignatureType();
        Assert.NotNull(signatureType);
        Assert.Equal(typeof(Generators.HmacSha1Generator), signatureType);
    }

    [Fact]
    public void Plain_HasNoSignatureType()
    {
        var signatureType = EncryptionMethod.Plain.GetSignatureType();
        Assert.Null(signatureType);
    }
}
