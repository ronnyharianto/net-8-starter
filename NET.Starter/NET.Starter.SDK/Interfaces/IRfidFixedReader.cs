namespace NET.Starter.SDK.Interfaces
{
    public interface IRfidFixedReader
    {
        void Connect(string connectionString);
        void Disconnect();
        string ReadTag();
        void WriteTag(string tagData);
    }
}
