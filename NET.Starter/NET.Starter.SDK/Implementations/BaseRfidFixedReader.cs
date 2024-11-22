using NET.Starter.SDK.Dtos;
using NET.Starter.SDK.Inputs;
using NET.Starter.SDK.Interfaces;
using System.Net;

namespace NET.Starter.SDK.Implementations
{
    internal abstract class BaseRfidFixedReader : IRfidFixedReader
    {
        public ConnectedInfoDto Connect(ConnectInput input)
        {
            if (!IPAddress.TryParse(input.HostName, out _))
                return new ConnectedInfoDto { IsConnected = false, Message = "Host name bukan merupakan IP Address" };

            if (input.Port < 0 || input.Port > 65535)
                return new ConnectedInfoDto { IsConnected = false, Message = "Port hanya boleh di antara 0 sampai dengan 65535" };

            if (input.TimeoutMilliseconds < 0)
                return new ConnectedInfoDto { IsConnected = false, Message = "Timeout tidak boleh lebih kecil dari 0" };

            return ConnectToDevice(input);
        }

        protected abstract ConnectedInfoDto ConnectToDevice(ConnectInput input);

        public async Task<ListeningInfoDto> Listen(ListenInput input)
        {
            if (!IPAddress.TryParse(input.HostName, out _))
                return new ListeningInfoDto { IsListening = false, Message = "Host name bukan merupakan IP Address" };

            if (input.Port < 0 || input.Port > 65535)
                return new ListeningInfoDto { IsListening = false, Message = "Port hanya boleh di antara 0 sampai dengan 65535" };

            //if (_ctsListen == null)
            //{
            //    _ctsListen = new();
            //    ListenToDevice(input, _ctsListen.Token);
            //}
            //else
            //{
            //    _ctsListen?.Cancel();
            //    _ctsListen = null;
            //}

            ListenToDevice(input);

            return await Task.FromResult(new ListeningInfoDto { IsListening = true });
        }

        protected abstract void ListenToDevice(ListenInput input);
    }
}
