using System.IO.Ports;
using System.Runtime.InteropServices;

namespace WeightServiceIOT
{
    public class Utils
    {
        ///
        /// <summary>
        ///  Os independent GetPortNames
        /// </summary>
        ///
        /// <exception cref="System.ComponentModel.Win32Exception">Failed to retrieve.</exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        public static string[] GetPortNames()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return Directory
                    .GetFiles("/dev/", "tty*")
                    .Where(port =>
                        port.StartsWith("/dev/ttyS")
                        || port.StartsWith("/dev/ttyUSB")
                        || port.StartsWith("/dev/ttyACM")
                    )
                    .ToArray();
            }
            else
            {
                return SerialPort.GetPortNames();
            }
        }
    }
}
