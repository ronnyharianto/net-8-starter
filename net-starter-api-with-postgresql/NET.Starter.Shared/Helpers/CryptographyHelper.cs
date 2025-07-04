using Microsoft.AspNetCore.Identity;
using NET.Starter.Shared.Objects.Configs;
using Serilog;
using System.Security.Cryptography;
using System.Text;

namespace NET.Starter.Shared.Helpers
{
    /// <summary>
    /// Provides helper methods for cryptographic operations, including:
    /// <para>RSA encryption/decryption.</para>
    /// <para>password hashing/verification.</para>
    /// </summary>
    public static class CryptographyHelper
    {
        #region RSA

        private static readonly RSA _rsa = RSA.Create();
        private static RsaConfig? _rsaConfig;

        /// <summary>
        /// Initializes the RSA configuration with public and private keys.
        /// Must be called before performing RSA encryption or decryption.
        /// </summary>
        /// <param name="config">The RSA configuration containing keys.</param>
        internal static void InitializeRsa(RsaConfig config)
        {
            _rsaConfig = config;
            Log.Logger.Information("RSA Cryptography enabled.");
        }

        /// <summary>
        /// Encrypts a string value using the RSA public key.
        /// </summary>
        /// <param name="value">The plain text value to encrypt.</param>
        /// <returns>
        /// The encrypted value encoded in Base64.
        /// Returns an empty string if RSA configuration is missing or encryption fails.
        /// </returns>
        public static string EncryptRsa(string value)
        {
            if (_rsaConfig == null)
            {
                Log.Logger.Error("EncryptRsa failed: RSA configuration is not initialized.");
                return string.Empty;
            }

            _rsa.ImportFromPem(_rsaConfig.PublicKey);

            var encrypted = _rsa.Encrypt(Encoding.UTF8.GetBytes(value), RSAEncryptionPadding.OaepSHA512);
            var encryptedBase64 = Convert.ToBase64String(encrypted);

            Log.Logger.Information("Successfully encrypted data using RSA public key.");
            return encryptedBase64;
        }

        /// <summary>
        /// Decrypts an encrypted Base64 string using the RSA private key.
        /// </summary>
        /// <param name="encryptedValue">The encrypted Base64 string.</param>
        /// <returns>
        /// The decrypted plain text value.
        /// Returns an empty string if RSA configuration is missing or decryption fails.
        /// </returns>
        public static string DecryptRsa(string encryptedValue)
        {
            if (_rsaConfig == null)
            {
                Log.Logger.Error("DecryptRsa failed: RSA configuration is not initialized.");
                return string.Empty;
            }

            _rsa.ImportFromPem(_rsaConfig.PrivateKey);

            var decrypted = _rsa.Decrypt(Convert.FromBase64String(encryptedValue), RSAEncryptionPadding.OaepSHA512);
            var value = Encoding.UTF8.GetString(decrypted);

            Log.Logger.Information("Successfully decrypted data using RSA private key.");
            return value;
        }

        #endregion

        #region Password Hashing

        /// <summary>
        /// Internal password hasher instance using Microsoft.AspNetCore.Identity defaults.
        /// </summary>
        private readonly static PasswordHasher<string> _passwordHasher = new();

        /// <summary>
        /// Hashes the specified plain-text password using a secure one-way algorithm.
        /// </summary>
        /// <param name="password">The plain-text password to hash.</param>
        /// <returns>The hashed representation of the password.</returns>
        public static string HashPassword(string password)
        {
            Log.Logger.Information("Hashing password.");
            return _passwordHasher.HashPassword(default!, password);
        }

        /// <summary>
        /// Verifies whether the specified plain-text password matches the hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The previously hashed password to compare against.</param>
        /// <returns>
        /// A <see cref="PasswordVerificationResult"/> indicating the result of the verification process.
        /// </returns>
        public static PasswordVerificationResult VerifyPassword(string password, string hashedPassword)
        {
            Log.Logger.Information("Verifying password hash.");
            return _passwordHasher.VerifyHashedPassword(default!, hashedPassword, password);
        }

        #endregion
    }
}
