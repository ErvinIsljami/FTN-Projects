using System;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Common.UserData;

namespace BankServiceApp.Replication
{
    [DataContract]
    [Serializable]
    public class ReplicationItem : IReplicationItem
    {
        public ReplicationItem()
        {
        }

        public ReplicationItem(IClient client = null, ReplicationType type = ReplicationType.UserData,
            X509Certificate2 certificate = null)
        {
            if ((type & ReplicationType.UserData) != 0)
            {
                if (client == null)
                    throw new ArgumentNullException(nameof(client));
            }
            else if ((type & ReplicationType.CertificateData) != 0)
            {
                if (certificate == null)
                    throw new ArgumentNullException(nameof(certificate));
            }

            Client = client;
            Type = type;
            Certificate = certificate;
        }

        [DataMember] public IClient Client { get; private set; }

        [DataMember] public ReplicationType Type { get; private set; }

        [DataMember] public X509Certificate2 Certificate { get; private set; }
    }
}