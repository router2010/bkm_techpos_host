namespace TechPosHost.Iso8583;

public class IsoMessage
{
    public string MTI { get; set; } = "";

    public Dictionary<int, string> Fields { get; set; }
        = new();

    public string? GetField(int fieldNo)
    {
        Fields.TryGetValue(fieldNo, out var value);
        return value;
    }

    public void SetField(int fieldNo, string value)
    {
        Fields[fieldNo] = value;
    }

    public bool HasField(int fieldNo)
    {
        return Fields.ContainsKey(fieldNo);
    }
}