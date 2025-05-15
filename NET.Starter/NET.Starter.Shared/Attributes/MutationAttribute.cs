using System.Net;

namespace NET.Starter.Shared.Attributes
{
    /// <summary>
    /// Attribute untuk menandai metode sebagai operasi mutasi.
    /// Biasanya digunakan untuk metode yang melakukan perubahan terhadap data,
    /// terutama dalam konteks transaksi.
    /// </summary>
    /// <remarks>
    /// <see cref="MutationAttribute"/> digunakan untuk menandai bahwa sebuah metode merupakan 
    /// operasi mutasi. Atribut ini memberi tahu middleware bahwa metode tersebut dapat 
    /// memicu commit terhadap transaksi database berdasarkan kode status HTTP hasil eksekusinya.
    /// 
    /// Jika tidak ada kode status yang didefinisikan secara eksplisit melalui 
    /// <paramref name="acceptedHttpStatusCodes"/>, maka secara default hanya 
    /// <see cref="HttpStatusCode.OK"/> (200) yang dianggap valid untuk memicu commit transaksi.
    /// Kode status lainnya akan memicu rollback.
    /// </remarks>
    /// <param name="acceptedHttpStatusCodes">
    /// (Opsional) Daftar kode status HTTP yang dianggap valid untuk memicu commit transaksi.
    /// Jika <c>null</c> atau tidak disediakan, maka hanya <see cref="HttpStatusCode.OK"/> yang dianggap valid.
    /// </param>
    [AttributeUsage(AttributeTargets.Method)]
    public class MutationAttribute(HttpStatusCode[]? acceptedHttpStatusCodes = null) : Attribute
    {
        /// <summary>
        /// Daftar kode status HTTP yang diterima untuk memicu commit transaksi.
        /// </summary>
        /// <remarks>
        /// Setiap kode status dalam array ini menunjukkan bahwa, jika metode menghasilkan 
        /// kode tersebut, middleware akan melakukan commit terhadap transaksi database. 
        /// Kode status yang tidak terdapat dalam daftar ini akan menyebabkan rollback.
        /// 
        /// <see cref="HttpStatusCode.OK"/> (200) selalu dimasukkan secara default,
        /// bahkan jika tidak ditentukan secara eksplisit.
        /// </remarks>
        public int[] AcceptedResponseCodes { get; } =
            (acceptedHttpStatusCodes?.Cast<int>() ?? []).Append((int)HttpStatusCode.OK).Distinct().ToArray();
    }
}
