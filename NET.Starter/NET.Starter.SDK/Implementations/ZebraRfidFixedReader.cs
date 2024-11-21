using NET.Starter.SDK.Interfaces;

namespace NET.Starter.SDK.Implementations
{
    internal class ZebraRfidFixedReader : IRfidFixedReader
    {
        public void Connect(string connectionString)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public string ReadTag()
        {
            return "Tag Zebra 1";
        }

        public void WriteTag(string tagData)
        {
            throw new NotImplementedException();
        }
    }
}
