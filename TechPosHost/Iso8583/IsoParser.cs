using System.Text;

namespace TechPosHost.Iso8583;

public class IsoParser
{
    public IsoMessage Parse(byte[] data)
    {
        var message = new IsoMessage();

        string text = Encoding.ASCII.GetString(data);

        string[] parts = text.Split('|');

        if (parts.Length > 0)
            message.MTI = parts[0];

        for (int i = 1; i < parts.Length; i++)
        {
            string[] field = parts[i].Split('=');

            if (field.Length == 2)
            {
                int fieldNo = int.Parse(field[0]);

                message.SetField(
                    fieldNo,
                    field[1]);
            }
        }

        return message;
    }
}