using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using Common.CertificateManager;
using Common.Transaction;

namespace ClientApp
{
    internal class Program
    {
        private static string _username;
        private static string _privateCertPath;
        private static string _publicCertPath;
        private static SecureString _password;
        private static X509Certificate2 _clientCertificate;
        private static ClientProxy _clientProxy;

        private static void Main(string[] args)
        {
            string odg;

            Console.WriteLine("Enter your username: ");
            _username = Console.ReadLine();

            Console.WriteLine("Enter your password: ");
            _password = GetClientPassword();

            try
            {
                _clientProxy = new ClientProxy(_username, _password);


                _privateCertPath = $"{ClientAppConfig.CertificatePath}{_username}.pfx";
                _publicCertPath = _privateCertPath.Replace(".pfx", ".cer");

                _clientCertificate = TryGetPrivateCertificate(_privateCertPath);

                if (_clientCertificate != null) _clientProxy.OpenTransactionServiceProxy(_clientCertificate);

                do
                {
                    Console.Clear();
                    Console.WriteLine("Welcome to BankService");
                    Console.WriteLine("Chose one of the following options:");
                    Console.WriteLine("0.InsertMasterCard");
                    Console.WriteLine("1.Withdrawal.");
                    Console.WriteLine("2.Deposit.");
                    Console.WriteLine("3.Check balance.");
                    Console.WriteLine("4.Revoke your MasterCard.");
                    Console.WriteLine("5.Request new MasterCard.");
                    Console.WriteLine("6.Renew your MasterCard.");
                    Console.WriteLine("7.Reset your pin code.");
                    Console.WriteLine("8.Exit");
                    odg = Console.ReadLine();
                    if (odg.Length > 0)
                        try
                        {
                            switch (odg?[0])
                            {
                                case '0':
                                    InsertMasterCard();
                                    break;
                                case '1': //Withdrawal
                                    Withdraw();
                                    break;

                                case '2': //Deposit
                                    Deposit();
                                    break;

                                case '3': //CheckBalance
                                    CheckBalance();
                                    break;

                                case '4': //Revoke MasterCard
                                    RevokeMasterCard();
                                    break;

                                case '5': //Request new MasterCard
                                    RequestNewMasterCard();
                                    break;
                                case '6': //Extend MasterCard
                                    RenewMasterCard();
                                    break;
                                case '7': //Reset Pin
                                    ResetPin();
                                    break;
                            }
                        }
                        catch (SecurityAccessDeniedException e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    Console.ReadLine();
                } while (odg.Length == 0 || odg?[0] != '8');
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
        }

        private static void InsertMasterCard()
        {
            var cert = _clientCertificate ?? (_clientCertificate = TryGetPrivateCertificate(_privateCertPath));
            if (cert != null)
                _clientProxy.OpenTransactionServiceProxy(_clientCertificate);
            else
                Console.WriteLine("Error no master card certificate found. Chose option 5. to request new card.");
        }

        private static SecureString GetClientPassword()
        {
            var pass = new SecureString();
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (pass.Length > 0)
                    {
                        pass.RemoveAt(pass.Length - 1);
                        Console.Write("\b \b");
                    }

                    continue;
                }

                if (key.Key == ConsoleKey.Enter) break;

                pass.AppendChar(key.KeyChar);
                Console.Write("*");
            }

            Console.WriteLine();
            return pass;
        }

        private static byte[] Sign(X509Certificate2 cert, ITransaction transaction)
        {
            byte[] signature;
            using (var hasAlg = new SHA512Cng())
            {
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, transaction);

                    var hash = hasAlg.ComputeHash(stream);
                    signature = cert.GetRSAPrivateKey()
                        .SignHash(hash, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
                }
            }

            return signature;
        }

        private static X509Certificate2 TryGetPrivateCertificate(string pathCert, string password = null)
        {
            X509Certificate2 cert = null;
            if (password == null)
            {
                Console.WriteLine("Enter your certificate password");
                password = Console.ReadLine();
            }

            try
            {
                cert = CertificateManager.Instance.GetPrivateCertificateFromFile(pathCert, password);
            }
#if DEBUG
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
#else
            catch 
            {
                //Console.WriteLine(ex.Message);
            }
#endif
            return cert;
        }

        private static bool CheckIfMasterCardExists(string path, string username)
        {
            if (!File.Exists(path))
                return false;

            try
            {
                var cert = CertificateManager.Instance.GetPublicCertificateFromFile(path);
                return cert.Subject.Equals($"CN={username}");
            }
#if DEBUG
            catch (Exception e)
            {
                return false;
            }
#else
            catch
            {
                return false;
            }
#endif
        }

        private static void Withdraw()
        {
            if (_clientCertificate != null)
            {
                Console.WriteLine("Enter your pin:");
                var pin = Console.ReadLine();

                Console.WriteLine("How much money do you wish to withdraw?");
                decimal.TryParse(Console.ReadLine(), out var amount);

                var transaction = new Transaction(TransactionType.Withdrawal, amount, pin);
                var signature = Sign(_clientCertificate, transaction);

                var result = false;
                string message = null;
                try
                {
                    result = _clientProxy.ExecuteTransaction(signature, transaction);
                }
                catch (SecurityAccessDeniedException ex)
                {
                    message = ex.Message;
                    _clientCertificate = null;
                }
                catch (FaultException ex)
                {
                    message = ex.Message;
                    _clientCertificate = null;
                }
                finally
                {
                    if (result)
                        Console.WriteLine("Successful withdrawal.");
                    else
                        Console.WriteLine($"Transaction declined. Reason: {message ?? "Not enough resources."}");
                }
            }
            else
            {
                Console.WriteLine("No card inserted chose option 0. to insert card.");
            }
        }

        private static void Deposit()
        {
            if (_clientCertificate != null)
            {
                Console.WriteLine("Enter your pin:");
                var pin = Console.ReadLine();

                Console.WriteLine("How much money do you wish to deposit?");
                decimal.TryParse(Console.ReadLine(), out var amount);

                var transaction = new Transaction(TransactionType.Deposit, amount, pin);
                var signature = Sign(_clientCertificate, transaction);
                var result = false;
                string message = null;
                try
                {
                    result = _clientProxy.ExecuteTransaction(signature, transaction);
                }
                catch (SecurityAccessDeniedException ex)
                {
                    message = ex.Message;
                    _clientCertificate = null;
                }
                catch (FaultException ex)
                {
                    message = ex.Message;
                    _clientCertificate = null;
                }
                finally
                {
                    if (result)
                        Console.WriteLine("Successful deposit.");
                    else
                        Console.WriteLine(
                            $"Transaction failed. {(message != null ? $"Reason: {message}." : string.Empty)}");
                }
            }
            else
            {
                Console.WriteLine("No card inserted chose option 0. to insert card.");
            }
        }

        private static void CheckBalance()
        {
            if (_clientCertificate != null)
            {
                Console.WriteLine("Enter your pin:");
                var pin = Console.ReadLine();

                var transaction = new Transaction(TransactionType.CheckBalance, 0, pin);
                var signature = Sign(_clientCertificate, transaction);

                var amount = _clientProxy.CheckBalance(signature, transaction);

                Console.WriteLine("Your current balance is {0}", amount);
            }
            else
            {
                Console.WriteLine("No card inserted chose option 0. to insert card.");
            }
        }

        private static void RevokeMasterCard()
        {
            if (_clientCertificate != null || CheckIfMasterCardExists(_publicCertPath, _username))
            {
                Console.WriteLine("Enter your pin:");
                var pin = Console.ReadLine();

                if (_clientProxy.RevokeExistingCard(pin))
                {
                    File.Delete(_privateCertPath);
                    File.Delete(_publicCertPath);
                    Console.WriteLine("Certificate revoked successfully.");
                    _clientCertificate = null;
                }
            }
            else
            {
                Console.WriteLine(
                    "You currently don't own a MasterCard!! If you wish to request new one select option 5.");
            }
        }

        private static void RequestNewMasterCard()
        {
            Console.WriteLine("Enter key encryption password:");
            var password = Console.ReadLine();

            if (_clientCertificate == null || !CheckIfMasterCardExists(_publicCertPath, _username))
            {
                var newCardResults = _clientProxy.RequestNewCard(password);
                Console.WriteLine("Pin code for your new MasterCard is: {0}", newCardResults.PinCode);
            }
            else
            {
                Console.WriteLine("You already own a MasterCard!!");
            }
        }

        private static void ResetPin()
        {
            if (_clientCertificate != null || CheckIfMasterCardExists(_publicCertPath, _username))
            {
                var newCardResults = _clientProxy.RequestResetPin();
                Console.WriteLine("Your new pin code is {0}", newCardResults.PinCode);
            }
            else
            {
                Console.WriteLine(
                    "You currently dont own a MasterCard!! If you wish to request new one select option 5.");
            }
        }

        private static void RenewMasterCard()
        {
            Console.WriteLine("Enter key encryption password of your MasterCard:");
            var password = Console.ReadLine();
            X509Certificate2 cert = null;
            cert = TryGetPrivateCertificate(_privateCertPath, password);
            if (cert == null)
            {
                Console.WriteLine("Wrong password or no card found.");
            }
            else
            {
                if (_clientCertificate != null && CheckIfMasterCardExists(_publicCertPath, _username))
                {
                    if (_clientProxy.ExtendCard(password))
                        Console.WriteLine("Successfully extended your MasterCard");
                    else
                        Console.WriteLine("Unsuccessfully extesion.");

                    _clientCertificate = null;
                }
                else
                {
                    Console.WriteLine(
                        "You currently don't own a MasterCard!! If you wish to request new one select option 5.");
                }
            }
        }
    }
}