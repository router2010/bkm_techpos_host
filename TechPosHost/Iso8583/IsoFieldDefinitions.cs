namespace TechPosHost.Iso8583;

public static class IsoFieldDefinitions
{
    public static readonly Dictionary<int, IsoFieldDefinition>
        Definitions = new()
    {
        {2,  new() { FieldNo = 2,  VariableLength = true }},
        {3,  new() { FieldNo = 3,  Length = 6 }},
        {4,  new() { FieldNo = 4,  Length = 12 }},
        {11, new() { FieldNo = 11, Length = 6 }},
        {37, new() { FieldNo = 37, Length = 12 }},
        {38, new() { FieldNo = 38, Length = 6 }},
        {39, new() { FieldNo = 39, Length = 2 }},
        {41, new() { FieldNo = 41, Length = 8 }},
        {52, new() { FieldNo = 52, Length = 16 }},
        {64, new() { FieldNo = 64, Length = 16 }}
    };
}