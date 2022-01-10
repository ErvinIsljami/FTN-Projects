using System.Runtime.Serialization;

namespace Common.Transaction
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        CheckBalance
    }

    public interface ITransaction
    {
        /// <summary>
        ///     Type of transaction.
        /// </summary>
        [DataMember]
        TransactionType TransactionType { get; }

        /// <summary>
        ///     Delta to be processed.
        /// </summary>
        [DataMember]
        decimal Amount { get; }

        /// <summary>
        ///     Pin to verify user.
        /// </summary>
        [DataMember]
        string Pin { get; }
    }
}