using System.Security.Cryptography;

namespace TechPosHost.Security;

public class KeyService
{
    public string GenerateKey()
    {
        byte[] bytes = new byte[16];

        RandomNumberGenerator.Fill(bytes);

        return Convert.ToHexString(bytes);
    }
}