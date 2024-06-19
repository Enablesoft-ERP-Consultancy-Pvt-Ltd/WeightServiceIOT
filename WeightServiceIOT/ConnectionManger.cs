namespace WeightServiceIOT
{
    /// <summay>
    /// Orchestrates a pool of connections, provide methods to perform on all of
    /// them.
    /// </summay>
    public class ConnectionManager : IDisposable
    {
        // Contains all the active connections
        // key : The port of the device
        // value : Connection
        public Dictionary<string, Connection> TotalConnections { get; set; }

        /// <summary>
        /// Primary constructor for ConnectionManager.
        /// </summary>
        public ConnectionManager()
        {
            TotalConnections = [];
        }

        /// <summary>
        /// Searches all the ports, tries to initialize a Connection and stores
        /// them in the lookup table.
        /// </summary>
        public void SearchAndAddConnections()
        {
            string[] ports = Utils.GetPortNames(); // for testing on linux: ["/dev/pts/3", "/dev/pts/4"];
            foreach (var a in ports)
            {
                Console.WriteLine($"Fetched port {a}");
            }

            foreach (var port in ports)
            {
                try
                {
                    var con = new Connection(port);
                    TotalConnections.Add(port, con);
                    Console.WriteLine($"Created connection. Port={port}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to create connection. Port={port}: {e.Message}");
                }
            }
        }

        /// <summary>
        /// Opens transmision for all the Connections present in the look up table.
        /// </summary>
        public void OpenAllConnections()
        {
            foreach (var con in TotalConnections)
            {
                try
                {
                    con.Value.OpenConnection();
                    Console.WriteLine($"Opened connection. Port={con.Key}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(
                        $"Failed to open connection, skipping. Port={con.Key}: {e.Message}"
                    );
                }
            }
        }

        /// <summary>
        /// Reads data from all connections.
        /// </summary>
        ///
        /// <returns>List of data from all connections</returns>
        public List<string> SafeReadAll()
        {
            List<string> data = [];
            foreach (var con in TotalConnections.Values)
            {
                data.Add(con.SafeReadData());
            }
            return data;
        }

        /// <summary>
        /// Reads data from a connection. returns empty string incase of read fails or
        /// portname doesnt exist.
        /// </summary>
        ///
        /// <param name="portName">Port of the Connection to read from.</param>
        public string SafeReadFrom(string portName)
        {
            TotalConnections.TryGetValue(portName, out Connection? value);
            if (value == null)
            {
                Console.WriteLine($"Port={portName} does not exists in connections pool.");
                return "";
            }

            return value.SafeReadData();
        }

        /// <summary>
        /// Writes data fro a connection serial port. Doesnt throw if any write fails
        /// or portname doesnt exist.
        /// </summary>
        ///
        /// <param name="portName">Port of the Connection to read from.</param>
        public void SafeWriteTo(string portName, string data)
        {
            TotalConnections.TryGetValue(portName, out Connection? value);
            if (value == null)
            {
                Console.WriteLine($"Port={portName} does not exists in connections pool.");
                return;
            }

            value.SafeWriteData(data);
        }

        /// <summary>
        /// Closes all connections, rescans all ports, and opens transmision to them.
        /// </summary>
        public void ResetAll()
        {
            CloseAllConnections();
            TotalConnections.Clear();
            SearchAndAddConnections();
            OpenAllConnections();
        }

        /// <summary>
        /// Implement IDispose for ConnectionManager to safely close all connections,
        /// because Destructors are not guranted to run.
        /// </summary>
        public void Dispose()
        {
            // we do NOT want GC to finalize this object before closing the connections.
            GC.SuppressFinalize(this);
            CloseAllConnections();
        }

        /// <summary>
        /// Helper function to get all keys connected connection keys
        /// </summary>S
        public List<string> GetAllConnectedPortNames()
        {
            return [.. TotalConnections.Keys];
        }

        /// <summary>
        /// Helper function to get all connected Connections.
        /// </summary>
        public List<Connection> GetAllConnectedConnections()
        {
            return [.. TotalConnections.Values];
        }

        /// <summary>
        /// closes all connections in the lookup table.
        /// </summary>
        private void CloseAllConnections()
        {
            foreach (var con in TotalConnections)
            {
                Console.WriteLine($"Closing connection. Port={con.Key}");
                con.Value.CloseConnection();
            }
        }
    }
}
