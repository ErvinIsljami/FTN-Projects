using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common.UserData;

namespace BankServiceApp.AccountStorage
{
    public class AccountStorage : IAccountStorage
    {
        /// <summary>
        /// Key -> client Name
        /// Value -> IClient
        /// Cached data
        /// </summary>
        private Dictionary<string, IClient> clientDictionary;

        private readonly ReaderWriterLockSlim _cacheLock;


        private static IAccountStorage _instance;
        private static object _sync = new object();

        public AccountStorage()
        {
            _cacheLock = new ReaderWriterLockSlim();

            clientDictionary = new Dictionary<string, IClient>();
        }

        public static IAccountStorage Instance
        {
            get
            {
                if(_instance == null)
                {
                    lock(_sync)
                    {
                        if (_instance == null)
                            _instance = new AccountStorage();
                    }
                }

                return _instance;
            }
        }

        public List<IClient> LoadClients(string filePath)
        {
            throw new NotImplementedException();
        }

        public void StoreClients(List<IClient> clients)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfClientExists(string clientName)
        {
            if (String.IsNullOrEmpty(clientName.Trim()))
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            bool exists;
            _cacheLock.EnterReadLock();

            exists = clientDictionary.ContainsKey(clientName);

            _cacheLock.ExitReadLock();

            return exists;
        }

        public bool AddNewClient(string clientName, string pin)
        {
            if (String.IsNullOrEmpty(clientName.Trim()))
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            if (String.IsNullOrEmpty(pin.Trim()))
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            _cacheLock.EnterWriteLock();

            Client newClient = new Client(clientName, new Account(), pin);

            clientDictionary.Add(clientName, newClient);

            _cacheLock.ExitWriteLock();

            return true;
        }

        public bool ValidateClientPin(string clientName, string pin)
        {
            if (String.IsNullOrEmpty(clientName.Trim()))
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            if (String.IsNullOrEmpty(pin.Trim()))
            {
                throw new ArgumentNullException(nameof(clientName));
            }

            bool isValid = false;

            _cacheLock.EnterReadLock();

            IClient client = null;

            if(clientDictionary.TryGetValue(clientName, out client))
            {
                if (client.CheckPin(pin))
                    isValid = true;
            }

            _cacheLock.ExitReadLock();

            return isValid;
        }

        public void ChangePinCode(string clientName, string currentPin, string newPin)
        {
            IClient client = null;

            _cacheLock.EnterWriteLock();

            if(clientDictionary.TryGetValue(clientName, out client))
            {
                client.ResetPin(currentPin, newPin);
            }

            _cacheLock.ExitWriteLock();
        }
    }
}
