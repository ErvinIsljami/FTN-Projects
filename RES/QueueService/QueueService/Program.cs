using QueueService.Service;
using QueueService.User;
using System;
using System.Collections.Generic;

namespace QueueService
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Client> clientList = new List<Client>();
            Server server = new Server();
            Client client1 = new Client(server);
            client1.ModelData.Items.Add(new Item("item1"));
            client1.ModelData.Items.Add(new Item("item2"));
            client1.ModelData.Items.Add(new Item("item3"));

            client1.ModelData.Positions.Add(new Position(1, 2, 3));
            client1.ModelData.Positions.Add(new Position(4, 5, 6));
            client1.ModelData.Positions.Add(new Position(7, 8, 9));
            clientList.Add(client1);
            string path = client1.ModelToXml();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose option: ");
                Console.WriteLine("1. Choose a client from a list.");
                Console.WriteLine("2. Make new client.");
                int option = 0;
                Client client = null;
                if (!Int32.TryParse(Console.ReadLine(), out option))
                {
                    Console.Clear();
                    Console.WriteLine("Error while processing option. Press any key to go back.");
                    Console.ReadKey();
                    continue;
                }
                if (option == 1)
                {
                    Console.WriteLine("Choose client: ");
                    for (int i = 0; i < clientList.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Client: {clientList[i].Id}");
                    }
                    int index = Int32.Parse(Console.ReadLine());
                    client = clientList[index - 1];
                }
                else if (option == 2)
                {
                    string id;
                    string items;
                    string positions;
                    Console.WriteLine("Input client id: ");
                    id = Console.ReadLine();
                    Console.WriteLine("Input items separated with semicolons ('name','quantity','isActive','destructivePower';...)");
                    items = Console.ReadLine();
                    Console.WriteLine("Input positions separated with semicolon ('x','y','z';...)");
                    positions = Console.ReadLine();
                    DataModel dataModel = new DataModel();
                    var itemsList = items.Split(';');
                    var positionList = positions.Split(';');
                    foreach (string item in itemsList)
                    {
                        var itemParts = item.Split(',');
                        Item newItem = new Item(itemParts[0], double.Parse(itemParts[1]), bool.Parse(itemParts[2]), double.Parse(itemParts[3]));
                        dataModel.Items.Add(newItem);
                    }
                    foreach (string position in positionList)
                    {
                        var posParts = position.Split(',');
                        Position newPosition = new Position(double.Parse(posParts[0]), double.Parse(posParts[1]), double.Parse(posParts[2]));
                        dataModel.Positions.Add(newPosition);
                    }
                    client = new Client(dataModel, id, server);
                    clientList.Add(client);
                }
                else
                {
                    Console.WriteLine("You entered wrong choise. Press any key to go back.");
                    Console.ReadKey();
                }
                ClientMenu(client);
            }
        }

        private static void ClientMenu(Client client)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose option.");
                Console.WriteLine("0. Go back.");
                Console.WriteLine("1. Subscribe to existing queues.");
                Console.WriteLine("2. Make new pair of queues.");
                Console.WriteLine("3. Update state.");
                int i = Int32.Parse(Console.ReadLine());
                if (i == 0)
                {
                    return;
                }
                else if(i == 1)
                {

                }
                else if(i == 2)
                {
                    Console.Clear();
                    client.Create();
                }
                else if(i == 3)
                {

                }
                else
                {
                    Console.WriteLine("You entered wrong choise. Press any key to go back");
                    Console.ReadKey();
                }
            }
        }
    }
}
