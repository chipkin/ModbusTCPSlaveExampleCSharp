using Chipkin;
using ModbusExample;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ModbusTCPSlaveExampleCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            ModbusSlave modbusSlave = new ModbusSlave();
            modbusSlave.Run();
        }

        unsafe class ModbusSlave
        {
            // Version 
            const string APPLICATION_VERSION = "0.0.1";

            // Configuration Options 
            public const byte SETTING_MODBUS_SERVER_SLAVE_ADDRESS = 0;
            public const ushort SETTING_MODBUS_SERVER_TCP_PORT = 502;
            const ushort SETTING_METER_COUNT = 25;
            const ushort SETTING_METER_REGISTERS_COUNT = 200;
            const ushort SETTING_MODBUS_DATABASE_MAX_SIZE = SETTING_METER_COUNT * SETTING_METER_REGISTERS_COUNT;
            const ushort SETTING_MODBUS_DATABASE_DEFAULT_VALUE = 0x0000;

            // TCP 
            Socket tcpListener;
            Socket tcpClient;

            // Database to hold the current values. 
            UInt16[] database;

            public void Run()
            {
                // Prints the version of the application and the CAS BACnet stack. 
                Console.WriteLine("Starting Modbus TCP Slave Example  version {0}.{1}", APPLICATION_VERSION, CIBuildVersion.CIBUILDNUMBER);
                Console.WriteLine("https://github.com/chipkin/ModbusTCPMasterExampleCSharp");
                Console.WriteLine("FYI: CAS Modbus Stack version: {0}.{1}.{2}.{3}",
                    CASModbusAdapter.GetAPIMajorVersion(),
                    CASModbusAdapter.GetAPIMinorVersion(),
                    CASModbusAdapter.GetAPIPatchVersion(),
                    CASModbusAdapter.GetAPIBuildVersion());

                // Set up the API and callbacks.
                uint returnCode = CASModbusAdapter.Init(CASModbusAdapter.TYPE_TCP, SendMessage, RecvMessage, CurrentTime);
                if (returnCode != CASModbusAdapter.STATUS_SUCCESS)
                {
                    Console.WriteLine("Error: Could not init the Modbus Stack, returnCode={0}", returnCode);
                    return;
                }

                // Set the modbus slave address. For Modbus TCP 0 and 255 are the only valid slave address. 
                CASModbusAdapter.SetSlaveId(ModbusSlave.SETTING_MODBUS_SERVER_SLAVE_ADDRESS);

                // Set up the call back functions for data. 
                CASModbusAdapter.RegisterGetValue(GetModbusValue);
                CASModbusAdapter.RegisterSetValue(SetModbusValue);

                // All done with the Modbus setup. 
                Console.WriteLine("FYI: CAS Modbus Stack Setup, successfuly");

                // Create the database and fill it with default data 
                this.database = new UInt16[SETTING_MODBUS_DATABASE_MAX_SIZE];
                for (UInt16 offset = 0; offset < SETTING_MODBUS_DATABASE_MAX_SIZE; offset++)
                {
                    this.database[offset] = SETTING_MODBUS_DATABASE_DEFAULT_VALUE;
                }

                // Configure the TCP Listen class. 
                // https://docs.microsoft.com/en-us/dotnet/framework/network-programming/synchronous-server-socket-example 
                IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, SETTING_MODBUS_SERVER_TCP_PORT);
                this.tcpListener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and   
                // listen for incoming connections.  
                try
                {
                    Console.WriteLine("FYI: Binding tcp port. port=[{0}]...", SETTING_MODBUS_SERVER_TCP_PORT);
                    this.tcpListener.Bind(localEndPoint);
                    this.tcpListener.Listen(1);
                    Console.WriteLine("FYI: Waiting on TCP connection TCP port=[{0}]...", SETTING_MODBUS_SERVER_TCP_PORT);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                // Program loop. 
                while (true)
                {
                    try
                    {
                        // Program is suspended while waiting for an incoming connection.
                        this.tcpClient = this.tcpListener.Accept();
                        Console.WriteLine("FYI: Got a connection from IP address=[{0}]", this.tcpClient.RemoteEndPoint.ToString());

                        // Do loop 
                        while (IsSocketConnected(this.tcpClient))
                        {
                            // Check for user input 
                            this.DoUserInput();

                            // Run the Modbus loop proccessing incoming messages.
                            CASModbusAdapter.Loop();

                            // Finally flush the buffer
                            CASModbusAdapter.Flush();

                            // Give some time to other applications. 
                            System.Threading.Thread.Sleep(1);
                        } // Client connected loop. 
                        Console.WriteLine("FYI: TCP Connection disconnected");
                            
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                } // Main program loop 
            }

            static bool IsSocketConnected(Socket s)
            {
                // https://stackoverflow.com/a/14925438/58456
                return !((s.Poll(1000, SelectMode.SelectRead) && (s.Available == 0)) || !s.Connected);
            }

            public bool SendMessage(System.UInt16 connectionId, System.Byte* payload, System.UInt16 payloadSize)
            {
                if (this.tcpClient == null || !this.tcpClient.Connected)
                {
                    return false;
                }
                Console.WriteLine("FYI: Sending {0} bytes to {1}", payloadSize, this.tcpClient.RemoteEndPoint.ToString());


                // Copy from the unsafe pointer to a Byte array. 
                byte[] message = new byte[payloadSize];
                Marshal.Copy((IntPtr)payload, message, 0, payloadSize);

                try
                {
                    // Send the message 
                    if (this.tcpClient.Send(message) == payloadSize)
                    {
                        // Message sent 
                        Console.Write("    ");
                        Console.WriteLine(BitConverter.ToString(message).Replace("-", " ")); // Convert bytes to HEX string. 
                        return true;
                    }
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    if (e.ErrorCode == 10054)
                    {
                        // Client disconnected. This is normal. 
                        return false;
                    }
                    Console.WriteLine(e.ToString());
                    return false;
                }
                // Could not send message for some reason. 
                return false;
            }
            public int RecvMessage(System.UInt16* connectionId, System.Byte* payload, System.UInt16 maxPayloadSize)
            {
                if (this.tcpClient == null || !this.tcpClient.Connected)
                {
                    return 0;
                }

                // Data buffer for incoming data.  
                byte[] bytes = new Byte[maxPayloadSize];

                // Try to get the data from the socket. 
                try
                {
                    if (this.tcpClient.Available <= 0)
                    {
                        return 0;
                    }
                    // An incoming connection needs to be processed.  
                    int bytesRec = this.tcpClient.Receive(bytes);
                    if (bytesRec <= 0)
                    {
                        return 0;
                    }


                    // Copy from the unsafe pointer to a Byte array. 
                    byte[] message = new byte[bytesRec];
                    Marshal.Copy(bytes, 0, (IntPtr)payload, bytesRec);

                    // Debug Show the data on the console.  
                    Console.WriteLine("FYI: Recived {0} bytes from {1}", bytesRec, this.tcpClient.RemoteEndPoint.ToString());
                    Console.Write("    ");
                    Console.WriteLine(BitConverter.ToString(bytes).Replace("-", " ").Substring(0, bytesRec * 3)); // Convert bytes to HEX string. 
                    return bytesRec;
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    if (e.ErrorCode == 10054)
                    {
                        // Client disconnected. This is normal. 
                        return 0;
                    }
                    Console.WriteLine(e.ToString());
                    return 0;
                }
            }
            public ulong CurrentTime()
            {
                // https://stackoverflow.com/questions/9453101/how-do-i-get-epoch-time-in-c
                return (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds;
            }

            public bool SetModbusValue(System.Byte slaveAddress, System.Byte function, System.UInt16 startingAddress, System.UInt16 length, System.Byte* data, System.UInt16 dataSize, System.Byte* errorCode)
            {
                Console.WriteLine("FYI: SetModbusValue slaveAddress=[{0}], function=[{1}], startingAddress=[{2}], length=[{3}], dataSize=[{4}]", slaveAddress, function, startingAddress, length, dataSize);

                if (startingAddress > SETTING_MODBUS_DATABASE_MAX_SIZE || startingAddress + length > SETTING_MODBUS_DATABASE_MAX_SIZE)
                {
                    // Out of range
                    *errorCode = CASModbusAdapter.EXCEPTION_02_ILLEGAL_DATA_ADDRESS;
                }

                switch (function)
                {
                    case CASModbusAdapter.FUNCTION_06_PRESET_SINGLE_REGISTER:
                    case CASModbusAdapter.FUNCTION_10_FORCE_MULTIPLE_REGISTERS:
                        {
                            short[] value = new short[length];
                            Marshal.Copy((IntPtr)data, value, 0, length);
                            for (ushort offset = 0; offset < length; offset++)
                            {
                                this.database[startingAddress + offset] = (ushort)value[offset];
                            }

                            return true;
                        }
                    default:
                        break;
                }


                return false;
            }
            public bool GetModbusValue(System.Byte slaveAddress, System.Byte function, System.UInt16 startingAddress, System.UInt16 length, System.Byte* data, System.UInt16 maxDataSize, System.Byte* errorCode)
            {
                Console.WriteLine("FYI: GetModbusValue slaveAddress=[{0}], function=[{1}], startingAddress=[{2}], length=[{3}]", slaveAddress, function, startingAddress, length);

                if (startingAddress > SETTING_MODBUS_DATABASE_MAX_SIZE)
                {
                    // Out of range
                    *errorCode = CASModbusAdapter.EXCEPTION_02_ILLEGAL_DATA_ADDRESS;
                }

                switch (function)
                {
                    case CASModbusAdapter.FUNCTION_03_READ_HOLDING_REGISTERS:
                    case CASModbusAdapter.FUNCTION_04_READ_INPUT_REGISTERS:

                        // Convert the USHORT into BYTE 
                        byte[] dataAsBytes = new byte[length * 2];
                        Buffer.BlockCopy(this.database, startingAddress, dataAsBytes, 0, length * 2);
                        Marshal.Copy(dataAsBytes, 0, (IntPtr)data, length * 2);
                        return true;

                    default:
                        break;
                }
                return false;
            }

            public void PrintHelp()
            {
                Console.WriteLine("FYI: Modbus Stack version: {0}.{1}.{2}.{3}",
                    CASModbusAdapter.GetAPIMajorVersion(),
                    CASModbusAdapter.GetAPIMinorVersion(),
                    CASModbusAdapter.GetAPIPatchVersion(),
                    CASModbusAdapter.GetAPIBuildVersion());

                Console.WriteLine("Help:");
                Console.WriteLine(" Q          - Quit");
                Console.WriteLine(" UP Arror   - Increase 40,001 by 1 ");
                Console.WriteLine(" Down Arror - Decrease 40,001 by 1 ");
                Console.WriteLine("\n");
            }

            private void DoUserInput()
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    Console.WriteLine("");
                    Console.WriteLine("FYI: Key {0} pressed. ", key.Key);

                    switch (key.Key)
                    {
                        case ConsoleKey.Q:
                            Console.WriteLine("FYI: Quit");
                            Environment.Exit(0);
                            break;
                        case ConsoleKey.UpArrow:
                            Console.WriteLine("FYI: Increase 40001 by 1. Before={0}, After={1}", this.database[0], this.database[0] + 1);
                            this.database[0]++;
                            break;
                        case ConsoleKey.DownArrow:
                            Console.WriteLine("FYI: Decrease 40001 by 1. Before={0}, After={1}", this.database[0], this.database[0] - 1);
                            this.database[0]--;
                            break;
                        default:
                            this.PrintHelp();
                            break;
                    }
                }
            }
        }
    }
}
