using Common;
using Manager;
using SecondaryServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    public partial class MainWindow : Window
    {
        public ICryptographyInterface CryptoAlg { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            SetChatRoomName();
            ClientConnect();
        }

        private void ClientConnect()
        {
            ClientServerChannel channel = new ClientServerChannel();

            channel.ProxyChat.ConnectTo();
            CryptoAlg = channel.ProxyChat.GetCryptoAlg();
        }

        private void SetChatRoomName()
        {
            WindowsIdentity winIdentity = WindowsIdentity.GetCurrent();

            foreach (IdentityReference group in winIdentity.Groups)
            {
                SecurityIdentifier sid = (SecurityIdentifier)group.Translate(typeof(SecurityIdentifier));
                var groupName = sid.Translate(typeof(NTAccount));
                if (groupName.ToString().Contains('\\'))
                {
                    string groupRealName = groupName.ToString().Split('\\')[1];
                    if (groupRealName == "Studenti")
                    {
                        Label.Content = "Students chat room";
                        break;
                    }
                    if (groupRealName == "Profesori")
                    {
                        Label.Content = "Profesors chat room";
                        break;
                    }
                }
            }
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(TextBox.Text))
            {
                using (ClientServerChannel channel = new ClientServerChannel())
                {
                    string message = TextBox.Text;
                    
                    byte[] encryptedMessage = CryptoAlg.EncryptData(message);
                  
                    channel.ProxyChat.SendMessageToServer(encryptedMessage);                   
                }
            }
            TextBox.Text = "";
        }
    }
}