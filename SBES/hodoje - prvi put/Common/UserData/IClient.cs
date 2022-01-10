using System.Runtime.Serialization;

namespace Common.UserData
{
    public interface IClient
    {
        [DataMember] string Name { get; set; }

        [DataMember] IAccount Account { get; set; }

        [DataMember] string Pin { get; set; }

        int Withdraw { get; set; }

        void ResetPin(string oldPin, string newPin);

        bool CheckPin(string pin);
    }
}