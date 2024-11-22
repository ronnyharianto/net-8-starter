using NET.Starter.SDK.Dtos;
using NET.Starter.SDK.Inputs;
using Symbol.RFID3;
using System.Net.Sockets;
using System.Net;
using System.Text;
using NET.Starter.SDK.Managers;

namespace NET.Starter.SDK.Implementations
{
    internal class ZebraRfidFixedReaderService(ZebraRfidReaderManager zebraRfidReaderManager) : BaseRfidFixedReader
    {
        private readonly ZebraRfidReaderManager _zebraRfidReaderManager = zebraRfidReaderManager;

        protected override ConnectedInfoDto ConnectToDevice(ConnectInput input)
        {
            try
            {
                var rfidReader = new RFIDReader(input.HostName, input.Port, input.TimeoutMilliseconds);
                rfidReader.Connect();

                _zebraRfidReaderManager.AddReader(rfidReader);

                return new ConnectedInfoDto { IsConnected = true };
            }
            catch
            {
                return new ConnectedInfoDto { IsConnected = false, Message = "Gagal terhubung ke RFID Reader Zebra" };
            }
        }

        protected async override void ListenToDevice(ListenInput input)
        {
            if (_zebraRfidReaderManager.CtsListen == null || _zebraRfidReaderManager.CtsListen.IsCancellationRequested)
            {
                _zebraRfidReaderManager.CtsListen = new CancellationTokenSource();

                var tcpListener = new TcpListener(IPAddress.Parse(input.HostName), input.Port);
                tcpListener.Start();

                while (!_zebraRfidReaderManager.CtsListen.Token.IsCancellationRequested)
                {
                    try
                    {
                        // Tunggu koneksi secara asinkron
                        var client = await tcpListener.AcceptTcpClientAsync(_zebraRfidReaderManager.CtsListen.Token);
                        _ = HandleTcpClientAsync(client, _zebraRfidReaderManager.CtsListen.Token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }

                tcpListener.Stop();
            }
            else
            {
                _zebraRfidReaderManager.CtsListen.Cancel();
            }
        }

        private static async Task HandleTcpClientAsync(TcpClient client, CancellationToken cancellationToken)
        {
            using var stream = client.GetStream();
            var buffer = new byte[1024];

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    int bytesRead = await stream.ReadAsync(buffer, cancellationToken);
                    if (bytesRead > 0)
                    {
                        string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Data received: {data}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}
