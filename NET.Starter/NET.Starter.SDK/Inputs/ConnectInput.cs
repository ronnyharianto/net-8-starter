namespace NET.Starter.SDK.Inputs
{
    public class ConnectInput
    {
        /// <summary>
        /// Host name from RFID Reader Device
        /// In mock mode use prefix 192 to successfully connected
        /// </summary>
        public string HostName { get; set; } = string.Empty;
        /// <summary>
        /// Port from RFID Reader Device
        /// In mock use port 5084 to successfully connected
        /// </summary>
        public uint Port { get; set; } = 5084;
        /// <summary>
        /// Timeout when trying to connect in milliseconds
        /// </summary>
        public uint TimeoutMilliseconds { get; set; } = 5;
    }
}
