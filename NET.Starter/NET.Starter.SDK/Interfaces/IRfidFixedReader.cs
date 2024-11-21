using NET.Starter.SDK.Dtos;
using NET.Starter.SDK.Inputs;

namespace NET.Starter.SDK.Interfaces
{
    public interface IRfidFixedReader
    {
        /// <summary>
        /// Connect to RFID Reader on Server Mode
        /// </summary>
        /// <returns></returns>
        ConnectedInfoDto Connect(ConnectInput input);

        /// <summary>
        /// Connect to RFID Reader on Client Mode
        /// </summary>
        /// <returns></returns>
        Task<ListeningInfoDto> Listen(ListenInput input);
    }
}
