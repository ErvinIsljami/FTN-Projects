using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Common.SymmetricEncryptionAlgorithms
{
	/// <summary>
	/// AES encryption provider.
	/// </summary>
	public class AESAlgorithmProvider : ISymmetricAlgorithmProvider
	{
		/// <inheritdoc/>
		public byte[] Decrypt(EncryptionInformation decryptionInfo, byte[] encryptedData)
		{
			byte[] decryptedData;
			AesCryptoServiceProvider aesCrypto = new AesCryptoServiceProvider()
			{
				Key = ASCIIEncoding.ASCII.GetBytes(decryptionInfo.Key),
				Mode = decryptionInfo.CipherMode,
				Padding = PaddingMode.None,
			};

			if (decryptionInfo.CipherMode == CipherMode.CBC)
			{
				aesCrypto.IV = encryptedData.Take(aesCrypto.BlockSize / 8).ToArray();
			}

			ICryptoTransform aesDecrypt = aesCrypto.CreateDecryptor();

			decryptedData = aesDecrypt.TransformFinalBlock(encryptedData, aesCrypto.IV.Length, encryptedData.Length);

			return decryptedData;
		}

		/// <inheritdoc/>
		public byte[] Encrypt(EncryptionInformation encryptionInfo, byte[] rawData)
		{
			byte[] encryptedData;
			byte[] initialVector = new byte[0];
			AesCryptoServiceProvider aesCrypto = new AesCryptoServiceProvider()
			{
				Key = ASCIIEncoding.ASCII.GetBytes(encryptionInfo.Key),
				Mode = encryptionInfo.CipherMode,
				Padding = PaddingMode.None,
			};

			if (encryptionInfo.CipherMode == CipherMode.CBC)
			{
				aesCrypto.GenerateIV();
				initialVector = aesCrypto.IV;
			}

			ICryptoTransform aesEncrypt = aesCrypto.CreateEncryptor();

			encryptedData = BitConverter.GetBytes(initialVector.Length).Concat(initialVector.Concat(aesEncrypt.TransformFinalBlock(rawData, 0, rawData.Length - (rawData.Length % encryptionInfo.Key.Length)))).ToArray();

			return encryptedData;
		}

		public byte[] EncryptWithIV(EncryptionInformation encryptionInfo, byte[] rawData, byte[] IV)
		{
			byte[] encryptedData;
			AesCryptoServiceProvider aesCrypto = new AesCryptoServiceProvider()
			{
				Key = ASCIIEncoding.ASCII.GetBytes(encryptionInfo.Key),
				Mode = encryptionInfo.CipherMode,
				Padding = PaddingMode.None,
			};

			if (encryptionInfo.CipherMode == CipherMode.CBC)
			{
				aesCrypto.IV = IV;
			}

			ICryptoTransform aesEncrypt = aesCrypto.CreateEncryptor();

			encryptedData = BitConverter.GetBytes(IV.Length).Concat(IV.Concat(aesEncrypt.TransformFinalBlock(rawData, 0, rawData.Length - (rawData.Length % encryptionInfo.Key.Length)))).ToArray();

			return encryptedData;
		}
	}
}
