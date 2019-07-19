# Modbus TCP Slave Example CSharp

A basic Modbus TCP Slave example written in CSharp using the CAS Modbus Stack

## User input

Below is the user input for this application.

- **Q** - Quit
- **UP Arror** - Increase 40,001 by 1
- **Down Arror** - Decrease 40,001 by 1

## Example output

```txt
Starting Modbus TCP Slave Example  version 0.0.1.0
https://github.com/chipkin/ModbusTCPMasterExampleCSharp
FYI: CAS Modbus Stack version: 2.3.11.0
FYI: CAS Modbus Stack Setup, successfuly
FYI: Binding tcp port. port=[502]...
FYI: Waiting on TCP connection TCP port=[502]...
FYI: Got a connection from IP address=[192.168.1.77:53786]
FYI: Recived 12 bytes from 192.168.1.77:53786
    00 02 00 00 00 06 FF 03 00 00 00 6E
FYI: GetModbusValue slaveAddress=[0], function=[3], startingAddress=[0], length=[110]
FYI: Sending 229 bytes to 192.168.1.77:53786
    00 02 00 00 00 DF 00 03 DC 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

FYI: Key UpArrow pressed.
FYI: Increase 40001 by 1. Before=0, After=1

FYI: Key UpArrow pressed.
FYI: Increase 40001 by 1. Before=1, After=2

FYI: Key UpArrow pressed.
FYI: Increase 40001 by 1. Before=2, After=3

FYI: Recived 12 bytes from 192.168.1.77:53786
    00 02 00 00 00 06 FF 03 00 00 00 6E
FYI: GetModbusValue slaveAddress=[0], function=[3], startingAddress=[0], length=[110]
FYI: Sending 229 bytes to 192.168.1.77:53786
    00 02 00 00 00 DF 00 03 DC 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

FYI: Key DownArrow pressed.
FYI: Decrease 40001 by 1. Before=3, After=2

FYI: Recived 12 bytes from 192.168.1.77:53786
    00 02 00 00 00 06 FF 03 00 00 00 6E
FYI: GetModbusValue slaveAddress=[0], function=[3], startingAddress=[0], length=[110]
FYI: Sending 229 bytes to 192.168.1.77:53786
    00 02 00 00 00 DF 00 03 DC 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00
FYI: Recived 12 bytes from 192.168.1.77:53786
    00 02 00 00 00 06 FF 06 00 00 00 71
FYI: SetModbusValue slaveAddress=[0], function=[6], startingAddress=[0], length=[1], dataSize=[2]
FYI: Sending 12 bytes to 192.168.1.77:53786
    00 02 00 00 00 06 00 06 00 00 00 71
FYI: Recived 12 bytes from 192.168.1.77:53786
    00 02 00 00 00 06 FF 03 00 00 00 6E
FYI: GetModbusValue slaveAddress=[0], function=[3], startingAddress=[0], length=[110]
FYI: Sending 229 bytes to 192.168.1.77:53786
    00 02 00 00 00 DF 00 03 DC 00 71 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

FYI: Key Spacebar pressed.
FYI: Modbus Stack version: 2.3.11.0
Help:
  Q         - Quit
 UP Arror   - Increase 40,001 by 1
 Down Arror - Decrease 40,001 by 1

FYI: Key Q pressed.
FYI: Quit
```

## Building

1. Copy *CASModbusStack_Win32_Debug.dll*, *CASModbusStack_Win32_Release.dll*, *CASModbusStack_x64_Debug.dll*, and *CASModbusStack_x64_Release.dll* from the [CAS Modbus Stack](https://store.chipkin.com/services/stacks/modbus-stack) project  into the /bin/ folder.
2. Use [Visual Studios 2019](https://visualstudio.microsoft.com/vs/) to build the project. The solution can be found in the */ModbusTCPSlaveExampleCSharp/* folder.

Note: The project is automaticly build on every checkin using GitlabCI.

