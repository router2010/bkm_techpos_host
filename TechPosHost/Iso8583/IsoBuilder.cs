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
}