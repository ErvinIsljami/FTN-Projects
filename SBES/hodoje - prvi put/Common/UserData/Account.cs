using System;
using System.Runtime.Serialization;
using System.Threading;
using Common.Transaction;

namespace Common.UserData
{
    [Serializable]
    [DataContract]
    public class Account : IAccount
    {
        private decimal _balance;
        private ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();

        public Account()
        {
        }

        public Account(decimal initialBalance = 0m)
        {
            Balance = initialBalance;
        }

        [DataMember]
        public decimal Balance
        {
            get
            {
                if (_rwLock == null) _rwLock = new ReaderWriterLockSlim();
                decimal balance;
                _rwLock.EnterReadLock();
                {
                    balance = _balance;
                }
                _rwLock.ExitReadLock();

                return balance;
            }
            private set
            {
                if (_rwLock == null) _rwLock = new ReaderWriterLockSlim();
                _rwLock.EnterWriteLock();
                {
                    _balance = value;
                }
                _rwLock.ExitWriteLock();
            }
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount > Balance)
                throw new NotEnoughResourcesException("User tried to withdraw more resources than available.");
            Balance -= amount;
        }

        public void SetBalance(decimal balance)
        {
            Balance = balance;
        }
    }
}