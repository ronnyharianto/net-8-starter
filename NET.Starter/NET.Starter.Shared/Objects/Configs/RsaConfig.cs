namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the configuration for RSA cryptographic keys.
    /// </summary>
    public class RsaConfig
    {
        /// <summary>
        /// RSA public key in PEM format.
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// RSA private key in PEM format.
        /// </summary>
        public string PrivateKey { get; set; } = string.Empty;
    }
}
