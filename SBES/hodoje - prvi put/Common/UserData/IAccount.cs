using System.Runtime.Serialization;

namespace Common.UserData
{
    public interface IAccount
    {
        [DataMember] decimal Balance { get; }

        void Deposit(decimal amount);

        void Withdraw(decimal amount);
    }
}