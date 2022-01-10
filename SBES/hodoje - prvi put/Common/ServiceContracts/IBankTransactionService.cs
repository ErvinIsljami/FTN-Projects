using System.ServiceModel;
using Common.Transaction;

namespace Common.ServiceContracts
{
    [ServiceContract]
    [ServiceKnownType(typeof(Transaction.Transaction))]
    public interface IBankTransactionService
    {
        /// <summary>
        ///     Execute requested transaction.
        /// </summary>
        /// <param name="transaction">Client created transaction.</param>
        /// <returns>
        ///     Remaining amount of money on the given account
        /// </returns>
        [OperationContract]
        bool ExecuteTransaction(byte[] signature, ITransaction transaction);

        [OperationContract]
        decimal CheckBalance(byte[] signature, ITransaction transaction);
    }
}