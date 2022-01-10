using System.Security.Cryptography;

namespace Common.SymmetricEncryptionAlgorithms
{
	/// <summary>
	/// Structure for information which is needed for encryption/decryption.
	/// </summary>
	public struct EncryptionInformation
	{
		/// <summary>
		/// Secret key for decryption.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Cipher mode for encryption/decryption.
		/// </summary>
		public CipherMode CipherMode { get; set; }
	}

	/// <summary>
	/// Interface which exposes methods used for encryption and decryption.
	/// </summary>
	public interface ISymmetricAlgorithmProvider
	{
		/// <summary>
		/// Performs encryption algorithm on given bytes.
		/// </summary>
		/// <param name="encryptionInfo">Information which is used for encryption.</param>
		/// <param name="rawData">Bytes to encrypt.</param>
		/// <returns>Encrypted data.</returns>
		byte[] Encrypt(EncryptionInformation encryptionInfo, byte[] rawData);

		/// <summary>
		/// Performs decryption algorithm on given encrypted data.
		/// </summary>
		/// <param name="decryptionInfo">Information which is used for decryption.</param>
		/// <param name="encryptedData">Encrypted bytes.</param>
		/// <returns>Decrypted data.</returns>
		byte[] Decrypt(EncryptionInformation decryptionInfo, byte[] encryptedData);

		byte[] EncryptWithIV(EncryptionInformation encryptionInfo, byte[] rawData, byte[] IV);
	}
}
