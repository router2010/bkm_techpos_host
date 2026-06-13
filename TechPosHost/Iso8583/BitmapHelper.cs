namespace TechPosHost.Iso8583;

public static class BitmapHelper
{
    public static string BuildBitmap(IEnumerable<int> fields)
    {
        bool[] bits = new bool[64];

        foreach (var field in fields)
        {
            if (field >= 1 && field <= 64)
            {
                bits[field - 1] = true;
            }
        }

        string binary = string.Concat(bits.Select(x => x ? "1" : "0"));

        string hex = "";

        for (int i = 0; i < 64; i += 4)
        {
            string nibble =
                binary.Substring(i, 4);

            hex += Convert.ToInt32(
                nibble,
                2).ToString("X");
        }

        return hex;
    }
}