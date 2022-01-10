using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Common.CertificateManager
{
    public interface ICertificateManager
    {
        /// <summary>
        ///     Get certificate from windows storage.
        /// </summary>
        /// <param name="storeLocation">Storage location.</param>
        /// <param name="storeName">Storage name.</param>
        /// <param name="subjectName">Certificate subject CN.</param>
        /// <returns>
        ///     X509Certificate instance or null if invalid subject name.
        /// </returns>
        X509Certificate2 GetCertificateFromStore(StoreLocation storeLocation, StoreName storeName, string subjectName);

        /// <summary>
        ///     Get public certificate from certificate file.
        /// </summary>
        /// <param name="filePath">Path to .cer file.</param>
        /// <exception cref="CryptographicException">Throws if <see cref="X509Certificate2" /> can't be created.</exception>
        /// <exception cref="CryptographicException">Throws if password is invalid.</exception>
        /// <returns>
        ///     X509Certificate instance or null if invalid path.
        /// </returns>
        X509Certificate2 GetPublicCertificateFromFile(string filePath);

        /// <summary>
        ///     Get full information from certificate file.
        /// </summary>
        /// <param name="filePath">Path to .pfx file.</param>
        /// <param name="password">Private key password.</param>
        /// <exception cref="CryptographicException">Throws if <see cref="X509Certificate2" /> can't be created.</exception>
        /// <exception cref="ArgumentNullException">Throws if any of arguments is null.</exception>
        /// <returns>
        ///     X509Certificate instance or null if invalid path.
        /// </returns>
        X509Certificate2 GetPrivateCertificateFromFile(string filePath, string password);

        /// <summary>
        ///     Creates new certificate, stores it in personal storage and returns path to .pfx file to distribute to client.
        /// </summary>
        /// <param name="subjectName">Client name.</param>
        /// <param name="pvkPass">Password for client private key.</param>
        /// <param name="signingCertificate">Certificate to be used to sign new certificate.</param>
        /// <param name="path">Path to dir to store new certificates.</param>
        /// <returns>
        ///     String containing path to generated .pfx file.
        /// </returns>
        string CreateAndStoreNewCertificate(string subjectName, string pvkPass, X509Certificate2 signingCertificate,
            string path = @".\certs\");

        /// <summary>
        ///     Creates new certificate and returns path to .pfx file.
        /// </summary>
        /// <param name="subjectName">Client name.</param>
        /// <param name="pvkPass">Password for private key.</param>
        /// <param name="issuer">Certificate to be used to sign new certificate.</param>
        /// <param name="path">Path to dir to store new certificate.</param>
        /// <returns>
        ///     String containing path to generated .pfx file.
        /// </returns>
        string CreateNewCertificate(string subjectName, string pvkPass, X509Certificate2 issuer,
            string path = @".\certs\");

        /// <summary>
        ///     Set CA private certificate to sign client certificates.
        /// </summary>
        /// <param name="certificate">The CA certificate.</param>
        void SetCACertificate(X509Certificate2 certificate);

        X509Certificate2 GetCACertificate();
    }
}