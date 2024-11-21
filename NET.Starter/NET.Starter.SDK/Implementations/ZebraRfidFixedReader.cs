using NET.Starter.SDK.Dtos;
using NET.Starter.SDK.Inputs;
using Symbol.RFID3;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace NET.Starter.SDK.Implementations
{
    internal class ZebraRfidFixedReader : BaseRfidFixedReader
    {
        protected override ConnectedInfoDto ConnectToDevice(ConnectInput input)
        {
            try
            {
                var rfidReader = new RFIDReader(input.HostName, input.Port, input.TimeoutMilliseconds);
                rfidReader.Connect();

                return new ConnectedInfoDto { IsConnected = true };
            }
            catch
            {
                return new ConnectedInfoDto { IsConnected = false, Message = "Gagal terhubung ke RFID Reader Zebra" };
            }
        }

        protected async override void ListenToDevice(ListenInput input, CancellationToken cancellationToken)
        {
            var tcpListener = new TcpListener(IPAddress.Parse(input.HostName), input.Port);
            tcpListener.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // Tunggu koneksi secara asinkron
                    var client = await tcpListener.AcceptTcpClientAsync(cancellationToken);
                    _ = HandleTcpClientAsync(client, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            tcpListener.Stop();
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
