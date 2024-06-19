using WeightServiceIOT;

using var connectionManager = new ConnectionManager();
connectionManager.SearchAndAddConnections();
connectionManager.OpenAllConnections();

foreach (var conPortName in connectionManager.GetAllConnectedPortNames())
{
    // for now we just log data from all connections.
    // we will implement a http server and send state there.
    Console.WriteLine(
        $"Read data from PORT={conPortName} : {connectionManager.SafeReadFrom(conPortName)}"
    );
}
