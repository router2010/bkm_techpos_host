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

        message.RawMessage = text;

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

        message.RawMessage = text;

        string[] lines =
            text.Replace("\r", "")
                .Split('\n',
                    StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 2)
        {
            return message;
        }

        message.MTI = lines[0];

        string bitmap = lines[1];

        var fields =
            BitmapHelper.ParseBitmap(bitmap);

        int lineIndex = 2;

        foreach (var fieldNo in fields)
        {
            if (lineIndex >= lines.Length)
                break;

            message.SetField(
                fieldNo,
                lines[lineIndex]);

            lineIndex++;
        }

        return message;
    }
}