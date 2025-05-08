using Microsoft.AspNetCore.Identity;
using NET.Starter.Shared.Objects.Configs;
using System.Security.Cryptography;
using System.Text;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides helper methods for cryptographic operations.
    /// </summary>
    public static class CryptographyHelper
    {
        #region RSA

        // Singleton RSA instance for cryptographic operations.
        private static readonly RSA _rsa = RSA.Create();

        // Configuration object to hold RSA public and private keys.
        private static RsaConfig? _rsaConfig;

        /// <summary>
        /// Initializes the RSA configuration with public and private keys.
        /// </summary>
        /// <param name="config">The RSA configuration containing keys.</param>
        public static void InitializeRsa(RsaConfig config)
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

            _rsa.ImportFromPem(_rsaConfig.PublicKey);

            var encrypted = _rsa.Encrypt(Encoding.UTF8.GetBytes(value), RSAEncryptionPadding.OaepSHA512);
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

            _rsa.ImportFromPem(_rsaConfig.PrivateKey);

            var decrypted = _rsa.Decrypt(Convert.FromBase64String(encryptedValue), RSAEncryptionPadding.OaepSHA512);
            var value = Encoding.UTF8.GetString(decrypted);

            return value;
        }

        #endregion

        #region Password Hashing

        /// <summary>
        /// The internal password hasher instance using the default options.
        /// </summary>
        private readonly static PasswordHasher<string> _passwordHasher = new();

        /// <summary>
        /// Hashes the specified plain-text password using a secure one-way algorithm.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>The hashed representation of the password.</returns>
        public static string HashPassword(string password) => _passwordHasher.HashPassword(default!, password);

        /// <summary>
        /// Verifies whether the specified plain-text password matches the hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The previously hashed password to compare against.</param>
        /// <returns>
        /// A <see cref="PasswordVerificationResult"/> indicating the result of the verification process.
        /// </returns>
        public static PasswordVerificationResult VerifyPassword(string password, string hashedPassword) => _passwordHasher.VerifyHashedPassword(default!, hashedPassword, password);

        #endregion
    }
}
