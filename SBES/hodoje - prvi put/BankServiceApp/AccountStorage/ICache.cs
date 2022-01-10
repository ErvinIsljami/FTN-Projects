using System;
using System.Collections.Generic;
using Common.UserData;

namespace BankServiceApp.AccountStorage
{
    public interface ICache : IDisposable
    {
        /// <summary>
        ///     Get client from storage.
        /// </summary>
        /// <param name="clientName">The client name.</param>
        /// <returns>
        ///     Client interface.
        /// </returns>
        bool TryGetClient(string clientName, out IClient client);

        /// <summary>
        ///     Get account from storage.
        /// </summary>
        /// <param name="clientName">The client name.</param>
        /// <returns>
        ///     Account interface.
        /// </returns>
        bool TryGetAccount(string clientName, out IAccount account);

        /// <summary>
        ///     Get all clients from cache.
        /// </summary>
        /// <returns>
        ///     All clients from cache.
        /// </returns>
        List<Client> GetAllClients();

        /// <summary>
        ///     Load data from configured location.
        /// </summary>
        void LoadData();

        /// <summary>
        ///     Store cache data at configured location.
        /// </summary>
        void StoreData();
    }
}