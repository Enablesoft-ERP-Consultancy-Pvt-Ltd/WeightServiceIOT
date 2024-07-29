using System.IO.Ports;

namespace WeightServiceIOT
{
    /// <summary>
    /// Abstraction over a weight machine connection.
    /// </summay>
    public class Connection
    {
        public string portName;
        public int baudRate;
        public SerialPort serialPort;

        ///
        /// <summary>
        /// Primary constructor for Connection.
        /// </summary>
        ///
        /// <param name="_port">Com Port name.</param>
        /// <param name="_baudRate">Baud Rate</param>
        ///
        /// <exception cref="IOException">Failed finding a port name.</exception>
        /// <exception cref="IOException">Failed creating serial port object.</exception>
        /// <exception cref="UnauthorizedAccessException">Access denied to the port.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One or more of the properties are invalid.</exception>
        /// <exception cref="ArgumentException">Port name is invalid</exception>
        /// <exception cref="IOException">Port in invalid state.</exception>
        public Connection(string _port, int _baudRate = 9600, int _readTimeout = 500)
        {
            portName = _port;
            baudRate = _baudRate;
            serialPort =
                new SerialPort(portName, baudRate)
                ?? throw new IOException(
                    $"Cannot create port with name:{portName}, baud rate:{baudRate}"
                );

            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.ReadTimeout = _readTimeout;
        }

        ///
        /// <summary>
        /// Opens connection, the constructor calls this automatically.
        /// </summary>
        ///
        /// <exception cref="UnauthorizedAccessException">Access denied to the port.</exception>
        /// <exception cref="ArgumentOutOfRangeException">One or more of the properties are invalid.</exception>
        /// <exception cref="ArgumentException">Port name is invalid</exception>
        /// <exception cref="IOException">Port in invalid state.</exception>
        public void OpenConnection()
        {
            // Only open if its not already open
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
            }
        }

        ///
        /// <summary>
        /// Closes connection of the serial port.
        /// </summary>
        ///
        /// <exception cref="IOException">Serial port is in invalid state.</exception>
        public void CloseConnection()
        {
            // close only if its already open.
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        /// <summary>
        /// Safely write data to a serial port.
        /// </summary>
        public void SafeWriteData(string data)
        {
            try
            {
                serialPort.WriteLine(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to write to serial port, Port={portName}: {e.Message}");
            }
        }

        /// <summary>
        /// Safely write data to a serial port.
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="TimeoutException"></exception>
        public void UnsafeWriteData(string data)
        {
            serialPort.WriteLine(data);
        }

        ///
        /// <summary>
        /// Safely read data from serial port. Returns empty string if fails to read from serial port.
        /// </summary>
        public string SafeReadData()
        {
            string data = string.Empty;
            try
            {
                data = serialPort.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to read from serial port, Port={portName}: {e.Message}");
            }

            return data;
        }

        ///
        /// <summary>
        /// Read data from serial port, can throw.
        /// </summary>
        ///
        /// <exception cref="TimeoutException">Time out while reading.</exception>
        /// <exception cref="InvalidOperationException">Time out while reading.</exception>
        public string UnsafeReadData()
        {
            return serialPort.ReadLine();
        }
    }
}
