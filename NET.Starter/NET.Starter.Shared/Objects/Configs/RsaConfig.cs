namespace NET.Starter.Shared.Objects.Configs
{
    /// <summary>
    /// Represents the configuration for RSA cryptographic keys.
    /// </summary>
    public class RsaConfig
    {
        /// <summary>
        /// Gets or sets the RSA public key in PEM format.
        /// </summary>
        public string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the RSA private key in PEM format.
        /// </summary>
        public string PrivateKey { get; set; } = string.Empty;
    }
}
