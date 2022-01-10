using Common.Commanding;
using Common.Communication;
using Common.ServiceInterfaces;
using Common.SymmetricEncryptionAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SectorService.Services
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
	public class SectorService : ISectorService
	{
		private SectorManager _sectorManager;
		private EncryptionInformation encryptionInformation;
		private ISymmetricAlgorithmProvider symmetricAlgorithm;

		public SectorService(string sectorType, int sectorQueueSize, int sectorQueueTimeoutPeriodInSeconds)
		{
			_sectorManager = new SectorManager(sectorType, sectorQueueSize, sectorQueueTimeoutPeriodInSeconds);
			symmetricAlgorithm = new AESAlgorithmProvider();
			encryptionInformation = new EncryptionInformation()
			{
				Key = " ?$&>?e?`d??[??????M<$H??????",
				CipherMode = System.Security.Cryptography.CipherMode.CBC
			};
		}

		public void CheckSectorAlive()
		{
			
		}

		public void SendRequest(BaseCommand command, byte[] integrityCheck)
		{
			byte[] rawData = ObjectSerializer.ObjectToByteArray(command);

			byte[] IV = new byte[16];
			Buffer.BlockCopy(integrityCheck, 4, IV, 0, 16);

			byte[] encryptedData = symmetricAlgorithm.EncryptWithIV(encryptionInformation, rawData, IV);

			if (!integrityCheck.SequenceEqual(encryptedData))
			{
				return;
			}

			_sectorManager.EnqueueCommand(command);
		}
	}
}
