using NET.Starter.Shared.Objects.Configs;
using System.Security.Cryptography;
using System.Text;

namespace NET.Starter.Shared.Helpers
{
    public static class CryptographyHelper
    {
        // Singleton RSA instance for cryptographic operations.
        private static readonly RSA _rsa = RSA.Create();

        // Configuration object to hold RSA public and private keys.
        private static RsaConfig? _rsaConfig;

        /// <summary>
        /// Initializes the RSA configuration with public and private keys.
        /// </summary>
        /// <param name="config">The RSA configuration containing keys.</param>
        public static void Initialize(RsaConfig config)
        {
            _rsaConfig = config; // Store the RSA configuration for later use.
        }

        /// <summary>
        /// Encrypts a string value using the RSA public key.
        /// </summary>
        /// <param name="value">The plain text value to encrypt.</param>
        /// <returns>The encrypted value encoded in Base64, or an empty string if configuration is missing.</returns>
        public static string EncryptRsa(string value)
        {
            if (_rsaConfig == null) // Check if RSA configuration is initialized.
                return string.Empty;

            _rsa.ImportFromPem(_rsaConfig.PublicKey); // Load the public key for encryption.

            // Encrypt the value using RSA and OAEP with SHA-512 padding.
            var encrypted = _rsa.Encrypt(Encoding.UTF8.GetBytes(value), RSAEncryptionPadding.OaepSHA512);

            // Convert the encrypted byte array to a Base64 string.
            var encryptedBase64 = Convert.ToBase64String(encrypted);

            return encryptedBase64;
        }

        /// <summary>
        /// Decrypts an encrypted Base64 string using the RSA private key.
        /// </summary>
        /// <param name="encryptedValue">The encrypted Base64 string.</param>
        /// <returns>The decrypted plain text value, or an empty string if configuration is missing.</returns>
        public static string DecryptRsa(string encryptedValue)
        {
            if (_rsaConfig == null) // Check if RSA configuration is initialized.
                return string.Empty;

            _rsa.ImportFromPem(_rsaConfig.PrivateKey); // Load the private key for decryption.

            // Convert the Base64 string to a byte array and decrypt it.
            var decrypted = _rsa.Decrypt(Convert.FromBase64String(encryptedValue), RSAEncryptionPadding.OaepSHA512);

            // Convert the decrypted byte array to a UTF-8 string.
            var value = Encoding.UTF8.GetString(decrypted);

            return value;
        }
    }
}
