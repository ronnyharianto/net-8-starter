using NET.Starter.SDK.Dtos;
using NET.Starter.SDK.Inputs;

namespace NET.Starter.SDK.Implementations
{
    internal class MockRfidFixedReader : BaseRfidFixedReader
    {
        protected override ConnectedInfoDto ConnectToDevice(ConnectInput input)
        {
            if (input.HostName.StartsWith("192") && input.Port == 5084) return new ConnectedInfoDto { IsConnected = true };

            return new ConnectedInfoDto { IsConnected = false, Message = "Gagal terhubung ke RFID Reader Mock" };
        }

        protected override void ListenToDevice(ListenInput input, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
