using System;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Common.UserData;

namespace BankServiceApp.Replication
{
    [Flags]
    public enum ReplicationType
    {
        UserData = 1,
        CertificateData = 2,
        RevokeCertificate = 4
    }

    public interface IReplicationItem
    {
        [DataMember] ReplicationType Type { get; }

        [DataMember] IClient Client { get; }

        [DataMember] X509Certificate2 Certificate { get; }
    }
}