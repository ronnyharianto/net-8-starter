using Symbol.RFID3;

namespace NET.Starter.SDK.Managers
{
    public class ZebraRfidReaderManager : IDisposable
    {
        private readonly List<RFIDReader> _readers = [];
        private bool _disposed;
        public CancellationTokenSource? CtsListen;

        // Tambahkan instance RFIDReader ke daftar
        public void AddReader(RFIDReader reader)
        {
            if (!_disposed)
            {
                lock (_readers)
                {
                    _readers.Add(reader);
                }
            }
            else
            {
                throw new ObjectDisposedException(nameof(ZebraRfidReaderManager));
            }
        }

        // Dispose semua RFIDReader
        public void DisposeAllReaders()
        {
            lock (_readers)
            {
                foreach (var reader in _readers)
                {
                    try
                    {
                        reader.Disconnect(); // Jika perlu, tambahkan logika untuk disconnect.
                        reader.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to dispose reader: {ex.Message}");
                    }
                }
                _readers.Clear();
            }
        }

        // Implementasi IDisposable
        public void Dispose()
        {
            if (!_disposed)
            {
                DisposeAllReaders();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
