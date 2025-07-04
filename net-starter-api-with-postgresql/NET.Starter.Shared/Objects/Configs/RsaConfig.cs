namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the configuration for RSA cryptographic keys in PEM format.
    /// </summary>
    public class RsaConfig
    {
        /// <summary>
        /// The RSA public key in PEM format.
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// The RSA private key in PEM format.
        /// </summary>
        public string PrivateKey { get; set; } = string.Empty;
    }
}
