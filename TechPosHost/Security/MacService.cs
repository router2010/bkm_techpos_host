using System.Security.Cryptography;
using System.Text;

namespace TechPosHost.Security;

public class MacService
{
    public string Calculate(string text)
    {
        using var sha =
            SHA256.Create();

        byte[] hash =
            sha.ComputeHash(
                Encoding.ASCII.GetBytes(text));

        return Convert
            .ToHexString(hash)
            .Substring(0, 16);
    }
    public bool Verify(string plainText, string receivedMac)
    {
        string calculatedMac =
            Calculate(plainText);

        return string.Equals(
            calculatedMac,
            receivedMac,
            StringComparison.OrdinalIgnoreCase);
    }
}
