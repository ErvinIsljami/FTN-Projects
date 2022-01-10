using System.Security.Cryptography.X509Certificates;

namespace Common.CertificateManagement
{
	public static class CertificateStorageReader
	{
		public static bool GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName, out X509Certificate2 certificate)
		{
			X509Store store = new X509Store(storeName, storeLocation);
			store.Open(OpenFlags.ReadOnly);

			X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);

			store.Close();

			if (certCollection.Count > 0)
			{
				certificate = certCollection[0];
				return true;
			}

			certificate = null;
			return false;
		}
	}
}
