# WeightServiceIOT

WeightServiceIOT is a .NET-based service that provides an abstraction over weight machine connections, allowing for the management of multiple connections, safe data transmission, and reception. This project is designed to facilitate real-time weight data collection and monitoring through serial port communication.

## Table of Contents

- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [Examples](#examples)
- [Contributing](#contributing)

## Features

- **Connection Management:** Manage multiple weight machine connections through serial ports.
- **Safe Data Transmission:** Safely write and read data from weight machines.
- **Port Scanning:** Automatically scan and add available serial ports.
- **Cross-Platform:** Supports both Windows and Linux for serial port communication.

## Installation

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/Enablesoft-ERP-Consultancy-Pvt-Ltd/WeightServiceIOT.git
   cd WeightServiceIOT
   ```

2. **Build the Project:**
   Ensure you have .NET SDK installed. Build the project using:
   ```bash
   dotnet build
   ```

## Usage

To use the WeightServiceIOT service, follow these steps:

1. **Create a ConnectionManager Instance:**
   ```csharp
   using WeightServiceIOT;

   using var connectionManager = new ConnectionManager();
   ```

2. **Search and Add Connections:**
   ```csharp
   connectionManager.SearchAndAddConnections();
   ```

3. **Open All Connections:**
   ```csharp
   connectionManager.OpenAllConnections();
   ```

4. **Read Data from Connections:**
   ```csharp
   foreach (var conPortName in connectionManager.GetAllConnectedPortNames())
   {
       Console.WriteLine(
           $"Read data from PORT={conPortName} : {connectionManager.SafeReadFrom(conPortName)}"
       );
   }
   ```

## Examples

### Creating a Connection
This can be used for granular control over connections, However it is suggested to use The ConnectionManager to manage multiple connections.
```csharp
// Create a new connection, using its Portname, baudrate and connection timeout (optional).
var connection = new Connection("/dev/ttyUSB0", 9600, 500);
// open the serial port connection.
connection.OpenConnection();
// and start reading from it.
string data = connection.SafeReadData();
Console.WriteLine(data);
// connection should always be closed.
connection.CloseConnection();
```

### Managing Multiple Connections
```csharp
// Create a connection manager
using var connectionManager = new ConnectionManager();
// This will search all the available ports and add them to a lookup table.
connectionManager.SearchAndAddConnections();
// Try to open all the connectons present in the lookup table.
connectionManager.OpenAllConnections();
// loop through the portnames which are in lookup table.
foreach (var conPortName in connectionManager.GetAllConnectedPortNames())
{
   // and read data.
    Console.WriteLine(
        $"Read data from PORT={conPortName} : {connectionManager.SafeReadFrom(conPortName)}"
    );
}

// This can be removed without any problem, because it is 
// guranted that .NET will Dispose after going out of scope.
connectionManager.Dispose();
```

### Utility for Getting Port Names
Utils class has utility methods and helpers, for example `GetPortNames`. The builtin `GetPortNames` method is not cross-platform, It will not work on unix-like OS. This method however is cross-platform and can be used on an unix-like OS too.
```csharp
string[] ports = Utils.GetPortNames();
foreach (var port in ports)
{
    Console.WriteLine($"Available port: {port}");
}
```
