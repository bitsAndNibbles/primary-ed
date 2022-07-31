# Build pre-requisites

These steps are for Windows. Check the [.NET downloads page](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) for other platforms.

* `winget install -e --moniker dotnet-sdk-6`
* `winget install -e --moniker dotnet-desktop-6`

# Build

* Build: `dotnet build`
* Build then run: `dotnet run`

# Runtime Pre-Requisites

* The app uses Windows Presentation Foundation. You'll need a [Windows OS that supports .NET 6 or later](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60).
* `winget install -e --moniker dotnet-desktop-6`

# Run

* `dotnet bin\Debug\net6.0-windows\PrimeNumber.Client.dll`
