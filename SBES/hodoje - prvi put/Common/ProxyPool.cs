using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Security.Principal;
using System.ServiceModel;

namespace Common
{
    public static class ProxyPool
    {
        private static readonly Dictionary<Type, object> _proxies = new Dictionary<Type, object>(20);

        public static void RegisterProxy<T>(T proxy)
        {
            if (_proxies.ContainsKey(typeof(T)))
                throw new ServiceLocatorException("Service of given type is already registered.");

            _proxies[typeof(T)] = proxy;
        }

        public static T GetProxy<T>()
        {
            if (!typeof(T).IsInterface)
                throw new ServiceLocatorException("Invalid generic T is not interface type.");

            if (!_proxies.TryGetValue(typeof(T), out var retVal))
                throw new ServiceLocatorException("Service of given type is not registered.");

            return (T) retVal;
        }

        public static ChannelFactory<T> CreateSecureProxyFactory<T>(string endpoint)
        {
            var binding = new NetTcpBinding(SecurityMode.Transport);
            binding.Security.Transport.ProtectionLevel = ProtectionLevel.EncryptAndSign;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.OpenTimeout = binding.CloseTimeout = TimeSpan.FromSeconds(2);
            binding.SendTimeout = binding.ReceiveTimeout = TimeSpan.FromSeconds(5);

            var factory = new ChannelFactory<T>(binding, endpoint);
            factory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            return factory;
        }

        public static void ResetState()
        {
            _proxies.Clear();
        }
    }
}