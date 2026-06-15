namespace TechPosHost.Iso8583;

public class IsoFieldDefinition
{
    public int FieldNo { get; set; }

    public int Length { get; set; }

    public bool VariableLength { get; set; }
}