using System.Text;
using System.Linq;

namespace TechPosHost.Iso8583;

public class IsoBuilder
{
    public byte[] Build(IsoMessage message)
    {
        var sb = new StringBuilder();

        sb.Append(message.MTI);

        foreach (var field in message.Fields.OrderBy(x => x.Key))
        {
            sb.Append('|');
            sb.Append(field.Key);
            sb.Append('=');
            sb.Append(field.Value);
        }

        return Encoding.ASCII.GetBytes(sb.ToString());
    }

    public string BuildBitmapMessage(IsoMessage message)
    {
        var sb = new StringBuilder();

        sb.AppendLine(message.MTI);

        var bitmap =
            BitmapHelper.BuildBitmap(
                message.Fields.Keys);

        sb.AppendLine(bitmap);

        foreach (var field in message.Fields.OrderBy(x => x.Key))
        {
            sb.AppendLine(field.Value);
        }

        return sb.ToString();
    }
}