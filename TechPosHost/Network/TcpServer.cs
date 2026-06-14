using System.Diagnostics.Metrics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TechPosHost.Iso8583;
using TechPosHost.Repository;
using TechPosHost.Routing;

namespace TechPosHost.Network;

public class TcpServer
{
    private readonly TcpListener _listener;
    private readonly IsoParser _parser;
    private readonly IsoBuilder _builder;
    private readonly MessageRouter _router;
    private readonly IsoLogRepository _isoLogRepository;

    public TcpServer(
        int port,
        MessageRouter router,
        IsoLogRepository isoLogRepository)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _parser = new IsoParser();
        _builder = new IsoBuilder();
        _router = router;
        _isoLogRepository = isoLogRepository;
    }

    public async Task StartAsync()
    {
        _listener.Start();

        while (true)
        {
            var client = await _listener.AcceptTcpClientAsync();

            _ = Task.Run(() => HandleClientAsync(client));
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        try
        {
            using var stream = client.GetStream();

            byte[] buffer = new byte[4096];

            int read = await stream.ReadAsync(buffer);

            if (read <= 0)
                return;

            byte[] data = buffer[..read];

            string requestText =
                Encoding.ASCII.GetString(data);

            IsoMessage message =
                _parser.Parse(data);

            Console.WriteLine($"MTI : {message.MTI}");

            if (message.HasField(3))
                Console.WriteLine($"F3  : {message.GetField(3)}");

            if (message.HasField(4))
                Console.WriteLine($"F4  : {message.GetField(4)}");

            if (message.HasField(11))
                Console.WriteLine($"F11 : {message.GetField(11)}");

            if (message.HasField(41))
                Console.WriteLine($"F41 : {message.GetField(41)}");

            IsoMessage responseMessage =
                _router.Route(message);

            byte[] response =
                _builder.Build(responseMessage);

            string responseText =
                Encoding.ASCII.GetString(response);

            _isoLogRepository.Add(
                requestText,
                responseText,
                message.MTI,
                message.GetField(41));

            await stream.WriteAsync(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
}