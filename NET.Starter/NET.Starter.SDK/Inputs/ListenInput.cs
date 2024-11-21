namespace NET.Starter.SDK.Inputs
{
    public class ListenInput
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
        public int Port { get; set; } = 5084;

        public int BackLog { get; set; } = 1;
    }
}
