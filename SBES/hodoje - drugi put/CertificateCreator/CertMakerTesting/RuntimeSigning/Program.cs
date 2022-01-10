using Org.BouncyCastle.Asn1;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace RuntimeSigning
{
	class Program
	{
		private const string SignatureAlgorithm = "SHA512WithRSA";
		private const int KeyStrength = 2048;

		static void Main(string[] args)
		{
			Console.Write("Username: ");
			string username = Console.ReadLine();
			Console.WriteLine("Organizational unit: ");
			string organizationalUnit = Console.ReadLine();
			X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
			Console.WriteLine("Opening certificate store...");
			Console.WriteLine();
			store.Open(OpenFlags.ReadWrite);
			Console.WriteLine("Certificate store opened...");
			Console.WriteLine();
			Console.WriteLine("Searching for CA certificate...");
			Console.WriteLine();
			X509Certificate2Collection collection = store.Certificates.Find(X509FindType.FindBySubjectName, "CA", true);
			store.Close();
			Console.WriteLine("Certificate store closed...");
			Console.WriteLine();
			if (collection.Count == 0)
			{
				Console.WriteLine("There is no CA certificate installed!");
				
				return;
			}
			else
			{
				Console.WriteLine("CA certificate found!");
				Console.WriteLine();
				X509Certificate2 caCertificate = collection[0];

				Console.WriteLine($"Creating certificate for {username} with organizational unit: \"{ organizationalUnit}\"...");
				Console.WriteLine();
				CreateSignedCertificate(username, organizationalUnit, caCertificate);
				Console.WriteLine($"Pfx and cer created for {username}...");
			}

			Console.WriteLine("Press <ANY> key to exit...");
			Console.ReadLine();
		}

		static void CreateSignedCertificate(string username, string organizationalUnit, X509Certificate2 caCertificate)
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
			List<DerObjectIdentifier> tempIdentifier = new List<DerObjectIdentifier>() { X509Name.CN, X509Name.OU };
			List<string> tempValues = new List<string>() { username, organizationalUnit };
			X509Name subjectDN = new X509Name(tempIdentifier, tempValues);
			generator.SetSubjectDN(subjectDN);

			generator.SetIssuerDN(new X509Name(caCertificate.SubjectName.Name));

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

			ISignatureFactory factory = new Asn1SignatureFactory(SignatureAlgorithm, DotNetUtilities.GetKeyPair(caCertificate.PrivateKey).Private, random);

			var certificate = generator.Generate(factory);

			// In memory store certificate entry
			var certificateEntry = new X509CertificateEntry(certificate);
			var store = new Pkcs12Store();
			store.SetCertificateEntry(username, certificateEntry);

			// store.SetKeyEntry => set private key if we are generating pfx part
			store.SetKeyEntry(
				username,
				new AsymmetricKeyEntry(subjectKeyPair.Private),
				new[] { certificateEntry });

			using (var stream = new MemoryStream())
			{
				// Convert bouncy castle certs in store to stream
				store.Save(stream, $"{username}123".ToCharArray(), random);

				// Generate .net x509Certificate2 from memory stream
				var convertedCertificate = new X509Certificate2(
					stream.ToArray(),
					$"{username}123",
					X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);    // Persist key set only if exporting to pfx or saving with private key

				var privateCert = convertedCertificate.Export(X509ContentType.Pfx, $"{username}123");
				var publicCert = convertedCertificate.Export(X509ContentType.Cert);

				convertedCertificate.Reset();
				convertedCertificate.Import(publicCert);

				var privatePath = @".\certs\" + $"{username}.pfx";
				var publicPath = @".\certs\" + $"{username}.cer";

				if (!Directory.Exists("certs"))
				{
					Directory.CreateDirectory("certs");
				}

				File.WriteAllBytes(privatePath, privateCert);
				File.WriteAllBytes(publicPath, publicCert);
			}
		}

	}
}
