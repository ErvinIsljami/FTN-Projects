using System;
using System.Runtime.Serialization;

namespace Common.Transaction
{
    [Serializable]
    [DataContract]
    public class Transaction : ITransaction
    {
        public Transaction()
        {
        }

        public Transaction(TransactionType type, decimal amount, string pin)
        {
            TransactionType = type;
            Amount = amount;
            Pin = pin;
        }

        [DataMember] public TransactionType TransactionType { get; private set; }

        [DataMember] public decimal Amount { get; private set; }

        [DataMember] public string Pin { get; private set; }
    }
}