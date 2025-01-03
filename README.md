## Description
This is a template i use

## Building:

Download .NET 9: https://dotnet.microsoft.com/en-us/download

Building for Windows:
1. Run this command: ``dotnet publish -o ./build/windows --sc true -r win-x64 -c release``
2. Copy the ``res/`` folder the the ``build/windows/`` directory

Building for Linux:
1. Run this command: ``dotnet publish -o ./build/linux --sc true -r linux-x64 -c release``
2. Copy the ``res/`` folder the the ``build/linux/`` directory