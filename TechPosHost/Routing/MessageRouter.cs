using TechPosHost.Iso8583;
using TechPosHost.Models;
using TechPosHost.Repository;
namespace TechPosHost.Routing;

public class MessageRouter
{
    private readonly TerminalRepository _terminalRepository;
    private readonly TransactionRepository _transactionRepository;
    public MessageRouter()
    {
        _terminalRepository = new TerminalRepository();
        _transactionRepository = new TransactionRepository();
    }
    public IsoMessage Route(IsoMessage request)
    {
        switch (request.MTI)
        {
            case "0800":
                return Build0810();

            case "0200":
                return Build0210(request);

            default:
                return BuildError();
        }
    }

    private IsoMessage Build0810()
    {
        var response = new IsoMessage();

        response.MTI = "0810";
        response.SetField(39, "00");

        return response;
    }

    private IsoMessage Build0210(IsoMessage request)
    {
        var response = new IsoMessage();

        response.MTI = "0210";

        if (request.HasField(11))
        {
            response.SetField(
                11,
                request.GetField(11)!);
        }

        if (!request.HasField(41))
        {
            response.SetField(39, "96");
            return response;
        }

        string terminalId =
            request.GetField(41)!;

        if (!_terminalRepository.Exists(terminalId))
        {
            response.SetField(39, "58");
            return response;
        }

        _transactionRepository.Add(
            new TransactionLog
            {
                CreatedAt = DateTime.Now,
                MTI = request.MTI,
                ProcessingCode = request.GetField(3),
                Amount = request.GetField(4),
                Stan = request.GetField(11),
                TerminalId = request.GetField(41),
                ResponseCode = "00"
            });
        Console.WriteLine($"Transaction Count : {_transactionRepository.Count()}");
        response.SetField(39, "00");

        return response;
    }

    private IsoMessage BuildError()
    {
        return new IsoMessage
        {
            MTI = "9999"
        };
    }
}
