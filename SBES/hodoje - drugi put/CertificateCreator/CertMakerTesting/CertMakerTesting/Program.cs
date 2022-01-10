using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System.Collections;

namespace CertMakerTesting
{
    /// <summary>
    /// BOUNCY CASTLE API SELF SIGNED CERT GEN DEMONSTRATION
    /// </summary>
    class Program
    {
        private const string SignatureAlgorithm = "SHA512WithRSA";
        private const int KeyStrength = 2048;

		public static Org.BouncyCastle.X509.X509Certificate GenerateCACertificate(string subjectName, string password, ref AsymmetricKeyParameter privateKey)
		{
			// Create ne certificate generator, 
			X509V3CertificateGenerator generator = new X509V3CertificateGenerator();

			// Create new pseudo random number as RSA input
			var randomGen = new CryptoApiRandomGenerator();
			var random = new SecureRandom(randomGen);

			// Certificate serial number for revocation identification
			var serialNumber =
				BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
			generator.SetSerialNumber(serialNumber);

			// Populate subject name
			var subName = $"CN={subjectName}";
			var subjectDN = new X509Name(subName);
			generator.SetSubjectDN(subjectDN);

			// Since certificate is self signed subject is also the issuer
			var issuerDN = subjectDN;
			generator.SetIssuerDN(issuerDN);

			// Set certificate validity period
			var notBefore = DateTime.UtcNow.Date;
			generator.SetNotBefore(notBefore);

			var notAfter = notBefore.AddYears(2);
			generator.SetNotAfter(notAfter);

			// Input parameters for RSA key pair generator
			var keyGenerationParameters = new KeyGenerationParameters(random, KeyStrength);

			// Generate key pair

			var keyPairGenerator = new RsaKeyPairGenerator();
			keyPairGenerator.Init(keyGenerationParameters);

			var subjectKeyPair = keyPairGenerator.GenerateKeyPair();

			// Insert public key into certificate
			generator.SetPublicKey(subjectKeyPair.Public);

			// Since certificate is self signed we use subjects private key as issuers
			var issuerKeyPair = subjectKeyPair;
			ISignatureFactory factory = new Asn1SignatureFactory(SignatureAlgorithm, issuerKeyPair.Private, random);

			var certificate = generator.Generate(factory);

			// With previous step we have generated certificate with public parameters
			// Next step is for converting bouncy castle certificate into .net x509Certificate
			// and if needed generation of phys .pfx file containing private key
			var store = new Pkcs12Store();
			// CN=cert_name => cert_name
			string friendlyName = certificate.SubjectDN.ToString().Split('=')[1];

			// In memory store certificate entry
			var certificateEntry = new X509CertificateEntry(certificate);
			store.SetCertificateEntry(friendlyName, certificateEntry);

			// store.SetKeyEntry => set private key if we are generating pfx part
			store.SetKeyEntry(
				friendlyName,
				new AsymmetricKeyEntry(subjectKeyPair.Private),
				new[] { certificateEntry });

			using (var stream = new MemoryStream())
			{
				// Convert bouncy castle certs in store to stream
				store.Save(stream, password.ToCharArray(), random);

				// Generate .net x509Certificate2 from memory stream
				var convertedCertificate = new X509Certificate2(
					stream.ToArray(),
					password,
					X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);    // Persist key set only if exporting to pfx or saving with private key

				var privateCert = convertedCertificate.Export(X509ContentType.Pfx, password);
				var publicCert = convertedCertificate.Export(X509ContentType.Cert);

				convertedCertificate.Reset();
				convertedCertificate.Import(publicCert);

				convertedCertificate.Dispose();

				var privatePath = @".\certs\" + $"{subjectName}.pfx";
				var publicPath = @".\certs\" + $"{subjectName}.cer";

				File.Create(privatePath);
				File.Create(publicPath);

				File.WriteAllBytes(privatePath, privateCert);
				File.WriteAllBytes(publicPath, publicCert);
			}

			privateKey = subjectKeyPair.Private;

			return certificate;
		}

		public static void GenerateClientCertificate(string subjectName, string organizationalUnit, string password, string caName, AsymmetricKeyParameter caPrivate, Org.BouncyCastle.X509.X509Certificate caCertificate)
		{
			// Create ne certificate generator, 
			X509V3CertificateGenerator generator = new X509V3CertificateGenerator();

			// Create new pseudo random number as RSA input
			var randomGen = new CryptoApiRandomGenerator();
			var random = new SecureRandom(randomGen);

			// Certificate serial number for revocation identification
			var serialNumber =
				BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
			generator.SetSerialNumber(serialNumber);

			// Populate subject name
			var subName = $"CN={subjectName}";//OU={organizationalUnit}";
			List<DerObjectIdentifier> tempIdentifier = new List<DerObjectIdentifier>() { X509Name.CN };
			List<string> tempValues = new List<string>() { subjectName };

			if (!String.IsNullOrEmpty(organizationalUnit))
			{
				tempIdentifier.Add(X509Name.OU);
				tempValues.Add(organizationalUnit);
			}

			X509Name subjectDN = new X509Name(tempIdentifier, tempValues);
			var temp = subjectDN.GetValueList();
			generator.SetSubjectDN(subjectDN);

			X509Name caDN = new X509Name($"CN={caName}");
			generator.SetIssuerDN(caDN);

			// Set certificate validity period
			var notBefore = DateTime.UtcNow.Date;
			generator.SetNotBefore(notBefore);

			var notAfter = notBefore.AddYears(2);
			generator.SetNotAfter(notAfter);

			// Input parameters for RSA key pair generator
			var keyGenerationParameters = new KeyGenerationParameters(random, KeyStrength);

			// Generate key pair
			var keyPairGenerator = new RsaKeyPairGenerator();
			keyPairGenerator.Init(keyGenerationParameters);

			var subjectKeyPair = keyPairGenerator.GenerateKeyPair();

			// Insert public key into certificate
			generator.SetPublicKey(subjectKeyPair.Public);


			// Since certificate is self signed we use subjects private key as issuers
			var issuerKeyPair = subjectKeyPair;
			ISignatureFactory factory = new Asn1SignatureFactory(SignatureAlgorithm, caPrivate, random);

			var certificate = generator.Generate(factory);

			// In memory store certificate entry
			var certificateEntry = new X509CertificateEntry(certificate);
			var store = new Pkcs12Store();
			store.SetCertificateEntry(caName, certificateEntry);

			// store.SetKeyEntry => set private key if we are generating pfx part
			store.SetKeyEntry(
				caName,
				new AsymmetricKeyEntry(subjectKeyPair.Private),
				new[] { certificateEntry });

			using (var stream = new MemoryStream())
			{
				// Convert bouncy castle certs in store to stream
				store.Save(stream, password.ToCharArray(), random);

				// Generate .net x509Certificate2 from memory stream
				var convertedCertificate = new X509Certificate2(
					stream.ToArray(),
					password,
					X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);    // Persist key set only if exporting to pfx or saving with private key

				var privateCert = convertedCertificate.Export(X509ContentType.Pfx, password);
				var publicCert = convertedCertificate.Export(X509ContentType.Cert);

				convertedCertificate.Reset();
				convertedCertificate.Import(publicCert);

				convertedCertificate.Dispose();

				var privatePath = @".\certs\" + $"{subjectName}.pfx";
				var publicPath = @".\certs\" + $"{subjectName}.cer";

				File.WriteAllBytes(privatePath, privateCert);
				File.WriteAllBytes(publicPath, publicCert);
			}
		}

		static void Main(string[] args)
        {
			AsymmetricKeyParameter caPrivateKey = null;
			Org.BouncyCastle.X509.X509Certificate caCertificate = GenerateCACertificate("CA", "123", ref caPrivateKey);
			GenerateClientCertificate("bankservice", "", "123", "CA", caPrivateKey, caCertificate);
			GenerateClientCertificate("user1", "users", "123", "CA", caPrivateKey, caCertificate);
			GenerateClientCertificate("admin1", "admins", "123", "CA", caPrivateKey, caCertificate);
        }
    }
}
