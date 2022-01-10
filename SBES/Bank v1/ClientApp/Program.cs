using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:9999/UserService";

            bool isExit = false;
            do
            {
                using (WCFClient proxy = new WCFClient(binding, new EndpointAddress(new Uri(address))))
                {
                    Console.WriteLine("Izaberite opciju: ");
                    Console.WriteLine("1. Otvorite racun.");
                    Console.WriteLine("2. Aplicirajte za kredit.");
                    Console.WriteLine("3. Uplata/Isplata.");
                    Console.WriteLine("4. Odobri kredit.");
                    Console.WriteLine("5. Odobri nalog.");
                    Console.WriteLine("6. Odbijanje kredita.");
                    Console.WriteLine("7. Odbijanje naloga.");
                    Console.WriteLine("0. Izlaz.");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            {
                                Console.WriteLine("Unesite user name novog naloga: ");
                                string userName = Console.ReadLine();
                                proxy.MakeNewAccount(userName);
                                break;
                            }
                        case "2":
                            {
                                Console.WriteLine("Unesite user name novog naloga: ");
                                string userName = Console.ReadLine();
                                Console.WriteLine("Unesite iznos kredita? ");
                                double amount = double.Parse(Console.ReadLine());
                                proxy.ApplyForCredit(userName, amount);
                                break;
                            }
                        case "3":
                            {
                                Console.WriteLine("Unesite user name: ");
                                string userName = Console.ReadLine();
                                Console.WriteLine("Unesite iznos +/- (uplata/isplata).");
                                double amount = double.Parse(Console.ReadLine());
                                proxy.ExecuteTransaction(userName, amount);
                                break;
                            }
                        case "4":
                            {
                                var lista = proxy.GetAllCreditRequests();
                                if (lista == null || lista.Count == 0)
                                {
                                    Console.WriteLine("Nema kredita koje je potrebno odobriti.");
                                    break;
                                }
                                lista.ForEach(x => Console.WriteLine($"{x.User} - {x.CreditAmount}"));
                                Console.WriteLine("Unesite user name: ");
                                string userName = Console.ReadLine();
                                proxy.ApproveCredit(userName);
                                break;
                            }
                        case "5":
                            {
                                var lista = proxy.GetAllAccountRequests();
                                if (lista == null || lista.Count == 0)
                                {
                                    Console.WriteLine("Nema naloga koje je potrebno odobriti.");
                                    break;
                                }
                                lista.ForEach(x => Console.WriteLine($"{x.User}"));
                                Console.WriteLine("Unesite user name: ");
                                string userName = Console.ReadLine();
                                proxy.ApproveAccount(userName);
                                break;
                            }
                        case "6":
                            {
                                var lista = proxy.GetAllCreditRequests();
                                if (lista == null || lista.Count == 0)
                                {
                                    Console.WriteLine("Nema kredita koje je potrebno odobriti.");
                                    break;
                                }
                                lista.ForEach(x => Console.WriteLine($"{x.User} - {x.CreditAmount}"));
                                Console.WriteLine("Unesite user name: ");
                                string userName = Console.ReadLine();
                                proxy.DenyCredit(userName);
                                break;
                            }
                        case "7":
                            {
                                var lista = proxy.GetAllAccountRequests();
                                if (lista == null || lista.Count == 0)
                                {
                                    Console.WriteLine("Nema naloga koje je potrebno odobriti.");
                                    break;
                                }
                                lista.ForEach(x => Console.WriteLine($"{x.User}"));
                                Console.WriteLine("Unesite user name: ");
                                string userName = Console.ReadLine();
                                proxy.DenyAccount(userName);
                                break;
                            }
                        case "0":
                            {
                                isExit = true;
                                break;
                            }
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine("Niste uneli dobru opciju.");
                                break;
                            }
                    }
                }
            } while (!isExit);
        }
    }
}
