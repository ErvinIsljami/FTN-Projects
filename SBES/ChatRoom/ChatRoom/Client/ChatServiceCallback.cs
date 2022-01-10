using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Common;

namespace Client
{
    public class ChatServiceCallback : IChatServiceCallback
    {
        public void SendMessageToClients(string name, byte[] cryptMessage)
        {
            MainWindow mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();         

            byte[] bytes = cryptMessage;
            string decryptedMessage = mw.CryptoAlg.DecryptData(bytes);
            int lenght = decryptedMessage.Length;
            string deo = "";
            for (int i = 0; i < lenght; i++)
            {
                deo = string.Concat(deo, decryptedMessage[i]);
            }
            mw.TextBlock.Text += name + deo + "\n";
        }
    }
}