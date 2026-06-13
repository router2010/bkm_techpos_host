using System.Text;

namespace TechPosHost.Iso8583;

public class IsoParser
{
    public IsoMessage Parse(byte[] data)
    {
        string text =
            Encoding.ASCII.GetString(data);

        if (text.Contains('|'))
        {
            return ParsePipeMessage(text);
        }

        return ParseBitmapMessage(text);
    }

    private IsoMessage ParsePipeMessage(string text)
    {
        var message = new IsoMessage();

        string[] parts = text.Split('|');

        if (parts.Length > 0)
        {
            message.MTI = parts[0];
        }

        for (int i = 1; i < parts.Length; i++)
        {
            string[] field =
                parts[i].Split('=');

            if (field.Length == 2)
            {
                int fieldNo =
                    int.Parse(field[0]);

                message.SetField(
                    fieldNo,
                    field[1]);
            }
        }

        return message;
    }

    private IsoMessage ParseBitmapMessage(string text)
    {
        var message = new IsoMessage();

        string[] lines =
            text.Replace("\r", "")
                .Split('\n',
                    StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length == 0)
        {
            return message;
        }

        message.MTI = lines[0];

        // Bitmap parsing sonraki adımda eklenecek

        return message;
    }
}