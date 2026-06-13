using TechPosHost.Iso8583;
using TechPosHost.Models;
using TechPosHost.Repository;
namespace TechPosHost.Routing;

public class MessageRouter
{
    private readonly TerminalRepository _terminalRepository;
    private readonly TransactionRepository _transactionRepository;
    public MessageRouter(TerminalRepository terminalRepository, TransactionRepository transactionRepository)
    {
        _terminalRepository = terminalRepository;
        _transactionRepository = transactionRepository;
    }
    public IsoMessage Route(IsoMessage request)
    {
        switch (request.MTI)
        {
            case "0800":
                return Build0810();

            case "0200":
                return Build0210(request);

            case "0420":
                return Build0430(request);
            case "0500":
                return Build0510(request);

            case "0820":
                return Build0830(request);

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

        string rrn =
            DateTime.Now.ToString("yyMMddHHmmss");

        string authCode =
            Random.Shared.Next(
                100000,
                999999).ToString();

        _transactionRepository.Add(
            new TransactionLog
            {
                CreatedAt = DateTime.Now,
                MTI = request.MTI,
                ProcessingCode = request.GetField(3),
                Amount = request.GetField(4),
                Stan = request.GetField(11),
                TerminalId = request.GetField(41),
                ResponseCode = "00",
                Rrn = rrn,
                AuthCode = authCode
            });

        Console.WriteLine(
            $"Transaction Count : {_transactionRepository.Count()}");

        response.SetField(37, rrn);

        response.SetField(38, authCode);

        response.SetField(39, "00");

        return response;
    }
    private IsoMessage Build0430(IsoMessage request)
    {
        var response = new IsoMessage();

        response.MTI = "0430";

        if (request.HasField(11))
        {
            response.SetField(
                11,
                request.GetField(11)!);
        }

        if (!request.HasField(11))
        {
            response.SetField(39, "12");
            return response;
        }

        string stan =
            request.GetField(11)!;

        bool reversed =
            _transactionRepository.Reverse(stan);

        response.SetField(
            39,
            reversed ? "00" : "25");

        return response;
    }
    private IsoMessage Build0510(IsoMessage request)
    {
        var response = new IsoMessage();

        response.MTI = "0510";

        if (request.HasField(11))
        {
            response.SetField(
                11,
                request.GetField(11)!);
        }

        int transactionCount =
            _transactionRepository.SettlementCount();

        response.SetField(39, "00");

        response.SetField(
            62,
            transactionCount.ToString());

        return response;
    }
    private IsoMessage Build0830(IsoMessage request)
    {
        var response = new IsoMessage();

        response.MTI = "0830";

        if (request.HasField(11))
        {
            response.SetField(
                11,
                request.GetField(11)!);
        }

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
