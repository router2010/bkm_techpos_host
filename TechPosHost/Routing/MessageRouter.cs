using TechPosHost.Iso8583;
using TechPosHost.Models;
using TechPosHost.Repository;
namespace TechPosHost.Routing;

public class MessageRouter
{
    private readonly TerminalRepository _terminalRepository;
    private readonly TransactionRepository _transactionRepository;
    private readonly CardRepository _cardRepository;
    public MessageRouter(TerminalRepository terminalRepository, TransactionRepository transactionRepository,
    CardRepository cardRepository)
    {
        _terminalRepository = terminalRepository;
        _transactionRepository = transactionRepository;
        _cardRepository = cardRepository;
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
            case "0600":
                return Build0610(request);

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

        string stan =
            request.GetField(11)!;

        Console.WriteLine($"CHECK STAN = [{stan}]");

        bool exists =
            _transactionRepository.ExistsByStan(stan);

        Console.WriteLine($"EXISTS = {exists}");

        if (exists)
        {
            response.SetField(39, "94");
            return response;
        }

        if (!request.HasField(2))
        {
            response.SetField(39, "14");
            return response;
        }

        string pan =
            request.GetField(2)!;

        var card =
            _cardRepository.Get(pan);

        if (card == null)
        {
            response.SetField(39, "14");
            return response;
        }

        if (!card.IsActive)
        {
            response.SetField(39, "62");
            return response;
        }

        decimal amount =
            decimal.Parse(
                request.GetField(4) ?? "0");

        if (card.Balance < amount)
        {
            response.SetField(39, "51");
            return response;
        }

        bool debitResult =
            _cardRepository.Debit(
                pan,
                amount);

        if (!debitResult)
        {
            response.SetField(39, "96");
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
                Stan = stan,
                TerminalId = terminalId,
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

        decimal totalAmount =
            _transactionRepository.SettlementAmount();

        response.SetField(39, "00");

        response.SetField(
            62,
            transactionCount.ToString());

        response.SetField(
            63,
            totalAmount.ToString("0"));

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
    private IsoMessage Build0610(IsoMessage request)
    {
        var response = new IsoMessage();

        response.MTI = "0610";

        if (!request.HasField(11))
        {
            response.SetField(39, "12");
            return response;
        }

        string stan =
            request.GetField(11)!;

        var trx =
            _transactionRepository.GetByStan(stan);

        if (trx == null)
        {
            response.SetField(39, "25");
            return response;
        }

        response.SetField(11, trx.Stan!);

        if (!string.IsNullOrEmpty(trx.Amount))
            response.SetField(4, trx.Amount);

        if (!string.IsNullOrEmpty(trx.Rrn))
            response.SetField(37, trx.Rrn);

        if (!string.IsNullOrEmpty(trx.AuthCode))
            response.SetField(38, trx.AuthCode);

        response.SetField(
            39,
            trx.ResponseCode ?? "00");

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
