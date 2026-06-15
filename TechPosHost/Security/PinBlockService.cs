namespace TechPosHost.Security;

public class PinBlockService
{
    public bool Verify(
        string pinBlock,
        string expectedPin)
    {
        return pinBlock == expectedPin;
    }
}
