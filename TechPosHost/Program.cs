using TechPosHost.Data;
using TechPosHost.Network;

TestDbConnection.Check();

var server = new TcpServer(23232);

Console.WriteLine("TechPosHost Started");

await server.StartAsync();