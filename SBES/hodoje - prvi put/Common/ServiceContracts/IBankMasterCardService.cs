using System.ServiceModel;
using Common.DataEncapsulation;

namespace Common.ServiceContracts
{
    [ServiceContract]
    public interface IBankMasterCardService
    {
        /// <summary>
        ///     Request new card certificate creation.
        /// </summary>
        /// <returns>
        ///     NewCardResults that contains all information that is relevant to client
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(CustomServiceException))]
        NewCardResults RequestNewCard(string password);

        [OperationContract]
        bool ExtendCard(string password);

        /// <summary>
        ///     Revoke existing certificate given to client.
        /// </summary>
        /// <param name="pin">Pin which authorizes revocation.</param>
        /// <returns>
        ///     True if existing card is successfully revoked.
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(CustomServiceException))]
        bool RevokeExistingCard(string pin);

        /// <summary>
        ///     Request password reset.
        /// </summary>
        /// <exception cref="CustomServiceException"></exception>
        /// <returns>
        ///     NewCardResults that contains all information that is relevant to client
        /// </returns>
        [OperationContract]
        [FaultContract(typeof(CustomServiceException))]
        NewCardResults RequestResetPin();

        /// <summary>
        ///     Checks user credentials arrived trough windows api.
        /// </summary>
        [OperationContract]
        void Login();

        /// <summary>
        ///     Returns arbitration state of current instance.
        /// </summary>
        /// <returns>
        ///     The arbitration state.
        /// </returns>
        [OperationContract]
        ServiceState CheckState();
    }
}