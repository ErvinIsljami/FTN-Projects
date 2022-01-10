using Common.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankServiceApp.AccountStorage
{
    public interface IAccountStorage
    {
        /// <summary>
        /// Check if client exists in the system by clients name
        /// </summary>
        /// <param name="clientName"></param>
        /// <exception cref="ArgumentNullException">Throws if clientName is null.</exception>
        /// <returns>
        /// True if the client exists otherwise false
        /// </returns>
        bool CheckIfClientExists(string clientName);

        /// <summary>
        /// Validates the client pin
        /// </summary>
        /// <param name="clientName">Name that is searched for</param>
        /// <param name="pin">Pin code to be validated</param>
        /// <exception cref="ArgumentNullException">Throws if any of arguments is null.</exception>
        /// <returns></returns>
        bool ValidateClientPin(string clientName, string pin);

        /// <summary>
        /// Creates new client and adds him to the system
        /// </summary>
        /// <param name="clientName">Client name</param>
        /// <param name="pin">Client pin code</param>
        /// <exception cref="ArgumentNullException">Throws if any of arguments is null.</exception>
        /// <returns>
        /// True if the client is successfully added otherwise false
        /// </returns>
        bool AddNewClient(string clientName, string pin);

        /// <summary>
        /// Changes the pin code for the given client
        /// </summary>
        /// <param name="clientName">Client name to be searched for</param>
        /// <param name="currentPin">Pin used for validation</param>
        /// <param name="newPin">New pin code</param>
        /// <exception cref="ArgumentNullException">Throws if any of arguments is null.</exception>
        void ChangePinCode(string clientName,string currentPin, string newPin);

        /// <summary>
        /// Serialize all clients to XML file
        /// </summary>
        /// <param name="cleints">Clients list.</param>
        void StoreClients(List<IClient> clients);

        /// <summary>
        /// Loads clients from given XML file
        /// </summary>
        /// <param name="filePath">Path to the XML file.</param>
        /// <exception cref="ArgumentNullException">Throws if the filePath is not valid.</exception>
        /// <returns>
        /// List of clients
        /// </returns>
        List<IClient> LoadClients(string filePath);
    }
}
